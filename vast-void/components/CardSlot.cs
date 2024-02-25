using Godot;
using System;

public partial class CardSlot : Node2D
{
	[Export]private AnimatedSprite2D _cardSlotCursor;

	public override void _Ready()
	{
		_cardSlotCursor.Play();
	}

	public override void _Process(double delta)
	{
	}
}
