using System.Data.Spatial;
using System.Web.Http.Controllers;
using IModelBinder = System.Web.Http.ModelBinding.IModelBinder;
using ModelBindingContext = System.Web.Http.ModelBinding.ModelBindingContext;

namespace App.GeoNow.Library.Web.Api.ModelBinders
{
    public class DbGeographyModelBinder : IModelBinder
    {

        private DbGeography BindModelImplementation(string value)
        {
            if (value == null)
            {
                return (DbGeography)null;
            }
            string[] latLongStr = value.Split(',');
            // TODO: More error handling here, what if there is more than 2 pieces or less than 2?
            //       Are we supposed to populate ModelState with errors here if we can't conver the value to a point?
            string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            //4326 format puts LONGITUDE first then LATITUDE
            DbGeography result = DbGeography.FromText(point, 4326);
            return result;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            //var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //return BindModelImpl(valueProviderResult != null ? valueProviderResult.AttemptedValue : null);

            if (bindingContext.ModelType != typeof(DbGeography))
            {
                return false;
            }

            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (val == null)
            {
                return false;
            }

            string key = val.RawValue as string;
            if (key == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
                return false;
            }

            //DbGeography result;
            var model = BindModelImplementation(key);
            if (model != null)
            {
                bindingContext.Model = model;
                return true;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value to Location");

            return false;
        }
    }
}