using BigProject.Data.Entities;
using BigProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BigProject.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;
    private readonly SignInManager<StoreUser> signInManager;
    private readonly UserManager<StoreUser> userManager;
    private readonly IConfiguration config;

    public AccountController(ILogger<AccountController> logger,
        SignInManager<StoreUser> signInManager,
        UserManager<StoreUser> userManager,
        IConfiguration config)
    {
        this.logger = logger;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.config = config;
    }

    public IActionResult Login()
    {
        if (this.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (Request.Query.Keys.Contains("ReturnUrl"))
                {
                    return Redirect(Request.Query["ReturnUrl"].First());
                }
                else
                {
                    return RedirectToAction("Shop", "App");
                }
            }
        }
        ModelState.AddModelError("", "Failed to login");
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "App");
    }

    [HttpPost]
    public async Task<ActionResult> CreateToken([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(
                    user, model.Password, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        config["Token:Issuer"],
                        config["Token:Audience"], 
                        claims,
                        signingCredentials: creds,
                        expires: DateTime.UtcNow.AddMinutes(20));

                    return Created("", new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
        }
        return BadRequest();
    }
}
