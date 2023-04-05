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

        public static string GetHeader(this byte[] buf, string header)
        {
            string tmp = buf.GetHeaders();
            if (tmp.Contains("Location:"))
            {
                tmp = tmp.Substring(buf.GetHeaders().IndexOf(header + ": ") + (header + ": ").Length);
                return tmp.Substring(0, tmp.IndexOf('\n'));
            }
            return string.Empty;
        }

        public static string GetErrorMessage(this SocketException sockEx)
        {
            // TODO - implement more messages
            switch (sockEx.SocketErrorCode)
            {
                case SocketError.Success:
                    return "Success";

                case SocketError.TimedOut:
                    return "Socket connection timed out";

                default:
                    return "fuck";
            }
        }
    }
}
