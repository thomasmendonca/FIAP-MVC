using System.Diagnostics;
using FIAP_MVC.Data;
using FIAP_MVC.DTOs;
using Microsoft.AspNetCore.Mvc;
using FIAP_MVC.Models;

namespace FIAP_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _dataContext;

    public HomeController(ILogger<HomeController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Register(RegisterDTO request)
    {
        var user = _dataContext.PZ_Users.FirstOrDefault(x => x.UserEmail==request.UserEmail);
        if (user != null)
        {
            return BadRequest("Usuário já existe");
        }

        User NewUser = new User
        {
            UserEmail = request.UserEmail,
            UserName = request.UserName,
            UserPassword = request.UserPassword,
            UserPhone = request.UserPhone
        };
        _dataContext.Add(NewUser); 
        _dataContext.SaveChanges();
        return View();
    }

    public IActionResult Login(LoginDTO request)
    {
        var find = _dataContext.PZ_Users.FirstOrDefault(x => x.UserEmail == request.UserEmail);
        if (find == null)
        {
            return NotFound();
        }

        if (find.UserPassword != request.UserPassword)
        {
            return BadRequest("Senha inválida");
        }

        ViewBag.userData = find;
        return View(find);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}