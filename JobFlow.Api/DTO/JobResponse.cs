namespace JobFlowApi.DTO;

public class JobResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}
