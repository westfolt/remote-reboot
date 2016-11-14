using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MinimalisticTelnet;

namespace remote_reboot
{
    class ViewModel:INotifyPropertyChanged
    {
        private LoggerModel logger;
        private NetworkModel network;
        private bool stopNetworkModel;
        private string display;
        private SimpleCommand simpleCommand;

        public ViewModel()
        {
            logger = LoggerModel.GetLogger("log.txt");
            network = new NetworkModel();
            stopNetworkModel = false;
            simpleCommand = new SimpleCommand(RebootRemote);
            Task rpcmAChecking = new Task(RpcmACheck);//checks availability of rpcm-a pc in background process
            Task rpcmBChecking = new Task(RpcmBCheck);
            rpcmAChecking.Start();
            rpcmBChecking.Start();
        }


        #region properties

        public bool RpcmAOn
        {
            get { return network.RpcmAOn; }
            set
            {
                network.RpcmAOn = value;
                OnPropertyChanged("RpcmAOn");
            }
        }

        public bool RpcmBOn
        {
            get { return network.RpcmBOn; }
            set
            {
                network.RpcmBOn = value;
                OnPropertyChanged("RpcmBOn");
            }
        }

        public string Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged("Display");
            }
        }

        public SimpleCommand SimpleCommand
        {
            get { return simpleCommand; }
        }

        #endregion

        #region methods

        public void RpcmACheck()
        {
            while (!stopNetworkModel)
            {
                if (network.IsAlive(network.rpcmALanAIP) || network.IsAlive(network.rpcmALanBIP))
                    RpcmAOn = true;
                else
                    RpcmAOn = false;

                Thread.Sleep(1000);
            }
        }//send ping to rpcm-a and b

        public void RpcmBCheck()
        {
            while (!stopNetworkModel)
            {
                if (network.IsAlive(network.rpcmBLanAIP) || network.IsAlive(network.rpcmBLanBIP))
                    RpcmBOn = true;
                else
                    RpcmBOn = false;

                Thread.Sleep(1000);
            }
        }

        public void RebootRemote(string ipAddress)
        {
            switch (ipAddress)//implement other lan's
            {
                case "A":
                    if (RpcmAOn)
                    {
                        ipAddress = network.rpcmALanAIP;
                    }
                    break;
                case "B":
                    if (RpcmBOn)
                    {
                        ipAddress = network.rpcmBLanAIP;
                    }
                    break;
            }
            network.telnetConnection = new TelnetConnection(ipAddress, 23);
            string s = network.telnetConnection.Login("san4eg", "1988", 500);
            Display += s;
            string command = "sudo shutdown -h now";
            network.telnetConnection.WriteLine(command);
            s = network.telnetConnection.Read();
            Display += s;
            network.telnetConnection.WriteLine("1988");
            s = network.telnetConnection.Read();
            Display += s;
        }

        public void ExecuteSimpleCommand(string message)
        {
            MessageBox.Show(message);
        }

        #endregion
        
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
