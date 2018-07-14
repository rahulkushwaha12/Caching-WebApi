using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CachingKeyValue.Entities
{
    public class Input
    {
        public string key { get; set; }
        public string value { get; set; }
        public long? expiryTime { get; set; }
    }
}