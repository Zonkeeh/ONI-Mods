using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessStorageGrid
{
    public interface IDataNetworkConnection
    {
        void OnDataNetworkConnectionChanged(bool connected);
    }
}
