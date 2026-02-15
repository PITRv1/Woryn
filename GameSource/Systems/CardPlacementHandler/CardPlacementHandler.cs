using Godot;
using System;

public partial class CardPlacementHandler : Node3D
{
	[Export] public float gapAmount = -0.25f;
	[Export] public Vector3 cardDisplayDirection = new Vector3(0,1,1);
	[Export] public bool curveCards = false;
	[Export] public float rotationStart = 10.0f;
	[Export] public float rotationAmount = -5.0f;
	[Export] public float angularRange = 180.0f;
	[Export] public float gapDecreasion = .75f;
	[Export] private bool _isShopHolder = false;

	public Godot.Collections.Array<Node3D> CardArray { get; private set; } = new();
	[Export] private UiCommunicator _uiCommunicator;

	public void AddCard(Node3D card)
	{
		CardArray.Add(card);
		
		AddChild(card);
		ReorganizeCards();
	}

	public void RemoveCard(Node3D card, bool reorginize = true)
	{
		CardArray.Remove(card);
		RemoveChild(card);
		card.QueueFree();

		if (reorginize)
		{
			ReorganizeCards();
		}
	}

	private void ReorganizeCards()
	{
		foreach (Node3D node in GetChildren())
		{
			node.GlobalPosition = GlobalPosition;
			node.GlobalRotation *= new Vector3(1,1,0);
		}

		var nextPos = 0f;
		var nextRot = 0f;
		var amount = 0;

		foreach (var node in CardArray)
		{
			if (curveCards) {
				var cardIndex = CardArray.IndexOf(node);

				node.RotateZ(Mathf.DegToRad(rotationStart + nextRot * cardIndex));
				var calculatedPosition = GlobalPosition + new Vector3(0, (float)Math.Abs(Math.Sin(angularRange / CardArray.Count * cardIndex)) * 0.1f, nextPos);
				node.GlobalPosition = calculatedPosition;

				nextPos += gapAmount * gapDecreasion;
				nextRot += rotationAmount;

			} else
			{
				var yValue = 0f;
				if (_isShopHolder)
				{
					yValue = amount > 3 ? -0.15f : 0.65f;
				}
				var calculatedPosition = GlobalPosition + new Vector3(nextPos, yValue, nextPos) * cardDisplayDirection;
				node.GlobalPosition = calculatedPosition;

				nextPos += gapAmount;
				if (amount == 3)
				{
					nextPos = 0f;
				}
				amount++;
			}
		}
	}
}
