using System;
using System.Drawing;
using WormholeClient.Data;
using Messages;

namespace WormholeClient.Data
{
	/// <summary>
	/// Colony Ship data structure
	/// This ship should have the ablility to create 
	/// bases on system hexes
	/// </summary>
	[Serializable]
	public class Trader:Ship
	{


		#region Private Members
		private static int	_tradeShipID = 0;
		private Trade		_trade;
		private int			_ID;
		private int			_mineralCollected;
		private int			_organicCollected;
		private int			_energyCollected;
		#endregion
		#region Properties
		public static int NextTradeShipID
		{
			get{return _tradeShipID++;}
		}
		public int TradeShipID
		{
			get{return _ID;}
		}
		public Trade Trade
		{
			get{return _trade;}
		}
		#endregion
		#region Constructors and Initialization
		public Trader(int ownerID, int tradeToID, Trade trade):base(ownerID, new Point(0,0))
		{
			_type = ShipType.Trader;
			_range = 0;
			_power = 0;
			_trade = trade;
			if(trade.OfferingPartyID == ownerID)
			{
				this._ID = trade.OfferingShipID;
				_mineralUsed = trade.OfferedMineral;
				_organicUsed = trade.OfferedOrganic;
				_energyUsed = trade.OfferedEnergy;
				_mineralCollected = trade.AskedMineral;
				_organicCollected = trade.AskedOrganic;
				_energyCollected = trade.AskedEnergy;
			}
			else
			{
				this._ID = trade.AskedShipID;
				_mineralUsed = trade.AskedMineral;
				_organicUsed = trade.AskedOrganic;
				_energyUsed = trade.AskedEnergy;
				_mineralCollected = trade.OfferedMineral;
				_organicCollected = trade.OfferedOrganic;
				_energyCollected = trade.OfferedEnergy;
			}
		}
		#endregion
		#region Methods
		public override void DoTask(ResourcePool resPool, SystemLog sysLog,DataMap dataMap)
		{
			resPool.AddMineral(_mineralCollected);
			resPool.AddOrganic(_organicCollected);
			resPool.AddEnergy(_energyCollected);
			this._acted = true;
        }			  
		#endregion
	}
}
