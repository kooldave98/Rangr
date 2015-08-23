using App.Domain;
using App.Domain.Geolocation;
using App.Domain.Posts.Commands;
using App.Domain.Posts.Queries;
using System;
using System.Data.Spatial;
using System.Web.Mvc;

namespace App.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetByGeoLocation GetPostsByGeoLocation;
        private readonly Insert InsertPostCommand;


        public HomeController()
        {
            GetPostsByGeoLocation = new GetByGeoLocation();
            InsertPostCommand = new Insert();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Winphone()
        {
            return View();
        }


        public ActionResult Test(string Geolocation = "40.69847032728747,-73.9514422416687")
        {

            //string[] latLongStr = Geolocation.Split(',');
            //// TODO: More error handling here, what if there is more than 2 pieces or less than 2?
            ////       Are we supposed to populate ModelState with errors here if we can't conver the value to a point?
            //string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            ////4326 format puts LONGITUDE first then LATITUDE
            //DbGeography result = DbGeography.FromText(point, 4326);

            try
            {
                var res = GetPostsByGeoLocation.Execute(new GetByGeolocationRequest() { GeoLocationString = Geolocation });

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
    }
}
