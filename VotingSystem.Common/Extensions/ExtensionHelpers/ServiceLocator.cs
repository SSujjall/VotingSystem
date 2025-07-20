using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Common.Extensions.ExtensionHelpers
{
    public static class ServiceLocator
    {
        public static IServiceProvider? Instance { get; private set; }

        public static void Init(IServiceProvider serviceProvider)
        {
            Instance = serviceProvider;
        }
    }
}
