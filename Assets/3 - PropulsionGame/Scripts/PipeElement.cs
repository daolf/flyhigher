using UnityEngine;
using System.Collections;


public class PipeElement
{
	public enum Type {
		PIPE_I,
		PIPE_L,
		PIPE_IN,
		PIPE_OUT
	}

	public enum Orientation
	{
		NORTH,
		SOUTH,
		EAST,
		WEST
	}

	private Type _type;
	public Type type {
		get {
			return _type;
		}
		private set {
			_type = value;
		}
	}

	private Orientation _orientation;
	public Orientation orientation {
		get {
			return _orientation;
		}
		set {
			_orientation = value;
		}
	}

	public bool isInstanciated = false;

	// neighbors, maybe a bit heavy, should be simplified
	private PipeElement north;
	private PipeElement south;
	private PipeElement east;
	private PipeElement west;

	public PipeElement (Type type, Orientation orientation)
	{
		this.orientation = orientation;
		this.type = type;
	}

	/**
	 * Return true if orientations given are connected together by this pipe.
	 */
	public bool isConnected(Orientation a, Orientation b) {
		// all pipes have their input oriented to the north by default
		if(a != this.orientation) {
			if(b != this.orientation)
				return false;

			// invert a and b, so we have always 'a' in the default direction
			Orientation swap = a;
			a = b;
			b = swap;
		}

		// depending the pipe type, check if 'b' is in the good direction
		switch(this.type) {
		case Type.PIPE_I:
			// two times 90° rotations
			return b == a.opposite();
		case Type.PIPE_L:
			// 90° rotation
			return b == a.rotateClockwise();
		}

		return false;
	}

	/**
	 * Get/set neighbors of this pipe.
	 * null is used if no neighbor is present
	 */
	public void setNeighbor(Orientation direction, PipeElement pipe) {
		switch (direction) {
		case Orientation.NORTH:
			north = pipe;
			break;
		case Orientation.SOUTH:
			south = pipe;
			break;
		case Orientation.EAST:
			east = pipe;
			break;
		case Orientation.WEST:
			west = pipe;
			break;
		}

		// auto-set in the opposite direction if needed
		if (pipe.getNeighbor (direction.opposite ()) != this) {
			pipe.setNeighbor(direction.opposite(), this);
		}
	}

	public PipeElement getNeighbor(Orientation direction) {
		switch (direction) {
		case Orientation.NORTH:
			return north;
		case Orientation.SOUTH:
			return south;
		case Orientation.EAST:
			return east;
		case Orientation.WEST:
			return west;
		}

		return null;
	}
}


// extension for orientations enum
public static class OrientationMethods
{
	public static PipeElement.Orientation rotateClockwise(this PipeElement.Orientation cur) {
		switch (cur) {
		case PipeElement.Orientation.NORTH:
			return PipeElement.Orientation.EAST;
		case PipeElement.Orientation.EAST:
			return PipeElement.Orientation.SOUTH;
		case PipeElement.Orientation.SOUTH:
			return PipeElement.Orientation.WEST;
		default:
			return PipeElement.Orientation.NORTH;
		}
	}

	public static PipeElement.Orientation opposite(this PipeElement.Orientation cur) {
		return cur.rotateClockwise ().rotateClockwise ();
	}

	public static float toDegrees(this PipeElement.Orientation cur) {
		switch(cur) {
		case PipeElement.Orientation.NORTH:
			return 0f;
		case PipeElement.Orientation.WEST:
			return 90f;
		case PipeElement.Orientation.SOUTH:
			return 180f;
		case PipeElement.Orientation.EAST:
			return 270f;
		}
		return 0f;
	}
}
