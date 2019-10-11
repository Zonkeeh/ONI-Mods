using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessStorageGrid
{
    public interface IDataEventSender : IDataNetworkConnection
    {
        void DataTick();

        int GetDataCell();

        int GetDataValue();
    }
}
