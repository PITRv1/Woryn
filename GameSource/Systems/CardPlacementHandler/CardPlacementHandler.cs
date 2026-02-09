using Godot;
using System;

public partial class CardPlacementHandler : Node3D
{
	[Export] public float gapAmount = -0.25f;
	[Export] public bool curveCards = true;

	public Godot.Collections.Array<Node3D> cardArray { get; private set; } = new();
	[Export] UiCommunicator UiCommunicator;

	public void AddCard(Node3D card)
	{
		if (card is PointCard3d pointCard) pointCard.UiCommunicator = UiCommunicator;
		else if (card is ModifierCard3d modifierCard) modifierCard.UiCommunicator = UiCommunicator;

		cardArray.Add(card);

		AddChild(card);
		ReorganizeCards();
	}

	public void RemoveCard(Node3D card)
	{
		cardArray.Remove(card);
		card.QueueFree();

		ReorganizeCards();
	}

	public void ReorganizeCards()
	{
		foreach (Node3D node in GetChildren())
		{
			node.GlobalPosition = GlobalPosition;
		}

		float nextPos = 0f;

		foreach (Node3D node in cardArray)
		{
			node.GlobalPosition = GlobalPosition + new Vector3(0, 0, nextPos);
			nextPos += gapAmount;
		}
	}
}
