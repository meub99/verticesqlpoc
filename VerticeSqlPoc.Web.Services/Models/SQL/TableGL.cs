using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticeSqlPoc.Web.Services.Models.SQL
{
    public class TableGL
    {
        public TableGL()
        {
        }


        public DateTime? Date { get; set; }
        public string DateStr { get; set; }
        public DateTime? DateEOM { get; set; }
        public string DateStrEOM { get; set; }
        public string DateStrBOM { get; set; }
        public string CompanyCode { get; set; }
        public string SecurityNo { get; set; }
        public decimal CostBasis { get; set; }
        public decimal NAV { get; set; }
        public decimal NAVTotal { get; set; }
        public decimal PerformanceGain { get; set; }
        public decimal ActiveValue { get; set; }

    }
}
