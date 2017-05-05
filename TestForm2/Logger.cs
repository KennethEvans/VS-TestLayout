using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm2 {
    class Logger {
        private static string SEPARATOR = ",";
        public static string LF = System.Environment.NewLine;
        private bool valid = true;
        private bool writeControlsListColumns = true;
        private string path;
        private string directory = @"C:\Scratch\Logs\TestForm2";
        private string namePrefix = "TestForm2";
        private Control[] controlList;

        public Logger() {
            this.path = directory + @"\" + namePrefix + nameTimeStamp();
        }

        public Logger(string directory, string namePrefix) {
            this.directory = directory;
            this.namePrefix = namePrefix;
            this.path = directory + @"\" + namePrefix + nameTimeStamp();
        }

        /// <summary>
        /// Logs a line with a timestamp plus the message.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        public void Log(string msg) {
            LogLine(timeStamp() + " " + SEPARATOR + msg);
        }

        /// <summary>
        /// Logs a line with msg only (no timestamp).
        /// </summary>
        /// <param name="msg">The message to log.</param>
        public void LogLine(string msg) {
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

        public void LogControls(string msg) {
            if (controlList == null) return;
            StringBuilder sb = new StringBuilder();
            if (writeControlsListColumns) {
                writeControlsListColumns = false;
                sb.Append("Time" + SEPARATOR + "Reason" + SEPARATOR);
                foreach (Control control in controlList) {
                    sb.Append(control.Name + SEPARATOR);
                }
                LogLine(sb.ToString());
            }
            sb.Clear();
            sb.Append(timeStamp() + SEPARATOR + msg + SEPARATOR);
            foreach (Control control in controlList) {
                sb.Append(control.Width + " " + control.Height
                    + SEPARATOR);
            }
            LogLine(sb.ToString());
        }

        public string timeStamp() {
            return DateTime.Now.ToString("s");
        }

        public string nameTimeStamp() {
            return DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss") + ".csv";
        }

        public string Path { get => path; }
        public string NamePrefix { get => namePrefix; set => namePrefix = value; }
        public string Directory { get => directory; set => directory = value; }
        public Control[] ControlList { get => controlList; set => controlList = value; }
        public bool Valid { get => valid; set => valid = value; }
    }
}
