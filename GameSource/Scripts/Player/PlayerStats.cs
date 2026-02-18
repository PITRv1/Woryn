using Godot;
using System;

public class PlayerStats
{
	public int MaxLevel = 3;
	public bool MaxLevelCap = true;
	
	// Noble/Politician
	public float PoliticanPassive = 0.00f;
	private int _politicianLevel = 0;
	private const float PoliticianBaseValue = 0.04f;
	private const float PoliticianAddition = 0.02f;

	public void UpgradePoliticianLevel()
	{
		GD.Print("Upgrade PoliticianLevel");
		PoliticanPassive = PoliticianBaseValue + PoliticianAddition * _politicianLevel;
		if (_politicianLevel < MaxLevel || !MaxLevelCap)
		{
			_politicianLevel++;
		}
	}
	
	// Gambler
	public float GamblerPassive = 0f;
	private int _gamblerLevel = 0;
	private const float GamblerAddition = 0.15f;
	private const float GamblerBaseValue = 0.10f;
	
	public void UpgradeGamblerLevel()
	{
		GD.Print("Upgrade UpgradeGamblerLevel");
		GamblerPassive = GamblerBaseValue + GamblerAddition * _gamblerLevel;
		if (_gamblerLevel < MaxLevel || !MaxLevelCap)
		{
			_gamblerLevel++;
		}
	}
	
	// Alchemist/transmutator
	public float PointsToGoldRatio = 0.25f;
	private int _alchemistLevel = 0;
	private const float AlchemistAddition = 0.25f;
	private const float AlchemistBaseValue = 0.25f;

	public void UpgradeAlchemistLevel()
	{
		GD.Print("Upgrade UpgradeAlchemistLevel");
		PointsToGoldRatio = AlchemistBaseValue + AlchemistAddition * _alchemistLevel;
		if (_alchemistLevel < MaxLevel || !MaxLevelCap)
		{
			_alchemistLevel++;
		}
	}
	
	// Mirror Maiden
	public int MaidenPassive = 0;
	private int _maidenLevel = 0;
	private const int MaidenAddition = 1;
	private const int MaidenBaseValue = 0;

	public void UpgradeMaidenLevel()
	{
		GD.Print("Upgrade UpgradeMaidenLevel");
		MaidenPassive = _maidenLevel + MaidenAddition * _maidenLevel;
		if (_maidenLevel < MaxLevel || !MaxLevelCap)
		{
			_maidenLevel++;
		}
	}
	
	// Clairvoyant
	public int ClairvoyantPassive = 0;
	private int _clairvoyantLevel = 0;
	private const int ClairvoyantAddition = 1;
	private const int ClairvoyantBaseValue = 1;

	public void UpgradeClairvoyantLevel()
	{
		GD.Print("Upgrade UpgradeClairvoyantLevel");
		ClairvoyantPassive = ClairvoyantBaseValue + ClairvoyantAddition * _clairvoyantLevel;
		if (_clairvoyantLevel < MaxLevel || !MaxLevelCap)
		{
			_clairvoyantLevel++;
		}
	}
	
	// Drunkard
	public int DrunkardPassive = 0;
	private int _drunkardLevel = 0;
	private const int DrunkardAddition = 1;
	private const int DrunkardBaseValue = 1;
	public void UpgradeDrunkardLevel()
	{
		GD.Print("Upgrade UpgradeDrunkardLevel");
		DrunkardPassive = DrunkardBaseValue + DrunkardAddition * _drunkardLevel;
		if (_drunkardLevel < MaxLevel || !MaxLevelCap)
		{
			_drunkardLevel++;
		}
	}

}
