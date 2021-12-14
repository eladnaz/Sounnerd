using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Sounnerd
{
    public delegate void HotkeyPressedEventHandler(object sender, HotkeyPressedEventArgs e);
    public class HotkeyPressedEventArgs : EventArgs
    {
        public Key Key { get; }
        public ModifierKeys Modifiers { get; }
        public bool Handled { get; set; }

        public HotkeyPressedEventArgs(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }
    }

    public class HotkeyManager
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [StructLayout(LayoutKind.Sequential)]
        private class KeyboardHookStruct
        {
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            public int vkCode;
            /// Specifies a hardware scan code for the key. 
            public int scanCode;
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            public int flags;
            /// Specifies the time stamp for this message.
            public int time;
            /// Specifies extra information associated with the message. 
            public int dwExtraInfo;
        }

        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam);

        private int keyboardHook = 0;

        public HotkeyManager()
        {
        }

        public event HotkeyPressedEventHandler HotkeyPressed;

        public void Start()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (keyboardHook == 0)
            {
                // Create an instance of HookProc.
                keyboardHook = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    KeyboardHookProc,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);

                // If SetWindowsHookEx fails.
                if (keyboardHook == 0)
                {
                    // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    // do cleanup
                    Stop(false);
                    // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        public void Stop(bool throwExceptions)
        {
            // if keyboard hook set and must be uninstalled
            if (keyboardHook != 0)
            {
                // uninstall hook
                int retKeyboard = UnhookWindowsHookEx(keyboardHook);

                // reset invalid handle
                keyboardHook = 0;

                // if failed and exception must be thrown
                if (retKeyboard == 0 && throwExceptions)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            bool handled = false;
            if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                if (HotkeyPressed != null)
                {
                    // read structure KeyboardHookStruct at lParam
                    KeyboardHookStruct khStruct =
                        (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                    if (khStruct != null)
                    {
                        Key key = KeyInterop.KeyFromVirtualKey(khStruct.vkCode);

                        HotkeyPressedEventArgs args = new HotkeyPressedEventArgs(key, Keyboard.Modifiers);
                        HotkeyPressed.Invoke(this, args);

                        handled = args.Handled;
                    }
                }
            }

            // if event handled in application do not handoff to other listeners
            if (handled)
            {
                return 1;
            }

            return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
        }
    }
}
