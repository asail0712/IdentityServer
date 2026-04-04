using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class CommonDefine
    {
        static public string SHA256_SALT = "ShowMeTheMoney";
    }

    static public class ProviderDefine
    {
        public const string Password    = "PASSWORD";
        public const string Line        = "LINE";
    }
}