using Godot;

namespace VastVoid.Managers;

public partial class SignalBus : Node
{
	#region clock events
	public static readonly string CLOCK_SHORT_TICKED = "ClockShortTicked";
	[Signal]public delegate void ClockShortTickedEventHandler();
	
	public static readonly string CLOCK_LONG_TICKED = "ClockLongTicked";
	[Signal]public delegate void ClockLongTickedEventHandler();
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
