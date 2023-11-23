using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UFGBank.Data;
using UFGBank.Models;

namespace UFGBank.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UFGBankDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(UFGBankDbContext context, ILogger<HomeController> logger)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var currentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _context.AspNetUsers.Where(i => i.Id == currentId).First();
        return View(user);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminSession()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}