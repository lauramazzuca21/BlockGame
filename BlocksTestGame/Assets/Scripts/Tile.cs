using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    
	private SpriteRenderer render;

	public bool isSelected { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }
	public Colour SpriteColour { get; set; }

	void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    
	public void AssignColourAndPosition(Colour colour, int row, int column)
    {
		SpriteColour = colour;
        Column = column;
        Row = row;
    }

	public void Select()
    {
        isSelected = true;
		render.color = Color.gray;
    }

	public void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
    }


    
}
