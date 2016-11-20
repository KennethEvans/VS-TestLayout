using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm2 {
    public partial class Form1 : Form {
        private static string LF = Environment.NewLine;

        public Form1() {
            InitializeComponent();
            setHandlers();
        }

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
                        Debug.Print("control=" + control.Name
                            + " control1=" + control1.Name
                           + " control2" + control2.Name);
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
            textBox1.Text = sb.ToString();
        }

        private void control_Leave(object sender, EventArgs e) {
            textBox1.Text = "";
        }

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
                sb.AppendLine("TextAlign=" + control1.TextAlign);
            } else if (control is Panel) {
                Panel control1 = (Panel)control;
                sb.AppendLine("AutoSizeMode=" + control1.AutoSizeMode);
            }
            return sb.ToString();
        }
    }
}
