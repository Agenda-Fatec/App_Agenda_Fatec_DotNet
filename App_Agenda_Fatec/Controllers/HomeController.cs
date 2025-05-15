using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using App_Agenda_Fatec.Models;

namespace App_Agenda_Fatec.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {

        this._logger = logger;

    }

    public IActionResult Index()
    {

        return View();

    }

    public IActionResult Team()
    {

        return View();

    }

    public IActionResult Help()
    {

        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    
    }

}