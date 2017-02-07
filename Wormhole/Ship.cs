using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for Ship.
	/// </summary>
	
	[Serializable]
	public abstract class Ship:MapObject


	{
		//Ship Constants
		public enum Task
		{
			Survey,
			Collect,
			Base,
			Move
		}
		public enum MineralSupport
		{
			Colony = 15,
			Scout = 15,
			Defender = 60,
			Base = 50
		}
		public enum OrganicSupport
		{
			Colony = 25,
			Scout = 10,
			Defender = 20,
			Base = 50
		}
		public enum EnergySupport
		{
			Colony = 10,
			Scout = 35,
			Defender = 25,
			Base = 50
		}
		protected enum BasePower
		{
			Colony = 2,
			Scout = 1,
			Defender = 5,
			Base = 7
		}
		protected enum BaseRange
		{
			Colony = 1,
			Scout = 4,
			Defender = 2,
			Base = 0
		}
		public enum ShipType
		{
			Colony,
			Scout,
			Defender,
			Base,
			Trader,
			None
		}
		#region Private Fields
		/// <summary>
		/// current map location of ship icon
		/// </summary>
		protected Point					_newLocation;
		/// <summary>
		/// ID of player owning ship
		/// </summary>
		protected int					_ownerID;
		/// <summary>
		/// Ship class
		/// </summary>
		protected ShipType				_type;
		/// <summary>
		/// index map location of ship
		/// </summary>
		protected BasePower				_power;
		/// <summary>
		/// Movement range of ship, used for moving and influence
		/// </summary>
		protected BaseRange				_range;
		/// <summary>
		/// Default task performed by ship if not ordered otherwise
		/// </summary>
		protected Task					_defaultTask;
		/// <summary>
		/// currently assigned task
		/// </summary>
		protected Task					_currentTask;
		/// <summary>
		/// flag to indicate whether a ahip has already acted this turn
		/// each ship may act only once per turn, including movement
		/// </summary>
		protected bool					_acted = false;
		/// <summary>
		/// index into fleet array;
		/// </summary>
		protected int					_mineralUsed;
		protected int					_organicUsed;
		protected int					_energyUsed;
  		#endregion	
		#region Properties
		public bool Acted
		{
			get{return _acted;}
			set{_acted = value;}
		}
		public ShipType Type
		{
			get { return _type; }
			set { _type = value;}
		}
		/// <summary>
		/// get/set ship current mapboard location
		/// </summary>
		public Point HexLocation
		{
			get { return _newLocation; }
			set { _newLocation = value; }
		}
		public int Range
		{
			get { return (int)_range;}
		}
		public int Power
		{
			get{return (int)_power;}
		}
		public Task DefaultTask
		{
			get{return _defaultTask;}
		}
		public Task CurrentTask
		{
			get{return _currentTask;}
			set{_currentTask = value;}
		}
		public int Owner
		{
			get{return _ownerID;}
		}
		public int MineralUsed
		{
			get{return _mineralUsed;}
		}
		public int OrganicUsed
		{
			get{return _organicUsed;}
		}
		public int EnergyUsed
		{
			get{return _energyUsed;}
		}
		#endregion
		#region Constructors and Initialization
		protected Ship( int ownerID, Point homeHex):base(homeHex)
		{
			_ownerID = ownerID;
			_newLocation = homeHex;
		}
		#endregion
		#region Methods
		/// <summary>
		/// attempts to move ship to new location, returns
		/// true if successful
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Move(int x, int y)
		{
			return false;
		}
		public void Set()
		{
			if(this.HomeHex != this.HexLocation)
			{
				CurrentTask = Task.Move;
				this.HomeHex = this.HexLocation;
			}
			else if(CurrentTask != Task.Base)
			{
				_currentTask = _defaultTask;
			}				
		}
		public virtual void DoTask(ResourcePool resPool, SystemLog sysLog,DataMap dataMap)
		{
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
