using System.Diagnostics;
using pr4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace pr4.Controllers;


public class AccountController : Controller
{
    ApplicationContext db;
    public AccountController(ApplicationContext context)
    {
        db = context;
    }

    // private readonly ILogger<HomeController> _logger;

    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

    [HttpPost]
    public IActionResult SignIn(User user)
    {
        bool UserExists = db.User.Any(x => x.Login == user.Login && x.Password == user.Password);
        if (UserExists)
        {
            return RedirectToAction("ShowMessages", "Messages", new { login = user.Login });
        }
        ViewBag.ErrorMessage = "Неверный логин и/или пароль";
        return View();
    }

    public IActionResult SignIn(){
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(User user)
    {
        bool UserExists = db.User.Any(x => x.Login == user.Login);

        if (UserExists)
        {
            ViewBag.ErrorMessage = "Пользователь с таким логином уже существует";
            return View("SignUp", user);
        }
        else
        {
            db.User.Add(user);
            db.SaveChangesAsync();

            return RedirectToAction("SignIn");
        }
    }

    public IActionResult SignUp(){
        return View();
    }

    // public IActionResult Register(User user)
    // {


    // }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
