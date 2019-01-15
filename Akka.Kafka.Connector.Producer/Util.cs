using System.Net.NetworkInformation;

namespace Akka.Kafka.Connector.Producer
{
    public static class Util
    {
        public static bool PingHost(string nameOrAddress, bool throwExceptionOnError = false)
        {
            bool pingable = false;
            using (Ping pinger = new Ping())
            {
                try
                {
                    PingReply reply = pinger.Send(nameOrAddress);
                    pingable = reply.Status == IPStatus.Success;
                }
                catch (PingException e)
                {
                    if (throwExceptionOnError) throw e;
                    pingable = false;
                }
            }
            return pingable;
        }

    }
}
