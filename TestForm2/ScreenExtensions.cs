namespace System.Windows.Forms {
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Extends the <see cref="System.Windows.Forms.Screen"/> class.
    /// </summary>
    public static class ScreenExtensions {
        /// <summary>
        /// Returns the scaling of the given screen.
        /// </summary>
        /// <param name="screen">The screen which scaling should be given back.</param>
        /// <param name="dpiType">The type of dpi that should be given back..</param>
        /// <param name="dpiX">Gives the horizontal scaling back (in dpi).</param>
        /// <param name="dpiY">Gives the vertical scaling back (in dpi).</param>
        public static void GetDpi(this Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY) {
            var point = new Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var hmonitor = MonitorFromPoint(point, MONITOR_DEFAULTTONEAREST);

            switch (GetDpiForMonitor(hmonitor, dpiType, out dpiX, out dpiY).ToInt32()) {
                case S_OK: return;
                case E_INVALIDARG:
                    throw new ArgumentException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
                default:
                    throw new COMException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
            }
        }

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062.aspx
        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromPoint([In]Point pt, [In]uint dwFlags);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx
        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);

        const int S_OK = 0;
        const int MONITOR_DEFAULTTONEAREST = 2;
        const int E_INVALIDARG = -2147024809;
    }

    /// <summary>
    /// Represents the different types of scaling.
    /// </summary>
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511.aspx"/>
    public enum DpiType {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
}
