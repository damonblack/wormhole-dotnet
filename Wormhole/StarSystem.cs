using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// System type for hex
	/// </summary>
	[Serializable]
	public class StarSystem:MapObject
	{

		public enum SystemType
		{			
			WhiteDwarf,
			RedGiant,
			Yellow,
			Empty
		}
		/// <summary>
		/// Primary planet type
		/// </summary>
		public enum PlanetType
		{
			green,		
			rock,		
			waste,
			gas,
			mystery
		}

		#region Private Fields
		/// <summary>
		/// Resources available
		/// </summary>
		private int								_mineral;
		private int								_organic;
		private int								_energy;
		/// <summary>
		/// Index into the system list
		/// </summary>
		private int								_index;
		[NonSerialized]
		private static Random					_rand = new Random();
		/// <summary>
		/// system type
		/// </summary>
		private SystemType						_type;
		/// <summary>
		/// Array of influence, indexed by player ID 
		/// </summary>
		private int[]							_aInfluence;
		/// <summary>
		/// checks to make sure players don't collect resources > once per turn
		/// </summary>
		private bool[]							_aCollected;
		/// <summary>
		/// Total influence being exherted on system
		/// </summary>
		private int								_totalInfluence;
		/// <summary>
		/// Id of player with controlling influence over system(-1 for no controller)
		/// </summary>
		private int								_ownerID = -1;
		/// <summary>
		/// indicates whether a base exists or not
		/// </summary>
		private Ship							_base = null;
		#endregion
		#region Properties
		/// <summary>
		/// get index into systemList
		/// </summary>
		public int Index
		{
			get{return _index;}
		}
		/// <summary>
		/// get/set map index location
		/// </summary>
		public Point MapLocation
		{
			get{return base.HomeHex;}
			set{base.HomeHex = value;}
		}
		/// <summary>
		/// get system type
		/// </summary>
		public SystemType Type
		{
			get{return _type;}
		}
		public Ship Base
		{
			get{return _base;}
			set
			{
				if(_base == value)
					return;
				else
				{
					if(_base != null)
						_base.Type = Ship.ShipType.Colony;
					_base = value;
				}
			}
		}

		/// <summary>
		/// get minerals available
		/// </summary>
		public int Mineral
		{
			get{return _mineral;}
		}
		/// <summary>
		/// get organics available
		/// </summary>
		public int Organic
		{
			get{return _organic;}
		}
		/// <summary>
		/// get energy available
		/// </summary>
		public int Energy
		{
			get{return _energy;}
		}
		/// <summary>
		/// get owning player
		/// </summary>
		public int Owner
		{
			get{return _ownerID;}
			set
			{
				if(value == _ownerID)
					return;
				else
				{
					_ownerID = value;
					Base = null;
				}
			}
		}
		#endregion
		#region Constructors and Initialization
		public StarSystem(StarSystem.SystemType type)
		{ 
			_type = type;
		}
		public StarSystem(int players, int index)//players = number of players
		{
			_index = index;
			switch(_rand.Next(3))
			{
				case 0:
				{
					_type = SystemType.RedGiant;
					_mineral = _rand.Next(30) + 20;//20-50
					_organic = _rand.Next(20) + 5;//5-25
					_energy = _rand.Next(20) + 15;//15-35
					Console.WriteLine("Red Giant created, mineral = " + _mineral.ToString());
					break;
				}
				case 1:
				{
					_type = SystemType.WhiteDwarf;
					_mineral = _rand.Next(20) + 5;//5-25
					_organic = _rand.Next(20) + 15;//15-35
					_energy = _rand.Next(30) + 20;//20-50
					break;
				}
				case 2:
				{
					_type = SystemType.Yellow;
					_mineral = _rand.Next(20) + 15;//15-35
					_organic = _rand.Next(30) + 20;//20-50
					_energy = _rand.Next(20) + 5;//5-25
					break;
				}
			}
			_aInfluence = new int[players];
			_aCollected = new bool[players];
			//for(int i = 0; i < players; i++)
				//_aInfluence[i] = 0;
		}
		#endregion
		#region Methods
		public void GetResources(ResourcePool resPool, Ship ship, SystemLog sysLog)
		{
			if(this.Type == StarSystem.SystemType.Empty)
				return;
			if(!_aCollected[ship.Owner])
			{
				int inf = _aInfluence[ship.Owner];
				resPool.AddMineral((inf * _mineral)/_totalInfluence);
				resPool.AddOrganic((inf * _organic)/_totalInfluence);
				resPool.AddEnergy((inf * _energy)/_totalInfluence);
				_aCollected[ship.Owner] = true;
				sysLog[_index].AddCollectedResourceInfo((inf * _mineral)/_totalInfluence,
													    (inf * _organic)/_totalInfluence,
													    (inf * _energy)/_totalInfluence);								
													
			}
		}

		/// <summary>
		/// Adds influence from a ship within range
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="power"></param>
		public void AddInfluence(int ID, int power)
		{
			_aInfluence[ID] += power;
			//Console.WriteLine("Player " + ID + "adds " + power + " Influence to hex " + 
									//base.HomeHex.X + "," + base.HomeHex.Y);
		}
		/// <summary>
		/// Generates resource shares and returns controlling player if any
		/// </summary>
		/// <returns></returns>
		public void ResolveInfluence()
		{
			//adds up all the influence and checks to see if anyone has
			for(int i = 0; i <= _aInfluence.GetUpperBound(0); i++)
			{
				_totalInfluence += _aInfluence[i];
			}
			int controllingInfluence = _totalInfluence/2;
			for(int i = 0; i <= _aInfluence.GetUpperBound(0); i++)
			{
				if(_aInfluence[i] > controllingInfluence&&_aInfluence[i]>1)
				{
					_ownerID = i;
				}
			}
		}
		/// <summary>
		/// Clears out influence from previous turn
		/// </summary>
		public void ClearSystemTempData()
		{
			for(int i = 0; i <= _aInfluence.GetUpperBound(0); i++)
				_aInfluence[i] = 0;
			for(int i = 0; i <= _aCollected.GetUpperBound(0); i++)
				_aCollected[i] = false;
			_totalInfluence = 0;
			//_ownerID = -1; was clearing this...now owner keeps it 
			//til someone else gets it.
		}
		public int GetInfluencePercentage(int playerID)
		{
			if(_totalInfluence == 0)
				return 0;
			else
				return (_aInfluence[playerID] * 100)/_totalInfluence;
		}
		public bool HasCollected(int ID)
		{
			if(_aCollected[ID] == true)
				return true;
			else
				return false;
		}

		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
		#endregion
	}
}
