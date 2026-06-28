using System.Text.Json.Serialization;

namespace JobFlowApi.Models;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int CompanyId { get; set; }

    [JsonIgnore]
    public Company? Company { get; set; }
}