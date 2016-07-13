using System;
using System.Collections;
using System.Collections.Generic;


namespace VerticeSqlPoc.Web.Services.Models
{
    public class ClientResponse
    {
        public string Id { get; set; }
        public string Created { get; set; }
        public ResponseStatus Status { get; set; }
        public IDictionary Payload { get; set; }

        public ClientResponse()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now.ToShortDateString();
            Payload = new Dictionary<string, object>();
            Status = ResponseStatus.InProcess;
        }
    }

    public enum ResponseStatus
    {
        Error = -1,
        InProcess = 0,
        Success = 1
    }
}

