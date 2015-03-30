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

	public TextAsset level;
	

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
	
	// used to track the state of 'win path' displaying
	private bool inWinPathDisplaying = false;
	private float winPathDisplayingElapsed = 0;
	private int winPathDisplayingCurrent = -1;
	
	// time between each pipe displaying
	private const float WIN_PATH_DISPLAYING_TIME = 0.2f;

	// used to wait a bit after win path display...
	private bool inAlmostFinished = false;
	private float almostFinishedElapsed = 0;
	private const float ALMOST_FINISHED_TIME = 1.5f;

	// used to choose the right pipe color
	private Gradient gradient;
	private GradientColorKey[] gck;
	private GradientAlphaKey[] gak;

	public cameraScript myCamera;
	
	public TimeBarscript timebar;
	
	private bool m_isPause = false;

	public bool isPause {
		get {
			return m_isPause;
		}
		set {
			m_isPause = value;
			// disable the timer if needed
			if(!inWinPathDisplaying && !inAlmostFinished)
				timebar.enabled = !value;
		}
	}

	void Update() {
		// really dirty proof of concept, check path every frame
		if(!hadWon) {
			if (checkReachDestination()) {
				hadWon = true;
				onWin();
			}
		}
		
		if(inWinPathDisplaying) {
			// consecutively display every pipe of the win path, one by one
			winPathDisplayingElapsed += Time.deltaTime;
			
			int nextVal = (int)(winPathDisplayingElapsed / WIN_PATH_DISPLAYING_TIME);
			if(nextVal > winPathDisplayingCurrent) {
				// display the next pipe
				winPathDisplayingCurrent = nextVal;
				if(nextVal > winPath.Count - 1) {
					inWinPathDisplaying = false;
					// start waiting before "you win" message is displayed
					inAlmostFinished = true;
				}
				else {
					// not good : accessing random item (but easier)
					winPath.ElementAt(nextVal).setWinPath(true);
					winPath.ElementAt(nextVal).fadingColorOut = gradient.Evaluate((float)nextVal/winPath.Count);
				}
			}
			
		}
		
		if(inAlmostFinished) {
			almostFinishedElapsed += Time.deltaTime;
			if(almostFinishedElapsed > ALMOST_FINISHED_TIME) {
				inAlmostFinished = false;
				onEffectiveWin();
			}
		}
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
		
		inWinPathDisplaying = true;
		winPathDisplayingElapsed = 0;
		winPathDisplayingCurrent = -1;
		
		// disable the timer and the Pause button
		timebar.enabled = false;
		GameObject.Find("ButtonPause").SetActive(false);
	}
	
	// internal : called when "you win" message is ready to be displayed
	private void onEffectiveWin() {
		// TODO display a menu!
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

		parentArea = GameObject.Find("/Container").transform;
		grid = instanciateLevelFromXml (level);
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
				
				switch(curNode.Attributes["dir"].Value) {
				case "east":
					orientation = PipeElement.Orientation.EAST;
					break;
				case "west":
					orientation = PipeElement.Orientation.WEST;
					break;
				case "south":
					orientation = PipeElement.Orientation.SOUTH;
					break;
				case "north":
					orientation = PipeElement.Orientation.NORTH;
					break;
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
