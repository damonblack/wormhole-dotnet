using System;
using System.Drawing;
using System.Collections;
namespace WormholeClient.Data
{
	/// <summary>
	/// Data used to update Main window and player views.
	/// This class must be used by the main window to 
	/// initialize the following:</p>
	/// </p>
	/// MapBoard
	///		HexArray
	///		TokenList
	/// </summary>
	[Serializable]
	public class GameData
	{
		#region Private Fields
		private	Point			_wormhole;
		/// <summary>
		/// encapsulates array of DataHexes
		/// </summary>
		private DataMap			_dataMap;
		/// <summary>
		/// List of Players and info
		/// Player fleets should be passed in
		/// the Player class
		/// </summary>
		private PlayerList		_playerList;
		/// <summary>
		/// List of systems in game
		/// </summary>
		private SystemList		_systemList;
		#endregion
		#region Properties
		/// <summary>
		/// gets/sets the DataMap
		/// </summary>
		public DataMap DataMap
		{
			get{return _dataMap;}
			set{ _dataMap = value; }
		}
		public PlayerList PlayerList
		{
			get{return _playerList;}
		}
		public SystemList SystemList
		{
			get{return _systemList;}
		}
		public Point wormhole
		{
			get{return _wormhole;}
		}
		#endregion
		#region Constructors and Initialization
	
		public GameData(DataMap dataMap, SystemList systemList,
						PlayerList playerList)
		{
			_wormhole = dataMap.Wormhole;
            _dataMap = dataMap;
			_systemList = systemList;
			_playerList = playerList;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Cleans up from last turn, resetting all temporary data structures:
		/// System data on influence and resource collection.. also players
		/// resource pools
		/// </summary>
		public void SetupTurn()
		{
			for(int i = 0; i < _systemList.Count; i++)
				_systemList[i].ClearSystemTempData();
			for(int i = 0; i < _playerList.Count; i++)
				_playerList[i].ResourcePool.Reset();
		}
		/// <summary>
		/// Processes Orders filling out the new playerlist with 
		/// updated fleet data for each player
		/// </summary>
		/// <param name="orders"></param>
		public void UpdateFleets(ArrayList orders)
		{
			foreach(object obj in orders)
			{
				Fleet fleet = (Fleet)obj;
				int x = fleet.Owner;
				_playerList[x].Fleet = fleet;
				int count = _playerList[x].Fleet.Count;
				for(int i = 0; i < count; i++)
				{
					_playerList[x].Fleet[i].Set();
				}
			}
		}
		public RangeFinder RangeFinder()
		{
			return _dataMap.RangeFinder();
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
