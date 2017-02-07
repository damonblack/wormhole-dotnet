using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
namespace WormholeClient.Data
{
	/// <summary>
	/// Object to manage temporary map data for GUI.  
	/// </summary>
	public class MapBoard
	{

		#region Private Fields
		private Pen[]			_aPlayerPens = {Pens.Blue, 
												   Pens.Magenta,
												   Pens.Aqua,
												   Pens.White,
												   Pens.DarkGray,
												   Pens.Red,
												   Pens.Green,
												   Pens.Yellow};
													
		/// <summary>
		/// this players integer ID, also index into Model's playerlist
		/// </summary>
		private int				_playerID;
		/// <summary>
		/// stores GameData for current turn as supplied by the model
		/// </summary>
		private GameData		_gameData;
		/// <summary>
		/// Array of graphical Hex objects
		/// </summary>
		private HexArray		_hexArray;
		/// <summary>
		/// list of hexes highlighted to show range for a ship
		/// </summary>
		private ArrayList		_range = new ArrayList(32);
		/// <summary>
		/// List of all active tokens to be displayed. 
		/// Ordered to paint fore images first. Should 
		/// hit tested in reverse order.
		/// </summary>
		private TokenList		_tokenList;
		/// <summary>
		/// Offset point for scrolling purposes
		/// </summary>
		private Point			_offset;
		/// <summary>
		/// Currently selected hex from HexArray
		/// Usually highlighted
		/// </summary>
		private Hex				_selectedHex = null;
		/// <summary>
		/// Object for calculationg legal moves on this board
		/// </summary>
		private RangeFinder     _rangeFinder;
		/// <summary>
		/// list of moveable tokens representing players ships
		/// </summary>
		#endregion
		#region Properties
		/// <summary>
		/// get/set SelectedHex
		/// </summary>
		public Hex SelectedHex
		{
			get { return _selectedHex; }
			set { _selectedHex = value; }
		}
		/// <summary>
		/// set hex size for zoom
		/// </summary>
		public int HexSize
		{
			set
			{
				_hexArray.HexSize = value;
				ReSetMap();
			}
			get
			{
				return _hexArray.HexSize;
			}
		}

		/// <summary>
		/// Calcutate and return current Map size in pixels.
		/// </summary>
		public Size MapPixelSize
		{
			get  
			{
				return new Size(_hexArray.MapWidth * 2 * HexSize + HexSize + 3,
					_hexArray.MapHeight * 3 * HexSize/2 + HexSize/2 + 5);
			}
		}
		/// <summary>
		/// Get/set map offset for scrolling
		/// </summary>
		public Point Offset
		{
			get
			{ 
				return _offset;
			}
			set
			{
				if(value==_offset)
					return;
				_offset = value;
				ReSetMap();
			}
		}
		#endregion
		#region Constructors and Initialization
		private MapBoard(){}//should never be used, hence private
		/// <summary>
		/// Default constructor
		/// </summary>
		public MapBoard(GameData gameData, int PlayerID)
		{
			//unassigned players have an ID of -1
			if(PlayerID != -1)
			{
				_playerID = PlayerID;
			}
			_gameData = gameData;
            _hexArray = new HexArray(gameData.DataMap);
			_rangeFinder = new RangeFinder(_hexArray.Array);
			_tokenList = new TokenList(gameData.PlayerList,
				gameData.SystemList, _hexArray, PlayerID);
			
			
			ReSetMap();
		}
		#endregion
		#region Methods
		/// <summary>
		/// Duh... Draws map?
		/// </summary>
		/// <param name="e"></param>
		public void DrawMap(Graphics grfx)
		{
			grfx.SmoothingMode = SmoothingMode.AntiAlias;
			for(int y = 0; y < _hexArray.MapHeight; y++)
				for(int x = 0; x < _hexArray.MapWidth; x++)
				{
					Hex hex = _hexArray[x,y];
					hex.Draw(grfx);
					if(hex.System != null && hex.System.Owner != -1)
					{ 
						grfx.DrawEllipse(_aPlayerPens[hex.System.Owner],
							hex.SystemToken.Location.X,
							hex.SystemToken.Location.Y,
							hex.SystemToken.Width,
							hex.SystemToken.Height);
					}
				}
			foreach (Hex hexObject in _range)
			{				
				hexObject.Highlighted = true;
				hexObject.Draw(grfx);
			}
			if(_selectedHex != null)
				_selectedHex.Draw(grfx);
				
		}
		public void PaintTokens(Graphics grfx)
		{
			int count = _tokenList.Count;
			for(int i = 0; i < count; i++)
			{
				_tokenList[i].Draw(grfx);
			}
		}
		/// <summary>
		/// Resets map after zoom or scroll offset change
		/// </summary>
		public void ReSetMap()
		{
			_hexArray.SetCenters(_offset);
		}
		/// <summary>
		/// Returns Hex from HexArray currently under pixel coordinates x,y
		/// </summary>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <returns></returns>
		public Hex HitTestMap(int cx, int cy)
		{
			cx -= _offset.X;
			cy -= _offset.Y;
			int x; int y;
			int delta;
			if( (cx / HexSize) % 2 == 0 )	//if the column is even
				delta = HexSize - ( cx % HexSize );
			else							//or if its odd
				delta = cx % HexSize;
			
			cy -= delta/2;
			//
			//  .....uh, don't ask.  There's probably a more straight forward 
			// way to do this, but setting up regions, hit testing sub-areas all 
			// involved looping which would really slow down as things get bigger
			// and more complicated. I'll look at this again sometime.
			//
			y = (cy/(3*HexSize))*2;
			if((cy % (3*HexSize))>(2*HexSize - delta))
				y++;
			if( y % 2 == 1 )
				x = ( cx - HexSize )/( 2 * HexSize );
			else
				x = cx /( 2 * HexSize );
			if( x < 0 || y < 0 || x >= _hexArray.MapWidth || y >= _hexArray.MapHeight)
				return null;
			else
				return _selectedHex = _hexArray[x,y];	
		}
		public Token HitTestTokens(int x, int y)
		{
			//cycle throught tokenlist backward, hittesting the 
			//last tokens drawn first
			for(int i = _tokenList.Count - 1; i > 0; i--)//"count" gives total					
				if(_tokenList[i].HitTest(x,y))			 //tokens, top index is -1
					return _tokenList[i];				 //stop at 1, not testing 
			return null;								 //wormhole at this time
			
		}
		public void MoveToken(Hex destHex, MoveableToken token)
		{
			//remove the ship token from its current location
			Hex hex = _hexArray[token.HexIndex.X,token.HexIndex.Y];
			if(hex!=null)
				hex.TokenList.RemoveToken(token);
			//add the token to its new hex
			destHex.TokenList.AddToken(token);
			//change the token's current hex
			token.HexIndex = destHex.Index;
			//resets the tokens pixel location
			destHex.SetPoints();
			Console.WriteLine("Token Moved from " + hex.Index + " to " + destHex.Index);
		}
		public void UnhighlightHexes()
		{
			_hexArray.UnhighlightHexes();
		}
		public void ShowRange(int X,int Y,int move)
		{
			ClearRange();
			_range = _rangeFinder.Find(X,Y,move);
			foreach (Hex hex in _range)
				hex.Highlighted = true;
		}
		
		public void ClearRange()
		{
			if(_range!=null)
			{
				foreach(Hex hex in _range)
					hex.Highlighted = false;
				_range.Clear();
			}
		}
		public void RemoveToken(Token token)
		{
			_tokenList.RemoveToken(token);
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
