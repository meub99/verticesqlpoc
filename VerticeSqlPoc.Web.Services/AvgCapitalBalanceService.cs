using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;

namespace VerticeSqlPoc.Web.Services
{
    public class AvgCapitalBalanceService : IAvgCapitalBalanceService
    {
        public IEnumerable<AvgCapitalBalance> Calculate(List<Models.SQL.TableGL> glItems)
        {
            return from gl in glItems
                   group gl by new { gl.CompanyCode, Month = gl.Date.HasValue ? gl.Date.Value.Month : 0, Year = gl.Date.HasValue ? gl.Date.Value.Year : 0 } into cc
                   orderby cc.Key.Year, cc.Key.Month
                   select new AvgCapitalBalance() { CompanyCode = cc.Key.CompanyCode, Month = cc.Key.Month, Year = cc.Key.Year, NavSum = cc.Sum(r => r.NAV), NumGLItems = cc.Count() };
        }

        public IEnumerable<CompanyInfo> GetCompanyCodes(List<TableGL> glItems)
        {
            return from gl in glItems
                   group gl by gl.CompanyCode into cc
                   orderby cc.Key
                   select new CompanyInfo() { CompanyCode = cc.Key, NavSum = cc.Sum(r => r.NAV), NumGLItems = cc.Count() };
        }
    }
}



