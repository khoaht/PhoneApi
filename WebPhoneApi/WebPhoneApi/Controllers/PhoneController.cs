using Infrastructure;
using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using WebPhoneApi.HhaExchange;

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

        public MessageRespone GetExtension(string userName, string password, string customerId, string callerId)
        {
            MessageRespone respone = new MessageRespone();
            respone.Items = new List<LookupData>();
            respone.ErrorInfo = new Infrastructure.Domain.ErrorInfo();
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(customerId) && !String.IsNullOrEmpty(callerId))
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
                            LookupData data = new LookupData();
                            data.Branch = item.Branch;
                            data.City = item.City;
                            data.Coordinator1 = item.Coordinator1;
                            data.Coordinator2 = item.Coordinator2;
                            data.Coordinator3 = item.Coordinator3;
                            data.Location = item.Location;
                            data.Nurse=item.Nurse;
                            data.PrimaryContract = item.PrimaryContract;
                            data.PrimaryLanguage = item.PrimaryLanguage;
                            data.State = item.State;
                            data.Team = item.Team;
                            data.Type = item.Type;
                            data.Zip = item.Zip;
                            data.Services = new List<Service>();
                            foreach (var service in item.Services)
                            {
                                Service s = new Service()
                                {
                                    ServiceName = service
                                };
                                data.Services.Add(s);
                            } 
                            //add to list 
                            respone.Items.Add(data);
                        }
                    }
                    else if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Failure.ToString()))
                    {
                        respone.Status = Infrastructure.StatusCode.Failure; 
                        respone.ErrorInfo.MessageInfo = restResult.Result.ErrorInfo.ErrorMessage;
                    }
                } 
                //3. Query Extension Number of Coordinator in TeleGo DB 
            }
            else
            {
                respone.Status = Infrastructure.StatusCode.Failure; 
                respone.ErrorInfo.MessageInfo = "Not enough information";
            }
            //4. Return Extension to PBX
            return respone;
        }


    }
}
