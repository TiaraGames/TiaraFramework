using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace TiaraFramework.Component
{
    static public class Underlying
    {
        #region SetWindowPosition
        // Copy from MSDN :D

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up or top-level window.
        /// </summary>

        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. (HWND value)</param>
        /// <param name="X">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="Y">The new position of the top of the window, in client coordinates.</param>
        /// <param name="W">The new width of the window, in pixels.</param>
        /// <param name="H">The new height of the window, in pixels.</param>
        /// <param name="uFlags">The window sizing and positioning flags. (SWP value)</param>
        /// <returns>Nonzero if function succeeds, zero if function fails.</returns>

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

        /// <summary>
        /// HWND values for hWndInsertAfter
        /// </summary>
        public static class HWND
        {
            public static readonly IntPtr
            NOTOPMOST = new IntPtr(-2),
            BROADCAST = new IntPtr(0xffff),
            TOPMOST = new IntPtr(-1),
            TOP = new IntPtr(0),
            BOTTOM = new IntPtr(1);
        }

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;
        }
        #endregion

        #region Stream -> PNG
        // by 火必烈
        public static void PreMultiplyAlphas(Texture2D ret)
        {
            Byte4[] data = new Byte4[ret.Width * ret.Height];
            ret.GetData<Byte4>(data);
            for (int i = 0; i < data.Length; i++)
            {
                Vector4 vec = data[i].ToVector4();
                float alpha = vec.W / 255.0f;
                int a = (int)(vec.W);
                int r = (int)(alpha * vec.X);
                int g = (int)(alpha * vec.Y);
                int b = (int)(alpha * vec.Z);
                uint packed = (uint)(
                    (a << 24) +
                    (b << 16) +
                    (g << 8) +
                    r
                    );
                data[i].PackedValue = packed;
            }
            ret.SetData<Byte4>(data);
        }
        #endregion
    }
}
