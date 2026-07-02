using JobFlowApi.Data;
using JobFlowApi.DTO;
using JobFlowApi.Models;

namespace JobFlowApi.Services;

public class AuthService
{

    private readonly AppDbContext _context;

    private readonly JwtService _jwtService;

    public AuthService(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }
        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();

        return new RegisterResponse
        {
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public LoginResponse Login(LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        string token = _jwtService.GenerateToken(user);
        
        return new LoginResponse
        {
            Message = "Logged in successfully",
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }

    public RoleRequest UpdateUserRole(RoleRequest roleRequest)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == roleRequest.Email);
        
        if(user == null)
        {
            throw new KeyNotFoundException($"User with email {roleRequest.Email} does not exist.");
        }

        if (!Enum.IsDefined(typeof(Role), roleRequest.Role))
        {
            throw new InvalidOperationException("Invalid role.");
        }
        
        user.Role = roleRequest.Role;
        _context.SaveChanges();

        return new RoleRequest
        {
            Email = user.Email,
            Role = user.Role
        };

    }
}