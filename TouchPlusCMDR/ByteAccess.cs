using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchPlusCMDR
{
    class ByteAccess
    {
        public UInt32 MakeLong(UInt16 high, UInt16 low)
        {
            return ((UInt32)low & 0xFFFF) | (((UInt32)high & 0xFFFF) << 16);
        }

        public UInt16 MakeWord(byte high, byte low)
        {
            return (UInt16)(((UInt32)low & 0xFF) | ((UInt32)high & 0xFF) << 8);
        }

        public UInt16 LoWord(UInt32 nValue)
        {
            return (UInt16)(nValue & 0xFFFF);
        }

        public UInt16 HiWord(UInt32 nValue)
        {
            return (UInt16)(nValue >> 16);
        }

        public Byte LoByte(UInt16 nValue)
        {
            return (Byte)(nValue & 0xFF);
        }

        public Byte HiByte(UInt16 nValue)
        {
            return (Byte)(nValue >> 8);
        }

        public string GetIntBinaryString(int n)
        {
            char[] b = new char[32];
            int pos = 31;
            int i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
    }
}
