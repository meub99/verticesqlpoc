using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using System.Data.SqlClient;
using VerticeSqlPoc.Web.Services;
using System.Drawing;
using System.Collections.Concurrent;

namespace VerticeSqlPoc.Web.Services
{
    public class StandardDeviationService : IStandardDeviationService
    {

        public IEnumerable<StandardDeviation> CalculateByCompany(IEnumerable<CompanyNavInfo> companyNavValues)
        {

            // This will give you StdDeviation for each company separate
            var intermidateResult = from n in companyNavValues
                                    group n by n.CompanyCode into cc
                                    select new { CompanyCode = cc.Key, Count = cc.Count(), Average = cc.Average(o => o.Nav), NavValues = cc.Select(o => o.Nav) };

            // Need to use a thread-safe datastructure like ConcurrentBag as List is not thread-safe and the Parrellel processing will
            // result in items getting added to the collection at different times (or at exactly the same time)
            // the data structure must handle multiple concurrent writes to it
            //https: //msdn.microsoft.com/en-us/library/ff458671(v=vs.110).aspx

            ConcurrentBag<StandardDeviation> result = new ConcurrentBag<StandardDeviation>();
            Parallel.ForEach(intermidateResult, ir =>
            {
                int mycount = ir.Count;
                double myavg = ir.Average;
                double mysum = ir.NavValues.Sum(d => (d - myavg) * (d - myavg));
                double mystddev = Math.Sqrt(mysum / mycount);

                result.Add(new StandardDeviation() { CompanyCode = ir.CompanyCode, StdDev = mystddev });
            });

            return result;


        }

        //{

        //    // This will give you StdDeviation for each company separate
        //    var intermidateResult = from n in companyNavValues
        //                            group n by n.CompanyCode into cc
        //                            select new { CompanyCode = cc.Key, Count = cc.Count(), Average = cc.Average(o => o.Nav), NavValues = cc.Select(o => o.Nav) };

        //    List<StandardDeviation> result = new List<StandardDeviation>();
        //    foreach (var ir in intermidateResult)
        //    {
        //        int mycount = ir.Count;
        //        double myavg = ir.Average;
        //        double mysum = ir.NavValues.Sum(d => (d - myavg) * (d - myavg));
        //        double mystddev = Math.Sqrt(mysum / mycount);

        //        result.Add(new StandardDeviation() { CompanyCode = ir.CompanyCode, StdDev = mystddev });
        //    }

        //    return result;


        //}

        // This will give you StdDeviation for all NAV values
        public double Calculate(IEnumerable<double> companyNavValues)
        {
            int mycount = companyNavValues.Count();
            double myavg = companyNavValues.Average();
            double mysum = companyNavValues.Sum(d => (d - myavg) * (d - myavg));
            return Math.Sqrt(mysum / mycount);
        }
    }
}
