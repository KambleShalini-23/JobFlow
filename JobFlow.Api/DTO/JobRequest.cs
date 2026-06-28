using System.ComponentModel.DataAnnotations;

namespace JobFlowApi.DTO;

public class JobRequest
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;
    [StringLength(200, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Location { get; set; } = string.Empty;
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
    public decimal Salary { get; set; }
    [Required]
    public int CompanyId { get; set; }
}