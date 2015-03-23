using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using AForge.Video.DirectShow;

namespace TouchPlusCMDR
{
    class TouchPlus
    {
        // Import the functions we will need to lookup the function pointers for the Touch+ lib
        // We can't directly import them since they use the @ charactor in the function entry point!
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        // Now let's setup the delegate function prototypes to later call the pointers
        // Note anything from the eSPDI dll needs to be Cdecl, and the other uses stdcall...
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int enumDevice(int* numDevices);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetDeviceName(int count, StringBuilder name, StringBuilder unknown);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SelectDevice(int device);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SWUnlock(int param);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SWLock(int param);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SetSensorType(int type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DisableAE();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DisableAWB();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int GetGPIOValue(int param, uint* value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SetGPIOValue(int param, uint value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int GetAccMeterValue(int* x, int* y, int* z);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int ReadFlash(byte[] value, int length);             // The SDK seems to say the second var is length of array to request?

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate int SPDI_Init(IntPtr* ptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetDeviceNumber(IntPtr ptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FindDevice(IntPtr ptr, int deviceCount);

        // Now lets declare some global function pointers
        enumDevice _enumDevice;
        GetDeviceName _GetDeviceName;
        SelectDevice _SelectDevice;
        SetSensorType _SetSensorType;
        SWUnlock _SWUnlock;
        SWLock _SWLock;
        DisableAE _DisableAE;
        DisableAWB _DisableAWB;
        GetGPIOValue _GetGPIOValue;
        SetGPIOValue _SetGPIOValue;
        GetAccMeterValue _GetAccMeterValue;
        ReadFlash _ReadFlash;
        SPDI_Init _SPDI_Init;
        GetDeviceNumber _GetDeviceNumber;
        FindDevice _FindDevice;

        // And normal globals
        private FilterInfoCollection VideoCaptureDevices;                                                           // All video devices available
        public ArrayList errors = new ArrayList();                                                                  // Store all errors encountered
        public ArrayList messages = new ArrayList();                                                                // Store output messages
        public Boolean busy = false;                                                                                // Flag to indicate if we are busy or not
        private int DeviceNum = -1;                                                                                 // Hold the device number once found, -1 otherwise
        ByteAccess b = new ByteAccess();                                                                            // Byte / Int conversion helper class

        enum eSPAEAWB_Return
        {
            ESPAEAWB_UNKNOWN = 10,
            ESPAEAWB_RET_OK = 0,
            ESPAEAWB_RET_ENUM_FAIL = -10,
            ESPAEAWB_RET_BAD_PARAM = -11,
            ESPAEAWB_RET_INVALID_DEVICE = -12,
            ESPAEAWB_RET_INVALID_SENSOR = -13,
            ESPAEAWB_RET_REG_READ_FAIL = -21,
            ESPAEAWB_RET_REG_WRITE_FAIL = -22,
            ESPAEAWB_RET_PROP_READ_FAIL = -23,
            ESPAEAWB_RET_PROP_WRITE_FAIL = -24,
            ESPAEAWB_RET_APP_NOT_SUPPORTED = -25
        };

        public int GetDeviceNum()
        {
            return DeviceNum;
        }

        public void LoadLibs()
        {
            busy = true;

            IntPtr _dllHandle;
            IntPtr _fptr;
            string _dll1 = "DLL\\eSPAEAWBCtrl.dll";
            string _dll2 = "DLL\\eSPDI.dll";
            string Function_Name = "";

            SetDllDirectory("DLL\\");
            _dllHandle = LoadLibrary(_dll1);
            if (_dllHandle == IntPtr.Zero)
            {
                errors.Add("Error loading DLL: " + _dll2 + "\n" + Marshal.GetLastWin32Error().ToString()); // Error Code while loading DLL
            }

            Function_Name = "_eSPAEAWB_EnumDevice@4";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _enumDevice = (enumDevice)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(enumDevice));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_GetDevicename@12";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _GetDeviceName = (GetDeviceName)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(GetDeviceName));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_SelectDevice@4";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SelectDevice = (SelectDevice)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SelectDevice));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_SetSensorType@4";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SetSensorType = (SetSensorType)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SetSensorType));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_SWUnlock@4";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SWUnlock = (SWUnlock)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SWUnlock));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_SWLock@4";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SWLock = (SWLock)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SWLock));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_DisableAE@0";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _DisableAE = (DisableAE)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(DisableAE));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_DisableAWB@0";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _DisableAWB = (DisableAWB)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(DisableAWB));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_GetGPIOValue@8";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _GetGPIOValue = (GetGPIOValue)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(GetGPIOValue));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_SetGPIOValue@8";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SetGPIOValue = (SetGPIOValue)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SetGPIOValue));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_GetAccMeterValue@12";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _GetAccMeterValue = (GetAccMeterValue)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(GetAccMeterValue));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "_eSPAEAWB_ReadFlash@8";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _ReadFlash = (ReadFlash)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(ReadFlash));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            // Switching to functions from second (ETron) DLL file now
            _dllHandle = LoadLibrary(_dll2);
            if (_dllHandle == IntPtr.Zero)
            {
                errors.Add("Error loading DLL: " + _dll2 + "\n" + Marshal.GetLastWin32Error().ToString()); // Error Code while loading DLL
            }

            Function_Name = "EtronDI_Init";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _SPDI_Init = (SPDI_Init)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(SPDI_Init));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "EtronDI_GetDeviceNumber";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _GetDeviceNumber = (GetDeviceNumber)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(GetDeviceNumber));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }

            Function_Name = "EtronDI_FindDevice";
            _fptr = GetProcAddress(_dllHandle, Function_Name);
            if (_fptr != IntPtr.Zero)
            {
                _FindDevice = (FindDevice)Marshal.GetDelegateForFunctionPointer(_fptr, typeof(FindDevice));
            }
            else
            {
                errors.Add("Error loading DLL function: " + Function_Name);
            }
            if (errors.Count == 0)
            {
                messages.Add("Loaded Libraries!");
            }

            busy = false;
        }

        public void GetSerialNum()
        {
            //Trying to figure out how to retrieve the serial number, but it's a unmanaged int8 (byte) array that needs to be passed and read out.
            // Throws an unknown error now that is not present in the ractiv sdk definitions. :(
            busy = true;
            eSPAEAWB_Return ret = 0;

            unsafe
            {
                byte[] Serial = new byte[10];
                if (eSPAEAWB_Function((eSPAEAWB_Return)_ReadFlash(Serial, 10), "ReadFlash(&Serial, 10)"))
                {
                    messages.Add("Serial: " + Serial.ToString());
                }
            }
            busy = false;
        }

        public void InitTouchPlus()
        {
            busy = true;

            int ret = 0;
            int devicecount = 0;
            int numDevices = 0;

            unsafe
            {
                IntPtr ptr = IntPtr.Zero;
                ret = _SPDI_Init(&ptr);
                messages.Add("SPDI_Init returned: " + ret.ToString() + " | PTR = " + ptr.ToString());
                devicecount = _GetDeviceNumber(ptr);
                messages.Add("Devicecount returned: " + devicecount.ToString());
                ret = _FindDevice(ptr, devicecount);
                messages.Add("Find device returned: " + ret.ToString());
                ret = _enumDevice(&numDevices);
                messages.Add("enumDevice: " + ret.ToString() + " | numDevices = " + numDevices.ToString());

                /*// This block iterates through the devices and lists their names. However the DLL appears to only give me 1 char back. Instead using directshow for this.
                for (int a = 0; a < numDevices; a++)
                {
                    StringBuilder name = new StringBuilder("",500);
                    StringBuilder unknown = new StringBuilder("",500);

                    _GetDeviceName(a, name, unknown);
                    if (name.ToString() == "T")
                    {
                        DeviceNum = a;
                    }
                    messages.Add("Item " + a.ToString() + ": Name = " + name + " Unknown = " + unknown);
                }
                */
            }
                // Directshow method of iterating through the available devices
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                for (int a = 0; a < VideoCaptureDevices.Count; a++)
                {
                    if (VideoCaptureDevices[a].Name == "Touch+ Camera")
                    {
                        messages.Add("Device [" + a.ToString() + "] found: " + VideoCaptureDevices[a].Name + " *SELECTED*");
                        DeviceNum = a;
                    }
                    else
                    {
                        messages.Add("Device [" + a.ToString() + "] found: " + VideoCaptureDevices[a].Name);
                    }
                }

            busy = false;
        }

        public void UnlockTouchPlus()
        {
            busy = true;

            eSPAEAWB_Return ret = 0;

            if (DeviceNum == -1)
            {
                messages.Add("Error: Couldn't find the Touch+ device!");
            }
            else
            {
                unsafe
                {
                    eSPAEAWB_Function((eSPAEAWB_Return)_SelectDevice(DeviceNum), "SelectDevice(" + DeviceNum.ToString() + ")");
                    eSPAEAWB_Function((eSPAEAWB_Return)_SetSensorType(1), "SetSensorType(1)");
                    eSPAEAWB_Function((eSPAEAWB_Return)_SWUnlock(263), "SWUnlock(263)");
                    eSPAEAWB_Function((eSPAEAWB_Return)_DisableAE(), "DisableAE()");
                    eSPAEAWB_Function((eSPAEAWB_Return)_DisableAWB(), "DisableAWB()");
                    int x = 0, y = 0, z = 0;
                    if (eSPAEAWB_Function((eSPAEAWB_Return)_GetAccMeterValue(&x, &y, &z), "GetAccMeterValue()"))
                    {
                        messages.Add("X: " + x.ToString());
                        messages.Add("Y: " + y.ToString());
                        messages.Add("Z: " + z.ToString());
                    }
                }
                if (errors.Count == 0)
                {
                    messages.Add("Done unlocking the Touch+!");
                }
            }

            busy = false;
        }

        private Boolean eSPAEAWB_Function(eSPAEAWB_Return ret, string p)
        {
            if (ret != 0)
            {
                errors.Add(p + " : ERRORED: " + ret.ToString());
                return false;
            }
            else
            {
                messages.Add(p + " : returned: " + ret.ToString());
                return true;
            }
        }

        public void IRLedOFF()
        {
            eSPAEAWB_Return ret = 0;
            uint GPIOValue = 0;
            unsafe
            {
                // // Appearently, each value (1 etc) pulls a different block of GPIO lines. Who knows what they all do. Anyone want to map the circuit traces?!?
                if (eSPAEAWB_Function((eSPAEAWB_Return)_GetGPIOValue(1, &GPIOValue), "GetGPIOValue(1,x)"))
                {
                    byte lo = b.LoByte(b.LoWord(GPIOValue));
                    messages.Add("GPIO Old Value: " + b.GetIntBinaryString((int)lo));
                    lo = (byte)(lo & 0xF7);                         // & F7 will turn off bit 3 in the LoByte, bit 3 seems to be our IR LEDs
                    if (eSPAEAWB_Function((eSPAEAWB_Return)_SetGPIOValue(1, lo), "SetGPIOValue(1," + lo.ToString() + ")"))
                    {
                        messages.Add("GPIO New Value: " + b.GetIntBinaryString((int)lo));
                    }
                }
            }
        }

        public void IRLedON()
        {
            eSPAEAWB_Return ret = 0;
            uint GPIOValue = 0;
            unsafe
            {
                // // Appearently, each value (1 etc) pulls a different block of GPIO lines. Who knows what they all do. Anyone want to map the circuit traces?!?
                if (eSPAEAWB_Function((eSPAEAWB_Return)_GetGPIOValue(1, &GPIOValue), "GetGPIOValue(1,x)"))
                {
                    byte lo = b.LoByte(b.LoWord(GPIOValue));
                    messages.Add("GPIO Old Value: " + b.GetIntBinaryString((int)lo));
                    lo = (byte)(lo | 8);                         // | 8 will turn on bit 3 in the LoByte, bit 3 seems to be our IR LEDs
                    if (eSPAEAWB_Function((eSPAEAWB_Return)_SetGPIOValue(1, lo), "SetGPIOValue(1," + lo.ToString() + ")"))
                    {
                        messages.Add("GPIO New Value: " + b.GetIntBinaryString((int)lo));
                    }
                }
            }
        }

        public void LockTouchPlus()
        {
            busy = true;

            eSPAEAWB_Return ret = 0;

            if (DeviceNum == -1)
            {
                errors.Add("Error: Not connected yet!");
            }
            else
            {
                unsafe
                {
                    eSPAEAWB_Function((eSPAEAWB_Return)_SWLock(0), "SWLock(0)");
                    DeviceNum = -1;
                }
                if (errors.Count == 0)
                {
                    messages.Add("Done Locking the Touch+!");
                }
            }

            busy = false;
        }
    }
}
