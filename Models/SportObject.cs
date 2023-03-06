namespace KorogodovMapApp.Models;

public class SportObject
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public bool IsActive { get; set; }
    public string URL { get; set; }
    public string? WorkingHoursMondayToFriday { get; set; }
    public string? WorkingHoursSaturday { get; set; }
    public string? WorkingHoursSunday { get; set; }
    public SportObjectDetail? SportObjectDetail { get; set; }
    public SportObjectType? SportObjectType { get; set; }
    public Curator? Curator { get; set; }

    public IList<SportType> SportTypes { get; set; } = new List<SportType>();
}
