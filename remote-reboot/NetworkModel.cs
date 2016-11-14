using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MinimalisticTelnet;

namespace remote_reboot
{
    //describes current network status and methods to interact with remote pc's
    class NetworkModel
    {
        public string rpcmALanAIP = "192.168.0.68";//"200.1.1.49";
        public string rpcmALanBIP = "200.1.2.49";
        public string rpcmBLanAIP = "192.168.0.99";//"200.1.1.177";
        public string rpcmBLanBIP = "200.1.2.177";
        private bool lanAMaster;//пока не используется, далее будет метод для определения активного лан
        //private bool lanBMaster;

        public bool RpcmAOn { get; set; }
        public bool RpcmBOn { get; set; }
        public TelnetConnection telnetConnection { get; set; }
        private string displayNow;//хранит содержимое коммандной строки общения с удаленным пк
        
        public NetworkModel()
        {
            lanAMaster = true;//сменить когда будет код определения лан
        }

        public bool IsAlive(string ipAddress)
        {
            PingReply pingReply;
            Ping ping = new Ping();
            try
            {
                pingReply = ping.Send(System.Net.IPAddress.Parse(ipAddress));
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
        
    }
}
