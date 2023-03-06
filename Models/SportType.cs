namespace KorogodovMapApp.Models;

public class SportType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IList<SportObject> SportObjects { get; set; } = new List<SportObject>();
}
