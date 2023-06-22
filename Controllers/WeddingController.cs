using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    private MyContext db;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }
    // -------------------------------------------------------
    
    
    [HttpGet("/weddings")]
    public IActionResult AllWeddings()
    {
        List<Wedding> allWeddings = db.Weddings.Include(w => w.Guests).ToList();
        return View("AllWeddings", allWeddings);
    }
    // -------------------------------------------------------
    [HttpGet("weddings/new")]
    public IActionResult NewWedding()
    {
        return View("New");
    }
    // -------------------------------------------------------
    [HttpPost("Weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            return View("New");
        }

        newWedding.UserID = (int)HttpContext.Session.GetInt32("UUID");

        db.Weddings.Add(newWedding);
        db.SaveChanges();

        return RedirectToAction("AllWeddings");
    }
    // -------------------------------------------------------
    [HttpPost("weddings/{weddingId}/delete")]
    public IActionResult DeleteWedding(int weddingId)
    {
        Wedding? wedding = db.Weddings.FirstOrDefault(wedding => wedding.WeddingId == weddingId);
        if (wedding != null)
        {
            db.Weddings.Remove(wedding);
            db.SaveChanges();
        }
        return RedirectToAction("AllWeddings");
    }
    // -------------------------------------------------------
    [HttpPost("weddings/{weddingId}/rsvp")]
    public IActionResult WeddingRSVP(int WeddingId)
    {
        RSVP? wedRegister = db.RSVPs.FirstOrDefault(r => r.UserId == HttpContext.Session.GetInt32("UUID") && r.WeddingId == WeddingId);

        if(wedRegister == null)
        {
            RSVP newRSVP = new RSVP()
            {
                WeddingId = WeddingId,
                UserId = (int)HttpContext.Session.GetInt32("UUID")
            };

            db.RSVPs.Add(newRSVP);
        }
        else
        {
            db.RSVPs.Remove(wedRegister);
        }

        db.SaveChanges();
        return RedirectToAction("AllWeddings");

    }
    // -------------------------------------------------------
    [HttpGet("weddings/{id}")]
    public IActionResult ViewWedding(int id)
    {
        Wedding? wedding = db.Weddings.Include(wedding => wedding.Creator).Include(wedding => wedding.Guests).ThenInclude(g => g.User).FirstOrDefault(wedding => wedding.WeddingId == id);

        if (wedding == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View("Details", wedding);
    }
    // -------------------------------------------------------
}

// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if (userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
