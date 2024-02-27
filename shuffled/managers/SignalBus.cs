using System;
using Godot;

namespace Shuffled.Managers;

public partial class SignalBus : Node
{
	#region card slot signals
	public static string CARD_SLOT_CLICKED = "CardSlotClicked";
	[Signal]public delegate void CardSlotClickedEventHandler(string cardSlotName);

	public static string CARD_SLOT_ENTERED = "CardSlotEntered";
	[Signal]public delegate void CardSlotEnteredEventHandler(string cardSlotName);

	public static string CARD_SLOT_EXITED = "CardSlotExited";
	[Signal]public delegate void CardSlotExitedEventHandler(string cardSlotName);
	#endregion

	#region card battle signals
	public static string CARD_BATTLE_ROUND_ENDED = "CardBattleRoundEnded";
	[Signal]public delegate void CardBattleRoundEndedEventHandler();

	public static string CARD_BATTLE_CARD_DRAWN = "CardBattleCardDrawn";
	[Signal]public delegate void CardBattleCardDrawnEventHandler(string handId);
	#endregion

	private static SignalBus _instance = null;
	public static SignalBus Instance
	{
		get
		{
			if (_instance == null) { _instance = new SignalBus(); }
			
			return _instance;
		}
	}
}
