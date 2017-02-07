using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// Colony Ship data structure
	/// This ship should have the ablility to create 
	/// bases on system hexes
	/// </summary>
	[Serializable]
	public class Colony:Ship
	{
		#region Private Fields
		/// <summary>
		/// indicates that the Colony is functioning as a base
		/// </summary>
		private bool			_base;	
		#endregion
		#region Properties
		public bool Base
		{
			get{return _base;}
			set
			{
				if(value == true)
				{
					base.Type = ShipType.Base;
					_power = BasePower.Base;
					_range = BaseRange.Base;
					_mineralUsed = (int)MineralSupport.Base;
					_organicUsed = (int)OrganicSupport.Base;
					_energyUsed = (int)EnergySupport.Base;
					_base = value;
				}
				else
				{
					base.Type = ShipType.Colony;
					_power = BasePower.Colony;
					_range = BaseRange.Colony;
					_mineralUsed = (int)MineralSupport.Colony;
					_organicUsed = (int)OrganicSupport.Colony;
					_energyUsed = (int)EnergySupport.Colony;
					_base = value;
				}
				this._acted = true;
			}
		}				
		#endregion
		#region Constructors and Initialization
		public Colony(int ownerID,Point homeHex):base(ownerID, homeHex)
		{
			_type = ShipType.Colony;
			_range = BaseRange.Colony;
			_power = BasePower.Colony;
			_defaultTask = Task.Collect;
			_currentTask = _defaultTask;
			_mineralUsed = (int)MineralSupport.Colony;
			_organicUsed = (int)OrganicSupport.Colony;
			_energyUsed = (int)EnergySupport.Colony;
		}
		#endregion
		#region Methods
		public override void DoTask(ResourcePool resPool, SystemLog sysLog,DataMap dataMap)
		{
			switch(_currentTask)
			{
				case Task.Base:
				{
					break;
				}
				case Task.Collect:
				{
					if(dataMap[_hexLocation.X,_hexLocation.Y].System == null)
					{
						_currentTask = Task.Survey;
						goto case Task.Survey;
					}
					else
					{
						StarSystem system = dataMap[_hexLocation.X,_hexLocation.Y].System;
						sysLog.AddInfo(dataMap[this.HexLocation.X,this.HexLocation.Y]);
						system.GetResources(resPool,this,sysLog);
						this._acted = true;
					}
					break;
				}
				case Task.Survey:
				{
					sysLog.AddInfo(dataMap[this.HexLocation.X,this.HexLocation.Y]);
					this._acted = true;
					break;
				}
				case Task.Move:
				{
					break;
				}
				default:
				{	//collect
					break;
				}
			}
		}			  
		#endregion
	}
}
