using System;
using System.Collections;

namespace WormholeClient.Data
{
	/// <summary>
	/// Class to transfer orders from players to the referee
	/// </summary>
	[Serializable]
	public class Orders
	{
		#region Private Fields
		private ArrayList			_fleetList;
		#endregion
		#region Properties
		public Fleet this[int index]
		{
			get { return (Fleet)_fleetList[index]; }
		}
		#endregion
		#region Constructors and Initialization
		public Orders()
		{
		}
		#endregion
		#region Methods
		public void Add(Fleet fleet)
		{
			_fleetList.Add(fleet);
		}
		public void Remove(Fleet fleet)
		{
			_fleetList.Remove(fleet);
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
