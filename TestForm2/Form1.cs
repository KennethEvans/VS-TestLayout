using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TestForm2 {
    public partial class Form1 : Form {
        private static string LF = Environment.NewLine;
        private float currentDpi = 0;
        private float initialDpi;
        private Font initialFont;
        private Size initialSize;

        public Form1() {
            // this.Font = SystemFonts.MessageBoxFont;
            // this.Font = new Font(this.Font.Name, 10);
            // this.Font = getScaledFont();
            InitializeComponent();

            initialDpi = getDpi();
            initialFont = this.Font;
            initialSize = this.Size;

            // Set the handlers to display info for the selected cControl
            setHandlers();
        }

        /// <summary>
        /// Set the handlers so each Control will print its info.
        /// </summary>
        private void setHandlers() {
            MouseEnter += new EventHandler(control_Enter);
            MouseLeave += new EventHandler(control_Leave);
            foreach (Control control in this.Controls) {
                control.MouseEnter += new EventHandler(control_Enter);
                control.MouseLeave += new EventHandler(control_Leave);
                foreach (Control control1 in control.Controls) {
                    control1.MouseEnter += new EventHandler(control_Enter);
                    control1.MouseLeave += new EventHandler(control_Leave);
                    foreach (Control control2 in control1.Controls) {
                        control2.MouseEnter += new EventHandler(control_Enter);
                        control2.MouseLeave += new EventHandler(control_Leave);
#if false
                        Debug.Print("control=" + control.Name
                            + " control1=" + control1.Name
                           + " control2" + control2.Name);
#endif
                    }
                }
            }
        }

        private void control_Enter(object sender, EventArgs e) {
            Control control = (Control)sender;
            StringBuilder sb = new StringBuilder();
            while (control != this && control != null) {
                sb.AppendLine(controlInfo(control));
                control = control.Parent;
            }

            // The Form
            sb.AppendLine(this.Name);
            sb.AppendLine("AutoSize=" + this.AutoSize);
            sb.AppendLine("AutoSizeMode=" + this.AutoSizeMode);
            sb.AppendLine("AutoScaleMode=" + this.AutoScaleMode);
            sb.AppendLine("AutoScaleDimensions=" + this.AutoScaleDimensions);
            sb.AppendLine("CurrentAutoScaleDimensions=" + this.CurrentAutoScaleDimensions);
            sb.AppendLine("AutoScaleFactor=" + this.AutoScaleFactor);
            Font font = this.Font;
            sb.AppendLine("Font=" + font.Name + " " + font.SizeInPoints + " pt" + " (" + font.Size
                + " " + Font.Unit + ")");
            sb.AppendLine(this.Width + "x" + this.Height);

            // The Display
            sb.AppendLine();
            sb.AppendLine("Display");
            sb.Append(displayInfo());
            sb.Append(deviceCapsInfo());
            sb.AppendLine();
            sb.AppendLine("Screen");
            sb.Append(displayScreenInfo());

            textBox5.Text = sb.ToString();
        }

        private void control_Leave(object sender, EventArgs e) {
            textBox5.Text = "";
        }

        /// <summary>
        /// Returns info about this control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private string controlInfo(Control control) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(control.Name);
            sb.AppendLine("Anchor=" + control.Anchor
                + "    Dock=" + control.Dock);
            sb.AppendLine("AutoSize=" + control.AutoSize);
            if (control is Label) {
                Label control1 = (Label)control;
                sb.AppendLine("TextAlign=" + control1.TextAlign);
            } else if (control is TextBox) {
                TextBox control1 = (TextBox)control;
                sb.AppendLine("TextAlign=" + control1.TextAlign);
            } else if (control is Button) {
                Button control1 = (Button)control;
                sb.AppendLine("AutoSizeMode=" + control1.AutoSizeMode);
                sb.AppendLine("TextAlign=" + control1.TextAlign);
            } else if (control is Panel) {
                Panel control1 = (Panel)control;
                sb.AppendLine("AutoSizeMode=" + control1.AutoSizeMode);
            }
            sb.AppendLine(control.Width + "x" + control.Height);
            return sb.ToString();
        }

        /// <summary>
        ///  Gets the current dpi.
        /// </summary>
        /// <returns></returns>
        private float getDpi() {
            Graphics g = this.CreateGraphics();
            float dpi = g.DpiY;
            g.Dispose();
            return dpi;
        }

        private void rescale() {
            if (initialDpi == 0) return;
            if (initialFont != null) {
                float size = initialFont.SizeInPoints * currentDpi / initialDpi;
                Font = new Font(initialFont.Name, size);
            }
            if (initialSize != null) {
                int width = (int)Math.Round(initialSize.Width * currentDpi / initialDpi);
                int height = (int)Math.Round(initialSize.Height * currentDpi / initialDpi);
                this.Size = new Size(width, height);
            }
        }

        /// <summary>
        /// Gets information about the display.
        /// </summary>
        /// <returns></returns>
        private string displayInfo() {
            StringBuilder sb = new StringBuilder();
            float dpiX, dpiY;
            Graphics g = this.CreateGraphics();
            dpiX = g.DpiX;
            dpiY = g.DpiY;
            g.Dispose();
            sb.AppendLine("Dpi=" + dpiX + "x" + dpiY);
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            sb.AppendLine("Screen Bounds=" + rect.Width + "x" + rect.Height);
            rect = Screen.PrimaryScreen.WorkingArea;
            sb.AppendLine("Screen WorkingArea=" + rect.Width + "x" + rect.Height);
            return sb.ToString();
        }

        /// <summary>
        /// Gets information about the screen.
        /// </summary>
        /// <returns></returns>
        private string displayScreenInfo() {
            StringBuilder sb = new StringBuilder();
            Screen screen = Screen.FromControl(this);
            uint dpiX, dpiY;
            screen.GetDpi(DpiType.Effective, out dpiX, out dpiY);
            sb.AppendLine("Screen DeviceName: " + screen.DeviceName);
            sb.AppendLine("  DPI: " + dpiX + "x" + dpiY);
            sb.AppendLine("  Bounds: X=" + screen.Bounds.X
                + " Y=" + screen.Bounds.Y
                + " Width=" + screen.Bounds.Width
                + " Height=" + screen.Bounds.Height);
            sb.AppendLine("  WorkingArea: " + screen.WorkingArea.Width + "x"
                + screen.WorkingArea.Height);
            return sb.ToString();
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap {
            HORZSIZE = 4, // mm
            VERTSIZE = 6, // mm
            HORZRES = 8,
            VERTRES = 10,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        /// <summary>
        /// Gets information from DeviceCaps.
        /// </summary>
        /// <returns></returns>
        private string deviceCapsInfo() {
            StringBuilder sb = new StringBuilder();
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hDC = g.GetHdc();
            int hSize = GetDeviceCaps(hDC, (int)DeviceCap.HORZSIZE);
            int vSize = GetDeviceCaps(hDC, (int)DeviceCap.VERTSIZE);
            int virtH = GetDeviceCaps(hDC, (int)DeviceCap.HORZRES);
            int virtV = GetDeviceCaps(hDC, (int)DeviceCap.VERTRES);
            int physH = GetDeviceCaps(hDC, (int)DeviceCap.DESKTOPHORZRES);
            int physV = GetDeviceCaps(hDC, (int)DeviceCap.DESKTOPVERTRES);
            int logpxH = GetDeviceCaps(hDC, (int)DeviceCap.LOGPIXELSX);
            int logpxV = GetDeviceCaps(hDC, (int)DeviceCap.LOGPIXELSY);
            sb.AppendLine("Size (mm)=" + hSize + "x" + vSize);
            sb.AppendLine("Logical Pixels=" + logpxH + "x" + logpxV);
            sb.AppendLine("Virtual Resolution=" + virtH + "x" + virtV);
            sb.AppendLine("Physical Resolution=" + physH + "x" + physV);
            sb.AppendLine("Scale (%)=" + 100f * (float)physH / (float)virtH
                + "x" + 100f * (float)physV / (float)virtV);

            g.Dispose();
            return sb.ToString();
        }

        private Font getScaledFont() {
            float dpiX, dpiY;
            Graphics g = this.CreateGraphics();
            dpiX = g.DpiX;
            dpiY = g.DpiY;
            g.Dispose();
            Font font = this.Font;
            return new Font(font.Name, font.SizeInPoints * 96F / dpiY);
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                //This message is sent when the form is dragged to a different monitor i.e. when
                //the bigger part of its are is on the new monitor. Note that handling the message immediately
                //might change the size of the form so that it no longer overlaps the new monitor in its bigger part
                //which in turn will send again the WM_DPICHANGED message and this might cause misbehavior.
                //Therefore we delay the scaling if the form is being moved and we use the CanPerformScaling method to 
                //check if it is safe to perform the scaling.
                case 0x02E0: //WM_DPICHANGED
                    {
                        int newDpi = m.WParam.ToInt32() & 0xFFFF;
                        currentDpi = newDpi;
                        rescale();
                        float scaleFactor = (float)newDpi / (float)96;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("WM_DPICHANGED");
                        sb.AppendLine("New DPI: " + newDpi);
                        sb.AppendLine("New Scale: " + scaleFactor);
                        sb.AppendLine(displayScreenInfo());
                        textBox5.Text = sb.ToString();
                    }
                    break;
            }
            base.WndProc(ref m);
        }

    }
}
