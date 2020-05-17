using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class UnitRoster
{
    public static List<Unit> Units { get; private set; }
    public static Unit SelectedUnit { get; private set; }
    public static Ability_Base SelectedAbility { get; private set; }
    
    private static GameController gameController;
    private static CanvasReferences canvasReferences;

    //----------------------------------------------------------------------------------------------
    public static void Initialize() {
        Units = new List<Unit>();
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        canvasReferences = gameController.CanvasReferences;
    }
 
    //----------------------------------------------------------------------------------------------
    public static void RegisterUnit(Unit unit) {
        
        Units.Add(unit);
        unit.SetPosition(GameBoard.GetTilesClosetToPoint(
            unit.transform.position,
            unit.UnitData.TileDiameter * unit.UnitData.TileDiameter
        ));

        GameObject healthBar = PrefabUtility.InstantiatePrefab(canvasReferences.HealthBar) as GameObject;
        healthBar.transform.parent = canvasReferences.transform;
        healthBar.gameObject.name = "Health Bar: " + unit.UnitData.DisplayName;
        healthBar.GetComponent<HealthBar>().AssignedUnit = unit;
    }
    
    //----------------------------------------------------------------------------------------------
    public static void SelectUnit(Unit unit) {
        SelectedUnit = unit;
        UiController.ShowUnitPanel(unit);
        GameBoard.ShowTileHighlightsForUnit(unit);
    }
    
    //----------------------------------------------------------------------------------------------
    public static void SelectAbilityForSelectedUnit(int abilityIndex) {
        SelectedAbility = SelectedUnit.UnitData.Abilities[abilityIndex];
        
        GameBoard.ShowTileHighlightsForAbility(
            SelectedUnit.UnitData.Abilities[
                canvasReferences.abilityFrames[abilityIndex].abilityIndex
            ],
            SelectedUnit
        );
                
        UiController.ShowAbilityFrameHighlight(abilityIndex);
    }
    
    //----------------------------------------------------------------------------------------------
    public static void IssueSelectedUnitMoveOrder(Tile destination) {
        UiController.HideAbilityFrameHighlights();
        bool stomp = SelectedUnit.UnitData.Owner == Player.Machines; 
        List<Tile> bestPath = Pathfinder.GetBestRoute(SelectedUnit, destination, stomp);
        
        // show route debug info, if it is enabled
        UiController.ClearDebugText();
        for (int i = 0; i < bestPath.Count; i++) {
            UiController.CreateDebugText((i).ToString(), bestPath[i].transform.position);
        }

        // if we ran over someone, move them out of the way
        if (stomp) {
            // assemble a path for each tile the unit occupies - really only useful if larger than 1x1 
            List<BoardDirection> stepDirs = new List<BoardDirection>();
            List<List<Tile>> allPaths = new List<List<Tile>>();
            
            for (int i = 1; i < bestPath.Count; i++) {
                stepDirs.Add(GameBoard.GetDirectionTowardTile(bestPath[i - 1], bestPath[i]));
            }

            foreach (Tile startTile in SelectedUnit.CurrentTiles) {
                List<Tile> newPath = new List<Tile> { startTile };
                foreach (BoardDirection dir in stepDirs) {
                    newPath.Add(newPath.Last().GetNeighborInDirection(dir));
                }
                allPaths.Add(newPath);
            }

            for (int i = 0; i < bestPath.Count; i++) {
                foreach (List<Tile> nextPath in allPaths) {
                    Unit occupyingUnit = nextPath[i].GetOccupyingUnit();
                    if (occupyingUnit != null && occupyingUnit != SelectedUnit) {
                        PushUnit(occupyingUnit, GameBoard.GetDirectionTowardTile(nextPath[i - 1], nextPath[i]));
                    }                    
                }
            }
        }
        
        // set our unit's new position
        if (SelectedUnit.UnitData.TileDiameter == 1) {
            SelectedUnit.SetPosition(destination);
        }
        else {
            Vector3 startOffsetFromUnit = SelectedUnit.transform.position - bestPath.First().transform.position;
            Vector3 tileScanPos = bestPath.Last().transform.position + startOffsetFromUnit;
            SelectedUnit.SetPosition(GameBoard.GetTilesClosetToPoint(tileScanPos, 4));
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void IssueSelectedAbilityOrder(Tile targetTile) {
        SelectedAbility.Execute(SelectedUnit.CurrentTiles[0], targetTile);
        
        // cleanup after the ability
        SelectedAbility = null;
        GameBoard.HideTileHighlights();
        UiController.HideAbilityFrameHighlights();
    }
    
    //----------------------------------------------------------------------------------------------
    private static void PushUnit(Unit unit, BoardDirection direction) {
        if (unit.UnitData.TileDiameter > 1) {
            Debug.LogWarning("Trying to push a unit larger than 1x1! PushUnit() is confused!");
        }
        Tile destTile = unit.CurrentTiles[0].GetNeighborInDirection(direction);
        if (destTile != null) {
            Unit dominoUnit = destTile.GetOccupyingUnit();
            if (dominoUnit != null) {
                PushUnit(dominoUnit, direction);
            }
            unit.SetPosition(destTile);
            unit.TakeDamage(1, DamageType.Pushed);
        }
        else {
            Debug.LogWarning("Trying to push a unit off the board! PushUnit() is confused!");
        }
    }
}
