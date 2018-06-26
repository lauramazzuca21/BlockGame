using System;
using UnityEngine;

public static class Utilities {

	public static bool AreVerticalOrHorizontalNeighbors(Tile tile1, Tile tile2)
    {
		return (tile1.Column == tile2.Column || tile1.Row == tile2.Row)
			    && Mathf.Abs(tile1.Column - tile2.Column) <= 1
			    && Mathf.Abs(tile1.Row - tile2.Row) <= 1;
    }
}