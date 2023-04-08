namespace RCp_Phase1
{
    /* Class responsible for containing any extension
     * methods used in the project; an extension method
     * is a method created to extend the capabilities
     * of a specific data type or structure such that
     * it is not needed to create a different, possibly
     * derived data type containing such methods. */
    public static class ExtensionMethods
    {

        /* Method responsible for appending a specific message with a
         * determined color for a multiline textbox. */
        private static void AppendWithColor(this RichTextBox rtb, string message, Color color)
        {
            /* Gets the end position of the textbox's text, and zeroes
             * the text selection length. */
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            /* Sets the text color to the specified color, appends the
             * given message to the textbox along with a new line, and
             * resets the text color to the previous one. */
            rtb.SelectionColor = color;
            rtb.AppendText(message + Environment.NewLine);
            rtb.SelectionColor = rtb.ForeColor;
        }

        /* Method responsible for setting a complementary message 
         * on the associated RichTextBox control. */
        public static void Message(this RichTextBox rtb, string message) => 
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0, 0x38, 0xff));

        /* Method responsible for setting a success message 
         * on the associated RichTextBox control. */
        public static void Success(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0, 0x86, 0));


        /* Method responsible for setting a warning message on
         * the associated RichTextBox control. */
        public static void Warn(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0xff, 0x82, 0));

        /* Method responsible for setting an error message on
         * the associated RichTextBox control. */
        public static void Error(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0xdd, 0, 0));

        /* Method responsible for retrieving the status code from a
         * buffer with the HTTP response from the remote host. */
        public static short GetStatusCode(this byte[] buf) =>
            short.Parse(Encoding.ASCII.GetString(buf).Split(' ')[1]);

        /* Method responsible for retrieving the response headers
         * from a buffer with the HTTP response from the remote
         * host. */
        public static string GetHeaders(this byte[] buf) =>
            Encoding.ASCII.GetString(buf).Split("\r\n\r\n")[0];

        /* Method responsible for retrieving the response content
         * from a buffer with the HTTP response from the remote
         * host. */
        public static string GetResponse(this byte[] buf) =>
            Encoding.ASCII.GetString(buf).Split("\r\n\r\n")[1];

        /* Method responsible for retrieving a specific header
         * from a buffer with the HTTP response from the remote
         * host. */
        public static string GetHeader(this byte[] buf, string header)
        {
            string tmp = buf.GetHeaders();
            if (tmp.Contains(header))
            {
                tmp = tmp.Substring(buf.GetHeaders().IndexOf(header + ": ") + (header + ": ").Length);
                return tmp.Contains("\r\n") ? tmp.Substring(0, tmp.IndexOf("\r\n")) : tmp;
            }
            return string.Empty;
        }

        /* Method responsible for returning a custom error
         * message associated to a specific socket error, such
         * as a socket disconnection or connection error. */
        public static string GetErrorMessage(this SocketException sockEx) => 
            sockEx.SocketErrorCode switch
            {
                SocketError.SocketError => "Unspecified socket error",
                SocketError.Success => "Operation completed successfully",
                SocketError.OperationAborted => "Operation aborted due to socket closure",
                SocketError.TooManyOpenSockets => "Too many open connection sockets",
                SocketError.MessageSize => "Request too long",
                SocketError.OperationNotSupported => "Address family not supported by protocol family",
                SocketError.AddressFamilyNotSupported => "Address family not supported by local machine",
                SocketError.AddressAlreadyInUse => "Address already in use by another socket",
                SocketError.AddressNotAvailable => "Invalid IP address",
                SocketError.NetworkDown => "Network deactivated in local machine",
                SocketError.NetworkUnreachable => "Network does not contain path to remote host",
                SocketError.NetworkReset => "KeepAlive set on a timed out connection",
                SocketError.ConnectionReset => "Connection reset by remote host",
                SocketError.NoBufferSpaceAvailable => "No free buffer space for socket operation",
                SocketError.IsConnected => "Socket already connected",
                SocketError.NotConnected => "Socket not connected",
                SocketError.Shutdown => "Operation cancelled due to closed socket",
                SocketError.TimedOut => "Connection timed out, or host did not respond",
                SocketError.ConnectionRefused => "Remote host actively refused connection",
                SocketError.HostDown => "Host is down",
                SocketError.HostUnreachable => "No network route to specified host",
                SocketError.HostNotFound => "Unknown host",
                SocketError.TryAgain => "Host could not be resolved, try again later",
                SocketError.NoRecovery => "Unrecoverable error",
                SocketError.NoData => "IP address not found on name server",
                _ => $"Unimplemented socket error message; check https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socketerror?view=net-7.0 for error code {sockEx.SocketErrorCode}",
            };

        /* Method responsible to check the connection
         * status of a given socket; if it is not
         * connected, the given socket instance created
         * for the connection is disposed of, recreated
         * and reconnected to the given previous remote 
         * host. */
        public static Socket CheckConnection(this Socket sock, string prevAddr)
        {
            if (!sock.Connected)
            {
                sock.Close(5000);
                sock.Dispose();

                sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(prevAddr, 80);
            }

            return sock;
        }
    }
}
