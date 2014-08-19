using Infrastructure;

using Infrastructure.HhaExchange;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace TeleGoApi.Controllers
{
    public class PhoneController : ApiController
    {
        private string appStart = string.Empty;
        private string appKey = string.Empty;
        private string appSecret = string.Empty;
        private string appName = string.Empty;
        private string filter = string.Empty;

        public string Filter
        {
            get {
                if (String.IsNullOrEmpty(filter))
                {
                    filter = ConfigurationManager.AppSettings["Filter"].ToString();
                } 
                return filter; }
            set { filter = value; }
        }
        public string AppName
        {
            get
            {
                if (String.IsNullOrEmpty(appName))
                {
                    appName = ConfigurationManager.AppSettings["AppName"].ToString();
                } 
                return appName;
            }
            set { appName = value; }
        }
        public string AppKey
        {
            get
            {
                if (String.IsNullOrEmpty(appKey))
                {
                    appKey = ConfigurationManager.AppSettings["AppKey"].ToString();
                }
                return appKey;
            }
            set { appKey = value; }
        } 
        public string AppSecret
        {
            get
            {
                if (String.IsNullOrEmpty(appSecret))
                {
                    appSecret = ConfigurationManager.AppSettings["AppSecret"].ToString();
                }
                return appSecret;
            }
            set { appSecret = value; }
        }
        public string AppStart
        {
            get
            {
                if (String.IsNullOrEmpty(appStart))
                {
                    appStart = ConfigurationManager.AppSettings["AppStart"].ToString();
                }
                return appStart;
            }
        }
        
        public string GetExtension(string userName, string password, string customerId, string callerId)
        {
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) 
                && !String.IsNullOrEmpty(customerId) && !String.IsNullOrEmpty(callerId))
            {
                //TODO:
                //1. Telego call HHAExchange : callerId, appname,AppKey, AppSercret
                SearchAPIRestClient client = new SearchAPIRestClient();

                CallerIDLookupResponse restResult = client.GetCallerDataByCallerID(AppName, AppSecret, AppKey, callerId, Filter);
                //2. HHAExchange return a Coordinator1 Name
                if (restResult!=null && restResult.Result!=null)
                 { 
                     if (restResult.Result.Status.Equals(Status.Success))
                     { 
                        
                     }
                     else if (restResult.Result.Status.Equals(Status.Failure))
                     { 
                     
                     }
                 }
                //3. Query Extension Number of Coordinator in TeleGo DB
                //4. Return Extension to PBX
            }
            return string.Empty;
        }

         
    }
}
