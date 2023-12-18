using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTOs;

namespace ProductsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserDTO userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded) return Ok();
        else return BadRequest();
    }
}
