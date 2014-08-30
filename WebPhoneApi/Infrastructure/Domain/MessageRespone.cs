using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Infrastructure.Domain
{
    public class MessageRespone
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

    //[XmlRoot("response")]
    //public class response
    //{
    //    [XmlElement("result")]
    //    public Response result { get; set; }
    //    [XmlElement("Status")]
    //    public string Status { get; set; }
    //    [XmlElement("Message")]
    //    public string Message { get; set; }
    //}
    [XmlRoot("response")]
    public class Response
    {
        [XmlElement("result")]
        public ResponseResult data { get; set; }
    }

    public class ResponseResult
    {
        [XmlElement("ivr_info")]
        public ivr_info ivr_info { get; set; }
    }
    [XmlRoot("ivr_info")]
    public class ivr_info
    {

        [XmlArray("variables")]
        [XmlArrayItem("variable")]
        public variable[] variables { get; set; }
    }

    [XmlRoot("variable")]
    public class variable
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("value")]
        public string value { get; set; }
    }
}
