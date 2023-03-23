using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RCp_Phase1
{
    public static class ExtensionMethods
    {
        public static void AppendWithColor(this RichTextBox rtb, string message, Color color)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.AppendText(message + Environment.NewLine);
            rtb.SelectionColor = rtb.ForeColor;
        }
        public static void Message(this RichTextBox rtb, string message) => 
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0, 0x38, 0xff));

        public static void Success(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0, 0x86, 0));

        public static void Warn(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0xff, 0x82, 0));

        public static void Error(this RichTextBox rtb, string message) =>
            rtb.AppendWithColor(message, Color.FromArgb(0xff, 0xdd, 0, 0));

        public static short GetStatusCode(this string s) => !s.StartsWith("HTTP") ? (short)-1 : short.Parse(s.Split(' ')[1]);

        public static string GetResponse(this string s) => s.Split("\r\n\r\n")[1];
    }
}
