namespace JobFlowApi.DTO;

public class CompanyResponse
{
    
    public string Name { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
}