using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Messages
{

	public enum MsgType
	{
		Text,
		Init,
		Orders,
		Cancel,
		ID,
		PlayerIDRequest,
		PlayerID,
		SystemName,
		Trade,
		Ping,
		Invalid
	}
	/// <summary>
	/// Chat message
	/// </summary>
	[Serializable]
	public class MsgText
	{
		private string		_text;
		private BitArray	_bArray;
		public BitArray PlayerTargets
		{
			get{return _bArray;}
		}
		public string Text
		{
			get{return _text;}
			set{_text = value;}
		}
		public MsgText(string text)
		{
			_text = text;
			_bArray = new BitArray(8,true);
		}
		public MsgText(string text, BitArray ba)
		{
			_text = text;
			_bArray = ba;
		}
	}
	[Serializable]
	public class NameAndID
	{
		private string		_password;
		private string		_name;
		private int			_id;
		public string Password
		{
			get{return _password;}
		}
		public string Name
		{
			get{return _name;}
		}
		public int ID
		{
			get{return _id;}
		}
		public NameAndID(int id, string name)
		{ 
			_id = id;
			_name = name;
			_password = "";
		}
		public NameAndID(int id, string name, string password)
		{
			_id = id;
			_name = name;
			_password = password;
		}
	}
	[Serializable]
	public class MsgSystemName
	{
		private string	_name;
		private int		_index;
		public string Name
		{
			get{return _name;}
		}
		public int Index
		{
			get{return _index;}
		}

		public MsgSystemName(string name, int index)
		{
			_name = name;
			_index = index;
		}
	}
	[Serializable]
	public class MsgTrade
	{
		public enum TradeMsgType
		{
			Offer,
			Accept,
			Reject,
			Cancel,
			Message,
			Busy
		}

		private TradeMsgType	_type;
		private Trade	_trade;
		private string	_msg;

		public TradeMsgType Type
		{
			get{return _type;}
			set{_type = value;}
		}
		public Trade Trade
		{
			get{return _trade;}
		}
		public string Msg
		{
			get{return _msg;}
			set{_msg = value;}
		}
		public MsgTrade(TradeMsgType type, Trade trade, string msg)
		{
			_type = type;
			_trade = trade;
			_msg = msg;
		}
	}
	[Serializable]
	public class Trade
	{

		private int		_offeringPartyID;
		private int		_offeringShipID;
		private string  _offeringName;
		private int		_askedPartyID;
		private int		_askedShipID;
		private string  _askedName;

		private int		_offeredMineral;
		private int		_offeredOrganic;
		private int		_offeredEnergy;
		private int		_askedMineral;
		private int		_askedOrganic;
		private int		_askedEnergy;

		public int OfferingPartyID
		{
			get{return _offeringPartyID;}
		}
		public int OfferingShipID
		{
			get{return _offeringShipID;}
		}
		public string OfferingName
		{
			get{return _offeringName;}
			set{_offeringName = value;}
		}
		public int AskedPartyID
		{
			get{return _askedPartyID;}
		}
		public int AskedShipID
		{
			get{return _askedShipID;}
			set{_askedShipID = value;}
		}
		public string AskedName
		{
			get{return _askedName;}
			set{_askedName = value;}
		}
		public int OfferedMineral
		{
			get{return _offeredMineral;}
		}
		public int OfferedOrganic
		{
			get{return _offeredOrganic;}
		}
		public int OfferedEnergy
		{
			get{return _offeredEnergy;}
		}
		public int AskedMineral
		{
			get{return _askedMineral;}
		}
		public int AskedOrganic
		{
			get{return _askedOrganic;}
		}
		public int AskedEnergy
		{
			get{return _askedEnergy;}
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public Trade(int offeringID,int offeringShipID,string offeringName,int acceptingID,
						int oMin, int oOrg, int oEng,int aMin, int aOrg, int aEng)
		{
			this._offeringPartyID = offeringID;
			this._offeringShipID = offeringShipID;
			this._offeringName = offeringName;
			this._askedPartyID = acceptingID;
			this._offeredMineral = oMin;
			this._offeredOrganic = oOrg;
			this._offeredEnergy = oEng;
			this._askedMineral = aMin;
			this._askedOrganic = aOrg;
			this._askedEnergy = aEng;
		}
		public override string ToString()
		{
			string text = this._askedName + " gets M" + this._offeredMineral +
												" O" + this._offeredOrganic +
												" E" + this._offeredEnergy + " from " +
						                        this._offeringName + ", in exchange for M" + this._askedMineral +
												" O" + this.AskedOrganic + 
												" E" + this.AskedEnergy;
			return text;
		}

	}
}
