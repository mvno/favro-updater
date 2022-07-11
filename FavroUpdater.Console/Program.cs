using System.Globalization;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("FavroUpdater.Console", LogLevel.Information)
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(
    errors =>
    {
        logger.LogError(string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        Environment.Exit(2);
    });

await parser.WithParsedAsync(options => DoWork(options));

static async Task DoWork(ActionInputs inputs)
{
    var favro = new Favro(inputs.Token, inputs.OrganizationId);
    var card = await favro.GetCard(inputs.CardId);

    if (!string.IsNullOrEmpty(inputs.AddTag) && !card.tags.Any(x => x.Equals(inputs.AddTag, StringComparison.OrdinalIgnoreCase)))
    {
        await favro.AddTagByName(card.cardId, inputs.AddTag);
    }

    if (!string.IsNullOrEmpty(inputs.RemoveTagId))
    {
        try
        {
            await favro.RemoveTag(card.cardId, inputs.RemoveTagId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    if (!string.IsNullOrEmpty(inputs.FieldType) && !string.IsNullOrEmpty(inputs.FieldId))
    {
        var field = inputs.FieldType.ToLower(CultureInfo.InvariantCulture) switch
        {
            "link" => favro.GenerateCustomFieldLink(inputs.FieldId, inputs.FieldValue, inputs.FieldDisplay),
            "text" => favro.GenerateCustomFieldText(inputs.FieldId, inputs.FieldValue),
            _ => throw new InvalidOperationException($"Field type {inputs.FieldType} not supported"),
        };
        var customFields = new[] { field };

        await favro.SetCustomFields(card.cardId, customFields);
    }
}
