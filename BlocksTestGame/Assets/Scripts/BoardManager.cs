using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	private GameState _state; //The states can be found in Utils/Constants.cs
	private TilesArray _tiles;
	private Stack<GameObject> _sequence;
    
	/* these fileds are serialized so that they can be assigned from the Unity editor */
	[SerializeField]
	private GameObject _tile;
	[SerializeField]
    private Sprite[] _tilesSprites;
	[SerializeField]
	private ScoreManager _scoreManager;
    
	private float _elapseTime;   //the elapse time ensures that the player has enough time to move the mouse on top of another tile
	private Vector2 offset;      //the offset will be initialized as the size of a tile

	void Start () 
	{
		_tiles = new TilesArray();

		_state = GameState.None;

		_sequence = new Stack<GameObject>();

		_elapseTime = Constants.ELAPSE_TIME;

		offset = _tile.GetComponent<SpriteRenderer>().bounds.size; 
		CreateBoard();


	}

	private void Update()
	{
		/*****STATE:NONE*****/
		/* If the mouse is held down, The Raycast returns what the 
		 * mouse is hitting at this time hit.
         * Then we make it into a gameObject and if it is not null
         * we push it into the stack of tiles selected and change game state.
         */
		if (_state == GameState.None)
		{
			if (Input.GetMouseButtonDown(0))       // 0 stands for the primary button (left click for computer)

			{
				var hit =  Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				GameObject tileToAdd = hit.collider.gameObject;

				if (tileToAdd != null)
				{
					tileToAdd.GetComponent<Tile>().Select();
					_sequence.Push(tileToAdd);
					_state = GameState.SelectionStarted;
				}
			}
		}
		/*****STATE:SELECTION_STARTED*****/
		/* Once the first tile has been selected we check each Elapse_time (0.5s)
		 * if the mouse is being dragged. If yes then we do the same as before with
		 * the Raycast and then we check that
		 * 1) the GameObject tileToAdd is not null
		 * 2) the sequence does not contain that tile already
		 * 3) that the tile to add is next by x or y to the last tile added to the stack
		 * 4) that the tile to be added and the last tile of the stack are same in colour
		 * if all these checks pass, then the tile is selected and added to the stack
		 * else we deselect all tiles and clear the sequence, returning to the State:None.
		 * IF the mouse is not being dragged and the sequence contains at least the MINIMUM_MATCHES (2)
		 * then we Clear the match and assign the score and bonus time to the player
         */      
		else if ( _state == GameState.SelectionStarted)
		{
			_elapseTime -= Time.deltaTime;
			//user is dragging the mouse
			if (Input.GetMouseButton(0) && _elapseTime <= 0)
			{
				_elapseTime = Constants.ELAPSE_TIME;
				var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				GameObject tileToAdd = hit.collider.gameObject;

				if (tileToAdd != null && !_sequence.Contains(tileToAdd) 
				    && Utilities.AreVerticalOrHorizontalNeighbors(tileToAdd.GetComponent<Tile>(), _sequence.Peek().GetComponent<Tile>())
				    && tileToAdd.GetComponent<Tile>().SpriteColour == _sequence.Peek().GetComponent<Tile>().SpriteColour)
                {
					tileToAdd.GetComponent<Tile>().Select();
					_sequence.Push(tileToAdd);
					_elapseTime = Constants.ELAPSE_TIME;

                }
				else
				{
					foreach (GameObject tile in _sequence) tile.GetComponent<Tile>().Deselect();
					_sequence.Clear();
					_state = GameState.None;
					_elapseTime = Constants.ELAPSE_TIME;

				}
			}
			else if (_sequence.Count >= Constants.MINIMUM_MATCHES && _elapseTime <= 0)
			{
				ClearMatchAndAssignScore();
				_state = GameState.None;
				_elapseTime = Constants.ELAPSE_TIME;

			}
		}
      
	}

    /*Initializes the board of tiles calling for each position a proper method */
	private void CreateBoard()
    {
        for (int x = 0; x < Constants.ROWS; x++)
        {
            for (int y = 0; y < Constants.COLUMNS; y++)
            {
                _tiles[x, y] = InstantiateRandomTile(x, y);
            }
        }
    }

	/* This method clears the tiles and saves the indexes at which new tiles
    * have to be generated. It then calls the Coroutine that makes the generation of new tiles
    * delay of the ELAPSE_TIME
    * */
    private void ClearMatchAndAssignScore()
    {
        _scoreManager.UpdateScoreAndTimer(_sequence.Count);
        int[,] indexes = new int[_sequence.Count, 2];
        int i = 0;
        foreach (GameObject tile in _sequence)
        {
            indexes[i, 0] = tile.GetComponent<Tile>().Row;
            indexes[i, 1] = tile.GetComponent<Tile>().Column;
            Destroy(_tiles[tile.GetComponent<Tile>().Row, tile.GetComponent<Tile>().Column]);

            i++;
        }
        _sequence.Clear();


        StartCoroutine(this.WaitTimeForRefill(Constants.ELAPSE_TIME, indexes, i));

    }

	/*Called by CreateBoard() and WaitTimeForRefill() to instatiate new random tiles at given
	 * x and y positions
	 */
	private GameObject InstantiateRandomTile(int x, int y)
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        GameObject newTile = Instantiate(_tile,
                                         new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                                         Quaternion.identity);

        return AssignSpriteColourAndPosition(newTile, x, y);
    }
    
	/* Method called by InstantiateRandomTile() to Assign to the given tile the right parameters
	 */
	private GameObject AssignSpriteColourAndPosition(GameObject tile, int row, int column)
    {
        int randomSprite = UnityEngine.Random.Range(0, _tilesSprites.Length);
        Sprite newSprite = _tilesSprites[randomSprite];

        switch (randomSprite)
        {
            case 0:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Red, row, column);
                break;
            case 1:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Blue, row, column);
                break;
            case 2:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Green, row, column);
                break;
            case 3:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Yellow, row, column);
                break;
            case 4:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Violet, row, column);
                break;
            case 5:
                tile.GetComponent<Tile>().AssignColourAndPosition(Colour.Orange, row, column);
                break;
        }

        tile.GetComponent<SpriteRenderer>().sprite = newSprite;

		return tile;

    }

	/* Coroutine called in ClearMatchAndAssignScore to wait and refill the board */
	public IEnumerator WaitTimeForRefill(float seconds, int[,] indexes, int i) 
	{
		yield return new WaitForSeconds(seconds);

		for (int j = 0; j < i; j++)
		{
			_tiles[indexes[j, 0], indexes[j, 1]] = InstantiateRandomTile(indexes[j, 0], indexes[j, 1]);
		}
	}
}
