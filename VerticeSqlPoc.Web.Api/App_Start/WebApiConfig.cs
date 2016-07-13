using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using VerticeSqlPoc.Web.Api.Models;
using VerticeSqlPoc.Web.Services.Models;

namespace VerticeSqlPoc.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ////OData API route
            //var builder = new ODataConventionModelBuilder();
            //builder.EntitySet<CompanyInfo>("CompanyInfo");
            //config.MapODataServiceRoute("ODataRoute", null, builder.GetEdmModel());
        }
    }
}
