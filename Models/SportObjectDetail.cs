namespace KorogodovMapApp.Models;

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
    public string? OKTMO { get; set; }
    public int? TotalFunding { get; set; }
    public SportObject? SportObject { get; set; }
}
