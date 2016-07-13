using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models.SQL;
using System.Data.SqlClient;

namespace VerticeSqlPoc.Web.Services.Interfaces
{
    public interface IStandardDeviationService
    {

        IEnumerable<StandardDeviation> CalculateByCompany(IEnumerable<CompanyNavInfo> companyNavValues);
        double Calculate(IEnumerable<double> companyNavValues);
    }
}