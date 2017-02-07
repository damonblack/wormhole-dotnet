using System;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for ResourcePool.
	/// </summary>
	[Serializable]
	public class ResourcePool
	{
		#region Private Fields
		private int			_mineral = 60;//base level
		private int			_organic = 70;//base level
		private int			_energy = 80;//base level
		#endregion
		#region Properties
		public int Mineral
		{
			get{ return _mineral;}
		}
		public int Organic
		{
			get{ return _organic;}
		}
		public int Energy
		{
			get{ return _energy;}
		}
		#endregion
		#region Constructors and Initialization
		public ResourcePool()
		{

		}
		#endregion
		#region Methods
		public void Reset()
		{
			_mineral = 60;
			_organic = 70;
			_energy  = 80;
		}
		public void AddMineral(int amt)
		{
			_mineral += amt;
		}
		public void AddOrganic(int amt)
		{
			_organic += amt;
		}
		public void AddEnergy(int amt)
		{
			_energy += amt;
		}
		public void TakeMineral(int amt)
		{
			_mineral -= amt;
		}
		public void TakeOrganic(int amt)
		{
			_organic -= amt;
		}
		public void TakeEnergy(int amt)
		{
			_energy -= amt;
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
