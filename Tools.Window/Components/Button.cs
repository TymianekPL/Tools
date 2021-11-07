using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Window.Components
{
    public class Button : Component
    {
        public override string Name { get; internal set; } = nameof(Button);

        private enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(
uint dwExStyle, 
string lpClassName, 
string lpWindowName, 
uint dwStyle, 
int x, 
int y, 
int nWidth, 
int nHeight, 
IntPtr hWndParent, 
IntPtr hMenu, 
IntPtr hInstance, 
IntPtr lpParam ); 
        public Button()
        {

        }

        internal override IntPtr Implement(IntPtr parent)
        {
            return CreateWindowEx(
                0,
                "BUTTON",  // Predefined class; Unicode assumed 
                "OK",      // Button text 
                0,  // Styles 
                10,         // x position 
                10,         // y position 
                100,        // Button width
                100,        // Button height
                parent,     // Parent window
                IntPtr.Zero,       // No menu.
                GetWindowLong(parent, (int)GWL.GWL_HINSTANCE),
                IntPtr.Zero);
        }
    }
}
