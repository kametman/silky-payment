using Godot;
using Shuffled.Managers;
using System;

namespace Shuffled.Scenes;

public partial class CardBattle : Node2D
{
	[Export]private BattleHand[] _battleHands;
	[Export]private Timer _battleTimer;
	[Export]private Label _deckCountLabel;
	[Export]private Sprite2D _clockSprite;
	
	private Guid _deckId = Guid.NewGuid();
	private int _clockTicks = 0;

	public override void _Ready()
	{
		DealNewHands();

		SignalBus.Instance.CardBattleRoundEnded += () => ResolveCombatRound();
		SignalBus.Instance.CardBattleCardDrawn += (handId) => DealCardToHand(handId);

		_battleTimer.Start();
	}

	public void OnBattleTimerTimeout()
	{
		_clockTicks = (_clockTicks + 1) % 5;
		if (_clockTicks == 0) 
		{
			SignalBus.Instance.EmitSignal(SignalBus.CARD_BATTLE_ROUND_ENDED);
		}

		_clockSprite.Frame = _clockTicks;
	}

	private void ResolveCombatRound()
	{
		DealNewHands();
	}

	private void DealNewHands()
	{
		for (var i = 0; i < _battleHands.Length; i++) { _battleHands[i].ClearHand(); }
		CardManager.Instance.ResetDeck(_deckId);

		var startingCards = CardManager.Instance.GetCardsFromDeck(_deckId, _battleHands.Length * 2);
		var cardCount = 0;

		for (var i = 0; i < 2; i++)
		{
			for (var j = 0; j < _battleHands.Length; j++)
			{
				_battleHands[j].PlaceCardInHand(startingCards[cardCount++]);
			}
		}

		_deckCountLabel.Text = $"{CardManager.Instance.GetCardCountFromDeck(_deckId)}";
	}

	private void DealCardToHand(string handId)
	{
		var handToDraw = Array.Find(_battleHands, x => x.HandID == handId);
		if (handToDraw == null) { return; }

		var newCard = CardManager.Instance.GetCardFromDeck(_deckId);
		handToDraw.PlaceCardInHand(newCard);

		_deckCountLabel.Text = $"{CardManager.Instance.GetCardCountFromDeck(_deckId)}";
	}
}
