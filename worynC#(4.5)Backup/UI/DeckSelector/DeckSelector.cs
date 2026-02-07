using Godot;
using System;
using System.Collections.Generic;

public partial class DeckSelector : Control
{
	[Export] Button mDeck1;
	[Export] Button mDeck2;
	[Export] Button mDeck3;
	[Export] Button mDeck4;

    [Export] HBoxContainer container;
    [Export] Button startGameButton;
    [Export] Label title;
    [Export] Control playerScene;
    [Export] Control deckselectScene;

    List<Button> mDecks = new List<Button>();

    bool selected = false;
    Button selectedMDeck;

    public override void _Ready()
    {
        mDecks.Add(mDeck1);
        mDecks.Add(mDeck2);
        mDecks.Add(mDeck3);
        mDecks.Add(mDeck4);
    }
    public override void _Process(double delta)
    {
        if (selected)
        {
            foreach (Button mDeck in mDecks)
            {
                if(mDeck != selectedMDeck)
                {
                    mDeck.QueueFree();
                    selected = false;
                }
            }
            selectedMDeck.Disabled = true;
            startGameButton.Visible = true;
            title.Text = "DECK SELECTED";
        }
    }
    private void OnStartGamePressed()
    {
        deckselectScene.Visible = false;
        playerScene.Visible = true;
    }
	private void OnMDeck1Pressed()
	{
        selectedMDeck = mDeck1;
        selected = true;
	}
    private void OnMDeck2Pressed()
    {
        selectedMDeck = mDeck2;
        selected = true;
    }
    private void OnMDeck3Pressed()
    {
        selectedMDeck = mDeck3;
        selected = true;
    }
    private void OnMDeck4Pressed()
    {
        selectedMDeck = mDeck4;
        selected = true;
    }
}
