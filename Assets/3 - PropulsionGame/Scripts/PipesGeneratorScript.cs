using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class PipesGeneratorScript : MonoBehaviour {

	// Used to reference the prefabs used from Unity interface
	public PipeElementScript pipeI;
	public PipeElementScript pipeL;
	public PipeElementScript pipeIn;
	public PipeElementScript pipeOut;
	
	public GenericTutoScript tutoScript;
	
	public AudioSource mainAudio;
	
	public TextAsset[] levelsDifficulty1;
	public TextAsset[] levelsDifficulty2;
	public TextAsset[] levelsDifficulty3;
	
	public int currentDifficulty = 3;
	
	// for tuto
	public TextAsset levelTuto;
	public Vector2 tutoBadPipePosition;
	

	// constants for pipe grid size, and (x,y) of top-left corner
	private const int GRID_SIZE = 9;
	private const float ORIGIN_X = 0;
	private const float ORIGIN_Y = 0;

	// PipeElement and PipeElementScript are stored in a stupid (x;y) array
	private PipeElement[,] grid;
	private PipeElementScript[,] objectGrid;

	// input position and direction
	private int inputX;
	private int inputY;
	private PipeElement.Orientation inputOrientation;
	
	// output (goal) position and direction
	private int outputX;
	private int outputY;
	private PipeElement.Orientation outputOrientation;
	
	// object used as parent for every pipe object
	private Transform parentArea;
	
	private bool hadWon = false;
	
	// set after win, with the 'win path'
	private LinkedList<PipeElementScript> winPath;
	
	// time between each pipe displaying
	private const float WIN_PATH_DISPLAYING_TIME = 0.2f;

	// used to wait a bit after win path display...
	private const float ALMOST_FINISHED_TIME = 0.7f;

	// used to choose the right pipe color
	private Gradient gradient;
	private GradientColorKey[] gck;
	private GradientAlphaKey[] gak;

	public cameraScript myCamera;
	
	public TimeBarscript timebar;
	public Score score;
	
	// is in tutorial mode?
	private bool inTuto = true;
	
	private bool m_isPause = false;

	public bool isPause {
		get {
			return m_isPause;
		}
		set {
			m_isPause = value;
			// disable the timer if needed
			timebar.activated = !value;
			GameObject.Find("ButtonPause").GetComponentInParent<Canvas>().enabled = !value;
		}
	}
	
	
	/**
	 * Texts used in tutorial, and other persuasive aspects of the game.
	 */
	private string[] msgPresentation1 = new string[] { 
		"Salut, je m'appelle Emilie et je suis ingénieure en propulsion aéronautique.",
		"Je suis chargée de concevoir une grande partie des équipements qui permettent à l'avion de voler,"
		 + " comme par exemple ses réacteurs."
	};
	
	private string[] msgTuto1 = new string[] {
		"Ici, le but du jeu est de résoudre un casse-tête.",
		"Tu dois tourner les bons tuyaux d'aération pour connecter l'entrée d'air du réacteur"
		 + " à sa sortie, dans le temps imparti.",
		"Pour tourner un tuyau, il te suffit de le toucher.\nBon, là c'est facile, je te montre!"
	};
	
	private string[] msgTuto2 = new string[] {
		"Et voilà, ce réacteur devrait fonctionner correctement!\nA toi de jouer maintenant."
	};
	
	private string[][] msgLevelBeginning = new string[][] {
		null,
		new string[] { 
			"Il y a plusieurs types de moteur d'avion, celui présenté en fond sur le jeu est"
			 + " celui d'un moteur à réaction, aussi appellé turbo-réacteur.",
			"Le but est, grace au carburant, de transformer l'air froid rentrant en air chaud"
			 + " pour produire de l'energie servant à pousser l'avion."
		},
		new string[] {
			"Le métier d'ingénieur aéronautique demande le sens des responsabilités ainsi que"
			  + " d'avoir le sang-froid.",
			 "Il faut aussi savoir faire preuve de beaucoup de patience car certains projets peuvent mettre"
			  + " plus de 15 ans avant d'aboutir réellement!",
			 "Attention, ça se corse un peu maintenant, mais je suis sûre que tu vas très bien t'en sortir."
		}
	};
	
	private string[][] msgLevelFinished = new string[][] {
		new string[] {
			"Bravo, tu viens de débloquer une nouvelle jauge de kérosène pour propulser ton"
			 + " avion encore plus longtemps !"
		},
		new string[] {
			"Excellent, une troisième jauge de kérosène va être disponible grâce à toi!"
		},
		new string[] {
			"Je savais bien que tu y arriverais!",
			"Tu as amélioré le réacteur du mieux que l'on puisse faire avec nos moyens, mais"
			 + " n'hésite pas à t'entrainer à nouveau."
		}
	};

	void Update() {
		// really dirty proof of concept, check path every frame
		if(!hadWon) {
			score.value = (int)(timebar.CurrentTime * 100);
			if (checkReachDestination()) {
				hadWon = true;
				onWin();
			}
		}
	}
	
	/**
	 * Display the win path, wait when needed, play sound, etc...
	 */
	private IEnumerator endGameCoroutine() {
		int curIndex = 0;
	
		// consecutively display every pipe of the win path, one by one
		foreach(PipeElementScript current in winPath) {
			current.setWinPath(true);
			current.fadingColorOut = gradient.Evaluate((float)curIndex/winPath.Count);
			curIndex++;
			yield return new WaitForSeconds(WIN_PATH_DISPLAYING_TIME);
		}
		
		yield return new WaitForSeconds(ALMOST_FINISHED_TIME);
		beforeEffectiveWin();
	}
	
	private PipeElement getPipeFromGrid(int x, int y) {
		if(x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE)
			return grid[x, y];
		return null;
	}
	
	// internal, called on game win event
	private void onWin() {
		// disable every pipe
		for(int i=0; i<GRID_SIZE; i++) {
			for(int j=0; j<GRID_SIZE; j++) {
				if(objectGrid[i, j] != null)
					objectGrid[i, j].setTouchEnable(false);
			}
		}
		
		// disable the timer and the Pause button
		isPause = true;
		
		// play the sound, with duration of total time needed to display the path
		GameObject winSound = GameObject.Find("WinSoundSource");
		if(winSound != null) {
			float soundDuration = WIN_PATH_DISPLAYING_TIME * winPath.Count;
			winSound.GetComponent<SineVariableAudioSource>().setDuration(soundDuration);
			winSound.GetComponent<AudioSource>().Play ();
		}
		
		StartCoroutine(endGameCoroutine());
	}
	
	// internal : called when "you win" message is ready to be displayed
	private void onEffectiveWin() {
		// FIXME move that in Main Game to unlock a level
		// save the maximum level allowed if needed
		int currentMaxLevel = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
		if(currentMaxLevel < (currentDifficulty + 1) && currentMaxLevel < 3) {
			PlayerPrefs.SetInt(Constants.PROPULSION_GAME_MAX_DIFFICULTY, currentDifficulty + 1);
			
			// FIXME not a good place to be called?
			PlayerPrefs.Save();
		}

		if(PlayerPrefs.GetInt("PROPULSION_GAME_LVL"+(currentDifficulty).ToString()+"_SUCCES") == 0 ) {
			GameObject.Find("WinMenu/MenuBg/Bonus").GetComponent<Button>().GetComponent<Image>().enabled = true;
		}
		else {
			GameObject.Find("WinMenu/MenuBg/Bonus").GetComponent<Button>().GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt ("PROPULSION_GAME_LVL" + (currentDifficulty).ToString () + "_SUCCES", 1);
		GameObject.Find("WinMenu").GetComponent<Canvas>().enabled = true;
	}
	
	private void beforeEffectiveWin() {
		int maxWon = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_WON, 0);
		
		if(inTuto) {
			// the goal is to reload the scene without tuto enable!
			StartCoroutine(prepareEndOfTutorial());
		}
		// display succeed message if needed
		else if(maxWon < currentDifficulty) {
			PlayerPrefs.SetInt(Constants.PROPULSION_GAME_MAX_WON, currentDifficulty);
			
			if(msgLevelFinished[currentDifficulty-1] != null)
				displayFinishedMessages(msgLevelFinished[currentDifficulty-1]);
			else
				onEffectiveWin();
		}
		else
			onEffectiveWin();
	}
	
	private IEnumerator prepareEndOfTutorial() {
		yield return new WaitForSeconds(0.5f);
		
		tutoScript.setBubbleVisibility(true);
		tutoScript.say(msgTuto2);
		while(tutoScript.state != GenericTutoScript.TutoState.Finish)
			yield return null;
		
		tutoScript.setBubbleVisibility(false);
		tutoScript.getOut();
		while(tutoScript.state != GenericTutoScript.TutoState.Hidden)
			yield return null;
		
		// TODO cleaner way?
		PlayerPrefs.SetInt(Constants.PROPULSION_GAME_MAX_PLAYED, 1);
		//PropulsionLevelConfiguration.showTutorial = false;
		Application.LoadLevel("IngameScene");
	}
	
	// internal : used as a callback when the time is elapsed
	private void onTimerEnd() {
		isPause = true;
		GameObject.Find("LoseMenu").GetComponent<Canvas>().enabled = true;
	}
	
	private void getNeighborCoordinates(int x, int y, PipeElement.Orientation dir, out int newX, out int newY) {
		newX = x;
		newY = y;
		switch(dir) {
		case PipeElement.Orientation.NORTH:
			newY--;
			break;
		case PipeElement.Orientation.SOUTH:
			newY++;
			break;
		case PipeElement.Orientation.EAST:
			newX++;
			break;
		case PipeElement.Orientation.WEST:
			newX--;
			break;
		}
	}
	
	private bool isValidPath(int x, int y, PipeElement.Orientation from) {
		// before anything else, check if (x;y) is the valid output
		if(x == outputX && y == outputY && from == outputOrientation) {
			// create the win path
			winPath = new LinkedList<PipeElementScript>();
			return true;
		}
		
		// check if the given (x;y) is a valid pipe
		PipeElement target = getPipeFromGrid(x, y);
		if(target == null)
			return false;
		
		// check if the (x;y) pipe may be connected with the origin direction
		PipeElement.Orientation outDir;
		if(!target.getDirectionConnected(from, out outDir))
			return false;
		
		// it's the time for... recursion \o/
		int newX, newY;
		getNeighborCoordinates(x, y, outDir, out newX, out newY);
		if(isValidPath(newX, newY, outDir.opposite())) {
			// okay, we are part of the win path, add ourself on top
			winPath.AddFirst(objectGrid[x, y]);
			return true;
		}
		
		return false;
	}
	
	private bool checkReachDestination() {
		// for now simple algorithm, supports only for single path
		int firstX, firstY;
		getNeighborCoordinates(inputX, inputY, inputOrientation, out firstX, out firstY);
		return isValidPath(firstX, firstY, inputOrientation.opposite());
	}
	

	// Use this for initialization
	void Start () {
		// On zoom
		myCamera.to = 3.22f;
		
		inTuto = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_PLAYED, 0) < 1;
		//inTuto = PropulsionLevelConfiguration.showTutorial;
		
		// TODO will not be needed to check max level later...
		int maxDifficulty = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
		currentDifficulty = PropulsionLevelConfiguration.currentLevel;
		currentDifficulty = currentDifficulty < 1 ? 1 : 
				(currentDifficulty > maxDifficulty ? maxDifficulty : currentDifficulty);

		parentArea = GameObject.Find("/Container").transform;
		// load Tutorial level if needed
		if(inTuto) {
			grid = instanciateLevelFromXml(levelTuto);
		}
		else {
			grid = instanciateLevelFromXml (getRandomLevel(currentDifficulty));
		}
		instanciatePipeGrid (grid);

		// pipe gradient, very ugly
		gradient = new Gradient();
		// Populate the color keys at the relative time 0 and 1 (0 and 100%)
		gck = new GradientColorKey[3];
		gck[0].color = new Color32(0,228,255,1);
		gck[0].time = 0.0f;
		gck[1].color = new Color32(228,198,109,1);
		gck[1].time = 0.5f;
		gck[2].color = new Color32(219,101,63,1);
		gck[2].time = 1.0f;
		
		// Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0f;
		gak[0].time = 0.0f;
		gak[1].alpha = 1.0f;
		gak[1].time = 1.0f;
		gradient.SetKeys(gck, gak);
		
		// manage timer
		timebar.endCallback = onTimerEnd;
		
		// start camera zoom (the game is virtually paused before zoom is done)
		isPause = true;
		Time.timeScale = 1;
		myCamera.zoomFinishedCallback = delegate() {
			emilieSpeakTime();
		};
	}
	
	
	/**
	 * Manage messages said by Emilie at the beginning of a level.
	 */
	private void emilieSpeakTime() {
		// tutorial if needed
		if(inTuto) {
			firstPlayTuto();
		}
		else {
			// maybe something else to say?
			
			int maxPlayed = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_PLAYED, 0);
			if(maxPlayed < currentDifficulty) {
				PlayerPrefs.SetInt(Constants.PROPULSION_GAME_MAX_PLAYED, currentDifficulty);
				
				if(msgLevelBeginning[currentDifficulty-1] != null)
					displayBeginningMessages(msgLevelBeginning[currentDifficulty-1]);
				else
					isPause = false;
			}
			else
				isPause = false;
		}
	}
	
	
	private void displayBeginningMessages(string[] messages) {
		GenericTutoScript.StateChangeDelegate unpause = delegate {
			isPause = false;
		};
		displayMessages(messages, unpause);
	}
	
	private void displayFinishedMessages(string[] messages) {
		displayMessages(messages, onEffectiveWin);
	}
	
	private void displayMessages(string[] messages, GenericTutoScript.StateChangeDelegate outCallback) {
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			
			tutoScript.say(messages);
		};
		
		tutoScript.dialogueEndCallback = delegate() {
			tutoScript.setBubbleVisibility(false);
			tutoScript.getOut();
		};
		
		tutoScript.outCallback = outCallback;
		
		tutoScript.getIn();
	}
	
	
	/**
	 * First tutorial
	 */
	private void firstPlayTuto() {
		//isPause = true;
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);

			tutoScript.say(msgPresentation1.Concat(msgTuto1));
		};
		
		tutoScript.dialogueEndCallback = delegate() {
			tutoScript.dialogueEndCallback = null;
			tutoScript.setBubbleVisibility(false);
			
			StartCoroutine(playTutoCoroutine());
		};
		
		tutoScript.outCallback = delegate() {
			//isPause = false;
		};
		
		tutoScript.getIn();
	}
	
	
	/**
	 * Play the 'fake game', using the tutorial hand to display how to finish a level...
	 */
	private IEnumerator playTutoCoroutine() {
		// move hand over the pipe to turn
		Vector3 worldPos = objectGrid[(int)tutoBadPipePosition.x, (int)tutoBadPipePosition.y].transform.position;
		tutoScript.hand.moveToWorldPosition(worldPos, 1.8f);
	
		// when the hand is over the pipe, display a 'click', and turn the pipe as if it was touched
		yield return new WaitForSeconds(2);
		tutoScript.hand.setHandKind(TutoHandScript.HandKind.HandClick);
		yield return new WaitForSeconds(0.1f);
		
		PipeElementScript badPipe = objectGrid[(int)tutoBadPipePosition.x, (int)tutoBadPipePosition.y];
		badPipe.rotateClockwiseOnce();
		
		yield return new WaitForSeconds(0.2f);
		tutoScript.hand.setHandKind(TutoHandScript.HandKind.HandNormal);
		
		// hide the hand, the rest of the tutorial is handled by onEffectiveWin() ;)
		yield return new WaitForSeconds(0.5f);
		tutoScript.hand.setVisibility(false);
		
		//tutoScript.getOut();
	}
	
	/**
	 * choose randomly a level, matching the given difficulty
	 */
	private TextAsset getRandomLevel(int difficulty) {
		TextAsset[] currentDifficultyLevels;
		switch(difficulty) {
		case 1:
			currentDifficultyLevels = levelsDifficulty1;
			break;
		case 2:
			currentDifficultyLevels = levelsDifficulty2;
			break;
		default:
			currentDifficultyLevels = levelsDifficulty3;
			break;
		}
		return currentDifficultyLevels[Random.Range(0, currentDifficultyLevels.Length)];
	}
		
		/**
	 * Read a level from an XML document. 
	 */
	private PipeElement[,] instanciateLevelFromXml(TextAsset document) {
		XmlDocument xml = new XmlDocument ();
		PipeElement[,] grid = new PipeElement[GRID_SIZE, GRID_SIZE];

		// load the document and get the level(s)
		xml.LoadXml(document.text);
		XmlNodeList levelNodes = xml.GetElementsByTagName("level");

		XmlAttribute levelEstimatedTime = levelNodes[0].Attributes["estimatedTime"];
		if(levelEstimatedTime != null) {
			// use it as the base time for countdown timer
			timebar.maxTime = int.Parse(levelEstimatedTime.Value);
		}
		// start and destination pipes
		// TODO init
		
		XmlNodeList pipeNodes = levelNodes[0].ChildNodes;
		foreach (XmlNode curNode in pipeNodes) {
			if(curNode.Name == "pipe") {
				int x=0;
				int y=0;
				PipeElement.Orientation orientation = PipeElement.Orientation.NORTH;
				PipeElement.Type type = PipeElement.Type.PIPE_I;

				// add the pipe if possible
				x = int.Parse(curNode.Attributes["x"].Value);
				y = int.Parse(curNode.Attributes["y"].Value);

				switch(curNode.Attributes["type"].Value) {
				case "L":
					type = PipeElement.Type.PIPE_L;
					break;
				case "I":
					type = PipeElement.Type.PIPE_I;
					break;
				case "in":
					type = PipeElement.Type.PIPE_IN;
					break;
				case "out":
					type = PipeElement.Type.PIPE_OUT;
					break;
				}
				
				XmlAttribute dirAttribute = curNode.Attributes["dir"];
				if(dirAttribute == null) {
					orientation = OrientationMethods.randomOrientation();
				}
				else {
					orientation = OrientationMethods.fromString(dirAttribute.Value);
				}


				if(x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE) {
					// add the new pipe to the grid, and set its neighbors
					PipeElement toAdd = new PipeElement(type, orientation);
					grid[x, y] = toAdd;
					
					if(toAdd.type == PipeElement.Type.PIPE_IN) {
						inputX = x;
						inputY = y;
						inputOrientation = orientation;
					}
					else if(toAdd.type == PipeElement.Type.PIPE_OUT) {
						outputX = x;
						outputY = y;
						outputOrientation = orientation;
					}
						
					// set neighbors
					
					// TODO neighbors
				}
			}
			else {

			}
		}

		return grid;
	}

	private void instanciatePipeGrid(PipeElement[,] grid) {
		objectGrid = new PipeElementScript[GRID_SIZE, GRID_SIZE];
		for (int y=0; y<GRID_SIZE; y++) {
			for (int x=0; x<GRID_SIZE; x++) {
				if(grid[x, y] != null) {
					PipeElement origin = grid[x, y];
					PipeElementScript prefab = null;
					switch(origin.type) {
					case PipeElement.Type.PIPE_I:
						prefab = pipeI;
						break;
					case PipeElement.Type.PIPE_L:
						prefab = pipeL;
						break;
					case PipeElement.Type.PIPE_IN:
						prefab = pipeIn;
						break;
					case PipeElement.Type.PIPE_OUT:
						prefab = pipeOut;
						break;
					}
					
					PipeElementScript pipeSprite = (PipeElementScript) Instantiate(prefab);
					
					// set internal pipe element
					pipeSprite.setPipeElement (origin);
					
					// set to the appropriate parent (Pipe Container) and position inside
					pipeSprite.transform.parent = parentArea;
					pipeSprite.transform.localPosition =
						new Vector3 (x + ORIGIN_X, -y + ORIGIN_Y, 0);
					
					// not very clever, allow/disallow rotation depending on pipe type
					if(origin.type == PipeElement.Type.PIPE_IN || origin.type == PipeElement.Type.PIPE_OUT)
						pipeSprite.setTouchEnable(false);
					
					objectGrid[x, y] = pipeSprite;
				}
			}
		}
	}

}
