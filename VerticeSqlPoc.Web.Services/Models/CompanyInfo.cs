using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VerticeSqlPoc.Web.Services.Models
{
    //public class CompanyInfo
    //{
    //    public string CompanyCode;
    //    public int NumGLItems;
    //}

    [DataContract(Name = "companyinfo", Namespace = "")]
    public class CompanyInfo
    {
        [DataMember(Name = "companycode")]
        public string CompanyCode;
        [DataMember(Name = "navsum")]
        public decimal? NavSum;
        [DataMember(Name = "numglitems")]
        public int NumGLItems;
    }
    [CollectionDataContract(Name = "results", Namespace = "")]
    public class CompanyInfos : List<CompanyInfo> { }

}
