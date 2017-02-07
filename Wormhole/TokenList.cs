using System;
using System.Drawing;
using System.Collections;
using WormholeClient;

namespace WormholeClient.Data
{
	/// <summary>
	/// List of all active tokens to be displayed for player
	/// MapBoard should create a new list every time update 
	/// is ready from Model.
	/// 
	/// 
	/// </summary>
	public class TokenList
	{
		//TODO Create three images for each token, to be displayed at 
		//different resolutions/hexsized
		#region Private Fields
		private ArrayList				_tokenArray = new ArrayList(32);
		#endregion
		#region Properties
		public Token this[int index]
		{
			get{return (Token)_tokenArray[index];}
        }
		public int Count
		{
			get{return _tokenArray.Count;}
		}
		#endregion
		#region Constructors and Initialization
		//default token list, used for hexes
		public TokenList()
		{
		}
		/// <summary>
		/// populate TokenList with all visible tokens drawn from
		/// GameData, compile these in order for re-paint and hit-
		/// testing purposes: system tokens, non-movable ships 
		/// including players own ships, then moveable 
		/// tokens, i.e. current players fleet
		/// </summary>
		public TokenList(PlayerList playerList, SystemList systemList,
							HexArray hexArray, int playerID)
		{
			
			//
			//  Create token for Worm hole, assign it to its hex and 
			//  add it to the token list
			//  
			Token wormholeToken = new Token("Wormhole.ico",hexArray.Wormhole);
			hexArray[hexArray.Wormhole.X,
				hexArray.Wormhole.Y].SystemToken = wormholeToken;
			_tokenArray.Add(wormholeToken);
			//  Cycle through systemList, creating tokens for
			//  each system, adding them to the list and
			//	assigning them to their home hex.
			for ( int i = 0; i < systemList.Count; i++)
			{
				StarSystem system = systemList[i];
				Point location = system.MapLocation; //Hex index of system
				Token token = CreateSystemToken(system,location);
				_tokenArray.Add(token);
				hexArray[location.X,location.Y].SystemToken = token;
			}
			//
			//  Cycle through playerlist, getting Fleets and creating 
			//  Tokens for fleets and adding them to the list
			//  and assigning their to a home hex. Skip this player 
			//  and create moveable tokens assigning them.
			//

			int count = playerList.Count;
			for(int i = 0; i < count; i++)
			{
				Fleet fleet = playerList[i].Fleet;
				//skip this players icons for now
				if(i == playerID)
					continue;
				
				for(int j = 0; j < fleet.Count; j++)
				{
					Ship ship = fleet[j];
					if(ship.Type == Ship.ShipType.Trader)
						continue;
					Token token = CreateShipToken(ship,i);
					_tokenArray.Add(token);
					hexArray[ship.HexLocation.X,
						ship.HexLocation.Y].TokenList.AddToken(token);
				}
	
			}
			//if this is the controller view, we're done
			if(playerID==-1)return;
			//otherwise
			//create this players fleet tokens
			//add them to the list and assign them to their hexes
			for(int i = 0; i < playerList[playerID].Fleet.Count; i++)
			{
			
				Ship ship = playerList[playerID].Fleet[i];
				if(ship.Type == Ship.ShipType.Trader)
					continue;
				MoveableToken token = CreatePlayerShipToken(ship, playerID);
				_tokenArray.Add(token);
				hexArray[ship.HexLocation.X,
					ship.HexLocation.Y].TokenList.AddToken(token);
			}
		
		}
		#endregion
		#region Methods
		public void AddToken(Token token)
		{
			_tokenArray.Add(token);
		}
		public void RemoveToken(Token token)
		{
			if(_tokenArray.Contains(token))
				_tokenArray.Remove(token);
		}
		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
		/// <summary>
		/// Creates and return system token
		/// </summary>
		/// <param name="system"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		private Token CreateSystemToken(StarSystem system,Point hexIndex)
		{
			switch(system.Type)
			{
				case StarSystem.SystemType.RedGiant:
				{
					Token token = new Token("StarRed.ico",hexIndex);
					token.Source = system;
					return token;
				}
				case StarSystem.SystemType.WhiteDwarf:
				{
					Token token = new Token("StarWhite.ico",hexIndex);
					token.Source = system;
					return token;

				}
				case StarSystem.SystemType.Yellow:
				{ 
					Token token = new Token("StarYellow.ico",hexIndex);
					token.Source = system;
					return token;
				}
				default:
					return null;
			}
		}
		private Token CreateShipToken(Ship ship, int ID)//why are we passing "ID"?
		{
			switch(ship.Type)
			{
				case Ship.ShipType.Colony:
				{
					Token token = new Token("ShipColony" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				case Ship.ShipType.Scout:
				{
					Token token = new Token("ShipScout" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				case Ship.ShipType.Defender:
				{
					Token token = new Token("ShipDefender" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				case Ship.ShipType.Base:
				{
					Token token = new Token("ColonyBase" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				default:
				{
					Console.WriteLine("default ship token created");
					return null;
				}
			}
		}
		private MoveableToken CreatePlayerShipToken(Ship ship, int ID)// ID?
		{
			switch(ship.Type)
			{
				case Ship.ShipType.Colony:
				{
					MoveableToken token = new MoveableToken("ShipColony" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				case Ship.ShipType.Scout:
				{
					MoveableToken token = new MoveableToken("ShipScout" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;;
				}
				case Ship.ShipType.Defender:
				{
					MoveableToken token = new MoveableToken("ShipDefender" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				case Ship.ShipType.Base:
				{
					MoveableToken token = new MoveableToken("ColonyBase" + ID + ".ico",ship.HexLocation);
					token.Source = ship;
					return token;
				}
				default:
				{
					Console.WriteLine("default moveable ship token created");
					return null;
				}
			}
		}
		#endregion
	}
}
