using Infrastructure;
using Infrastructure.Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml.Linq;

namespace TeleGoApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PhoneController : ApiController
    {
        private string appStart = string.Empty;
        private string appKey = string.Empty;
        private string appSecret = string.Empty;
        private string appName = string.Empty;
        private string filter = string.Empty;
        private readonly IUserService userService;
        private readonly ICustomerCoordinateService customerCoordinateService;

        public PhoneController(IUserService userService, ICustomerCoordinateService customerCoordinateService)
        {
            this.customerCoordinateService = customerCoordinateService;
            this.userService = userService;
        }


        public string Filter
        {
            get
            {
                if (String.IsNullOrEmpty(filter))
                {
                    filter = ConfigurationManager.AppSettings["Filter"].ToString();
                }
                return filter;
            }
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

        /// <summary>
        /// Get Extension
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="customerId"></param>
        /// <param name="callerId"></param>
        /// <returns></returns>
        public MessageRespone GetExtension(string userName, string password, int customerId, string callerId)
        {
            MessageRespone respone = new MessageRespone();

            //check login 
            bool isLogined = userService.ValidateUser(userName, password);
            if (!isLogined)
            {
                respone.Status = Infrastructure.StatusCode.Failure;
                respone.Message = "not authorized";
                return respone;
            }
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) && customerId > 0 && !String.IsNullOrEmpty(callerId))
            {
                //TODO:
                //1. Telego call HHAExchange : callerId, appname,AppKey, AppSercret
                WebPhoneApi.HhaExchange2.SearchAPISoapClient clientSoap = new WebPhoneApi.HhaExchange2.SearchAPISoapClient();
                WebPhoneApi.HhaExchange2.CallerIDLookupResponse restResult = clientSoap.GetCallerDataByCallerID(AppName, AppSecret, AppKey, callerId, Filter);

                //2. HHAExchange return a Coordinator1 Name
                if (restResult != null && restResult.Result != null)
                {
                    if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Success))
                    {
                        respone.Status = Infrastructure.StatusCode.Success;
                        foreach (var item in restResult.LookupData)
                        {
                            if (!String.IsNullOrEmpty(item.Coordinator1))
                            {
                                // get extension and coordinator
                                //3. Query Extension Number of Coordinator in TeleGo DB 
                                respone.Data = customerCoordinateService.GetExtension(customerId, item.Coordinator1);
                                
                            }
                        }
                    }
                    else if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Failure.ToString()))
                    {
                        respone.Status = Infrastructure.StatusCode.Failure;
                        respone.Message = restResult.Result.ErrorInfo.ErrorMessage;
                    }
                }

            }
            else
            {
                respone.Status = Infrastructure.StatusCode.Failure;
                respone.Message = "Not enough information";
            }
            //4. Return Extension to PBX
            return respone;
        }


    }
}
