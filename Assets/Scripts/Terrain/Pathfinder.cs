using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder {
    
    private const bool showgDebugMessages = false;

    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetBestRoute(Unit sourceUnit, Unit destUnit, bool allowStomps) {
        List<Tile> bestRoute = new List<Tile>();
        List<Tile> nextRoute;

        foreach (Tile destTile in destUnit.Position) {
            nextRoute = GetBestRoute(sourceUnit, destTile, allowStomps);
            if (nextRoute != null && (bestRoute.Count == 0 || nextRoute.Count < bestRoute.Count)) {
                bestRoute = nextRoute;
            }
        }

        return bestRoute.Count == 0 ? null : bestRoute;
    }
    
    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetBestRoute(Unit sourceUnit, Tile destTile, bool allowStomps) {
        List<Tile> bestRoute = new List<Tile>();
        List<Tile> nextRoute;

        foreach (Tile sourceTile in sourceUnit.Position) {
            nextRoute = GetBestRoute(sourceTile, destTile, allowStomps);
            if (nextRoute != null && (bestRoute.Count == 0 || nextRoute.Count < bestRoute.Count)) {
                bestRoute = nextRoute;
            }
        }

        return bestRoute.Count == 0 ? null : bestRoute;
    }

    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetBestRoute(Tile sourceTile, Tile destTile, bool allowStomps) {
        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        // https://www.youtube.com/watch?v=QhaKb5N3Hj8&index=5&list=PLbghT7MmckI55gwJLrDz0UtNfo9oC0K1Q
        
        if (sourceTile == destTile) {
            return null;
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        List<Tile> routeList = new List<Tile>();
        List<Tile> Q = new List<Tile>();
        int permutations = 0;

        if (sourceTile == destTile) {
            return new List<Tile>();
        }

        // init all tiles to have infinity distance from the sourceUnit
        foreach (Tile tile in GameBoard.Tiles) {
            dist[tile] = Mathf.Infinity;
            prev[tile] = null;
            Q.Add(tile);
        }

        dist[sourceTile] = 0.0f;

        while (Q.Count > 0) {
            permutations++;

            // set 'u' to the Tile with the shortest distance
            Tile u = null;
            foreach (Tile t in Q) {
                if (u == null || dist[t] < dist[u]) {
                    u = t;
                }
            }

            if (u == destTile) {
                break;
            }

            Q.Remove(u);

            foreach (var neighbor in u.Neighbors) {
                if (allowStomps || (!neighbor.IsOccupied && !neighbor.IsOccupied)) {
                    float alt = dist[u] + 1;

                    if (alt < dist[neighbor]) {
                        dist[neighbor] = alt;
                        prev[neighbor] = u;
                    }
                }
            }
        }

        if (prev[destTile] == null) {
            // either we have found the shortest route to destUnit, or there is
            // no valid route to the destUnit at all.
            return null;
        }

        // we found a valid route
        Tile curr = destTile;
        while (curr != null) {
            routeList.Add(prev[curr]);
            curr = prev[curr];
        }

        routeList.Reverse();
        stopwatch.Stop();

        if (showgDebugMessages) {
            Debug.Log("Pathfinding took " + permutations + " permutations (" + stopwatch.ElapsedMilliseconds + " ms).");
        }

        // why do we need to do this?
        routeList.Remove(routeList[0]);
        routeList.Add(destTile);

        return routeList;
    }
    
    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetTilesWithinRangeOfUnit(Unit unit, int range, bool allowStomps) {
        List<(Tile, int)> tileRanges = new List<(Tile, int)>();

        foreach (Tile startTile in unit.Position) {
            tileRanges.Add((startTile, 0));
        
            for (int currentRange = 0; currentRange < range; currentRange++) {
                for (int c = 0; c < tileRanges.Count; c++) {
                    if (tileRanges[c].Item2 == currentRange) {
                        foreach (Tile tile in tileRanges[c].Item1.Neighbors) {
                            if (allowStomps || !tile.IsOccupied) {
                                tileRanges.Add((tile, currentRange + 1));
                            }
                        }
                    }
                }
            }
        }

        List<Tile> result = new List<Tile>();
        foreach((Tile tile, int _) in tileRanges) {
            if (!result.Contains(tile) && !unit.Position.Contains(tile)) {
                result.Add(tile);
            }
        }

        return result;
    }
}
