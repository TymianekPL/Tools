using System;
using System.Runtime.InteropServices;


namespace Tools
{
    public partial class Display
    {
        public enum Orientations
        {
            DEGREES_CW_0 = 0,
            DEGREES_CW_90 = 3,
            DEGREES_CW_180 = 2,
            DEGREES_CW_270 = 1
        }

        public bool Rotate(uint DisplayNumber, Orientations Orientation)
        {
            if (DisplayNumber == 0)
                throw new ArgumentOutOfRangeException(nameof(DisplayNumber), DisplayNumber, "First display is 1.");

            bool result = false;
            DISPLAY_DEVICE d = new();
            DEVMODE dm = new();
            d.cb = Marshal.SizeOf(d);

            if (!NativeMethods.EnumDisplayDevices(null, DisplayNumber - 1, ref d, 0))
                throw new ArgumentOutOfRangeException(nameof(DisplayNumber), DisplayNumber, "Number is greater than connected displays.");

            if (0 != NativeMethods.EnumDisplaySettings(
                d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                if ((dm.dmDisplayOrientation + (int)Orientation) % 2 == 1) // Need to swap height and width?
                {
                    int temp = dm.dmPelsHeight;
                    dm.dmPelsHeight = dm.dmPelsWidth;
                    dm.dmPelsWidth = temp;
                }

                switch (Orientation)
                {
                    case Orientations.DEGREES_CW_90:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_270;
                        break;
                    case Orientations.DEGREES_CW_180:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_180;
                        break;
                    case Orientations.DEGREES_CW_270:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_90;
                        break;
                    case Orientations.DEGREES_CW_0:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
                        break;
                    default:
                        break;
                }

                DISP_CHANGE ret = NativeMethods.ChangeDisplaySettingsEx(
                    d.DeviceName, ref dm, IntPtr.Zero,
                    DisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);

                result = ret == 0;
            }

            return result;
        }

        public void ResetAllRotations()
        {
            try
            {
                uint i = 0;
                while (++i <= 64)
                {
                    Rotate(i, Orientations.DEGREES_CW_0);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Everything is fine, just reached the last display
            }

        }
        #region NativeMethods
        internal class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern DISP_CHANGE ChangeDisplaySettingsEx(
                string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd,
                DisplaySettingsFlags dwflags, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern bool EnumDisplayDevices(
                string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
                uint dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.Ansi)]
            internal static extern int EnumDisplaySettings(
                string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

            public const int DMDO_DEFAULT = 0;
            public const int DMDO_90 = 1;
            public const int DMDO_180 = 2;
            public const int DMDO_270 = 3;

            public const int ENUM_CURRENT_SETTINGS = -1;

        }
        #endregion


        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183565(v=vs.85).aspx
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]

        #region DEVMODE
        internal struct DEVMODE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [System.Runtime.InteropServices.FieldOffset(0)]
            public string dmDeviceName;
            [System.Runtime.InteropServices.FieldOffset(32)]
            public Int16 dmSpecVersion;
            [System.Runtime.InteropServices.FieldOffset(34)]
            public Int16 dmDriverVersion;
            [System.Runtime.InteropServices.FieldOffset(36)]
            public Int16 dmSize;
            [System.Runtime.InteropServices.FieldOffset(38)]
            public Int16 dmDriverExtra;
            [System.Runtime.InteropServices.FieldOffset(40)]
            public DM dmFields;

            [System.Runtime.InteropServices.FieldOffset(44)]
            readonly Int16 dmOrientation;
            [System.Runtime.InteropServices.FieldOffset(46)]
            readonly Int16 dmPaperSize;
            [System.Runtime.InteropServices.FieldOffset(48)]
            readonly Int16 dmPaperLength;
            [System.Runtime.InteropServices.FieldOffset(50)]
            readonly Int16 dmPaperWidth;
            [System.Runtime.InteropServices.FieldOffset(52)]
            readonly Int16 dmScale;
            [System.Runtime.InteropServices.FieldOffset(54)]
            readonly Int16 dmCopies;
            [System.Runtime.InteropServices.FieldOffset(56)]
            readonly Int16 dmDefaultSource;
            [System.Runtime.InteropServices.FieldOffset(58)]
            readonly Int16 dmPrintQuality;

            [System.Runtime.InteropServices.FieldOffset(44)]
            public POINTL dmPosition;
            [System.Runtime.InteropServices.FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [System.Runtime.InteropServices.FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;

            [System.Runtime.InteropServices.FieldOffset(60)]
            public short dmColor;
            [System.Runtime.InteropServices.FieldOffset(62)]
            public short dmDuplex;
            [System.Runtime.InteropServices.FieldOffset(64)]
            public short dmYResolution;
            [System.Runtime.InteropServices.FieldOffset(66)]
            public short dmTTOption;
            [System.Runtime.InteropServices.FieldOffset(68)]
            public short dmCollate;
            [System.Runtime.InteropServices.FieldOffset(72)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [System.Runtime.InteropServices.FieldOffset(102)]
            public Int16 dmLogPixels;
            [System.Runtime.InteropServices.FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [System.Runtime.InteropServices.FieldOffset(108)]
            public Int32 dmPelsWidth;
            [System.Runtime.InteropServices.FieldOffset(112)]
            public Int32 dmPelsHeight;
            [System.Runtime.InteropServices.FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [System.Runtime.InteropServices.FieldOffset(116)]
            public Int32 dmNup;
            [System.Runtime.InteropServices.FieldOffset(120)]
            public Int32 dmDisplayFrequency;
        }

        #endregion
        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183569(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        // See: https://msdn.microsoft.com/de-de/library/windows/desktop/dd162807(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL
        {
            readonly long x;
            readonly long y;
        }

        internal enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        // http://www.pinvoke.net/default.aspx/Enums/DisplayDeviceStateFlags.html
        [Flags()]
        internal enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        // http://www.pinvoke.net/default.aspx/user32/ChangeDisplaySettingsFlags.html
        [Flags()]
        internal enum DisplaySettingsFlags : int
        {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        [Flags()]
        internal enum DM : int
        {
            Orientation = 0x00000001,
            PaperSize = 0x00000002,
            PaperLength = 0x00000004,
            PaperWidth = 0x00000008,
            Scale = 0x00000010,
            Position = 0x00000020,
            NUP = 0x00000040,
            DisplayOrientation = 0x00000080,
            Copies = 0x00000100,
            DefaultSource = 0x00000200,
            PrintQuality = 0x00000400,
            Color = 0x00000800,
            Duplex = 0x00001000,
            YResolution = 0x00002000,
            TTOption = 0x00004000,
            Collate = 0x00008000,
            FormName = 0x00010000,
            LogPixels = 0x00020000,
            BitsPerPixel = 0x00040000,
            PelsWidth = 0x00080000,
            PelsHeight = 0x00100000,
            DisplayFlags = 0x00200000,
            DisplayFrequency = 0x00400000,
            ICMMethod = 0x00800000,
            ICMIntent = 0x01000000,
            MediaType = 0x02000000,
            DitherType = 0x04000000,
            PanningWidth = 0x08000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }


        #region Struct Pointl
        [StructLayout(LayoutKind.Sequential)]
        public struct Pointl
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }
        #endregion

        #region Struct Devmode
        [StructLayout(LayoutKind.Sequential,
            CharSet = CharSet.Ansi)]
        public struct Devmode
        {
            // You can define the following constant
            // but OUTSIDE the structure because you know
            // that size and layout of the structure
            // is very important
            // CCHDEVICENAME = 32 = 0x50
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            // In addition you can define the last character array
            // as following:
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public Char[] dmDeviceName;

            // After the 32-bytes array
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSpecVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverVersion;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmSize;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmDriverExtra;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmFields;

            public Pointl dmPosition;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayOrientation;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFixedOutput;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmColor;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmDuplex;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmYResolution;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmTTOption;

            [MarshalAs(UnmanagedType.I2)]
            public Int16 dmCollate;

            // CCHDEVICENAME = 32 = 0x50
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            // Also can be defined as
            //[MarshalAs(UnmanagedType.ByValArray,
            //    SizeConst = 32, ArraySubType = UnmanagedType.U1)]
            //public Byte[] dmFormName;

            [MarshalAs(UnmanagedType.U2)]
            public UInt16 dmLogPixels;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmBitsPerPel;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPelsHeight;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFlags;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDisplayFrequency;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMMethod;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmICMIntent;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmMediaType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmDitherType;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved1;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmReserved2;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningWidth;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dmPanningHeight;
        }
        #endregion
    }


    public partial class Display
    {
        public class Resolution
        {
            #region Fields
            private static Devmode _oldDevmode;
            #endregion

            #region Consts
            private const int EnumCurrentSettings = -1;     // Retrieves the current display mode.
            private const int DispChangeSuccessful = 0;     // Indicates that the function succeeded.
            private const int DispChangeBadmode = -2;       // The graphics mode is not supported.
            private const int DispChangeRestart = 1;        // The computer must be restarted 
                                                            // for the graphics mode to work.
            #endregion

            #region DllImport
            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern Boolean EnumDisplaySettings(
                [param: MarshalAs(UnmanagedType.LPTStr)] string lpszDeviceName,
                [param: MarshalAs(UnmanagedType.U4)] int iModeNum,
                [In, Out] ref Devmode lpDevMode);

            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.I4)]
            private static extern int ChangeDisplaySettings(
                [In, Out] ref Devmode lpDevMode,
                [param: MarshalAs(UnmanagedType.U4)] uint dwflags);
            #endregion

            #region IsDisplayModeSupported
            /// <summary>
            /// Checks if the given display mode is supported
            /// </summary>
            /// <param name="width" />
            /// <param name="height" />
            /// <param name="supportedModes" />
            /// <returns>
            public bool IsDisplayModeSupported(int width, int height, out string supportedModes)
            {
                var mode = new Devmode();
                mode.dmSize = (ushort)Marshal.SizeOf(mode);

                var modeIndex = 0; // 0 = The first mode
                supportedModes = string.Empty;
                var previousSupportedMode = string.Empty;

                while (EnumDisplaySettings(null,
                    modeIndex,
                    ref mode)) // Mode found
                {
                    if (mode.dmPelsWidth == (uint)width && mode.dmPelsHeight == (uint)height)
                        return true;

                    var newSupportedMode = mode.dmPelsWidth + "x" + mode.dmPelsHeight;
                    if (newSupportedMode != previousSupportedMode)
                    {
                        if (supportedModes == string.Empty)
                            supportedModes += newSupportedMode;
                        else
                            supportedModes += ", " + newSupportedMode;

                        previousSupportedMode = newSupportedMode;
                    }

                    modeIndex++; // The next mode
                }

                return false;
            }
            #endregion

            #region ChangeDisplaySettings
            /// <summary>
            /// Changes the display settings
            /// </summary>
            /// <param name="width" />
            /// <param name="height" />
            public bool ChangeDisplaySettings(int width, int height)
            {
                _oldDevmode = new Devmode();
                _oldDevmode.dmSize = (ushort)Marshal.SizeOf(_oldDevmode);

                // Retrieving current settings
                // to edit them
                EnumDisplaySettings(null,
                    EnumCurrentSettings,
                    ref _oldDevmode);

                // Making a copy of the current settings
                // to allow reseting to the original mode
                var newMode = _oldDevmode;

                // Changing the settings
                newMode.dmPelsWidth = (uint)width;
                newMode.dmPelsHeight = (uint)height;

                // Capturing the operation result, 1 = update registry
                var result =
                    ChangeDisplaySettings(ref newMode, 1);

                switch (result)
                {
                    case DispChangeSuccessful:
                        return true;

                    case DispChangeBadmode:
                        MessageBox.Show("Mode not supported.", "DISPLAY ERROR", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        return false;

                    case DispChangeRestart:
                        MessageBox.Show("Restart required.", "DISPLAY ERROR", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        return false;

                    default:
                        MessageBox.Show("Failed. Error code = " + result, "DISPLAY ERROR", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        return false;
                }
            }
            #endregion

            #region RestoreDisplaySettings
            /// <summary>
            /// Restores the old display settings
            /// </summary>
            public void RestoreDisplaySettings()
            {
                ChangeDisplaySettings(ref _oldDevmode, 1);
            }
            #endregion
        }
    }
}