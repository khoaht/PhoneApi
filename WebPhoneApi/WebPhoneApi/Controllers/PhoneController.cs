using Infrastructure;
using Infrastructure.Domain;
using Infrastructure.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        private string urlRest = string.Empty;

        public string UrlRest
        {
            get
            {
                if (String.IsNullOrEmpty(urlRest))
                {
                    urlRest = ConfigurationManager.AppSettings["URLRest"].ToString();
                }
                return urlRest;
            }
            set { urlRest = value; }
        }
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

        private CallerIDLookup ParseXMlToCallerIDLookup(string xml)
        {
            CallerIDLookup result = new CallerIDLookup();
            result.LookupData = new LookupData();
            if (!String.IsNullOrEmpty(xml))
            {
                XDocument doc = XDocument.Parse(xml);
                if (doc != null && doc.Root != null)
                {
                    string nsp = doc.Root.GetDefaultNamespace() != null ? doc.Root.GetDefaultNamespace().NamespaceName : string.Empty;
                    XElement root = doc.Root;
                    //parse Result
                    XElement eleResult = root.Element(XName.Get("Result", nsp));

                    if (eleResult != null)
                    {
                        XElement eleErrorInfo = eleResult.Element(XName.Get("ErrorInfo", nsp));
                        result.Result = new Result();
                        if (eleErrorInfo != null)
                        {
                            result.Result.ErrorInfo = new ErrorInfo();
                            string errorId = eleErrorInfo.Element(XName.Get("ErrorID", nsp)) != null ? eleErrorInfo.Element(XName.Get("ErrorID", nsp)).Value : string.Empty;
                            string errorMessage = eleErrorInfo.Element(XName.Get("ErrorMessage", nsp)) != null ? eleErrorInfo.Element(XName.Get("ErrorMessage", nsp)).Value : string.Empty;
                            result.Result.ErrorInfo.ErrorID = errorId;
                            result.Result.ErrorInfo.ErrorMessage = errorMessage;
                        }
                        //parse Status
                        XElement status = eleResult.Element(XName.Get("Status", nsp));
                        if (status != null)
                        {
                            result.Result.Status = status.Value;
                        }
                        //LookupData
                        XElement lookupData = root.Element(XName.Get("LookupData", nsp));
                        if (lookupData != null)
                        {
                            XElement item = lookupData.Element(XName.Get("Item", nsp));
                            if (item != null)
                            {
                                XElement type = item.Element(XName.Get("Type", nsp));
                                result.LookupData.Type = type != null ? type.Value : string.Empty;
                                XElement coordinator1 = item.Element(XName.Get("Coordinator1", nsp));
                                result.LookupData.Coordinator1 = coordinator1 != null ? coordinator1.Value : string.Empty;
                                XElement Coordinator2 = item.Element(XName.Get("Coordinator2", nsp));
                                result.LookupData.Coordinator2 = Coordinator2 != null ? Coordinator2.Value : string.Empty;
                                XElement Coordinator3 = item.Element(XName.Get("Coordinator3", nsp));
                                result.LookupData.Coordinator3 = Coordinator3 != null ? Coordinator3.Value : string.Empty;
                                XElement PrimaryContract = item.Element(XName.Get("PrimaryContract", nsp));
                                result.LookupData.PrimaryContract = PrimaryContract != null ? PrimaryContract.Value : string.Empty;
                                XElement Branch = item.Element(XName.Get("Branch", nsp));
                                result.LookupData.Branch = Branch != null ? Branch.Value : string.Empty;
                                XElement Team = item.Element(XName.Get("Team", nsp));
                                result.LookupData.Team = Team != null ? Team.Value : string.Empty;
                                XElement Location = item.Element(XName.Get("Location", nsp));
                                result.LookupData.Location = Location != null ? Location.Value : string.Empty;
                                XElement PrimaryLanguage = item.Element(XName.Get("PrimaryLanguage", nsp));
                                result.LookupData.PrimaryLanguage = PrimaryLanguage != null ? PrimaryLanguage.Value : string.Empty;
                                XElement Zip = item.Element(XName.Get("Zip", nsp));
                                result.LookupData.Zip = Zip != null ? Zip.Value : string.Empty;
                                XElement State = item.Element(XName.Get("State", nsp));
                                result.LookupData.State = State != null ? State.Value : string.Empty;
                                XElement City = item.Element(XName.Get("City", nsp));
                                result.LookupData.City = City != null ? City.Value : string.Empty;

                            }
                        }
                    }
                }
            }

            return result;
        }
        private string ParseObjectToXml(response res)
        {
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");
            XmlSerializer xsSubmit = new XmlSerializer(typeof(response));

            XmlDocument doc = new XmlDocument();

            System.IO.StringWriter sww = new System.IO.StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, res,ns);
            var xml = sww.ToString(); // Your xml

            return xml;
        }
        public string GetExtension(string userName, string password, int customerId, string callerId)
        {
            response res = new response();
            res.result = new result()
            {
                ivr_info = new ivr_info()
                {
                    variables = new variable[1]
                }
            };
            //check login 
            bool isLogined = userService.ValidateUser(userName, password);
            if (!isLogined)
            {
                res.Status = Infrastructure.StatusCode.Failure;
                res.Message = "not authorized";
                return ParseObjectToXml(res);
            }
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) && customerId > 0 && !String.IsNullOrEmpty(callerId))
            {
                //TODO:
                //1. Telego call HHAExchange : callerId, appname,AppKey, AppSercret 
                var getUrl = UrlRest + AppName + "/" + AppSecret + "/" + AppKey + "/" + callerId + "/" + Filter;
                WebClient client = new WebClient();
                string s = client.DownloadString(getUrl);

                CallerIDLookup restResult = ParseXMlToCallerIDLookup(s);

                //2. HHAExchange return a Coordinator1 Name
                if (restResult != null && restResult.Result != null)
                {
                    if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Success))
                    {
                        res.Status = Infrastructure.StatusCode.Success;
                        if (restResult.LookupData != null)
                            if (!String.IsNullOrEmpty(restResult.LookupData.Coordinator1))
                            {
                                // get extension and coordinator
                                //3. Query Extension Number of Coordinator in TeleGo DB 
                                string strvalue = customerCoordinateService.GetExtension(customerId, restResult.LookupData.Coordinator1);
                                variable varextension = new variable() { name = "extension", value = strvalue };
                                res.result.ivr_info.variables[0] = varextension;
                                return ParseObjectToXml(res);
                            }

                    }
                    else if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Failure.ToString()))
                    {
                        res.Status = Infrastructure.StatusCode.Failure;
                        res.Message = restResult.Result.ErrorInfo.ErrorMessage;
                    }
                }

            }
            else
            {
                res.Status = Infrastructure.StatusCode.Failure;
                res.Message = "Not enough information";
            }
            //4. Return Extension to PBX
            return ParseObjectToXml(res);
        }

        /// <summary>
        /// Get Extension
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="customerId"></param>
        /// <param name="callerId"></param>
        /// <returns></returns>
        #region Comment
        //public MessageRespone GetExtension(string userName, string password, int customerId, string callerId)
        //{
        //    MessageRespone respone = new MessageRespone();

        //    //check login 
        //    bool isLogined = userService.ValidateUser(userName, password);
        //    if (!isLogined)
        //    {
        //        respone.Status = Infrastructure.StatusCode.Failure;
        //        respone.Message = "not authorized";
        //        return respone;
        //    }
        //    if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) && customerId > 0 && !String.IsNullOrEmpty(callerId))
        //    {
        //        //TODO:
        //        //1. Telego call HHAExchange : callerId, appname,AppKey, AppSercret 
        //        var getUrl = UrlRest + AppName + "/" + AppSecret + "/" + AppKey + "/" + callerId + "/" + Filter;
        //        WebClient client = new WebClient();
        //        string s = client.DownloadString(getUrl);

        //        CallerIDLookup restResult = ParseXMlToCallerIDLookup(s);

        //        //WebPhoneApi.HhaExchange2.SearchAPISoapClient clientSoap = new WebPhoneApi.HhaExchange2.SearchAPISoapClient();
        //        //WebPhoneApi.HhaExchange2.CallerIDLookupResponse restResult = clientSoap.GetCallerDataByCallerID(AppName, AppSecret, AppKey, callerId, Filter);


        //        //2. HHAExchange return a Coordinator1 Name
        //        if (restResult != null && restResult.Result != null)
        //        {
        //            if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Success))
        //            {
        //                respone.Status = Infrastructure.StatusCode.Success;
        //                if(restResult.LookupData!=null )
        //                    if (!String.IsNullOrEmpty(restResult.LookupData.Coordinator1))
        //                {
        //                    // get extension and coordinator
        //                    //3. Query Extension Number of Coordinator in TeleGo DB 
        //                    respone.Data = customerCoordinateService.GetExtension(customerId, restResult.LookupData.Coordinator1);

        //                }

        //            }
        //            else if (restResult.Result.Status.Equals(Infrastructure.StatusCode.Failure.ToString()))
        //            {
        //                respone.Status = Infrastructure.StatusCode.Failure;
        //                respone.Message = restResult.Result.ErrorInfo.ErrorMessage;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        respone.Status = Infrastructure.StatusCode.Failure;
        //        respone.Message = "Not enough information";
        //    }
        //    //4. Return Extension to PBX
        //    return respone;
        //}
        #endregion

    }
}
