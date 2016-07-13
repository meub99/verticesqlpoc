using VerticeSqlPoc.Web.Services.Models;
using System.Collections.Generic;
using VerticeSqlPoc.Web.Services.Models.SQL;

namespace VerticeSqlPoc.Web.Services.Interfaces
{
    public interface ISqlService
    {

        //ClientResponse FindAll<T>(T arg) where T : class;
        IEnumerable<TableGL> FindTop(int count);
        TableGL Find(string id);
        IEnumerable<CompanyInfo> GetCompanyCodes();
        IEnumerable<TableGL> GetGLItems(string companyCode);
        IEnumerable<TableGL> GetAllGLItems();
        IEnumerable<CompanyNavInfo> GetAllNav();
    }
}
