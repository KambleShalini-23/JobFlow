using JobFlowApi.DTO;
using JobFlowApi.Models;
using JobFlowApi.Services;
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
}