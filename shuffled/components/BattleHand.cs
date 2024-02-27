using Godot;
using Shuffled.Components;
using Shuffled.Managers;
using System;

public partial class BattleHand : Area2D
{
	private string[] _slotNames = new string[]
	{
		"CardSlotA", "CardSlotB", "CardSlotC", "CardSlotD", "CardSlotE", 
	};

	private readonly string _handId = Guid.NewGuid().ToString();
	public string HandID { get { return _handId; } }

	private int _handScore = 0;
	public int HandScore 
	{ 
		get {return _handScore; } 
		set 
		{
			_handScore = value;
			_handScoreLabel.Text = $"{value}";
			CalculateBonusDamage();
		}
	}

	private int _bonusDamage;
	public int BonusDamage { get { return _bonusDamage; } } 

	private int _penaltyDamage;
	public int PenaltyDamage { get { return _penaltyDamage; } }

	private int _cardsInHand = 0;
	private CardSlot[] _cardSlots;
	private Label _handScoreLabel;
	private Label _bonusDamageLabel;
	private Label _penaltyDamageLabel;
	private CollisionShape2D _handScoreCollision;

	public override void _Ready()
	{
		_cardSlots = new CardSlot[_slotNames.Length];
		for (var i = 0; i < _cardSlots.Length; i++)
		{
			_cardSlots[i] = GetNode<CardSlot>(_slotNames[i]);
			_cardSlots[i].Name = $"{_cardSlots[i].Name}_{_handId}";
			_slotNames[i] = _cardSlots[i].Name;
		}

		_handScoreLabel = GetNode<Label>("HandScoreLabel");
		_bonusDamageLabel = GetNode<Label>("BonusDamageLabel");
		_penaltyDamageLabel = GetNode<Label>("PenaltyDamageLabel");
		_handScoreCollision = GetNode<CollisionShape2D>("HandScoreCollision");

		_handScoreLabel.Text = $"{_handScore}";
	}

	public void OnBattleHandInputEvent(Node viewport, InputEvent @event, int shapeIndex)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.IsPressed())
			{
				if (_cardsInHand < 5 && _handScore < 21)
				{
					SignalBus.Instance.EmitSignal(SignalBus.CARD_BATTLE_CARD_DRAWN, _handId);
				}
			}
		}
	}

	public bool PlaceCardInHand(PlayingCard handCard)
	{
		if (_cardsInHand == 5) { return false; }

		_cardSlots[_cardsInHand++].SetCard(handCard);
		CalculateHandValue();
		return true;
	}
	
	public void ClearHand()
	{
		HandScore = 0;
		for (var i = 0; i < _cardSlots.Length; i++)
		{
			if (!_cardSlots[i].HasCard) { continue; }

			var card = _cardSlots[i].SetCard(null);
			card.QueueFree();
			_cardsInHand--;
		}
	}

	public void RevealCards()
	{
		for (var i = 0; i < _cardSlots.Length; i++)
		{
			_cardSlots[i].FlipCard(false);
		}
	}

	private void CalculateHandValue()
	{
		HandScore = 0;
		var hasAce = false;

		for (var i = 0; i < _cardsInHand; i++)
		{
			var cardValue = _cardSlots[i].CardValue;
			if (cardValue < 0) { continue; }
			var rankValue = CardManager.Instance.GetRankValue(cardValue);
			if (rankValue == 1) { hasAce = true; }
			HandScore += Mathf.Clamp(rankValue, 1, 10);
		}

		if (hasAce && HandScore <= 11) { HandScore += 10; }
	}

	private void CalculateBonusDamage()
	{
		_penaltyDamage = 0;
		if (_handScore < 17) { _bonusDamage = 0; }
		else if (_handScore == 17) { _bonusDamage = 1; }
		else if (_handScore == 18) { _bonusDamage = 2; }
		else if (_handScore == 19) { _bonusDamage = 3; }
		else if (_handScore == 20) { _bonusDamage = 5; }
		else if (_handScore == 21) 
		{ 
			_bonusDamage = 10; 
			if (_cardsInHand == 2) { _bonusDamage += 5; }
		}
		else { 
			_bonusDamage = 0; 
			_penaltyDamage = _handScore - 21;
		}

		if (_cardsInHand == 5 && _handScore <= 21) { _bonusDamage += 5; }

		_bonusDamageLabel.Text = _bonusDamage > 0 ? $"+{_bonusDamage}" : "";
		_penaltyDamageLabel.Text = _penaltyDamage > 0 ? $"-{_penaltyDamage}" : "";
	}
}
