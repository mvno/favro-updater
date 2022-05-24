namespace FavroUpdater.Console.Models;

public class Board
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string WorkingColumn { get; set; }
	public string ReviewColumn { get; set; }
	public string ReviewedColumn { get; set; }
	public string TestColumn { get; set; }
	public string TestedColumn { get; set; }
	public string ProductionColumn { get; set; }
}
