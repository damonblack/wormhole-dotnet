using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// List of Player registered for this game.
	/// </summary>
	[Serializable]
	public class PlayerList
	{
		#region Private Fields
		/// <summary>
		/// a private array of Players
		/// </summary>
		private Player []	_playerArray;
		#endregion
		#region Properties
		public int Count
		{
			get{return _playerArray.GetUpperBound(0)+1;}
		}
		public Player this[int index]
		{
			get{ return _playerArray[index]; }
			set{ _playerArray[index] = value;}
		}
		#endregion
		#region Constructors and Initialization
		private PlayerList(){}
		public PlayerList(int players, Point wormhole)
		{
			_playerArray = new Player[players];
			for(int i = 0; i < players; i++)
				_playerArray[i] = new Player(i,wormhole);
		}	
		#endregion
		#region Methods
		public void GenLogs(SystemList sysList)
		{
			foreach(Player player in _playerArray)
				player.GenLog(sysList, player.ID);
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
