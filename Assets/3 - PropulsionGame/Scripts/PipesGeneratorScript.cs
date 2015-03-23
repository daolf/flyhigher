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

	public Text toShow;

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
	
	private const float WIN_PATH_DISPLAYING_TIME = 0.2f;
	

	void Update() {
		// really dirty proof of concept, check path every frame
		if(!hadWon) {
			if (checkReachDestination()) {//isPathValid (origin.getNeighbor(PipeElement.Orientation.SOUTH), origin.orientation.opposite())) {
				toShow.text = "You win!";
				hadWon = true;
				onWin();				
			}
			else {
				toShow.text = "Try again...";
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
				}
				else {
					// not good : accessing random item (but easier)
					winPath.ElementAt(nextVal).setWinPath(true);
				}
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
		//foreach(PipeElementScript pipe in winPath)
		//	pipe.setWinPath(true);
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
		parentArea = GameObject.Find("/Container").transform;
		grid = instanciateLevelFromXml (level);
		instanciatePipeGrid (grid);
		return;
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
