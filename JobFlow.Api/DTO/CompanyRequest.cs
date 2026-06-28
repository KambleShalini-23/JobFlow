using System.ComponentModel.DataAnnotations;

namespace JobFlowApi.DTO;

public class CompanyRequest
{

    [Required]
    [StringLength(50)]
    public string Name {get; set;} = string.Empty;

    [Url]
    public string Website {get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Industry {get; set;} = string.Empty;

    [Required]
    [StringLength(50)]
    public string Location {get; set;} = string.Empty;

    [Range(1, 10000000)]
    public int EmployeeCount { get; set; }
}