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

        string refreshToken = Guid.NewGuid().ToString();

        var refresh_token = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refresh_token);
        _context.SaveChanges();
        
        return new LoginResponse
        {
            Message = "Logged in successfully",
            Email = user.Email,
            Role = user.Role,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public void Logout(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        var refreshTokens = _context.RefreshTokens.FirstOrDefault(rt => rt.UserId == user.Id);
        
        if (refreshTokens == null)
        {
            throw new UnauthorizedAccessException("User is logged out.");
        }
        _context.RefreshTokens.Remove(refreshTokens);
        _context.SaveChanges();
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

    public LoginResponse RefreshToken(string refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

        if (token == null || token.ExpiresAt < DateTime.UtcNow || token.IsRevoked)
        {
            throw new InvalidOperationException("Invalid or expired refresh token.");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == token.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with Id {token.UserId} does not exist.");
        }

        string newToken = _jwtService.GenerateToken(user);

        return new LoginResponse
        {
            Message = "Token refreshed successfully",
            Email = user.Email,
            Role = user.Role,
            Token = newToken,
            RefreshToken = refreshToken
        };
    }
}