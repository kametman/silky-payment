using Godot;

namespace Shuffled.Managers;

public partial class SignalBus : Node
{
	public static string CARD_SLOT_CLICKED = "CardSlotClicked";
	[Signal]public delegate void CardSlotClickedEventHandler(string cardSlotName);

	public static string CARD_SLOT_ENTERED = "CardSlotEntered";
	[Signal]public delegate void CardSlotEnteredEventHandler(string cardSlotName);

	public static string CARD_SLOT_EXITED = "CardSlotExited";
	[Signal]public delegate void CardSlotExitedEventHandler(string cardSlotName);

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
