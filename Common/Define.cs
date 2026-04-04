using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class ClaimLoginType
    {
        public const string AvatarUrl   = "AVATARURL";
        public const string Provider    = "PROVIDER";
    }

    static public class CommonDefine
    {
        public const string SHA256_SALT = "ShowMeTheMoney";
    }

    static public class ProviderDefine
    {
        public const string Password    = "PASSWORD";
        public const string Line        = "LINE";
    }
}