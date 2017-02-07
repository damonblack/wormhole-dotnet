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
		private int			_mineral = 80;//base level
		private int			_organic = 85;//base level
		private int			_energy = 95;//base level
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
			_mineral = 80;
			_organic = 85;
			_energy  = 95;
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
