using Godot;
using System;

public partial class CardPlacementHandler : Node3D
{
	[Export] public float GapAmount = -0.25f;
	[Export] public bool CurveCards = true;

	public Godot.Collections.Array<Node3D> CardArray { get; private set; } = new();
	[Export] private UiCommunicator _uiCommunicator;

	public void AddCard(Node3D card)
	{
		if (card is PointCard3d pointCard) pointCard.UiCommunicator = _uiCommunicator;
		else if (card is ModifierCard3d modifierCard) modifierCard.UiCommunicator = _uiCommunicator;

		CardArray.Add(card);
		
		AddChild(card);
		ReorganizeCards();
	}

	public void RemoveCard(Node3D card)
	{
		CardArray.Remove(card);
		card.QueueFree();

		ReorganizeCards();
	}

	private void ReorganizeCards()
	{
		foreach (Node3D node in GetChildren())
		{
			node.GlobalPosition = GlobalPosition;
		}

		var nextPos = 0f;

		foreach (var node in CardArray)
		{
			node.GlobalPosition = GlobalPosition + new Vector3(0, 0, nextPos);
			nextPos += GapAmount;
		}
	}
}
