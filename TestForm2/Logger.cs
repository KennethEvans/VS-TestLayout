using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm2 {
    class Logger {
        private static string SEPARATOR = "\t";
        public static string LF = System.Environment.NewLine;
        private bool valid = true;
        private string path;
        private string directory = @"C:\Scratch\Logs\TestForm2";
        private string namePrefix = "TestForm2";
        private Control[] controlList;

        public Logger() {
            setPath();
        }

        public Logger(string directory, string namePrefix) {
            this.directory = directory;
            this.namePrefix = namePrefix;
            setPath();
        }

        /// <summary>
        /// Constructs the path from the current values of directory and
        /// namePrefix.
        /// </summary>
        private void setPath() {
            this.path = directory + @"\" + namePrefix
                + DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss") + ".csv";
        }

        /// <summary>
        /// Logs a line with a timestamp plus the message.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        public void log(string msg) {
            logLine(timeStamp() + " " + SEPARATOR + msg);
        }

        public void log(string msg1, string msg2) {
            logLine(timeStamp() + " " + SEPARATOR + msg1 + SEPARATOR + msg2);
        }

        /// <summary>
        /// Logs a line with msg only (no timestamp).
        /// </summary>
        /// <param name="msg">The message to log.</param>
        public void logLine(string msg) {
            if (valid == false || path == null) return;
            System.IO.StreamWriter sw = null;
            try {
                sw = System.IO.File.AppendText(path);
            } catch (Exception ex) {
                MessageBox.Show(msg += LF + "Exception: " + ex + LF
                + ex.Message, "Exception");
                valid = false;
                return;
            }
            try {
                sw.WriteLine(msg);
            } finally {
                sw.Close();
            }
        }

        /// <summary>
        /// Logs the column labels.  Should be called first.
        /// </summary>
        public void logControlsLabels() {
            if (controlList == null) return;
            StringBuilder sb = new StringBuilder();
            sb.Append("Time" + SEPARATOR + "Message" + SEPARATOR);
            foreach (Control control in controlList) {
                sb.Append(control.Name + SEPARATOR);
            }
            logLine(sb.ToString());
        }

        /// <summary>
        /// Fills in information for each cControl in the list.
        /// </summary>
        /// <param name="msg"></param>
        public void logControls(string msg) {
            if (controlList == null) return;
            StringBuilder sb = new StringBuilder();
            sb.Append(timeStamp() + SEPARATOR + msg + SEPARATOR);
            foreach (Control control in controlList) {
                sb.Append(control.AutoSize
                    + " [" + control.Anchor + "] [" + control.Dock + "] "
                    + control.Width + " " + control.Height
                    + SEPARATOR);
            }
            logLine(sb.ToString());
        }

        /// <summary>
        /// Generates a timestamp for a log entry.
        /// </summary>
        /// <returns></returns>
        public string timeStamp() {
            return DateTime.Now.ToString("s");
        }

        public string Path { get => path; }
        public string NamePrefix { get => namePrefix; set => namePrefix = value; }
        public string Directory { get => directory; set => directory = value; }
        public Control[] ControlList { get => controlList; set => controlList = value; }
        public bool Valid { get => valid; set => valid = value; }
    }
}
