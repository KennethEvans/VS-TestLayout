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
        internal static void GetDpi(this Screen screen,
            NativeMethods.DpiType dpiType, out uint dpiX, out uint dpiY) {
            var point = new Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var hmonitor = NativeMethods.MonitorFromPoint(point,
                NativeMethods.MONITOR_DEFAULTTONEAREST);

            switch (NativeMethods.GetDpiForMonitor(hmonitor, dpiType, out dpiX,
                out dpiY).ToInt32()) {
                case NativeMethods.S_OK: return;
                case NativeMethods.E_INVALIDARG:
                    throw new ArgumentException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
                default:
                    throw new COMException("Unknown error. See https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx for more information.");
            }
        }

    }

    /// <summary>
    /// Class for native methods.
    /// </summary>
    internal static class NativeMethods {
        /// <summary>
        /// Represents the different types of scaling.
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511.aspx"/>
        internal enum DpiType {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062.aspx
        [DllImport("User32.dll")]
        internal static extern IntPtr MonitorFromPoint([In]Point pt,
            [In]uint dwFlags);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510.aspx
        [DllImport("Shcore.dll")]
        internal static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor,
            [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);

        internal const int S_OK = 0;
        internal const int MONITOR_DEFAULTTONEAREST = 2;
        internal const int E_INVALIDARG = -2147024809;

    }
}
