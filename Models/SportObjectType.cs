namespace KorogodovMapApp.Models;

public class SportObjectType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public IList<SportObject> SportObjects { get; set; } = new List<SportObject>();
}
