using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class TerrainTools : MonoBehaviour
{
    public GameObject tileObject;
    public GameObject terrainParent;
    public int rows;
    public int columns;
    public float spacingX;
    public float spacingY;

    private float rowFirstX;
    private float rowFirstY;
    private float currentX;
    private float currentY;
    private int currentRow;
    private int currentCol;
    private int sortOrder;
    private List<Tile> tiles;

    //----------------------------------------------------------------------------------------------
    public  void CreateLayout() {
        ClearLayout();

        tiles = new List<Tile>();
        rowFirstX = (columns - 1) / -2f * spacingX;
        rowFirstY = rows * spacingY - spacingY / 2f;
        currentX = 0f;
        currentRow = 0;
        currentCol = 0;
        
        Vector3 neighborPosition;
        GameObject newObj;
        Tile newTile;
        BoardDirection oppositeDir;

        for (int row = 0; row < rows; row++) {
            currentX = rowFirstX;
            
            for (int col = 0; col < columns; col++) {
                // determine tile XY coordinates
                currentY = rowFirstY;
                sortOrder = currentRow * 100;
                
                if (currentCol % 2 != 0) {
                    currentY -= spacingY;
                    sortOrder += 1;
                }

                // create the tile
                newObj = PrefabUtility.InstantiatePrefab(tileObject) as GameObject;
                newObj.name = "Tile (" + (row + 1) + "," + (col + 1) + ")";
                newObj.transform.parent = terrainParent.transform;
                newObj.transform.position = new Vector3(currentX, currentY, 0f);
                newObj.GetComponentInChildren<SpriteRenderer>().sortingOrder = sortOrder;
                newTile = newObj.GetComponent<Tile>();

                // search for neighbors
                newTile.Neighbors = new List<Tile>();
                newTile.NeighborDirections = new List<BoardDirection>();

                foreach (BoardDirection dir in Enum.GetValues(typeof(BoardDirection))) {
                    neighborPosition = new Vector3(
                        x:currentX + spacingX * GetOffsetsForDirection(dir).Item1,
                        y:currentY + spacingY * GetOffsetsForDirection(dir).Item2,
                        z:0f
                    );
                    
                    foreach (Tile neighbor in tiles) {
                        if (neighbor.transform.position == neighborPosition) {
                            newTile.Neighbors.Add(neighbor);
                            newTile.NeighborDirections.Add(dir);
                            neighbor.Neighbors.Add(newTile);
                            neighbor.NeighborDirections.Add(GetOppositeDirection(dir));
                            break;
                        }
                    }
                }
                
                // prep for next tile
                tiles.Add(newTile);
                currentX += spacingX;
                currentCol++;
            }

            rowFirstY -= spacingY * 2;
            currentRow++;
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private void ClearLayout() {
        // we have to clone the list first because it doesn't work otherwise???????
        List<Transform> oldTiles = terrainParent.transform.Cast<Transform>().ToList();
        foreach (Transform child in oldTiles) {
            DestroyImmediate(child.gameObject);
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private static (int, int) GetOffsetsForDirection(BoardDirection dir) {
        switch (dir) {
            case BoardDirection.DownRight:
                return (1, -1);
            case BoardDirection.UpLeft:
                return (-1, 1);
            case BoardDirection.UpRight:
                return (1, 1);
        }

        // this must be BoardDirection.DownLeft
        return (-1, -1);
    }
    
    //----------------------------------------------------------------------------------------------
    private static BoardDirection GetOppositeDirection(BoardDirection dir) {
        switch (dir) {
            case BoardDirection.DownRight:
                return BoardDirection.UpLeft;
            case BoardDirection.UpLeft:
                return BoardDirection.DownRight;
            case BoardDirection.UpRight:
                return BoardDirection.DownLeft;
        }

        // this must be BoardDirection.DownLeft
        return BoardDirection.UpRight;
    }
}
