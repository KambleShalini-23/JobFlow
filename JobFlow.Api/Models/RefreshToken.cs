using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobFlowApi.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    
    public bool IsRevoked { get; set; } = false;

    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}