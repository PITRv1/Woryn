using Godot;
using System;
using System.Collections.Generic;

public partial class TurnData : PacketInfo
{
    public List<ModifierCard> ModifierCardList;
    public int SenderId;
    public int PointCardValue;
    public static TurnData Create(int id, byte value, List<ModifierCard> modifierCards)
    {
        TurnData info = new TurnData();

        info.PacketType = PACKET_TYPES.TURN_DATA;
        info.SenderId = id;
        info.PointCardValue = value;
        info.ModifierCardList = modifierCards;

        return info;
    }

    public static TurnData CreateFromData(byte[] data)
    {
        TurnData info = new TurnData();
        info.Decode(data);
        return info;
    }

    public new byte[] Encode()
    {
        // 1 byte: packet type
        // 1 byte: id
        // 1 bte: value
        // N bytes: modif card + value pairs

        byte[] baseData = base.Encode();
        byte[] data = new byte[2 + ModifierCardList.Count * 2]; // *2 because we need the type plus the value of the action or the turns until activation.

        data[0] = baseData[0];
        data[1] = (byte)SenderId;

        for (int i = 0; i < ModifierCardList.Count; i *= 2)
        {
            data[2 + i] = (byte)ModifierCardList[i].ModifierType;
            data[2 + i + 1] = ModifierCardList[i].PacketValue();
        }
            
        return data;
    }

    public new void Decode(byte[] data)
    {
        base.Decode(data);

        SenderId = data[1];
        ModifierCardList.Clear();

        for (int i = 0; i < ModifierCardList.Count; i *= 2)
        {
            MODIFIER_TYPES type = (MODIFIER_TYPES)data[2 + i];
            byte value = data[2 + i + 1];

            ModifierCardList.Add(InstantiateClassByEnum(type, value));
        }  
    }

    public new void Broadcast(ENetConnection server)
    {
        server.Broadcast(0, Encode(), (int)Flag);
    }

    public ModifierCard InstantiateClassByEnum(MODIFIER_TYPES type, byte value)
    {
        ModifierCard card = null;

        switch (type)
        {
            case MODIFIER_TYPES.MULTIPLIER:
                ModifierCardMultiplier _card = new();
                // _card.Amount = value;

                card = _card;
                break;
        }

        return card;
    }
}
