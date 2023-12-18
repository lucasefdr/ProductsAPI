using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpGet]
    public ActionResult<string> Get() => "AuthorizeController :: " + DateTime.Now.ToLongDateString();

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(UserDTO model)
    {
        IdentityUser user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);

        if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

        await _signInManager.SignInAsync(user, false);

        return Ok(GenerateToken(model));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserDTO userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded) return Ok(GenerateToken(userInfo));
        else return BadRequest("Invalid login");
    }

    private UserTokenDTO GenerateToken(UserDTO userInfo)
    {
        // Definições dos usuários
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("message", "helloWorld"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Gera uma chave com base em um algoritmo simétrico
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        // Gera a assinatura digital do token usando o algoritmo HMAC e a chave privada
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Tempo de expiração do token
        var expiration = DateTime.UtcNow.AddHours(double.Parse(_config["TokenConfiguration:ExpireHours"]!));

        // Classe que representa um token JWT e gera o token
        JwtSecurityToken token = new (
            issuer: _config["TokenConfiguration:Issuer"],
            audience: _config["TokenConfiguration:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new UserTokenDTO()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Message = "Token JWT ok"
        };
    }
}
