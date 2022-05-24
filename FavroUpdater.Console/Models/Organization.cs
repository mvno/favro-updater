namespace FavroUpdater.Console.Models;

public class Organization
{
	public string Id { get; set; }
	public string ServiceAccountId { get; set; }
	public string ManualTag { get; set; }
	public string FastTag { get; set; }
	public string DeployTag { get; set; }
	public string RollbackTag { get; set; }
	public string CheckingTag { get; set; }
	public string ShadowingTag { get; set; }
	public string BuildStatusLinkField { get; set; }
	public string ReviewLinkField { get; set; }
	public string VersionField { get; set; }
	public string ColorField { get; set; }
	public string BuildingTag { get; set; }
	public string DeleteTag { get; set; }
	public string DeletingTag { get; set; }
}
