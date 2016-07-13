using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VerticeSqlPoc.Web.Services.Models
{   
    [DataContract(Name = "standarddeviation", Namespace = "")]
    public class StandardDeviation
    {
        [DataMember(Name = "companycode")]
        public string CompanyCode;
        [DataMember(Name = "stdev")]
        public double StdDev;
    }

    [CollectionDataContract(Name = "results", Namespace = "")]
    public class StandardDeviations : List<StandardDeviation> { }

}
