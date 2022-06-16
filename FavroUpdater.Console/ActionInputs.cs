namespace FavroUpdater.Console;

public class ActionInputs
{
    [Option('t', "token",
        Required = true,
        HelpText = "The Favro API token.")]
    public string Token { get; set; } = null!;

    [Option('f', "fieldid",
        Required = true,
        HelpText = "Field id")]
    public string FieldId { get; set; } = null!;

    [Option('v', "value",
        Required = true,
        HelpText = "Field value")]
    public string FieldValue { get; set; } = null!;

    [Option('d', "display",
        Required = true,
        HelpText = "Field display")]
    public string FieldDisplay { get; set; } = null!;

    [Option('c', "cardid",
        Required = true,
        HelpText = "Card id")]
    public string CardId { get; set; } = null!;

    [Option('y', "fieldtype",
        Required = true,
        HelpText = "Custom field type")]
    public string FieldType { get; set; } = null!;

    [Option('o', "organizationid",
        Required = true,
        HelpText = "Organization id")]
    public string OrganizationId { get; set; } = null!;

    [Option('a', "tag",
        Required = false,
        HelpText = "Tag")]
    public string Tag { get; set; } = null!;
}
