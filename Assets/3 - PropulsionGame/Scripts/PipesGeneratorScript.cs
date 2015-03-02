using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Text;
using System.Xml;
using System.IO;

public class PipesGeneratorScript : MonoBehaviour {

	// Used to reference the prefabs used from Unity interface
	public PipeElementScript pipeI;
	public PipeElementScript pipeL;
	public PipeElementScript pipeIn;
	public PipeElementScript pipeOut;

	public TextAsset level;

	public Text toShow;

	// constants for pipe grid size, and (x,y) of top-left corner
	private const int GRID_SIZE = 8;
	private const float ORIGIN_X = -4;
	private const float ORIGIN_Y = -4;

	private PipeElement origin = null;


	void Update() {
		// really dirty proof of concept, check path every frame
		if (true) {//isPathValid (origin.getNeighbor(PipeElement.Orientation.SOUTH), origin.orientation.opposite())) {
			toShow.text = "You win!";
		}
		else {
			toShow.text = "Try again...";
		}
	}

	// true if a path from the given element come to a PIPE_OUT element
	private bool isPathValid(PipeElement from, PipeElement.Orientation orgDirection) {
		if (from == null)
			return false;

		if (from.type == PipeElement.Type.PIPE_OUT && from.orientation == orgDirection)
			return true;

		if (from.isConnected (orgDirection, PipeElement.Orientation.NORTH)
		    	&& isPathValid (from.getNeighbor (PipeElement.Orientation.NORTH), PipeElement.Orientation.SOUTH))
			return true;
		if (from.isConnected (orgDirection, PipeElement.Orientation.SOUTH)
		 	   && isPathValid (from.getNeighbor (PipeElement.Orientation.SOUTH), PipeElement.Orientation.NORTH))
			return true;
		if (from.isConnected (orgDirection, PipeElement.Orientation.EAST)
		 	   && isPathValid (from.getNeighbor (PipeElement.Orientation.EAST), PipeElement.Orientation.WEST))
			return true;
		if (from.isConnected (orgDirection, PipeElement.Orientation.WEST)
		    	&& isPathValid (from.getNeighbor (PipeElement.Orientation.WEST), PipeElement.Orientation.EAST))
			return true;

		return false;
	}
	
	// Use this for initialization
	void Start () {
		PipeElement[,] grid = instanciateLevelFromXml (level);
		instanciatePipeGrid (grid);
		return;

		// test level
		origin = new PipeElement(PipeElement.Type.PIPE_IN, PipeElement.Orientation.SOUTH);
		PipeElement cur, prev;

		cur = new PipeElement(PipeElement.Type.PIPE_L, PipeElement.Orientation.NORTH);
		origin.setNeighbor (PipeElement.Orientation.SOUTH, cur);
		prev = cur;

		cur = new PipeElement (PipeElement.Type.PIPE_I, PipeElement.Orientation.EAST);
		prev.setNeighbor (PipeElement.Orientation.EAST, cur);
		prev = cur;

		cur = new PipeElement (PipeElement.Type.PIPE_L, PipeElement.Orientation.EAST);
		prev.setNeighbor (PipeElement.Orientation.EAST, cur);
		prev = cur;

		cur = new PipeElement (PipeElement.Type.PIPE_I, PipeElement.Orientation.EAST);
		prev.setNeighbor (PipeElement.Orientation.SOUTH, cur);
		prev = cur;

		cur = new PipeElement (PipeElement.Type.PIPE_OUT, PipeElement.Orientation.NORTH);
		prev.setNeighbor (PipeElement.Orientation.SOUTH, cur);

			

		/*for (int i=0; i<GRID_SIZE; i++) {
			for(int j=0; j<GRID_SIZE; j++) {
				Instantiate(pipeL, new Vector3(i + ORIGIN_X, j + ORIGIN_Y, 0), Quaternion.identity);
			}
		}*/

		instanciatePipePath (origin, 0, GRID_SIZE);
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


				if(x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE) {
					// add the new pipe to the grid, and set its neighbors
					PipeElement toAdd = new PipeElement(type, orientation);
					grid[x, y] = toAdd;
					// TODO neighbors
				}
			}
			else {

			}
		}

		return grid;
	}

	private void instanciatePipeGrid(PipeElement[,] grid) {
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
					
					PipeElementScript pipeSprite;
					pipeSprite = (PipeElementScript) 
						Instantiate(prefab, new Vector3 (x + ORIGIN_X, y + ORIGIN_Y, 0), Quaternion.identity);
					
					// set internal pipe element
					pipeSprite.setPipeElement (origin);
				}
			}
		}
	}

	/**
	 * Temporary, instanciate each pipe from the given one and all its neighbors.
	 * (warning : this method will never return if there is a cycle in the path!)
	 */
	private void instanciatePipePath (PipeElement origin, int posx, int posy) {
		if (origin == null || origin.isInstanciated)
			return;
		
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

		PipeElementScript pipeSprite;
		pipeSprite = (PipeElementScript) 
			Instantiate(prefab, new Vector3 (posx + ORIGIN_X, posy + ORIGIN_Y, 0), Quaternion.identity);

		// set internal pipe element
		pipeSprite.setPipeElement (origin);


		origin.isInstanciated = true;
		// recurcivity for each neighbor
		instanciatePipePath (origin.getNeighbor (PipeElement.Orientation.NORTH), posx, posy + 1);
		instanciatePipePath (origin.getNeighbor (PipeElement.Orientation.SOUTH), posx, posy - 1);
		instanciatePipePath (origin.getNeighbor (PipeElement.Orientation.WEST), posx - 1, posy);
		instanciatePipePath (origin.getNeighbor (PipeElement.Orientation.EAST), posx + 1, posy);
	}

	/**
	 * Temp function, not optimized : try to remove a "pipe", and to keep a valid
	 * path between input and output. Pipes are represented here by an array of
	 * booleans, their shape and orientation is not defined at this time.
	 * return false when no more pipe seem to be possible to remove
	 */
	private bool oneStepErode(int[,] pipeMatrix) {
		bool isValid = true;

		return false;
	}

	private bool pipeMatrixIsValid(int[,] pipeMatrix) {
		return false;
	}
}
