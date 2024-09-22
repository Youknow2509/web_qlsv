using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
//
using qlsv.Models;
using qlsv.ViewModels;
using qlsv.Helpers;
using qlsv.Data;


namespace qlsv.Identity.Controllers;

[Area("Identity")]
public class LoginController : Controller
{
    // Variable
    private readonly ILogger<LoginController> _logger;
    private readonly qlsv.Data.IdentityDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly SecurityHelper _securityHelper;

    // Constructor
    public LoginController(
        ILogger<LoginController> logger,
        qlsv.Data.IdentityDbContext context,
        JwtHelper jwtHelper,
        SecurityHelper securityHelper
    ) {
        _logger = logger;
        _context = context;
        _jwtHelper = jwtHelper;
        _securityHelper = securityHelper;
    }

    /**
     * Get Login basic
     */
    public IActionResult Index()
    {
        return View();
    }

    /**
     * Post Login basic
     */
    [HttpPost]
    public IActionResult Index(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            string passwordHash = _securityHelper.Hash(model.Password);
            var user = _context.Users.FirstOrDefault(
                u => 
                    (u.UserName.ToUpper() == model.UserNameOrEmail.ToUpper() ||
                        u.Email.ToUpper() == model.UserNameOrEmail.ToUpper())
                    && u.PasswordHash == passwordHash
            );
            if (user!=null)
            {
                string jwtToken = _jwtHelper.GenerateJwtToken(
                    user.Id,
                    user.UserName,
                    null
                );
                Response.Cookies.Append("jwt", jwtToken);
                return RedirectToAction("Index", "Home", new { area = ""});
            }
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
