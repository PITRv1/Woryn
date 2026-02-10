using Godot;
using System;

public partial class CardPlacementHandler : Node3D
{
	[Export] public float GapAmount = -0.25f;
	[Export] public bool CurveCards = true;
	[Export] public float rotationStart = 10.0f;
	[Export] public float rotationAmount = -5.0f;

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
		var nextRot = 0f;

		foreach (var node in CardArray)
		{
			if (CurveCards) {
				node.RotateZ(Mathf.DegToRad(rotationStart + nextRot * CardArray.IndexOf(node)));

				node.GlobalPosition = GlobalPosition + new Vector3(0, (float)Math.Abs(Math.Sin(180f / CardArray.Count * CardArray.IndexOf(node))) * 0.1f, nextPos);

				nextPos += GapAmount * .75f;
				nextRot += rotationAmount;
			} else
			{
				node.GlobalPosition = GlobalPosition + new Vector3(0, 0, nextPos);
				nextPos += GapAmount;
			}
		}
	}
}
