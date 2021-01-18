namespace WirelessPower.Components.Interfaces
{
	public interface IWirelessConnecter
	{
		bool IsOperational { get; }

		bool IsConnectedToGrid { get; }

		int Channel { get; set; }

		void OnConnectionChanged();

		bool ConnectToGrid();

		bool DisconnectFromGrid();
	}
}
