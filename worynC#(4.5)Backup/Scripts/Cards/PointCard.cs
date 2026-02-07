using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

public partial class PointCard : Node, CardInterface
{
	public string CardName { get; private set; }
	public int PointValue { get; private set; }
	public CardRaritiesEnum CardRarity { get; set; }
	public List<ModifierCard> ModifierList;
	public PointCard(string cardName, int pointValue, CardRaritiesEnum cardRarity)
	{
		CardName = cardName;
		PointValue = pointValue;
		CardRarity = cardRarity;
		ModifierList = new List<ModifierCard>();
	}

	public PointCard(int pointValue)
	{
		PointValue = pointValue;
		SetCardRarity(PointValue);
		ModifierList = new List<ModifierCard>();
	}

	public PointCard()
	{
		
	}

	private void SetCardRarity(int num)
	{
		switch (num)
		{
			case 1:
				CardRarity = CardRaritiesEnum.LEGENDARY;
				return;
			case 2:
			case 3:
				CardRarity = CardRaritiesEnum.RARE;
				return;
			default:
				CardRarity = CardRaritiesEnum.COMMON;
				return;
		}
	}

	public bool AddModifier(ModifierCard card)
	{
		return false;
	}

	public void RemoveModifier(ModifierCard card)
	{
		
	}
}
