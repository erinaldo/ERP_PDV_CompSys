using System.Net.NetworkInformation;

namespace PDV.UTIL
{
    public static class ConexaoInternet
    {
        public static string DefaultHost { get; } = "google.com";
        public static bool ExisteConexao(string host = "google.com", int timeout = 1000)
        {
            try
            {
                var reply = new Ping().Send(host, timeout, new byte[32], new PingOptions());
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
            
        }

        public static bool ExisteConexao(int timeout)
        {
            return ExisteConexao(DefaultHost, timeout);
        }
    }
}
