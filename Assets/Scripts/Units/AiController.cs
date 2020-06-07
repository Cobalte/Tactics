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

                foreach (Unit target in UnitRoster.Units) {
                    if (target.UnitData.Owner != unit.UnitData.Owner && target.CurrentHealth > 0) {
                        route = Pathfinder.GetBestRoute(unit, target, true);
                        Debug.Log("Distance to " + target.UnitData.DisplayName + ": " + route.Count);
                        if (closestRoute.Count == 0 || route.Count < closestRoute.Count) {
                            closestRoute = route;
                        }
                    }
                }

                // verify we found a target and it's actually in range - if not, bail.
                // since the route list includes the source and target tiles, we +2 the range here
                if (closestRoute.Count == 0 || closestRoute.Count > ability.MoveRange + 2) {
                    break;
                }
                
                // move to the target (not on top of them, as the last tile in the route will have you do)
                UnitRoster.IssueSelectedUnitMoveOrder(closestRoute[closestRoute.Count - 2]);
                
                // use the ability on the tile where our target is standing
                ability.Execute(closestRoute[closestRoute.Count - 1]);
                
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
