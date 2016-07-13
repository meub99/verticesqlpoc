using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models.SQL;

namespace VerticeSqlPoc.Web.Services.Interfaces
{
    public interface IAvgCapitalBalanceService
    {

        IEnumerable<AvgCapitalBalance> Calculate(List<TableGL> glItems);
        IEnumerable<CompanyInfo> GetCompanyCodes(List<TableGL> glItems);
    }
}