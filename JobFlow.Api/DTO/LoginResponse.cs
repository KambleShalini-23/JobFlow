using JobFlowApi.Models;

namespace JobFlowApi.DTO;

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}