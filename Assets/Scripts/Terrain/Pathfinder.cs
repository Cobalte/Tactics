﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    private static bool showgDebugMessages = false;

    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetBestRoute(Unit mover, Tile dest, bool stomp) {
        List<Tile> bestRoute = new List<Tile>();
        List<Tile> nextRoute;

        for (int i = 0; i < mover.CurrentTiles.Count; i++) {
            nextRoute = GetBestRoute(mover.CurrentTiles[i], dest, stomp);
            if (i == 0 || nextRoute.Count < bestRoute.Count) {
                bestRoute = nextRoute;
            }
        }

        return bestRoute.Count == 0 ? null : bestRoute;
    }

    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetBestRoute(Tile source, Tile dest, bool stomp) {
        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        // https://www.youtube.com/watch?v=QhaKb5N3Hj8&index=5&list=PLbghT7MmckI55gwJLrDz0UtNfo9oC0K1Q
        
        if (source == dest) {
            return null;
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        List<Tile> routeList = new List<Tile>();
        List<Tile> Q = new List<Tile>();
        int permutations = 0;

        if (source == dest) {
            return new List<Tile>();
        }

        // init all tiles to have infinity distance from the source
        foreach (Tile tile in GameBoard.Tiles) {
            dist[tile] = Mathf.Infinity;
            prev[tile] = null;
            Q.Add(tile);
        }

        dist[source] = 0.0f;

        while (Q.Count > 0) {
            permutations++;

            // set 'u' to the Tile with the shortest distance
            Tile u = null;
            foreach (Tile t in Q) {
                if (u == null || dist[t] < dist[u]) {
                    u = t;
                }
            }

            if (u == dest) {
                break;
            }

            Q.Remove(u);

            foreach (var neighbor in u.Neighbors) {
                if (stomp || (neighbor.IsPathable && !neighbor.IsOccupied)) {
                    float alt = dist[u] + 1;

                    if (alt < dist[neighbor]) {
                        dist[neighbor] = alt;
                        prev[neighbor] = u;
                    }
                }
            }
        }

        if (prev[dest] == null) {
            // either we have found the shortest route to dest, or there is
            // no valid route to the dest at all.
            return null;
        }

        // we found a valid route
        Tile curr = dest;
        while (curr != null) {
            routeList.Add(prev[curr]);
            curr = prev[curr];
        }
        routeList.Reverse();

        stopwatch.Stop();

        if (showgDebugMessages) {
            Debug.Log("Pathfinding took " +
                permutations + " permutations (" +
                stopwatch.ElapsedMilliseconds + " ms)."
            );
        }

        // why do we need to do this?
        routeList.Remove(routeList[0]);
        routeList.Add(dest);

        return routeList;
    }
    
    //----------------------------------------------------------------------------------------------
    public static List<Tile> GetTilesWithinMoveRange(Unit mover, int moveRange, bool stomp) {
        List<(Tile, int)> tileRanges = new List<(Tile, int)>();

        foreach (Tile startTile in mover.CurrentTiles) {
            tileRanges.Add((startTile, 0));
        
            for (int currentRange = 0; currentRange < moveRange; currentRange++) {
                for (int c = 0; c < tileRanges.Count; c++) {
                    if (tileRanges[c].Item2 == currentRange) {
                        foreach (Tile tile in tileRanges[c].Item1.Neighbors) {
                            if (stomp || IsTileOpen(tile)) {
                                tileRanges.Add((tile, currentRange + 1));
                            }
                        }
                    }
                }
            }
        }

        List<Tile> result = new List<Tile>();
        foreach((Tile tile, int _) in tileRanges) {
            if (!result.Contains(tile) && !mover.CurrentTiles.Contains(tile)) {
                result.Add(tile);
            }
        }

        return result;
    }
    
    //----------------------------------------------------------------------------------------------
    private static bool IsTileInTupleList(Tile tile, List<(Tile, int)> list) {
        foreach(var tuple in list) {
            if (tuple.Item1 == tile) {
                return true;
            }
        }
         
        return false;
    }
    
    //----------------------------------------------------------------------------------------------
    private static bool IsTileOpen(Tile tile) {
        return !tile.IsOccupied && tile.IsPathable;
    }
}