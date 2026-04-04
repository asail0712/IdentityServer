using AetherCore.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Settings
{
    [AppSettings]
    public class LineLoginSettings
    {
        public string ChannelId { get; set; } = string.Empty;
    }
}
