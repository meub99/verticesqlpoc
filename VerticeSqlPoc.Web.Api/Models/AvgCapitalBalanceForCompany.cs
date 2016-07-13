using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerticeSqlPoc.Web.Services.Models;

namespace VerticeSqlPoc.Web.Api.Models
{
    public class AvgCapitalBalanceForCompany
    {
        public string CompanyCode;
        public IEnumerable<AvgCapitalBalance> Balances;
    }
}