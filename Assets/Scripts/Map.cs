using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* A class to contain the tile grid and serve as a base class for the path algorithms.*/
public abstract class Map : MonoBehaviour
{
    [HeaderAttribute("Prefab to create individual tiles")]
    [SerializeField]
    private Tile tilePrefab = null;

    [HeaderAttribute("Grid size")]
    [SerializeField]
    protected int sizeX = 10;
    [SerializeField]
    protected int sizeY = 10;

    // Store tiles in grid.
    protected Tile[,] tileGrid;

    // Start and end of path.
    protected Vector2Int startPos;
    protected Vector2Int destPos;

    // The calculated path to display.
    protected List<Tile> pathTiles;
    protected int displayIndex; // Current path object to display.

    // All possible directions. (could be simplified to just up, down, left, right)
    protected readonly Vector2Int[] allDirections =
                                           {Vector2Int.up,
                                            Vector2Int.up + Vector2Int.left,
                                            Vector2Int.up + Vector2Int.right,
                                            Vector2Int.down,
                                            Vector2Int.down + Vector2Int.left,
                                            Vector2Int.down + Vector2Int.right,
                                            Vector2Int.left,
                                            Vector2Int.right };

    // Drawing board state.
    private TileType _boardDrawingState;

    public TileType BoardDrawingState
    {
        get
        {
            return _boardDrawingState;
        }
        set
        {
            _boardDrawingState = value;
        }
    }

    // Virtual so that it can be overriden in derived class.
    protected virtual void Start()
    {
        // Create simple grid test layout.
        InitGrid();
        PlaceObstacle(2,3,4,2);

        SetStartPos(4, 1);
        SetDestPos(5, 7);

      
    }

    // Virtual so that it can be overriden in derived class.
    protected virtual void Update()
    {
        
    }

    // Initialize empty grid.
    private void InitGrid()
    {
        SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        float gridW = spriteRenderer.bounds.size.x; // Get size of sprite.
        float gridH = spriteRenderer.bounds.size.y;
        Vector3 tilePos = transform.position;

        if (tileGrid != null)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Destroy(tileGrid[x,y].gameObject);
                }
            }
        }

        tileGrid = new Tile[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {            
            for (int y = 0; y < sizeY; y++)
            {
                Tile newTile = Instantiate(tilePrefab, tilePos, Quaternion.identity);

                newTile.transform.parent = transform;
                newTile.Pos = new Vector2Int(x, y);
                tileGrid[x, y] = newTile;

                tilePos.y += gridH;
            }

            tilePos.x += gridW;
            tilePos.y = transform.position.y;
        }


        // Fit grid to screen.
        // Taken from https://pressstart.vip/tutorials/2018/06/6/37/understanding-orthographic-size.html
        Vector3 boardSize = new Vector3(gridW * sizeX, gridH * (sizeY+1), 0.0f);
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = boardSize.x / boardSize.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = boardSize.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = boardSize.y / 2 * differenceInSize;
        }

        // Center grid inside camera view.
        Vector3 cameraPos = new Vector3((boardSize.x * 0.5f) - (gridW/2.0f),
            (boardSize.y/ 2.0f) - (gridH / 2.0f),
            Camera.main.transform.position.z);
        Camera.main.transform.position = cameraPos;
    }

    // Place a rectangular obstacle.
    private void PlaceObstacle(int posX, int posY, int sizeX, int sizeY)
    {
        for (int x = posX; x < posX + sizeX; x++)
        {
            for (int y = posY; y < posY + sizeY; y++)
            {
                tileGrid[x, y].State = TileType.Obstacle;
            }
        }
    }

    // Set Starting position.
    private void SetStartPos(int posX, int posY)
    {
        startPos = new Vector2Int(posX, posY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].State == TileType.Start) // If Start is already set, clear it.
                {
                    tileGrid[x, y].State = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].State = TileType.Start;
    }

    // Set Destination position.
    private void SetDestPos(int posX, int posY)
    {
        destPos = new Vector2Int(posX, posY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].State == TileType.End) // If Destination is already set, clear it.
                {
                    tileGrid[x, y].State = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].State = TileType.End;
    }


    // Is the position on the board and not blocked?
    public bool IsValidTile(Vector2Int pos)
    {
        if ((pos.x >= 0) &&
            (pos.y >= 0) &&
            (pos.x < sizeX) &&
            (pos.y < sizeY))
        {
            if ((tileGrid[pos.x, pos.y].State == TileType.Normal) ||
                (tileGrid[pos.x, pos.y].State == TileType.End))
            {
                return true;
            }
        }

        return false;
    }

    // Reset path overlay display.
    protected void ClearDisplay()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                tileGrid[x, y].ClearRouteOverlay();
            }
        }

        displayIndex = 0;
    }

    // Display the next point in the path overlay.
    private void AdvanceDisplay()
    {

        if (displayIndex < pathTiles.Count)
        {
            for (int i = 0; i < displayIndex; i++)
            {
                tileGrid[pathTiles[i].Pos.x, pathTiles[i].Pos.y].DisplayAsRoute();
            }

            tileGrid[pathTiles[displayIndex].Pos.x, pathTiles[displayIndex].Pos.y].DisplayAsCursor();

            displayIndex++;
        }

        
    }

    // Display the the whole path overlay.
    public void DisplayAllRoute()
    {
        displayIndex = pathTiles.Count - 1;
        AdvanceDisplay();


    }


    // Mouse clicked on a tile
    public void TileClick(Tile tileClicked)
    {
        
        if (BoardDrawingState == TileType.Start)
        {
            startPos = tileClicked.Pos;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (tileGrid[x, y].State == TileType.Start)
                    {
                        tileGrid[x, y].State = TileType.Normal;
                    }
                }
            }
        }

        if (BoardDrawingState == TileType.End)
        {
            destPos = tileClicked.Pos;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (tileGrid[x, y].State == TileType.End)
                    {
                        tileGrid[x, y].State = TileType.Normal;
                    }
                }
            }
        }

        tileClicked.State = BoardDrawingState;
    }

    // Set grid to empty.
    private void ClearGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
               tileGrid[x, y].State = TileType.Normal;
                tileGrid[x, y].ClearRouteOverlay();
            }
        }

        SetStartPos(0, 0);
        SetDestPos(sizeX-1, sizeY-1);
    }

    // For buttons
    public void ButtonSetStart()
    {
        BoardDrawingState = TileType.Start;
    }

    public void ButtonSetEnd()
    {
        BoardDrawingState = TileType.End;
    }

    public void ButtonSetObstacle()
    {
        BoardDrawingState = TileType.Obstacle;
    }

    public void ButtonSetNormal()
    {
        BoardDrawingState = TileType.Normal;
    }

    public void ButtonClear()
    {
        ClearGrid();
        InitGrid();
    }

    public virtual void RunPathfindingAlgorithm()
    {
        
    }


}

