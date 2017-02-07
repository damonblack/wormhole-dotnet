using System;
using System.Drawing;
using System.Collections;

namespace WormholeClient.Data
{
	/// <summary>
	/// List of ships owned by a particular player
	/// </summary>
	[Serializable]
	public class Fleet
	{
		//public event SupportUpdateDelegate eSupportUpdate;
		#region Private Fields
		private bool					_fleetSupported = true;
		/// <summary>
		/// Id number of owning player
		/// </summary>
		private int						_ownerID;
		/// <summary>
		/// Arraylist of ships
		/// </summary>
		private ArrayList				_shipArray = new ArrayList(10);

		private int						_excessMineral;
		private int						_excessOrganic;
		private int						_excessEnergy;
		private int						_supportMineral;
		private int						_supportOrganic;
		private int						_supportEnergy;

		#endregion
		#region Properties
		public int Owner
		{
			get{return _ownerID;}
		}
		public int ExMineral
		{
			get{return _excessMineral;}
		}
		public int ExOrganic
		{
			get{return _excessOrganic;}
		}
		public int ExEnergy
		{
			get{return _excessEnergy;}
		}
		public int SupMineral
		{
			get{return _supportMineral;}
		}
		public int SupOrganic
		{
			get{return _supportOrganic;}
		}
		public int SupEnergy
		{
			get{return _supportEnergy;}
		}
		public bool FleetSupported
		{
			get{return _fleetSupported;}
		}
		public int Count
		{
			get{ return _shipArray.Count; }
		}
		public Ship this[int index]
		{
			get{ return (Ship)_shipArray[index];}
		}
		#endregion
		#region Constructors and Initialization
		// Fleets intially constructed with two ships, Colony and scout
		// at the wormhole.
		public Fleet(int id, Point wormhole)
		{
			_ownerID = id;
			
			_shipArray.Add(new Scout(id,wormhole));
			_shipArray.Add(new Colony(id,wormhole));
		}
		#endregion		
		#region Methods
		public void Remove(Ship ship)
		{
			_shipArray.Remove(ship);
		}
		public void RemoveTradeShip(int shipID)
		{
			// find trade ship in fleet with desired ID
			Trader trader = null;
			object remShip = null;
			foreach(object obj in _shipArray)
			{
				if(obj.GetType()== typeof(Trader))
				{
					trader = (Trader)obj;
					if(trader.TradeShipID == shipID)
						remShip = obj;
				}
			}
			//if found remove it
			if(remShip != null)
				this._shipArray.Remove(remShip);
		}

		public void Add(Ship.ShipType type, Point hexLocation)
		{
			switch(type)
			{
				case Ship.ShipType.Colony:
				{
					_shipArray.Add(new Colony(_ownerID,hexLocation));
					break;
				}
				case Ship.ShipType.Scout:
				{
					_shipArray.Add(new Scout(_ownerID,hexLocation));
					break;
				}						
				case Ship.ShipType.Defender:
				{
					_shipArray.Add(new Defender(_ownerID,hexLocation));
					break;
				}
			}
		}
		/// <summary>
		/// for adding trade ships and possible other 'special ships'
		/// </summary>
		/// <param name="ship"></param>
		public void AddSpecialShip(Ship ship)
		{
			_shipArray.Add(ship);
		}
		public void Support(ResourcePool resPool)
		{
			_excessMineral = 0; 
			_excessOrganic = 0; 
			_excessEnergy = 0;
			int mineralNeeded = 0;
			int organicNeeded = 0;
			int energyNeeded = 0;
			foreach(Ship ship in _shipArray)
			{
				_supportMineral = mineralNeeded += ship.MineralUsed;
				_supportOrganic = organicNeeded += ship.OrganicUsed;
				_supportEnergy = energyNeeded += ship.EnergyUsed;
			}
			if(mineralNeeded > resPool.Mineral||organicNeeded > resPool.Organic
				||energyNeeded > resPool.Energy)
				_fleetSupported = false;
			else
				_fleetSupported = true;
			_excessMineral = resPool.Mineral - mineralNeeded; 
			_excessOrganic = resPool.Organic - organicNeeded; 
			_excessEnergy = resPool.Energy - energyNeeded;
			/*if(eSupportUpdate != null)
			{
				EventArgs ea = new EventArgs();
				eSupportUpdate(this,ea);
			}*/
		}
		#endregion
	}
}
