namespace FavroUpdater.Console;

public class ActionInputs
{
    [Option('t', "token",
        Required = true,
        HelpText = "The Favro API token.")]
    public string Token { get; set; } = null!;

    [Option('c', "cardid",
    Required = true,
    HelpText = "Card id")]
    public string CardId { get; set; } = null!;

    [Option('o', "organizationid",
        Required = true,
        HelpText = "Organization id")]
    public string OrganizationId { get; set; } = null!;

    [Option('f', "fieldid",
        Required = false,
        HelpText = "Field id")]
    public string FieldId { get; set; } = null!;

    [Option('v', "value",
        Required = false,
        HelpText = "Field value")]
    public string FieldValue { get; set; } = null!;

    [Option('d', "display",
        Required = false,
        HelpText = "Field display")]
    public string FieldDisplay { get; set; } = null!;

    [Option('y', "fieldtype",
        Required = false,
        HelpText = "Custom field type")]
    public string FieldType { get; set; } = null!;

    [Option('a', "tag",
        Required = false,
        HelpText = "Add tag")]
    public string AddTag { get; set; } = null!;

    [Option('r', "removetagid",
        Required = false,
        HelpText = "Remove tag with id")]
    public string RemoveTagId { get; set; } = null!;
}
