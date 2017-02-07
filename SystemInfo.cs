using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for SystemInfo.
	/// </summary>
	[Serializable]
	public class SystemInfo
	{
		#region Private Fields
		private bool						_explored;
		private string						_name;
		private Point						_mapLocation;
		private StarSystem.SystemType		_type;
		private int							_mineral = -1;
		private int							_organic = -1;
		private int							_energy = -1;
		private int							_influencePercentage = 0;
		private int							_mineralCollected = -1;
		private int							_organicCollected = -1;
		private int							_energyCollected = -1;

		#endregion
		#region Properties
		public ListViewItem ListViewItem
		{
			get
			{
				ListViewItem lvi = new ListViewItem(_name);
				lvi.SubItems.AddRange(new string[] {	Location,
													   Type,
													   Mineral,
													   Organic,
													   Energy,
													   InfluencePercentage,
													   MineralCollected,
													   OrganicCollected,
													   EnergyCollected});
				return lvi;
			}
		}
		public string Location
		{
			get
			{
				string location = _mapLocation.X.ToString()
					+ "," + _mapLocation.Y.ToString();
				return location;
			}
		}

		public string Name
		{
			get{return _name;}
			set{_name = value;}
		}
		public string Type
		{
			get
			{
				switch(_type)
				{
					case StarSystem.SystemType.RedGiant:
						return "Red Giant";
					case StarSystem.SystemType.WhiteDwarf:
						return "White Dwarf";
					case StarSystem.SystemType.Yellow:
						return "Yellow";
					default:
						return "Unknown";
                }
			}
		}
		public string Mineral
		{
			get
			{
				if(_mineral == -1)
					return "?";
				else
					return _mineral.ToString();
			}
		}
		public string Organic
		{
			get
			{
				if(_organic == -1)
					return "?";
				else
					return _organic.ToString();
			}
		}
		public string Energy
		{
			get
			{
				if(_energy == -1)
					return "?";
				else
					return _energy.ToString();
			}
		}
		public string InfluencePercentage
		{
			get{return _influencePercentage.ToString() + "%";}
		}
		public string MineralCollected
		{
			get
			{
				if(_mineralCollected == -1)
					return "--";
				else
					return _mineralCollected.ToString();
			}
		}
		public string OrganicCollected
		{
			get
			{
				if(_organicCollected == -1)
					return "--";
				else
					return _organicCollected.ToString();
			}
		}
		public string EnergyCollected
		{
			get
			{
				if(_energyCollected == -1)
					return "--";
				else
					return _energyCollected.ToString();
			}
		}
		public int InfPercent
		{
			get{return _influencePercentage;}
			set{_influencePercentage = value;}
		}

		#endregion
		#region Constructors and Initialization
		public SystemInfo(Point mapLocation, StarSystem.SystemType type)
		{
			_mapLocation = mapLocation;
			_name = "WMA " + _mapLocation.X + "." + _mapLocation.Y;	
			_type = type;
		}
		#endregion
		#region Methods
		public void AddResourceInfo(StarSystem system)
		{
			_mineral = system.Mineral;
			_organic = system.Organic;
			_energy = system.Energy;
		}
		public void AddCollectedResourceInfo(int minCollected, int orgCollected,
			int engCollected)
		{
			_mineralCollected = minCollected;
			_organicCollected = orgCollected;
			_energyCollected = engCollected;
		}
		public void ClearCollectedResources()
		{
			_mineralCollected = 0;
			_organicCollected = 0;
			_energyCollected = 0;
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
