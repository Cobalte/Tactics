using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class AiController : MonoBehaviour {

    private Unit unit;
    
    //----------------------------------------------------------------------------------------------
    private void Start() {
        unit = gameObject.GetComponent<Unit>();
    }

    //----------------------------------------------------------------------------------------------
    public void ExecuteTurn() {
        // select us
        UnitRoster.SelectUnit(unit);
        
        // verify we have at least 1 ai ability - if we don't, bail        
        if (unit.UnitData.AiAbilities.Length == 0) {
            FinalizeTurn();
            return;
        }

        // choose a random ability
        AiAbility ability = unit.UnitData.AiAbilities[Random.Range(0, unit.UnitData.AiAbilities.Length - 1)];

        // find the best target
        switch (ability.TargetType) {
            case AiAbilityTargetType.ClosestThreat:
                // find the closest unit
                List<Tile> route;
                List<Tile> closestRoute = new List<Tile>();
                Tile targetTile = unit.Position[0]; // we have to init something - we change this soon

                foreach (Unit target in UnitRoster.Units) {
                    if (target.UnitData.Owner != unit.UnitData.Owner) {
                        route = Pathfinder.GetBestRoute(unit, target, true);
                        Debug.Log("Distance to " + target.UnitData.DisplayName + ": " + route.Count);
                        if (closestRoute.Count == 0 || route.Count < closestRoute.Count) {
                            closestRoute = route;
                            targetTile = closestRoute[closestRoute.Count - 1];
                        }
                    }
                }

                // the route currently includes the tiles occupied by the source and target units. 
                // remove those to get the tiles we actually want to move along.
                closestRoute.RemoveAt(0);
                closestRoute.RemoveAt(closestRoute.Count - 1);
                
                // verify we found a target and it's actually in range - if not, bail
                if (closestRoute.Count > ability.MoveRange) {
                    break;
                }
                
                // if we're not already adjacent to our target, move to them
                if (closestRoute.Count > 0) {
                    UnitRoster.IssueSelectedUnitMoveOrder(closestRoute[closestRoute.Count - 1]);    
                }
                
                // use the ability on the tile where our target is standing
                ability.Execute(targetTile);
                
                break;
            default:
                Debug.LogError("Unhandled ability target type '" + ability.TargetType + "'. Skipping turn.");
                break;
        }
        
        FinalizeTurn();
    }
    
    //----------------------------------------------------------------------------------------------
    private void FinalizeTurn() {
        unit.HasActed = true;
        unit.HasMoved = true;
    }
}
