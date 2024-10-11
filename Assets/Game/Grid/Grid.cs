using UnityEngine;
public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Vector3 originPosition;

    /// <summary>
    /// Create a new Grid Object ased on the width, height, cellSize and originPosition
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    /// <param name="originPosition"></param>
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        this.originPosition = originPosition;
    }

    /// <summary>
    /// Returns the world position giving an XY coordinates in the grid
    /// </summary>
    /// <param name="x"> x position in the grid</param>
    /// <param name="y"> y position in the grid</param>
    /// <returns>Returns a Vector3</returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector3(x, y) * cellSize + originPosition);
        return  new Vector3(position.x, position.y, 0);
    }

    /// <summary>
    /// Debug method to instance the grid entirely and check the positions
    /// </summary>
    public void InstanceTheGridEntirely()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                var pos = new Vector3(i,j,0);
                GameObject tile = new GameObject(i+"-"+j);
                tile.transform.position = GetWorldPosition(i, j);
            }
        }
    }
}