using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace VerticeSqlPoc.Web.Services.Models
{
    //[XmlRoot("Results")]
    //public class AvgCapitalBalance
    //{
    //    public string CompanyCode;
    //    public int Month;
    //    public int Year;
    //    public decimal? NavSum;
    //    public int NumGLItems;
    //}

    [DataContract(Name = "avgcapitalbalance", Namespace = "")]
    public class AvgCapitalBalance
    {
        [DataMember(Name = "companycode")]
        public string CompanyCode;
        [DataMember(Name = "month")]
        public int Month;
        [DataMember(Name = "year")]
        public int Year;
        [DataMember(Name = "navsum")]
        public decimal? NavSum;
        [DataMember(Name = "numglitems")]
        public int NumGLItems;
    }
    [CollectionDataContract(Name = "results", Namespace = "")]
    public class AvgCapitalBalances : List<AvgCapitalBalance> { }

}
