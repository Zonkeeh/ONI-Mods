namespace WirelessPower.Components.Interfaces
{
	public interface IWirelessTransferer : IWirelessConnecter
	{
		float JoulesToTransfer { get; set; }

		float FalloffJoulesToTransfer { get; }

		float WirelessBatteryThreshold { get; set; }
	}
}
