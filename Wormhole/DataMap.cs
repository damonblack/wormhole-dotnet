using System;
using System.Drawing;
using System.Collections;

namespace WormholeClient.Data
{
	/// <summary>
	/// Stores data for gameboard. Implement as an array of hex 
	/// objects each with information as to their contents, owner,
	/// resources, etc.
	/// --For now......... an array of DataHexes
	/// </summary>
	[Serializable]
	public class DataMap
	{
		#region Private Fields
		private int				_width;
		private int				_height;
		private DataHex [,]		_hexArray; 
		private Point			_wormhole;
		#endregion
		#region Properties
		public int Width
		{
			get{return _width;}
		}
		public int Height
		{
			get{return _height;}
		}
		public DataHex this[int x, int y]
		{
			get{ return _hexArray[x,y];}
		}
		public Point Wormhole
		{
			get{return _wormhole;}
		}
		#endregion
		#region Constructors and Initialization
		private DataMap(){}
		/// <summary>
		/// Constructor creates array of DataHexes, assignes them 
		/// locations and seeds them with systems from the system 
		/// list
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="systemList"></param>
		public DataMap(int x, int y, SystemList systemList, Point wormhole)
		{
			_width = x;
			_height = y;

			_wormhole = wormhole;

			_hexArray = new DataHex[x,y];
			for(int j = 0; j < y; j++)
				for(int i = 0; i < x; i++)
					_hexArray[i,j] = new DataHex(i,j);
			SeedSystems(systemList);
			//Console.WriteLine("DataMap created");//test string
		}
		#endregion
		#region Methods
		public RangeFinder RangeFinder()
		{
			return new RangeFinder(_hexArray);
		}
		#endregion
		#region Utility Functions
		/// <summary>
		/// Cycle through System list, randomly placing systems 
		/// checking to see if there is already a system there 
		/// before placing 
		/// </summary>
		/// <param name="systemList"></param>
		private void SeedSystems(SystemList systemList)
		{
			Random rand = new Random();
			RangeFinder finder = new RangeFinder(_hexArray);
			ArrayList taken = finder.Find(_wormhole.X,_wormhole.Y,3);
			//Assign hexes within two of the wormhole as Empty
			foreach(object hex in taken)
			{
				DataHex dataHex = (DataHex)hex;
				dataHex.System = new StarSystem(StarSystem.SystemType.Empty);
			}
			for(int i = 0; i < systemList.Count; i++)
			{
				bool placed = false;
				//Genrate random locations for systems until a viable one is found
				while(!placed)
				{
					int x = rand.Next(_width);
					int y = rand.Next(_height);
					//if this hex has been assigned as Empty, skip it.
					if(_hexArray[x,y].System != null && 
						_hexArray[x,y].System.Type == StarSystem.SystemType.Empty)
						continue;
					//if this System slot has not been assigned and its not 
					//reserved for the black hole
					if(null == _hexArray[x,y].System && 
							x != _wormhole.X && y != _wormhole.Y)
					{
						//generate a list of hexes surrounding this one
						taken.Clear();
						taken = finder.Find(x,y,1);
						//assume there is room
						bool gotRoom = true;
						//check to see if any systems within one hex
						foreach(object hex in taken)
						{
							DataHex dataHex = (DataHex)hex;
							if(dataHex.System != null && 
								dataHex.System.Type != StarSystem.SystemType.Empty)
							{
								gotRoom = false;
								break;
							}

						}
						if(gotRoom)
						{
							_hexArray[x,y].System = systemList[i];
							systemList[i].MapLocation = new Point(x,y);
							placed = true;
						}

					}
				}
			}
		}
		#endregion
	}
}
