using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Common.Extensions.Config
{
    public class EncryptionConfig
    {
        public string Key { get; set; } = string.Empty;
        public string IV { get; set; } = string.Empty;
    }
}
