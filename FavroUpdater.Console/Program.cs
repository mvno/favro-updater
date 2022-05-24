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

await parser.WithParsedAsync(options => DoWork(options, logger));

static async Task DoWork(ActionInputs inputs, ILogger logger)
{
	var favro = new Favro(inputs.Token, inputs.OrganizationId, logger);
	var card = await favro.GetCard(inputs.CardId);

	var customFields = new[]
					{favro.GenerateCustomFieldLink(inputs.FieldId, inputs.FieldValue, inputs.FieldDisplay) };

	await favro.SetCustomFields(card.cardId, customFields);
}