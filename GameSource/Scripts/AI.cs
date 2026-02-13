using Godot;
using System;

public class AI : Node
{
    [Export] MultiplayerPlayerClass multiplayerPlayer;
    public PointCard LowestPointCard()
    {
        PointCard smallestCard = multiplayerPlayer.PlayerClass.PointCardList[0]; 
        foreach (PointCard card in multiplayerPlayer.PlayerClass.PointCardList)
        {
            if (smallestCard.PointValue>card.PointValue)
            {
                smallestCard=card;
            }
        }
        return smallestCard;
    }
}