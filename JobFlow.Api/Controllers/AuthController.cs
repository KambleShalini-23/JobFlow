using System.Security.Claims;
using JobFlowApi.DTO;
using JobFlowApi.Models;
using JobFlowApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFlowApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var user = _authService.Register(request);
        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authService.Login(request);
        return Ok(result);
    }

    [HttpGet("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
           
        if (email == null)
        {
            throw new UnauthorizedAccessException("Invalid token.");
        }
        _authService.Logout(email);

        return Ok(new { Message = "Logged out successfully." });
    }

    [HttpGet("{me}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetLoggedInUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in.");
        }
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId,
            email,
            role
        });
    }

    [HttpPost("{role}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateUserRole(RoleRequest request)
    {
        var result = _authService.UpdateUserRole(request);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public IActionResult RefreshToken(RefreshTokenRequest request)
    {
        var result = _authService.RefreshToken(request.RefreshToken);
        return Ok(result);
    }


}