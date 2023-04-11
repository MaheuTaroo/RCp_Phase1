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
        string ipAddress = string.Empty,

        /* Strint that stores the file to request; it may be updated
         * every time the request file location textbox's text changes,
         * or when the request contains a 3xx status code (so the new
         * location's file is fetched). */
               reqFile = string.Empty;

        /* Flag created to suppress excessive messages in the results
         * textbox. */
        bool retry = false;

        public MainWindow()
        {
            /* Initialize and draw all window components;
             * auto-generated code on project creation. */
            InitializeComponent();

            /* Sets the window icon to the icon stored in
             * the application resources class. */
            Icon = Properties.Resources.icon;
        }

        void DisableAll()
        {
            /* Deactivates all textboxes, buttons and
             * combobox used to connect to a host and 
             * transmit information. */
            btnConnect.Enabled = btnRequest.Enabled = mtbIP.Enabled = rtbRequest.Enabled = false;
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
            btnRequest.Enabled = rtbRequest.Enabled = socket.Connected;
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
                    rtbResults.Message("Socket successfully connected to host");
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
                // TODO - comment this code section and finish file submission

                /* Prepares the request to be sent encoded using the ASCII
                 * code table. */
                string request = $"GET {(reqFile.StartsWith('/') ? reqFile : '/' + reqFile)} HTTP/1.1\r\n" +
                                  "Host: localhost\r\nConnection: Keep-Alive\r\nAccept: text/html;q=0.9,text/json;q=0.8,*/*;q=0.7\r\nAccept-Language: *\r\n\r\n";

                if (!retry) rtbResults.AppendText("Sending request...\n");

                /* Sends the buffered message through the socket
                 * to the host it is connected to, and presents a
                 * success message announcing so. */
                socket.Send(Encoding.ASCII.GetBytes(request));
                if (!retry) rtbResults.Success("Request sent, awaiting server response");

                /* Creates a new and clean buffer, waits for a
                 * response from the server, stores the amount
                 * of bytes read from it and informs the user 
                 * of that amount, as well as the answer yielded
                 * from the server. */
                byte[] buf = new byte[1024 * 1024];
                int bytesRead = socket.Receive(buf);
                rtbResults.Message($"Server answer received: total of {bytesRead} bytes read");
                switch (buf.GetStatusCode())
                {
                    /* The 2xx status codes represent a successful response
                     * from the server, with the file's content appended to
                     * its end. Depending of the selected HTTP method, the
                     * client may store the provided response payload on the
                     * user's desktop folder. */

                    case 200:
                        try
                        {
                            /* Announces a successful response from the server,
                             * and announces the attempt to write the response 
                             * to disk. */
                            rtbResults.Success($"200: Successfully obtained {(reqFile.StartsWith('/') ? reqFile : '/' + reqFile)}; saving results in Desktop...");

                            /* Obtains the path to the user's desktop, writes the
                             * entire response from the server to an HTML or JSON 
                             * file in said path, and announces the success of the
                             * operation. */
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\' +
                                      ipAddress + (reqFile.StartsWith('/') ? reqFile : '/' + reqFile).Replace('/', '_');

                            path += buf.GetHeader("Content-Type").Contains("text/html") ? ".html" : ".json";

                            File.WriteAllText(path, buf.GetResponse());
                            rtbResults.Success($"Successfully saved response on {path}.");

                        }
                        catch (Exception ex)
                        {
                            /* Announces the failure to save the server's response to
                             * the disk, paired with the message associated to the
                             * error. */
                            rtbResults.Error($"Could not write response to disk: {ex.Message}\nPlease try again later.");
                        }
                        break;

                    case 201:
                        rtbResults.Success($"201: Successfully created the provided resource at {ipAddress + (reqFile.StartsWith('/') ? reqFile : '/' + reqFile)}");
                        break;

                    case 202:
                        rtbResults.Warn("202: The request has been accepted, and is being processed by an external server or by a batch processor. Try again later.");
                        break;

                    case 203:
                        try
                        {
                            /* Announces a successful response from an external
                             * server, and announces the attempt to write the
                             * response to disk. */
                            rtbResults.Success($"203: Successfully obtained {(reqFile.StartsWith('/') ? reqFile : '/' + reqFile)} (from external host); saving results in Desktop...");

                            /* Obtains the path to the user's desktop, writes the
                             * entire response from the server to an HTML or JSON 
                             * file in said path, and announces the success of the
                             * operation. */
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '\\' +
                                          ipAddress + (reqFile.StartsWith('/') ? reqFile : '/' + reqFile).Replace('/', '_');

                            path += buf.GetHeader("Content-Type").Contains("text/html") ? ".html" : ".json";

                            File.WriteAllText(path, buf.GetResponse());
                            rtbResults.Success($"Successfully saved response on {path}");
                        }
                        catch (Exception ex)
                        {
                            /* Announces the failure to save the server's response to
                             * the disk, paired with the message associated to the
                             * error. */
                            rtbResults.Error($"Could not write response to disk: {ex.Message}\nPlease try again later");
                        }
                        break;

                    case 204:
                        rtbResults.Success("204: The server received the request, but no payload was provided.");
                        break;


                    /* The 3xx status codes represent a redirection that
                     * needs to be handled by the client. Each of the codes
                     * are handled by announcing their meaning as a warning,
                     * followed by a request to the new location. In the
                     * case that the server returned the codes 301, 302 or
                     * 303, this client changes the HTTP method to acomodate
                     * the status code; if the server returned status code
                     * 303, the previous HTTP method is restored after the
                     * file is fetched from the new location. 
                     * In the cases specified above, the "Location" header 
                     * result needs to be filtered; this is due to the fact
                     * that said result comes with the format
                     * "http://<host>/<intended page>". Since the intended
                     * format is "/<intended page>", this filtering has to
                     * be executed.
                     */

                    case 301:
                        rtbResults.Warn("301: The requested file was permanently moved to a new location. A new request will be issued shortly.");
                        reqFile = buf.GetHeader("Location");

                        if (reqFile.StartsWith("http://"))
                        {
                            string temp = reqFile.Replace("http://", string.Empty);
                            reqFile = temp.Substring(temp.IndexOf("/"));
                        }

                        socket = socket.CheckConnection(ipAddress);
                        btnRequest_Click(sender, e);
                        break;

                    case 302:
                        rtbResults.Warn("302: The requested file was temporarily moved to a new location. A new request will be issued shortly.");
                        reqFile = buf.GetHeader("Location");

                        if (reqFile.StartsWith("http://"))
                        {
                            string temp = reqFile.Replace("http://", string.Empty);
                            reqFile = temp.Substring(temp.IndexOf("/"));
                        }

                        socket = socket.CheckConnection(ipAddress);
                        btnRequest_Click(sender, e);
                        break;

                    case 303:
                        rtbResults.Warn("303: The server recommended an alternate location for this operation. A new request will be issued shortly.");
                        reqFile = buf.GetHeader("Location");

                        if (reqFile.StartsWith("http://"))
                        {
                            string temp = reqFile.Replace("http://", string.Empty);
                            reqFile = temp.Substring(temp.IndexOf("/"));
                        }

                        socket = socket.CheckConnection(ipAddress);
                        btnRequest_Click(sender, e);
                        break;

                    case 304:
                        rtbResults.Warn("304: The current request yielded a response that contains an unaltered version of an older, similar request. No further action has been made.");
                        break;

                    case 305:
                        rtbResults.Warn("305: The request can only be answered by the server if a proxy server is used to make a connection. Connect to a proxy server and try again later.");
                        break;

                    case 307:
                        rtbResults.Warn("307: The server has sent a temporary redirection to a new location. A new request will be issued shortly.");
                        reqFile = buf.GetHeader("Location");

                        if (reqFile.StartsWith("http://"))
                        {
                            string temp = reqFile.Replace("http://", string.Empty);
                            reqFile = temp.Substring(temp.IndexOf("/"));
                        }

                        socket = socket.CheckConnection(ipAddress);
                        btnRequest_Click(sender, e);
                        break;

                    case 308:
                        rtbResults.Warn("308: The server has sent a permanent redirection to a new location. A new request will be issued shortly.");
                        reqFile = buf.GetHeader("Location");

                        if (reqFile.StartsWith("http://"))
                        {
                            string temp = reqFile.Replace("http://", string.Empty);
                            reqFile = temp.Substring(temp.IndexOf("/"));
                        }

                        socket = socket.CheckConnection(ipAddress);
                        btnRequest_Click(sender, e);
                        break;

                    /* The 4xx status codes represent an error made by
                     * the client at request time; these are handled
                     * by announcing their specific meaning as an error. */

                    case 400:
                        rtbResults.Error("400: The server could not understand the request. Please retry your request with different parameters.");
                        break;

                    case 401:
                        rtbResults.Error("401: The server requests credentials to make a successful request.");
                        break;

                    case 402:
                        rtbResults.Error("402: The server requests a successful transaction to fulfill this request.");
                        break;

                    case 403:
                        rtbResults.Error("403: The requested file cannot be retrieved, as its access is forbidden.");
                        break;

                    case 404:
                        rtbResults.Error("404: The requested file does not exist. This may be because the path is misspelled or the file is non-existent.");
                        break;

                    case 405:
                        rtbResults.Error($"405: The server does not allow access to this file under the \"GET\" method.");
                        break;

                    case 406:
                        rtbResults.Error("406: The requested file did not comply with the \"Accept\" or \"Accept-Language\" header specifications.");
                        break;

                    case 407:
                        rtbResults.Error($"407: The proxy autentication credentials are wrong or non-existent.\nCredentials required: \"{buf.GetHeader("Proxy-Authenticate")}\"");
                        break;

                    case 408:
                        rtbResults.Error("408: The request reached the server outside its acceptable time period. The connection has been terminated by the host.");
                        break;

                    case 410:
                        rtbResults.Error("410: The requested file is permanently unavailable.");
                        break;

                    case 413:
                        rtbResults.Error("413: The contents of the request are too long for the server to process.");
                        break;

                    case 414:
                        rtbResults.Error("414: The requested file's path is too long.");
                        break;

                    case 415:
                        rtbResults.Error("415: The contents of the request are not in a media type accepted by the server.");
                        break;

                    case 418:
                        rtbResults.Error("418: The server is a teapot! It cannot brew coffee!");
                        break;

                    case 451:
                        rtbResults.Error("451: The requested file is not available under your location's legislation.");
                        break;

                    /* The 5xx status codes represent an error during
                     * the processing of a valid request; when these
                     * occur, the server announces what caused the
                     * incomplete processing of the request. These
                     * errors are handled by announcing their
                     * specific meanings as an error. */

                    case 500:
                        rtbResults.Error("500: The server ran into an internal error and the request could not be completed.");
                        break;

                    case 501:
                        rtbResults.Error("501: The requested feature has not been implemented by the server.");
                        break;

                    case 502:
                        rtbResults.Error("502: The upstream server this server acts as a proxy/gateway on has sent an invalid response.");
                        break;

                    case 503:
                        rtbResults.Error("503: The service you tried to reach is unavailable. Please try again later.");
                        break;

                    case 504:
                        rtbResults.Error("504: The upstream server this server acts as a proxy/gateway on has not sent a response.");
                        break;

                    case 505:
                        rtbResults.Error("505: The server does not accept the use of HTTP/1.1 requests.");
                        break;

                    case 507:
                        rtbResults.Error("506: The server does not contain enough storage space to accommodate your request.");
                        break;

                    case 511:
                        rtbResults.Error("511: To make a request to a server, you need to authenticate on the network you are currently connected to.");
                        break;

                    /* The default branch is executed if the returned status code
                     * is not being handled by this code block. If that happens,
                     * the user is notified that the client is not prepared to
                     * handle it. */

                    default:
                        rtbResults.Warn($"The server yielded the status code {buf.GetStatusCode()}; this application is not prepared to react to it.");
                        break;
                }
            }
            catch (Exception ex)
            {
                string err;

                /* Checks if the error came from the socket; if this condition
                 * holds true and the error was a connection abortion, tries to
                 * reconnect and resend the request. */
                if (ex is SocketException)
                {
                    if ((ex as SocketException).SocketErrorCode == SocketError.ConnectionAborted)
                    {
                        socket = socket.CheckConnection(ipAddress);
                        retry = true;
                        btnRequest_Click(sender, e);
                        return;
                    }
                    err = (ex as SocketException).GetErrorMessage();
                }
                else
                    err = ex.Message;

                /* Else, notifies the user about the obtained
                 * error. */
                rtbResults.Error("Impossible to perform any operations with the host: " + err);

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

                /* Reenables all relevant controls and aborts
                 * the operation. */
                ReenableAll();
                return;
            }

            /* To avoid errors, checks if the connection
             * socket is still connected. */
            socket = socket.CheckConnection(ipAddress);

            /* Disables the retry flag in case that it has
             * been activated (for example, due to a timeout
             * error) */
            if (retry) retry = false;

            /* Reenables all relevant controls to allow new
             * user interactions. */
            ReenableAll();
        }

        /* Method called everytime the request file textbox
         * changes text; when it is executed, it updates the
         * string associated to the file that needs to be
         * requested. */
        private void rtbRequest_TextChanged(object sender, EventArgs e) => reqFile = rtbRequest.Text;
    }
}