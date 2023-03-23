using System.Net.Sockets;
using System.Text;

namespace RCp_Phase1
{
    public partial class MainWindow : Form
    {
        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        string ipAddress = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        void DisableAll() => btnConnect.Enabled = btnRequest.Enabled = mtbIP.Enabled = rtbRequest.Enabled = false;

        void ReenableAll() 
        {
            btnConnect.Text = socket.Connected ? "Disconnect" : "Connect";
            lblIP.Text = $"Path to File (relative to {(string.IsNullOrEmpty(ipAddress) ? "<none>" : ipAddress)})";

            btnConnect.Enabled = true;
            mtbIP.Enabled = !socket.Connected;
            btnRequest.Enabled = rtbRequest.Enabled = socket.Connected;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            DisableAll();
            try
            {
                if (socket.Connected)
                {
                    ipAddress = string.Empty;
                    socket.Disconnect(false);
                    socket.Close(5000);
                    socket.Dispose();

                    socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                    rtbResults.Message("Socket successfully disconnected from host");
                }
                else
                {
                    string[] splits = mtbIP.Text.Replace(" ", string.Empty).Split('.');
                    
                    for (int i = 0; i < splits.Length; i++)
                        splits[i] = int.Parse(splits[i]) == 0 ? "0" : splits[i].Replace("0", string.Empty);

                    ipAddress = splits[0] + '.' + splits[1] + '.' + splits[2] + '.' + splits[3];

                    socket.Connect(ipAddress, 80);

                    rtbResults.Success("Socket successfully connected to host");
                }
            }
            catch (SocketException s)
            {
                rtbResults.Error("Error connecting: " + s.Message);
            }
            finally
            {
                ReenableAll();
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                rtbResults.AppendText($"Sending following request:\n{cmbReqMethod.SelectedItem} {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)} HTTP/1.1\nHost: {ipAddress}\nConnection: Keep-Alive\nAccept: */*\n");

                byte[] buf = Encoding.ASCII.GetBytes($"{cmbReqMethod.SelectedItem} {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)} HTTP/1.1\r\n" +
                                                     "Host: localhost\r\nConnection: Keep-Alive\r\nAccept: */*\r\n\r\n");

                // TODO - implement HTTP methods into TCP message

                socket.Send(buf);

                rtbResults.Success("Request sent, awaiting server response");

                buf = new byte[1024 * 1024];
                int bytesRead = socket.Receive(buf);

                rtbResults.Message($"Server answer received: total of {bytesRead} bytes read");

                switch (Encoding.ASCII.GetString(buf).GetStatusCode())
                {
                    case 200:
                        try
                        {
                            rtbResults.Success($"Successfully obtained {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)}; saving results in Desktop");
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\' + 
                                          ipAddress + (rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text).Replace('/', '_') + ".html";
                            File.WriteAllText(path, Encoding.ASCII.GetString(buf).GetResponse());
                            rtbResults.Success($"Successfully saved response on {path}");
                            
                        }
                        catch
                        {
                            rtbResults.Error("Could not write response to disk, please try again later");
                        }
                        break;
                    default:
                        rtbResults.Warn($"Not yet implemented: code {Encoding.ASCII.GetString(buf).GetStatusCode()}");
                        string cod = Encoding.ASCII.GetString(buf);
                        if (Encoding.ASCII.GetString(buf).GetStatusCode() == -1)
                            throw new Exception("server response is invalid (message contained no status code)");
                        break;
                }       
            }
            catch (Exception ex)
            {
                rtbResults.Error("Impossible to perform any operations with the host: " + ex.Message);

                socket.Disconnect(false);
                socket.Close(5000);
                socket.Dispose();

                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                ReenableAll();
            }
        }
    }
}