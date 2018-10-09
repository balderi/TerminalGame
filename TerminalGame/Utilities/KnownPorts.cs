using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.Utilities
{
    /// <summary>
    /// List of known (named) ports.
    /// </summary>
    public enum KnownPorts
    {
        FTP = 21,
        SSH = 22,
        Telnet = 23,
        SMTP = 25,
        DNS = 53,
        DHCP = 67,
        TFTP = 69,
        HTTP = 80,
        POP = 110,
        NTP = 123,
        IMAP = 143,
        HTTPS = 443
    };
}
