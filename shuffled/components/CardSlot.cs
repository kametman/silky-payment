using Godot;
using Shuffled.Managers;

namespace Shuffled.Components;

public partial class CardSlot : Area2D
{
	[Export]private bool _hideSlotSprite = false;

	public bool HasCard
	{
		get { return _currentCard != null; }
	}
	public int CardValue
	{
		get { return _currentCard?.CardValue ?? -1; }
	}

	[Export] private PlayingCard _currentCard = null;

	private Sprite2D _slotSprite;

	public override void _Ready()
	{
		_slotSprite = GetNode<Sprite2D>("SlotSprite");

		_slotSprite.Visible = !_hideSlotSprite;
	}

	public PlayingCard SetCard(PlayingCard newCard)
	{
		PlayingCard displacedCard = _currentCard;
		if (displacedCard != null)
		{
			RemoveChild(displacedCard);
		}

		_currentCard = newCard;
		if (newCard != null) { AddChild(newCard); }
		
		return displacedCard;
	}

	public void FlipCard(bool? isFaceDown = null)
	{
		if (!HasCard) { return; }

		if (!isFaceDown.HasValue) { _currentCard.IsFaceDown = !_currentCard.IsFaceDown; }
		else { _currentCard.IsFaceDown = isFaceDown.Value; }
	}

	public void OnCardSlotInputEvent(Node viewport, InputEvent @event, int shapeIndex)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left)
			{
				SignalBus.Instance.EmitSignal(SignalBus.CARD_SLOT_CLICKED, Name);
			}
		}
	}
	public void OnCardSlotMouseEntered()
	{
		SignalBus.Instance.EmitSignal(SignalBus.CARD_SLOT_ENTERED, Name);
	}
	public void OnCardSlotMouseExited()
	{
		SignalBus.Instance.EmitSignal(SignalBus.CARD_SLOT_EXITED, Name);
	}
}
