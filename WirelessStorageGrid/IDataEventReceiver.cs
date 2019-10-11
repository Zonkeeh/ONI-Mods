using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessStorageGrid
{
    public interface IDataEventReceiver : IDataNetworkConnection
    {
        void ReceiveDataEvent(int value);

        int GetDataCell();
    }
}
