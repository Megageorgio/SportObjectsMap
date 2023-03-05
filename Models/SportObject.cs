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
    public IList<SportType> SportTypes { get; set; } = new List<SportType>();
}

public class SportObjectDetail
{
    public int Id { get; set; }
    public string? ShortDescription { get; set; }
    public string? AdditionalDescription { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? FederalSubject { get; set; }
    public string? MunicipalDistrict { get; set; }
    public DateTime? ActionStartDate { get; set; }
    public DateTime? ActionEndDate { get; set; }
    public bool IsReconstruction { get; set; }
    public int SportObjectId { get; set; }
    public SportObject? SportObject { get; set; }
}

public class SportObjectType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public IList<SportObject> SportObjects { get; set; } = new List<SportObject>();
}

public class SportType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IList<SportObject> SportObjects { get; set; } = new List<SportObject>();
}
