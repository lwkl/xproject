using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLua
{
    public class RawString
    {
        byte[] data_;

        public byte[] Data
        {
            get { return data_; }
        }

        public RawString(byte[] bytes)
        {
            data_ = bytes;
        }
    }
}
