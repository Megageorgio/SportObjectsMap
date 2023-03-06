namespace KorogodovMapApp.Models;

public class Curator
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public IList<SportObject> SportObjects { get; set; } = new List<SportObject>();
}
