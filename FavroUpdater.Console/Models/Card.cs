namespace FavroUpdater.Console.Models;

public class Sender
{
	public string userId { get; set; }
}

public class Assignment
{
	public string userId { get; set; }
}

public class CustomFieldLink
{
	public string url { get; set; }
	public string text { get; set; }
}

public class CustomField
{
	public string customFieldId { get; set; }
	public string color { get; set; }
	public CustomFieldLink link { get; set; }
}

public class Card
{
	public string cardId { get; set; }
	public string cardCommonId { get; set; }
	public string name { get; set; }
	public string widgetCommonId { get; set; }
	public string columnId { get; set; }
	public string detailedDescription { get; set; }
	public List<string> tags { get; set; }
	public int sequentialId { get; set; }
	public List<Assignment> assignments { get; set; }
	public string laneId { get; set; }
	public List<CustomField> customFields { get; set; }
}

public class Column
{
	public string columnId { get; set; }
}

public class SourceColumn
{
	public string columnId { get; set; }
}

public class CardEvent
{
	public string action { get; set; }
	public Sender sender { get; set; }
	public Card card { get; set; }
	public Column column { get; set; }
	public SourceColumn sourceColumn { get; set; }
}
