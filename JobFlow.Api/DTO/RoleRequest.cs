using JobFlowApi.Models;

namespace JobFlowApi.DTO;

public class RoleRequest
{
    public string Email { get; set; } = string.Empty;
    
    public Role Role { get; set; } = Role.User;
}