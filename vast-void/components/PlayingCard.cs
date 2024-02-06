using Godot;
using System;
using VastVoid.Managers;

namespace VastVoid.Components;

[Tool]
public partial class PlayingCard : Node2D
{
	public int CardIndex { get; set; }

	[Export]private bool _faceDown = true;
	[Export]private int _cardValue;

	private Sprite2D _cardFront;
	private Sprite2D _cardBack;
	private CollisionShape2D _coll2D;

	public override void _Ready()
	{
		_cardFront = GetNode<Sprite2D>("CardFrontSprite");
		_cardBack = GetNode<Sprite2D>("CardBackSprite");
		_coll2D = GetNode<CollisionShape2D>("CardCollision2D");
	}

	public override void _Process(double delta)
	{
		_cardFront.Visible = !_faceDown;
		_cardBack.Visible = _faceDown;
		_cardFront.Frame = _cardValue % 52;
		_coll2D.Disabled = _faceDown;
	}

	public void FlipCard(bool? faceDown = null)
	{
		if (!faceDown.HasValue) { _faceDown = !_faceDown; }
		else { _faceDown = faceDown.Value; }
	}

	public void SetCardValue(int cardValue)
	{
		_cardValue = cardValue;
	}

	public void OnPlayingCardInputEvent(Node viewport, InputEvent @event, int shape_idx)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
			{
				SignalBus.Instance.EmitSignal(SignalBus.PLAYING_CARD_CLICKED, CardIndex);
			}
		}
	}
}
