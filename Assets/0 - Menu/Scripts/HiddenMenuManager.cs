using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HiddenMenuManager : MonoBehaviour {

	public ColorBlock lockedColors = ColorBlock.defaultColorBlock;
	
	public ColorBlock unlockedColors = ColorBlock.defaultColorBlock;
	
	public ColorBlock successedColors = ColorBlock.defaultColorBlock;
	
	
	public Transform painter;
	public Transform propulsion;
	public Transform mecano;
	
	private enum GameType { PAINTER, PROPULSION, MECANO };
	
	private enum LevelState { LOCKED, UNLOCKED, SUCCESSED };
	

	// Use this for initialization
	void Start () {
		// initialize button styles, stupid Ctrl-C/Ctrl-V
		initButtonStyle(painter.FindChild("Level1").GetComponent<Button>(), GameType.PAINTER, 1);
		initButtonStyle(painter.FindChild("Level2").GetComponent<Button>(), GameType.PAINTER, 2);
		initButtonStyle(painter.FindChild("Level3").GetComponent<Button>(), GameType.PAINTER, 3);
		
		initButtonStyle(mecano.FindChild("Level1").GetComponent<Button>(), GameType.MECANO, 1);
		initButtonStyle(mecano.FindChild("Level2").GetComponent<Button>(), GameType.MECANO, 2);
		initButtonStyle(mecano.FindChild("Level3").GetComponent<Button>(), GameType.MECANO, 3);
		
		initButtonStyle(propulsion.FindChild("Level1").GetComponent<Button>(), GameType.PROPULSION, 1);
		initButtonStyle(propulsion.FindChild("Level2").GetComponent<Button>(), GameType.PROPULSION, 2);
		initButtonStyle(propulsion.FindChild("Level3").GetComponent<Button>(), GameType.PROPULSION, 3);
	}
	
	public void Menu() {
		Application.LoadLevel("MainMenu");
	}
	
	public void LevelButtonPressed(Button button) {
		// get game type and level from meta-data, really ugly...
		GameType type = GameType.MECANO;
		int level = 1;
		
		if(button.transform.parent == painter)
			type = GameType.PAINTER;
		else if(button.transform.parent == propulsion)
			type = GameType.PROPULSION;
		
		if(button.name == "Level2")
			level = 2;
		else if(button.name == "Level3")
			level = 3;
		
		LevelState curState = getLevelState(type, level);
		switch(curState) {
		case LevelState.LOCKED:
			curState = LevelState.UNLOCKED;
			break;
		case LevelState.UNLOCKED:
			curState = LevelState.SUCCESSED;
			break;
		case LevelState.SUCCESSED:
			curState = LevelState.LOCKED;
			break;
		}
		setLevelState(type, level, curState);
		initButtonStyle(button, type, level);
	}
	
	
	
	
	private LevelState getLevelState(GameType type, int level) {
		switch(type) {
		case GameType.PAINTER:
			if(PlayerPrefs.GetInt("PAINT_GAME_LVL" + level + "_SUCCES", 0) > 0)
				return LevelState.SUCCESSED;
			else if(PlayerPrefs.GetInt("PAINT_GAME_LVL" + level + "_UNLOCK", 0) > 0)
				return LevelState.UNLOCKED;
			break;
		
		case GameType.MECANO:
			if(PlayerPrefs.GetInt("MECANO_GAME_LVL" + level + "_SUCCES", 0) > 0)
				return LevelState.SUCCESSED;
			else if(PlayerPrefs.GetInt("MECANO_GAME_LVL" + level + "_UNLOCK", 0) > 0)
				return LevelState.UNLOCKED;
			break;
			
		case GameType.PROPULSION:
			if(PlayerPrefs.GetInt("PROPULSION_GAME_MAX_WON", 0) >= level)
				return LevelState.SUCCESSED;
			else if(PlayerPrefs.GetInt("PROPULSION_GAME_LVL" + level + "_UNLOCK", 0) > 0)
				return LevelState.UNLOCKED;
			break;
		}
		
		return LevelState.LOCKED;
	}
	
	
	private void setLevelState(GameType type, int level, LevelState state) {
		int successed = state == LevelState.SUCCESSED ? 1 : 0;
		int unlocked = (state == LevelState.UNLOCKED || state == LevelState.SUCCESSED) ? 1 : 0;
		
		switch(type) {
		case GameType.PAINTER:
			PlayerPrefs.SetInt("PAINT_GAME_LVL" + level + "_SUCCES", successed);
			PlayerPrefs.SetInt("PAINT_GAME_LVL" + level + "_UNLOCK", unlocked);
			break;
			
		case GameType.MECANO:
			PlayerPrefs.SetInt("MECANO_GAME_LVL" + level + "_SUCCES", successed);
			PlayerPrefs.SetInt("MECANO_GAME_LVL" + level + "_UNLOCK", unlocked);
			break;
			
		case GameType.PROPULSION:
			// stupid way to do it :s
			if(successed == 1) {
				PlayerPrefs.SetInt("PROPULSION_GAME_MAX_WON", level);
				PlayerPrefs.SetInt("PROPULSION_GAME_MAX_DIFFICULTY", level);
			}
			else {
				PlayerPrefs.SetInt("PROPULSION_GAME_MAX_WON", 0);
				PlayerPrefs.SetInt("PROPULSION_GAME_MAX_DIFFICULTY", 0);
			}
				
			PlayerPrefs.SetInt("PROPULSION_GAME_LVL" + level + "_UNLOCK", unlocked);
			break;
		}
	}
	
	private void initButtonStyle(Button button, GameType type, int level) {
		switch(getLevelState(type, level)) {
			case LevelState.UNLOCKED:
				button.colors = unlockedColors;
				break;
			case LevelState.SUCCESSED:
				button.colors = successedColors;
				break;
			default:
				button.colors = lockedColors;
				break;
		}
	}
}
