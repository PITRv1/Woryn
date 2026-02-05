using Godot;
using System;

public partial class TestModifierCardUi : Control
{
	[Export] RichTextLabel text;
	[Export] NinePatchRect ninePatchRect;

	ModifierCard _modifierCard;
	public PlayerClass playerClass;
	public bool isPreview = false;

	public ModifierCard modifierCard
	{
		get {return _modifierCard;}
		set
		{
			_modifierCard = value;
			EditText();
		}
	}

	public void EditText()
	{
		switch (modifierCard.ModifierType)
		{
			case MODIFIER_TYPES.MULTIPLIER:
				text.Text = $"[wave freq=1][rainbow freq=0.1]*{(modifierCard as ModifierCardMultiplier).Amount}[/rainbow][/wave]";
				break;
			case MODIFIER_TYPES.ADDITION:
				text.Text = $"[wave freq=1][rainbow freq=0.1]+{(modifierCard as ModifierCardAddition).Amount}[/rainbow][/wave]";
				break;
			case MODIFIER_TYPES.SKIP:
				text.Text = "[wave freq=1][rainbow freq=0.1]Skip[/rainbow][/wave]";
				break;
			case MODIFIER_TYPES.REVERSE:
				text.Text = "[wave freq=1][rainbow freq=0.1]Reverse[/rainbow][/wave]";
				break;
			case MODIFIER_TYPES.CHANGE_DECK:
				text.Text = "[wave freq=1][rainbow freq=0.1]Change[/rainbow][/wave]";
				break;
			case MODIFIER_TYPES.GIVE_DECK_AROUND:
				text.Text = "[wave freq=1][rainbow freq=0.1]Around[/rainbow][/wave]";
				break;
		}
	}

	public void HandleSelection()
	{
		
		if (playerClass.chosenModifierCards.Contains(modifierCard))
		{
			playerClass.chosenModifierCards.Remove(modifierCard);
			RemoveCard();
		} else
		{
			if (playerClass.AddToChosenModifierCards(modifierCard)) 
			SelectCard();
		}
	}

	public void RemoveCard()
	{
		ninePatchRect.Modulate = Colors.White;
	}

	public void SelectCard()
	{
		ninePatchRect.Modulate = Color.FromString("#ffffad", Colors.Purple);
	}

}
