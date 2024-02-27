using Godot;
using Shuffled.Managers;
using System;

namespace Shuffled.Components;

public partial class CardHand : Node2D
{
	[Export]private bool _fillSlotsOnInit = false;
	[Export]private bool _dealCardsFromDeck = false;

	private string[] _slotNames = new string[]
	{
		"CardSlotA", "CardSlotB", "CardSlotC", "CardSlotD", "CardSlotE", 
	};

	private Sprite2D _selectionSprite;
	private Label _cardDescriptionLabel;
	private Sprite2D _deckSprite;

	private CardSlot[] _cardSlots;

	private Guid _deckId = Guid.Empty;
	private Guid _handId = Guid.NewGuid();
	private int _selectedIndex = -1;

	public override void _Ready()
	{
		_selectionSprite = GetNode<Sprite2D>("SelectionSprite");
		_cardDescriptionLabel = GetNode<Label>("CardDescriptionLabel");
		_deckSprite = GetNode<Sprite2D>("DeckSprite");

		InitializeCardSlots();

		_deckSprite.Visible = _dealCardsFromDeck;

		SignalBus.Instance.CardSlotClicked += (slotName) => CardSlotClicked(slotName);
		SignalBus.Instance.CardSlotEntered += (slotName) => CardSlotEntered(slotName);
		SignalBus.Instance.CardSlotExited += (slotName) => CardSlotExited(slotName);
	}

	private void InitializeCardSlots()
	{
		_cardSlots = new CardSlot[5];
		_deckId = CardManager.Instance.CreateNewDeck();

		var cards = _fillSlotsOnInit
			? CardManager.Instance.GetCardsFromDeck(_deckId, 5)
			: Array.Empty<PlayingCard>();

		for (var i = 0; i < _cardSlots.Length; i++)
		{
			_cardSlots[i] = GetNode<CardSlot>(_slotNames[i]);
			_cardSlots[i].Name = $"{_cardSlots[i].Name}_{_handId}";
			_slotNames[i] = _cardSlots[i].Name;

			if (_fillSlotsOnInit)
			{
				var newCard = cards[i];
				_cardSlots[i].SetCard(newCard);
			}
		}
	}

	private void CardSlotClicked(string slotName)
	{
		var slotIndex = Array.IndexOf(_slotNames, slotName);
		if (slotIndex < 0) { return; }
		if (!_cardSlots[slotIndex].HasCard) { return; }

		if (slotIndex == _selectedIndex)
		{
			_selectedIndex = -1;
			_selectionSprite.Visible = false;
		}
		else
		{
			_selectedIndex = slotIndex;
			_selectionSprite.GlobalPosition = _cardSlots[slotIndex].GlobalPosition;
			_selectionSprite.Visible = true;
		}
	}
	private void CardSlotEntered(string slotName)
	{
		var slotIndex = Array.IndexOf(_slotNames, slotName);
		if (slotIndex < 0) { return; }
		if (!_cardSlots[slotIndex].HasCard) { return; }

		_cardDescriptionLabel.Text = CardManager.Instance.GetDescriptionFromValue(_cardSlots[slotIndex].CardValue);
	}
	private void CardSlotExited(string slotName)
	{
		var slotIndex = Array.IndexOf(_slotNames, slotName);
		if (slotIndex < 0) { return; }
		if (!_cardSlots[slotIndex].HasCard) { return; }

		_cardDescriptionLabel.Text = "";
	}
}
