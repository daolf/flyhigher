using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PipesGeneratorScript : MonoBehaviour {

	// Used to reference the prefabs used from Unity interface
	public PipeElementScript pipeI;
	public PipeElementScript pipeL;
	public PipeElementScript pipeIn;
	public PipeElementScript pipeOut;

	public Text toShow;

	// constants for pipe grid size, and (x,y) of top-left corner
	private const int GRID_SIZE = 8;
	private const float ORIGIN_X = -4;
	private const float ORIGIN_Y = -4;

	private PipeElement origin = null;


	void Update() {
		// really dirty proof of concept, check path every frame
		if (isPathValid (origin.getNeighbor(PipeElement.Orientation.SOUTH), origin.orientation.opposite())) {
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
