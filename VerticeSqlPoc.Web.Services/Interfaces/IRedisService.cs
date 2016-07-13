using VerticeSqlPoc.Web.Services.Models;
using System.Collections.Generic;

namespace VerticeSqlPoc.Web.Services.Interfaces
{
    public interface IRedisService
    {
        ClientResponse InsertList<T>(string key, List<T> items);

        ClientResponse GetList<T>(string key);
    }
}
