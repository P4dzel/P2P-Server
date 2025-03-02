using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace clientServer
{
    public partial class Form1 : Form
    {
        private const int Port = 5000; // Port UDP do komunikacji P2P
        private UdpClient udpClient;
        private Thread listenerThread;
        private Dictionary<string, DateTime> players = new Dictionary<string, DateTime>(); // Lista graczy
        private bool isRunning = false;
        private string myNick = "";
        private System.Windows.Forms.Timer updateTimer; // Timer do aktualizacji listy graczy
        private System.Windows.Forms.Timer ipCheckTimer;

        public Form1()
        {
            InitializeComponent();
            StartUpdateTimer();

            StartIPCheckTimer();
        }

        private void StartIPCheckTimer()
        {
            ipCheckTimer = new System.Windows.Forms.Timer();
            ipCheckTimer.Interval = 1000; // Co 1 sekunda
            ipCheckTimer.Tick += (sender, e) => UpdateLocalIP();
            ipCheckTimer.Start();
        }

        private void UpdateLocalIP()
        {
            string localIP = GetLocalIPAddress1();
            lblLocalIP.Text = $"Twój IP: {localIP}"; // Przyk³adowa etykieta do wyœwietlania IP
        }

        private string GetLocalIPAddress1()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // Filtrujemy tylko IPv4
                {
                    return ip.ToString();
                }
            }
            return "Brak po³¹czenia";
        }

        private void btnDolacz_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                myNick = txtNick.Text.Trim();
                if (string.IsNullOrEmpty(myNick))
                {
                    MessageBox.Show("Proszê wprowadziæ nick!");
                    return;
                }

                players[myNick] = DateTime.Now; // Dodanie siebie do listy graczy
                StartListener();
                SendMessageToKnownIPs("JOIN");
                lblStatus.Text = "Po³¹czono z sieci¹ P2P.";
                btnDolacz.Enabled = false;

                Thread.Sleep(500);
                SendMessageToKnownIPs("REQUEST_PLAYERS");
                Thread.Sleep(500);
                SendMessageToKnownIPs("PLAYER_LIST"); // Wysy³anie listy graczy po do³¹czeniu
                UpdatePlayerList(); // Aktualizacja listy po do³¹czeniu
            }
        }

        private void StartListener()
        {
            isRunning = true;
            udpClient = new UdpClient(Port);
            listenerThread = new Thread(ListenForMessages);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private void ListenForMessages()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, Port);
            while (isRunning)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    string[] parts = message.Split('|');

                    if (parts.Length >= 2)
                    {
                        string receivedNick = parts[0];
                        string action = parts[1];

                        if (action == "JOIN")
                        {
                            if (!players.ContainsKey(receivedNick))
                            {
                                players[receivedNick] = DateTime.Now;
                                SendMessageToKnownIPs("PLAYER_LIST"); // Wysy³anie aktualnej listy graczy do wszystkich
                            }
                        }
                        else if (action == "LEAVE")
                        {
                            if (players.ContainsKey(receivedNick))
                            {
                                players.Remove(receivedNick);
                            }
                        }
                        else if (action == "REQUEST_PLAYERS")
                        {
                            SendMessageToKnownIPs("PLAYER_LIST");
                        }
                        else if (action == "PLAYER_LIST" && parts.Length > 2)
                        {
                            for (int i = 2; i < parts.Length; i++)
                            {
                                string playerNick = parts[i];
                                if (!players.ContainsKey(playerNick))
                                {
                                    players[playerNick] = DateTime.Now;
                                }
                            }
                        }

                        UpdatePlayerList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("B³¹d nas³uchiwania: " + ex.Message);
                }
            }
        }

        private void SendMessageToKnownIPs(string action)
        {
            List<string> knownIPs = GetLocalNetworkIPs();
            foreach (var ip in knownIPs)
            {
                using (UdpClient senderUdp = new UdpClient())
                {
                    string message = $"{myNick}|{action}";
                    if (action == "PLAYER_LIST")
                    {
                        foreach (var player in players.Keys)
                        {
                            message += "|" + player;
                        }
                    }
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), Port);
                    senderUdp.Send(data, data.Length, endpoint);
                }
            }
        }

        private List<string> GetLocalNetworkIPs()
        {
            List<string> ipList = new List<string>();
            string myIP = GetLocalIPAddress();
            string baseIP = myIP.Substring(0, myIP.LastIndexOf(".") + 1);

            for (int i = 1; i < 255; i++)
            {
                string testIP = baseIP + i.ToString();
                if (testIP != myIP)
                {
                    ipList.Add(testIP);
                }
            }

            return ipList;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Brak po³¹czenia z sieci¹!");
        }

        private void StartUpdateTimer()
        {
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += (sender, e) => UpdatePlayerList();
            updateTimer.Start();
        }

        private void UpdatePlayerList()
        {
            if (txtGracze.InvokeRequired)
            {
                txtGracze.Invoke(new MethodInvoker(UpdatePlayerList));
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var player in players)
                {
                    TimeSpan onlineTime = DateTime.Now - player.Value;
                    sb.AppendLine($"{player.Key} (online: {onlineTime.Minutes}:{onlineTime.Seconds:D2})");
                }
                txtGracze.Text = sb.ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            SendMessageToKnownIPs("LEAVE");
            udpClient?.Close();
        }
    }
}
