using Godot;
using System;
using System.Globalization;
using VastVoid.Managers;

namespace VastVoid.Components;

[Tool]
public partial class CardHand : Node2D
{
	[Export]private int[] _cardsInHand;

	private PlayingCard[] _playingCards;
	private Sprite2D _cursorSprite;
	[Export]private int _selectionIndex = -1;

	public override void _Ready()
	{
		InitializeSubcomponents();
		InitializePlayingCards();

		SignalBus.Instance.PlayingCardClicked += (cardIndex) => SetCursor(cardIndex);
	}

	public override void _Process(double delta)
	{
		for (var i = 0; i < _playingCards.Length; i++)
		{
			var cardValue = _cardsInHand[i];
			if (cardValue < 0)
			{
				_playingCards[i].Visible = false;
			}
			else
			{
				_playingCards[i].SetCardValue(cardValue);
				_playingCards[i].Visible = true;
			}
		}

		if (_selectionIndex < 0)
		{
			_cursorSprite.Visible = false;
		}
		else
		{
			_cursorSprite.Position = _playingCards[_selectionIndex].Position;
			_cursorSprite.Visible = true;
		}
	}

	private void InitializeSubcomponents()
	{
		var cardsNode = GetNode<Node2D>("PlayingCards");
		_playingCards = new PlayingCard[cardsNode.GetChildCount()];
		for (var i = 0; i < _playingCards.Length; i++)
		{
			_playingCards[i] = cardsNode.GetChild<PlayingCard>(i);
			_playingCards[i].CardIndex = i;
		}

		_cursorSprite = GetNode<Sprite2D>("CursorSprite");
	}

	private void InitializePlayingCards()
	{
		_cardsInHand = new int[5];
		GetNewHand();
	}

	public void ShowCards()
	{
		ToggleCardState(false);
	}
	
	public void HideCards()
	{
		ToggleCardState(true);
	}

	private void ToggleCardState(bool faceDown)
	{
		for (var i = 0; i < _playingCards.Length; i++)
		{
			_playingCards[i].FlipCard(faceDown);
		}
	}

	public void GetNewHand()
	{
		for (var i = 0; i < _cardsInHand.Length; i++) { _cardsInHand[i] = (int)(GD.Randi() % 52); }
		_selectionIndex = -1;
	}

	private void SetCursor(int cardIndex)
	{
		_selectionIndex = _selectionIndex == cardIndex ? -1 : cardIndex;
	}
}
