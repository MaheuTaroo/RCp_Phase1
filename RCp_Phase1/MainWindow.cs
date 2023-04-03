using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RCp_Phase1
{
    /* Auto-generated class, containing some definitions for the graphical
     * window and the graphical controls contained within it. */
    public partial class MainWindow : Form
    {
        // TCP client used to connect and transmit information to a host.
        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        /* String associated to the provided IP address, used on the
         * label positioned above the file request textbox. */
        string ipAddress = string.Empty;

        /* Flag associated to the selected request method, active when
         * the user chooses the POST or PUT requests. */
        bool sendingFiles = false;
        public MainWindow()
        {
            /* Initialize and draw all window components;
             * auto-generated code on project creation. */
            InitializeComponent();

            /* Forcing the HTTP request method combobox's
             * default value to the GET method. */
            cmbReqMethod.SelectedIndex = 0;
        }

        void DisableAll()
        {
            /* Deactivates all textboxes, buttons and
             * combobox used to connect to a host and 
             * transmit information. */
            btnConnect.Enabled = btnRequest.Enabled = mtbIP.Enabled = rtbRequest.Enabled = cmbReqMethod.Enabled = false;
        }

        void ReenableAll()
        {
            /* Changes the connection button and the
             * "Path to File" label's text depending
             * on the socket connection status. */
            btnConnect.Text = socket.Connected ? "Disconnect" : "Connect";
            lblIP.Text = $"Path to File (relative to {(string.IsNullOrEmpty(ipAddress) ? "<none>" : ipAddress)})";

            /* Selectively enables the associated textboxes,
             * buttons and combobox depending on the socket
             * connection status. */
            btnConnect.Enabled = true;
            mtbIP.Enabled = !socket.Connected;
            btnRequest.Enabled = rtbRequest.Enabled = cmbReqMethod.Enabled = socket.Connected;

            /* Forcing the HTTP request method combobox's
             * default value to the GET method. */
            cmbReqMethod.SelectedIndex = 0;
        }

        /* Method called every time the user wants to connect to
         * a host through the previously constructed socket. */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            /* Disables all relevant controls to prevent user interaction
             * that may result in unwanted behavior, like supplemental
             * unnecessary connections. */
            DisableAll();

            /* Tries to execute the code instructions contained in this
             * block; any socket connection error is processed by the 
             * catch block. */
            try
            {
                // Checks if the socket is connected to a host.
                if (socket.Connected)
                {
                    /* Disconnects the socket from the connection
                     * without reusability, closes it with a 5
                     * second grace period to send any extra
                     * packets, and disposes of it. */
                    ipAddress = string.Empty;
                    socket.Disconnect(false);
                    socket.Close(5000);
                    socket.Dispose();

                    /* Creates a fresh new copy of a TCP socket
                     * for future connections, and notifies the
                     * user of a successful disconnection. */
                    socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    rtbResults.Message("Socket successfully disconnected from host");
                }
                /* Alternatively, this code block is executed if the
                 * connection flag is not active. */
                else
                {
                    /* Formats the user-given IP address, obtaining
                     * a "beautified" version; this is done by the
                     * following manner: 
                     * - The IP address gets its spaces eliminated
                     * (any unfilled spaces in the textbox are filled
                     * with an empty string), and is separated into
                     * an array with its individual fields for further
                     * processing
                     * - If a certain field is empty, it is replaced
                     * with "0"; else, the field is converted to an
                     * integer and saved back to the string array
                     * containing the address fields
                     * - The global string associated to the given IP
                     * address is appended with every field, separated
                     * with the character '.' */
                    string[] splits = mtbIP.Text.Replace(" ", string.Empty).Split('.');
                    for (int i = 0; i < splits.Length; i++)
                        splits[i] = splits[i] == string.Empty ? "0" : int.Parse(splits[i]).ToString();
                    ipAddress = splits[0] + '.' + splits[1] + '.' + splits[2] + '.' + splits[3];

                    /* Connects to the "beautified" IP address on port
                     * 80, and informs the user that the connection was
                     * successfully established. */
                    socket.Connect(ipAddress, 80);
                    rtbResults.Success("Socket successfully connected to host");
                }
            }
            catch (SocketException s)
            {
                /* Processes any connection-specific error obtained
                 * during the try block above. */
                rtbResults.Error("Error connecting to the given IP address: " + s.Message);

                // Resets the IP address string to an empty one.
                ipAddress = string.Empty;
            }

            /* Reenables all relevant controls to allow new
             * user interactions. */
            ReenableAll();
        }

        /* Method called every time the user wants to retrieve or
         * send a file through the previously constructed socket. */
        private void btnRequest_Click(object sender, EventArgs e)
        {

            /* Disables all relevant controls to prevent user interaction
             * that may result in unwanted behavior, like supplemental
             * unnecessary connections. */
            DisableAll();

            /* Tries to execute the code instructions contained in this
             * block; any socket connection error is processed by the
             * catch block. */
            try
            {
                // TODO - comment this code section and implement HTTP methods into TCP message
                rtbResults.AppendText($"Sending following request:\n{cmbReqMethod.SelectedItem} {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)} HTTP/1.1\nHost: {ipAddress}\nConnection: Keep-Alive\nAccept: */*\n");

                byte[] buf = Encoding.ASCII.GetBytes($"{cmbReqMethod.SelectedItem} {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)} HTTP/1.1\r\n" +
                                                     "Host: localhost\r\nConnection: Keep-Alive\r\nAccept: */*\r\n\r\n");

                socket.Send(buf);

                rtbResults.Success("Request sent, awaiting server response");

                buf = new byte[1024 * 1024];
                int bytesRead = socket.Receive(buf);

                rtbResults.Message($"Server answer received: total of {bytesRead} bytes read");

                switch (buf.GetStatusCode())
                {
                    case 200:
                        try
                        {
                            rtbResults.Success($"200: Successfully obtained {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)}; saving results in Desktop");
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\' +
                                          ipAddress + (rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text).Replace('/', '_') + ".html";
                            File.WriteAllText(path, buf.GetResponse());
                            rtbResults.Success($"Successfully saved response on {path}");

                        }
                        catch
                        {
                            rtbResults.Error("Could not write response to disk, please try again later");
                        }
                        break;

                    case 201:
                        rtbResults.Success($"201: Successfully created the provided resource at {ipAddress + (rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)}");
                        break;

                    case 202:
                        rtbResults.Warn("202: The request has been accepted, and is being processed by an exterior server or by a batch processor. Try again later.");
                        break;

                    case 203:
                        try
                        {
                            rtbResults.Success($"203: Successfully obtained {(rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text)} (from external host); saving results in Desktop");
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\' +
                                          ipAddress + (rtbRequest.Text.StartsWith('/') ? rtbRequest.Text : '/' + rtbRequest.Text).Replace('/', '_') + ".html";
                            File.WriteAllText(path, JsonDocument.Parse(buf.GetResponse()).RootElement.GetProperty("response").GetRawText());
                            rtbResults.Success($"Successfully saved response on {path}");

                        }
                        catch
                        {
                            rtbResults.Error("Could not write response to disk, please try again later");
                        }
                        break;

                    case 204:
                        rtbResults.Success("204: The server received the request; no response was provided");
                        break;

                    default:
                        rtbResults.Warn($"Not yet implemented: code {buf.GetStatusCode()}");
                        string cod = Encoding.ASCII.GetString(buf);
                        if (buf.GetStatusCode() == -1)
                            throw new Exception("server response is invalid (message contained no status code)");
                        break;
                }
            }
            catch (Exception ex)
            {
                /* Processes any connection-specific error obtained
                 * during the try block above. */
                rtbResults.Error("Impossible to perform any operations with the host: " + ex.Message);

                /* Disconnects the socket from the connection
                    * without reusability, closes it with a 5
                    * second grace period to send any extra
                    * packets, disposes of it and creates a
                    * fresh new copy for future connections. */
                socket.Disconnect(false);
                socket.Close(5000);
                socket.Dispose();
                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                // Resets the IP address string to an empty one.
                ipAddress = string.Empty;
            }

            /* Reenables all relevant controls to allow new
             * user interactions. */
            ReenableAll();
        }

        /* Method called every time the HTTP request method
         * combobox changes selection; when it is executed,
         * it checks whether the selected item's index within
         * the combobox's data source is the same as either the
         * POST or PUT methods' selection index within the
         * combobox. */
        private void cmbReqMethod_SelectedIndexChanged(object sender, EventArgs e) =>
            sendingFiles = cmbReqMethod.SelectedIndex == 2 || cmbReqMethod.SelectedIndex == 3;
    }
}