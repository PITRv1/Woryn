using Godot;
using System;

public partial class ModifierCardNextPlayer : ModifierCard
{
	public MODIFIER_TYPES ModifierType => MODIFIER_TYPES.GIVE_DECK_AROUND;
	public int Amount { get; set; } = 2;

    public string CardName { get; } = "DeckAround";

    public bool IsCardModifier { get; } = false;

    public int TurnsUntilActivation { get; set; } = 0;

    public void ActivateEffect()
    {
        throw new NotImplementedException();
    }

    public void ApplyDeckModifier(PlayPile playPile)
    {
        throw new NotImplementedException();
    }

	public void RandomizeProperties()
	{
		// RandomNumberGenerator rng = new RandomNumberGenerator();
		// Amount = rng.RandiRange(2, 6);
        // // Amount = 1;
	}

	public int Calculate(int value)
	{
		return value;
	}

    public byte PacketValue()
    {
        throw new NotImplementedException();
    }
}
