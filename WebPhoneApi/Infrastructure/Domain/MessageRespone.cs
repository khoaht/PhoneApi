using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class MessageRespone
    {
        public string Status { get; set; }
        public ErrorInfo ErrorInfo { get; set; }
        public List<Extension> Extensions { get; set; }
    }

    public class ErrorInfo
    {
        public string MessageInfo{get;set;}
    }

    public class Extension {
        public string Data { get; set; }
    }
}
