using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using MinimalisticTelnet;

namespace remote_reboot
{
    //describes current network status and methods to interact with remote pc's
    class NetworkModel
    {
        private bool lanAMaster;//пока не используется, далее будет метод для определения активного лан
        private bool rpcmAOn;
        private bool rpcmBOn;
        private TelnetConnection telnetConnection;
        private string displayNow;//хранит содержимое коммандной строки общения с удаленным пк

        private const string rpcmALanAIP = "200.1.1.49";
        private const string rpcmALanBIP = "200.1.2.49";
        private const string rpcmBLanAIP = "200.1.1.177";
        private const string rpcmBLanBIP = "200.1.2.177";

        public NetworkModel()
        {
            lanAMaster = true;
            if (IsAlive(rpcmALanAIP) || IsAlive(rpcmALanBIP))
                rpcmAOn = true;
            else
                rpcmAOn = false;
            if (IsAlive(rpcmBLanAIP) || IsAlive(rpcmBLanBIP))
                rpcmBOn = true;
            else
                rpcmBOn = false;
        }

        private bool IsAlive(string IPAddress)
        {
            PingReply pingReply;
            Ping ping = new Ping();
            try
            {
                pingReply = ping.Send(System.Net.IPAddress.Parse(IPAddress));
            }
            catch (Exception ex)
            {
                //NetworkException excpetion = new NetworkException();
                //excpetion.InnerException = ex.GetBaseException();

                throw new NetworkException();
            }
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        public void RebootRemote(string ipAddress)
        {
            
        }
    }
}
