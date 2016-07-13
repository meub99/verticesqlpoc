using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;

namespace VerticeSqlPoc.Web.Api.Models
{
    public class GLItemsForCompany
    {
        public string CompanyCode;
        public List<TableGL> GLItems;
    }
}