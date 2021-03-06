﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Constants {
	
	//Const Main game
	public const string MAIN_GAME_HIGH_SCORE = "MAIN_GAME_HIGH_SCORE"; 	
	public const string	MAIN_GAME_MAX_LIFE = "MAIN_GAME_MAX_LIFE";
	public const string MAIN_GAME_LVL = "MAIN_GAME_LVL";
	public const string MAIN_GAME_ALREADY_PLAYED = "MAIN_GAME_ALREADY_PLAYED";

	// Propulsion game
	public const string PROPULSION_GAME_MAX_DIFFICULTY = "PROPULSION_GAME_MAX_DIFFICULTY";	
	public const string PROPULSION_GAME_LVL1_UNLOCK = "PROPULSION_GAME_LVL1_UNLOCK";
	public const string PROPULSION_GAME_LVL2_UNLOCK = "PROPULSION_GAME_LVL2_UNLOCK";
	public const string PROPULSION_GAME_LVL3_UNLOCK = "PROPULSION_GAME_LVL3_UNLOCK";
	public const string PROPULSION_GAME_ALREADY_PLAYED = "PROPULSION_GAME_ALREADY_PLAYED";
	// integer used to indicate the more important difficulty already played once
	// (used for tutorial and message at the beginning of each level)
	public const string PROPULSION_GAME_MAX_PLAYED = "PROPULSION_GAME_MAX_PLAYED";
	// integer set to the maximum level succeed once (needed in order to manage last level message...)
	public const string PROPULSION_GAME_MAX_WON = "PROPULSION_GAME_MAX_WON";


	// Paint game
	public const string PAINT_GAME_LVL1_UNLOCK = "PAINT_GAME_LVL1_UNLOCK";
	public const string PAINT_GAME_LVL2_UNLOCK = "PAINT_GAME_LVL2_UNLOCK";
	public const string PAINT_GAME_LVL3_UNLOCK = "PAINT_GAME_LVL3_UNLOCK";
	public const string PAINT_GAME_LVL4_UNLOCK = "PAINT_GAME_LVL4_UNLOCK";
	public const string PAINT_GAME_LVL1_SUCCES = "PAINT_GAME_LVL1_SUCCES";
	public const string PAINT_GAME_LVL2_SUCCES = "PAINT_GAME_LVL2_SUCCES";
	public const string PAINT_GAME_LVL3_SUCCES = "PAINT_GAME_LVL3_SUCCES";
	public const string PAINT_GAME_LVL4_SUCCES = "PAINT_GAME_LVL4_SUCCES";
	public const string PAINT_GAME_ALREADY_PLAYED = "PAINT_GAME_ALREADY_PLAYED";

	// Mecano game
	public const string MECANO_GAME_LVL1_UNLOCK = "MECANO_GAME_LVL1_UNLOCK";
	public const string MECANO_GAME_LVL2_UNLOCK = "MECANO_GAME_LVL2_UNLOCK";
	public const string MECANO_GAME_LVL3_UNLOCK = "MECANO_GAME_LVL3_UNLOCK";
	public const string MECANO_GAME_LVL4_UNLOCK = "MECANO_GAME_LVL4_UNLOCK";
	public const string MECANO_GAME_LVL1_SUCCES = "MECANO_GAME_LVL1_SUCCES";
	public const string MECANO_GAME_LVL2_SUCCES = "MECANO_GAME_LVL2_SUCCES";
	public const string MECANO_GAME_LVL3_SUCCES = "MECANO_GAME_LVL3_SUCCES";
	public const string MECANO_GAME_ALREADY_PLAYED1 = "MECANO_GAME_ALREADY_PLAYED1";
	public const string MECANO_GAME_ALREADY_PLAYED2 = "MECANO_GAME_ALREADY_PLAYED2";
	public const string MECANO_GAME_ALREADY_PLAYED3 = "MECANO_GAME_ALREADY_PLAYED3";
}
