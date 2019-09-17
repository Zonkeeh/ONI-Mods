using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessStorageGrid
{
    public class MachineChannelInfo
    {
        public int ID { get; set; }
        public int Channel { get; set; }
        public MachineType Type { get; set; }
        public Storage Store { get; set; }

        public enum MachineType
        {
            Mainframe,
            AccessPoint,
            StorageCell
        }
    }
}
