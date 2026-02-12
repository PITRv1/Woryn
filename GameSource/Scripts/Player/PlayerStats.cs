using Godot;
using System;

public class PlayerStats
{
	// Noble/Politician
	public float PoliticanPassive = 0.00f;
	private int _politicanLevel = 0;
	private float _politicanBaseValue = 0.04f;
	private float _politicanAddition = 0.02f;
	
	// Gambler
	public float GamblerPassive = 0f;
	private int _gamblerLevel = 0;
	private float _gamblerAddition = 0.15f;
	private float _gamblerBaseValue = 0.10f;
	
	// Alchemist/transmutator
	public float PointsToGoldRatio = 0.25f;
	private int _alchemistLevel = 0;
	private float _alchemistAddition = 0.25f;
	private float _alchemistBaseValue = 0.25f;
	
	// Mirror Maiden
	// public float 
	
}
