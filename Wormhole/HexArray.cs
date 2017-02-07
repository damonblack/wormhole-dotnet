using System;
using System.Drawing;




namespace WormholeClient.Data
{
	/// <summary>
	/// Array of graphical Hex objects containing player, map and GUI 
	/// related data.  Initiallzed and updated from the Model each turn.
	/// </summary>
	public class HexArray
	{
		#region Private Fields
		/// <summary>
		/// Actual array of hex objects
		/// </summary>
		private Hex [,] _hexArray;
		private int		_mapWidth;
		private int		_mapHeight;
		private int		_hexSize = 35;
		private Point   _wormhole;
		#endregion
		#region Properties
		public Hex[,] Array
		{
			get{ return _hexArray;}
		}
		/// <summary>
		/// Gets map Width
		/// </summary>
		public int MapWidth
		{
			get{ return _mapWidth; }
		}
		/// <summary>
		/// Gets MapHeight
		/// </summary>
		public int MapHeight
		{
			get{ return _mapHeight; }
		}
		public int HexSize
		{
			set
			{
				if(value < 6)
					_hexSize = 6;
				else if (value > 120)
					_hexSize = 120;
				else
					_hexSize = value;
			}
			get 
			{
				return _hexSize;
			}
		}

		/// <summary>
		/// Indexer interface for array -- 
		/// read only
		/// </summary>
		public Hex this[int x, int y]
		{
			get
			{
				return _hexArray[x,y];
			}
		}
		public Point Wormhole
		{
			get{ return _wormhole; }
		}
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// Default Constructor
		/// </summary>
		private HexArray()
		{
		}
		public HexArray(DataMap dataMap)
		{
			_mapWidth = dataMap.Width;
			_mapHeight = dataMap.Height;
			_hexArray = new Hex [_mapWidth,_mapHeight];
			_wormhole = dataMap.Wormhole;
			for (int y = 0; y < _mapHeight; y++)
				for (int x = 0; x < _mapWidth; x++)
				{
					_hexArray[x,y] = new Hex(dataMap[x,y]);
					_hexArray[x,y].Index = new Point(x,y);
				}
		}
		
		#endregion
		#region Methods
		public void UnhighlightHexes()
		{
			foreach (Hex hex in _hexArray)
				hex.Highlighted = false;
		}
		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
		/// <summary>
		/// Re sets hexes location based on current scroll offset and zoom.
		/// </summary>
		/// <param name="offset"></param>
		public void SetCenters(Point offset)
		{
			for (int y = 0; y < _mapHeight; y++)
				for (int x = 0; x < _mapWidth; x++)
				{
					_hexArray[x,y].HexSize = _hexSize;
					if (y % 2 != 0)
						_hexArray[x,y].Center = new Point(2 * _hexSize + 2 * x * _hexSize + offset.X, 
							_hexSize + ((3 * _hexSize * y) / 2) + offset.Y);
					else 
						_hexArray[x,y].Center = new Point(_hexSize + 2 * x * _hexSize + offset.X,
							_hexSize + ((3 * _hexSize * y) / 2) + offset.Y);

				}
		}
		#endregion
	}
}
