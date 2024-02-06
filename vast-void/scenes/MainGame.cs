using Godot;
using System;
using VastVoid.Components;
using VastVoid.Managers;

public partial class MainGame : Node2D
{
	[ExportGroup("InternalComponents")]
	[Export]private MainGameUI _uiNode;
	[Export]private CardHand _cardHand;

	[ExportGroup("ClockSettings")]
	[Export]private float _tickInterval = 0.5f;
	[Export]private ulong _tickEventCount = 10;

	private IdleClock _idleClock;

	public override void _Ready()
	{
		InitializeIdleClock();

		_cardHand.ShowCards();
	}

	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }

    private void InitializeIdleClock()
	{
		_idleClock = GetNode<IdleClock>("/root/IdleClock");
		_idleClock.PauseClock();
		_idleClock.SetClockParams(_tickInterval, _tickEventCount);

		SignalBus.Instance.ClockLongTicked += () => GetNewHand();

		_idleClock.StartClock();
	}

	private void StartGame()
	{

	}

	public void GetNewHand()
	{
		_cardHand.GetNewHand();
	}
}
