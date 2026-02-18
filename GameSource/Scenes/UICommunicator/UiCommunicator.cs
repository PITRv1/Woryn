using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class UiCommunicator : Node
{
    [Export] public CardPlacementHandler pointCards {private set; get;}
    [Export] public CardPlacementHandler modifierCards {private set; get;}
    [Export] public CardPlacementHandler shopCards;
    [Export] MultiplayerPlayerClass multiplayerPlayer;
    [Export] PackedScene modifierCard3D;
    [Export] private PackedScene _itemCard;
    [Export] PlayerVisualController playerVisualController;
    [Export] GoldConverterController goldConverterController;

    private List<ModifierCard> _currentPrivateCards = new List<ModifierCard>();
    private List<int> _currentPrivatePrices = new List<int>();
    private List<ItemType> _currentPublicCards = new List<ItemType>();

    public PointCard3d selectedPointCard3D { set; get; }
    public List<ModifierCard3d> selectedModifierCard3Ds { private set; get; } = new();
    public bool PlayerSelectionMode = false;

    public override void _Ready()
    {
        Global.multiplayerClientGlobals.HandleRoundSuccess += HandleRoundSuccess;
        goldConverterController.goldConverterUi.timerObject.Timeout += SendShopReadyPacket;
        Global.multiplayerClientGlobals.ShopItems += HandleShopItems;
        Global.multiplayerClientGlobals.GoToPrivateShop += ShowPrivateShop;
        Global.multiplayerClientGlobals.StopShop += CloseShop;
    }

    private async void HandleShopItems(byte[] data)
    {
        var packet = ShopItems.CreateFromData(data);

        GD.Print("ModifierCards: " + packet.ModifierTypes.Length);

        _currentPublicCards = packet.ItemTypes.ToList();
        _currentPrivateCards = packet.ModifierTypes.Select(ModifierCardTypeConverter.TypeToClass).ToList();
        _currentPrivatePrices = packet.modifierPrices;

        var index = 0;
        multiplayerPlayer._playerHud.StartCountdownTimer(10);
        foreach (var item in _currentPublicCards)
        {
            var itemCard3D = _itemCard.Instantiate<ModifierCard3dShopVersion>();
            itemCard3D.isShopCard = true;
            itemCard3D.ItemType = item;
            itemCard3D.PriceLabel.Text = packet.itemPrices[index].ToString();
            index++;
            shopCards.AddCard(itemCard3D);

            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        }
        
    }

    private async void ShowPrivateShop()
    {
        foreach (Node3D n in shopCards.GetChildren())
        {
            shopCards.RemoveCard(n);
        }
        var index = 0;
        multiplayerPlayer._playerHud.StartCountdownTimer(10);
        foreach (var modifierCard in _currentPrivateCards)
        {
            GD.Print("Modifier CARD: " + modifierCard);
            var modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
            modifierCard3DInstance.isShopCard = true;
            modifierCard3DInstance.ModifierCard = modifierCard;
            modifierCard3DInstance.modifCardPriceLabel.Text = _currentPrivatePrices[index].ToString();
            index++;
            shopCards.AddCard(modifierCard3DInstance);

            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        }
    }

    private void SendShopReadyPacket()
    {
        var packet = new ShopReadyPacket()
        {
            SenderId = Global.multiplayerPlayerClass.Id
        };
        
        Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
    }

    private void HandleRoundSuccess(byte[] data)
    {
        var packet = RoundSuccessPacket.CreateFromData(data);

        if (packet.PlayerId != multiplayerPlayer.Id)
        {
            return;
        }

        RemoveSelectedCard();
    }
 
    public void SelectPointCard(PointCard3d pointCard)
    {
        if (selectedPointCard3D != null) selectedPointCard3D.isSelected = false;
        GD.Print("CARD SELCTED");
        selectedPointCard3D = pointCard;
        selectedPointCard3D.isSelected = true;
    }

    public void DeselectPointCard()
    {
        GD.Print("CARD DESELCTED");
        selectedPointCard3D.isSelected = false;
        selectedPointCard3D = null;

        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            RemoveModifierCard(modifierCard);
        }
    }

    public void AddModifierCard(ModifierCard3d modifierCard)
    {
        if (selectedPointCard3D == null) return;
        
        if (selectedModifierCard3Ds.Count >= (int)selectedPointCard3D.PointCard.CardRarity) return;

        modifierCard.isSelected = true;
        selectedModifierCard3Ds.Add(modifierCard);
    }

    public void RemoveModifierCard(ModifierCard3d modifierCard)
    {
        modifierCard.isSelected = false;
        selectedModifierCard3Ds.Remove(modifierCard);
    }

    public void PlayCards()
    {
        if (PlayerSelectionMode)
        {
            return;
        }
        GD.Print("PlayCard called");
        if (selectedPointCard3D == null)
        {
            GD.Print("selectedPointCard3D is null");
            return;
        }
        
        multiplayerPlayer.PlayerClass.ChosenPointCard = selectedPointCard3D.PointCard;

        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            multiplayerPlayer.PlayerClass.AddToChosenModifierCards(modifierCard.ModifierCard);
        }

        multiplayerPlayer.PlayCard();
    }

    private void RemoveSelectedCard()
    {
        pointCards.RemoveCard(selectedPointCard3D);
        selectedPointCard3D = null;
    
        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            modifierCards.RemoveCard(modifierCard);
        }
        selectedModifierCard3Ds.Clear();
    }

    public void StartShop()
    {
        GD.Print("Shop started!");
        Random random = new();
        goldConverterController.OpenGoldConverter();
        goldConverterController.goldConverterUi.timerObject.Start();
        multiplayerPlayer._playerHud.StopCountdownTimer();

        playerVisualController.moveCamera(1);
    }

    public async void CloseShop()
    {
        foreach (Node3D modifCard in shopCards.GetChildren())
        {
            shopCards.RemoveCard(modifCard);
            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        }

        playerVisualController.moveCamera(0);
    }

    public void AddShopModifierCardToPlayerCards(ModifierCard modifierCard)
    {
        var modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
        modifierCard3DInstance.ModifierCard = modifierCard;

        modifierCards.AddCard(modifierCard3DInstance);
    }
}
