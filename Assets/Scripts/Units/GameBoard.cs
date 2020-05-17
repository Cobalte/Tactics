using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class GameBoard
{
    public static List<Tile> Tiles { get; private set; }
    
    private static TerrainTools terrainTools;

    //----------------------------------------------------------------------------------------------
    public static void Initialize() {
        terrainTools = GameObject.Find("Terrain").GetComponent<TerrainTools>();
        Tiles = terrainTools.GetComponentsInChildren<Tile>().ToList();
    }

    //----------------------------------------------------------------------------------------------
    public static void ShowTileHighlightsForUnit(Unit unit) {
        HideTileHighlights();
        if (unit == null) {
            return;
        }
        List<Tile> tiles = Pathfinder.GetTilesWithinMoveRange(
            mover: unit,
            moveRange: unit.UnitData.MoveRange,
            stomp: unit.UnitData.Owner == Player.Machines
        );
        foreach (Tile tile in tiles) {
            tile.SetHighlightActive(true);
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void ShowTileHighlightsForAbility(Ability_Base ability, Unit caster) {
        HideTileHighlights();
        List<Tile> tiles = new List<Tile>();
        
        foreach (BoardDirection dir in Enum.GetValues(typeof(BoardDirection))) {
            tiles = GetAllTilesInDirection(caster.CurrentTiles[0], dir, ability.Range);
            foreach (Tile tile in tiles) {
                tile.SetHighlightActive(true);
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void HideTileHighlights() {
        foreach (Tile tile in Tiles) {
            tile.SetHighlightActive(false);
        }
    }

    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetTilesClosetToPoint(Vector3 point, int neededTiles) {
        List<Tile> closestTiles = new List<Tile>();
        List<float> distances = new List<float>();
        float nextDist;

        for (int i = 0; i < neededTiles; i++) {
            closestTiles.Add(null);
            distances.Add(float.MaxValue);
        }
        
        foreach (var tile in Tiles) {
            nextDist = Vector3.Distance(tile.transform.position, point);

            for (int n = 0; n < neededTiles; n++) {
                if (nextDist < distances[n]) {
                    distances.Insert(n, nextDist);
                    distances.RemoveAt(neededTiles);
                    closestTiles.Insert(n, tile);
                    closestTiles.RemoveAt(neededTiles);
                    break;
                }
            }
        }
        
        return closestTiles;
    }
    
    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetAllTilesInDirection(Tile source, BoardDirection dir, int range) {
        List<Tile> result = new List<Tile>();
        Tile nextTile = source.GetNeighborInDirection(dir);
        int rangeLeft = range;

        while (rangeLeft > 0 && nextTile != null) {
            result.Add(nextTile);
            nextTile = nextTile.GetNeighborInDirection(dir);
            rangeLeft--;
        }
        
        return result;
    }
    
    //----------------------------------------------------------------------------------------------
    public static BoardDirection GetDirectionTowardTile(Tile source, Tile target) {
        List<Tile> tiles = new List<Tile>();
        
        foreach (BoardDirection dir in Enum.GetValues(typeof(BoardDirection))) {
            tiles = GetAllTilesInDirection(source, dir, int.MaxValue);
            foreach (Tile tile in tiles) {
                if (tile == target) {
                    return dir;
                }
            }
        }
        
        Debug.LogError("Could not find direction from " + source.gameObject.name + " to " + target.gameObject.name);
        return BoardDirection.DownLeft;
    }
}
