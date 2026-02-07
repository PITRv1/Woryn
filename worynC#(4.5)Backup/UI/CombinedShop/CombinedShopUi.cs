using Godot;
using System;

public partial class CombinedShopUi : Control
{
	[Export] GoldConverterUi goldConverterUi;
	[Export] PublicShop publicShop;
	[Export] PrivateShop privateShop;

	public override void _Ready()
	{
		goldConverterUi.GoldTimerTimeout += () =>
		{	
			goldConverterUi.Visible = false;
			publicShop.StartPublicShop();
		};

		publicShop.PublicShopTimerTimeout += () =>
		{
			publicShop.Visible = false;
			privateShop.StartPrivateShop();
		};

		goldConverterUi.StartGoldUI();
	}

}
