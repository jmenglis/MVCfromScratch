using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Security.Claims;
using MVCfromScratch.Models;

namespace MVCfromScratch.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Lists list) {
            if (ModelState.IsValid) {
                string timeToday = DateTime.Now.ToString("h:mm:ss tt");
                string dateToday = DateTime.Now.ToString("M/dd/yyyy");
                string check_public = Request.Form["check_public"];
                using (var db = new MainDbContext()) {
                    Claim sessionEmail = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email);
                    string userEmail = sessionEmail.Value;
                    var userIdQuery = db.Users.Where(u => u.Email == userEmail).Select(u => u.Id);
                    var userId = userIdQuery.ToList();

                    var dbList = db.Lists.Create();
                    dbList.Details = list.Details;
                    dbList.Date_Posted = dateToday;
                    dbList.Time_Posted = timeToday;
                    dbList.User_Id = userId[0];
                    if (check_public != null) {
                        dbList.Public = "YES";
                    } else {
                        dbList.Public = "NO";
                    }
                    db.Lists.Add(dbList);
                    db.SaveChanges();
                }
            } else {
                ModelState.AddModelError("", "Incorrect format has been placed");
            }
            return View();
        }
    }
}