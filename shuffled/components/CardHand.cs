using Godot;
using Shuffled.Managers;
using System;

namespace Shuffled.Components;

public partial class CardHand : Node2D
{
	[Export] private string[] _slotNames = new string[]
	{
		"CardSlotA", "CardSlotB", "CardSlotC", "CardSlotD", "CardSlotE", 
	};

	private Sprite2D _selectionSprite;
	private Label _cardDescriptionLabel;

	private CardSlot[] _cardSlots;

	private Guid _handId = Guid.NewGuid();
	private int _selectedIndex = -1;

	public override void _Ready()
	{
		_selectionSprite = GetNode<Sprite2D>("SelectionSprite");
		_cardDescriptionLabel = GetNode<Label>("CardDescriptionLabel");

		InitializeCardSlots();

		SignalBus.Instance.CardSlotClicked += (slotName) => CardSlotClicked(slotName);
		SignalBus.Instance.CardSlotEntered += (slotName) => CardSlotEntered(slotName);
		SignalBus.Instance.CardSlotExited += (slotName) => CardSlotExited(slotName);
	}

	private void InitializeCardSlots()
	{
		_cardSlots = new CardSlot[5];
		for (var i = 0; i < _cardSlots.Length; i++)
		{
			_cardSlots[i] = GetNode<CardSlot>(_slotNames[i]);
			_cardSlots[i].Name = $"{_cardSlots[i].Name}_{_handId}";
			_slotNames[i] = _cardSlots[i].Name;

			var newCard = CardFactory.Instance.GetRandomCard();
			_cardSlots[i].SetCard(newCard);
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

		_cardDescriptionLabel.Text = CardFactory.Instance.GetDescriptionFromValue(_cardSlots[slotIndex].CardValue);
	}
	private void CardSlotExited(string slotName)
	{
		var slotIndex = Array.IndexOf(_slotNames, slotName);
		if (slotIndex < 0) { return; }
		if (!_cardSlots[slotIndex].HasCard) { return; }

		_cardDescriptionLabel.Text = "";
	}
}
