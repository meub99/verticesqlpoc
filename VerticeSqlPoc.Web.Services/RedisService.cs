using StackExchange.Redis;
using System;
using System.Diagnostics;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

//Proof of Concept, not intended for production
namespace VerticeSqlPoc.Web.Services
{
    public class RedisService : IRedisService
    {

        #region setup
        private static string _connection;
        public RedisService(string connection)
        {
            _connection = connection;
        }
        #endregion

        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions config = ConfigurationOptions.Parse(_connection);
            config.SyncTimeout = (9000);
            config.AbortOnConnectFail = (false);

            return ConnectionMultiplexer.Connect(config);
        });
    



        private static IDatabase Cache
        {
            get
            {
                return Connection.GetDatabase();
            }
        }

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }


        public ClientResponse InsertList<T>(string key,List<T> items)
        {
            var _response = new ClientResponse();

            try
            {
                Cache.StringSet(key, JsonConvert.SerializeObject(items));
                _response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {

                _response.Payload.Add("ErrorMessage", ex.Message);
                _response.Status = ResponseStatus.Error;
                Trace.TraceError(ex.Message);

            }

            return _response;

        }

        public ClientResponse GetList<T>(string key)
        {
            var _response = new ClientResponse();

            try
            {
                string serializedItems = Cache.StringGet(key);
                if (!String.IsNullOrEmpty(serializedItems))
                {
                    _response.Payload.Add("KeyValue", JsonConvert.DeserializeObject<List<T>>(Cache.StringGet(key)));
                }
                 _response.Status = ResponseStatus.Success;
             }
            catch (Exception ex)
            {

                _response.Payload.Add("ErrorMessage", ex.Message);
                _response.Status = ResponseStatus.Error;
                Trace.TraceError(ex.Message);

            }

            return _response;

        }


    }

}
