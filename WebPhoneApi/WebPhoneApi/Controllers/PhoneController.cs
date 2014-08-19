using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebPhoneApi.Controllers
{
    public class PhoneController : ApiController
    {
        public  string GetExtension(string userName,string password,string customerId,string callerId)
        {
            //TODO:
            //1. Telego call HHAExchange : callerId, appname,AppKey, AppSercret
            //2. HHAExchange return a Coordinator1 Name
            //3. Query Extension Number of Coordinator in TeleGo DB
            //4. Return Extension to PBX
        }
    }
}
