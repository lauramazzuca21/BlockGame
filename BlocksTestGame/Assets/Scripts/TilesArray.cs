using System;
using System.Collections.Generic;
using UnityEngine;

public class TilesArray {

	private GameObject[,] tiles = new GameObject[Constants.ROWS, Constants.COLUMNS];
	private List<GameObject> selectedTiles;

	public GameObject this[int x, int y] {
		get
		{
			return tiles[x, y];
		}
		set
		{
			tiles[x, y] = value;
		}
	}
    
	public void Remove(GameObject item)
    {
        tiles[item.GetComponent<Tile>().Row, item.GetComponent<Tile>().Column] = null;
    }
}
