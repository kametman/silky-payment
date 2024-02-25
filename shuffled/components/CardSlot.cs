using Godot;
using Shuffled.Managers;

namespace Shuffled.Components;

public partial class CardSlot : Area2D
{
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
