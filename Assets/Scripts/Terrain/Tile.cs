using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour {
    public List<Tile> Neighbors;
    public List<BoardDirection> NeighborDirections;
    public bool IsPathable = true;
    public GameObject Highlight;

    private bool IsMarked => Highlight.activeSelf;
    public bool IsOccupied => GetOccupyingUnit() != null;

    //----------------------------------------------------------------------------------------------
    private void OnMouseEnter() {
        UiController.HighlightTile(this);
    }

    //----------------------------------------------------------------------------------------------
    private void OnMouseDown() {
        if (IsMarked) {
            // if a unit has an ability selected, execute it on this tile
            if (UnitRoster.SelectedAbility != null) {
                UnitRoster.IssueSelectedAbilityOrder(targetTile:this);
            }
            // if a unit is selected, move it to this tile
            else if (UnitRoster.SelectedUnit != null) {
                UnitRoster.IssueSelectedUnitMoveOrder(this);
            }
            else {
                Debug.LogError("User clicked a marked tile with no selected unit or ability.");
            }
        }
        // if there's a unit here, select it
        else if (GetOccupyingUnit() != null) {
            UnitRoster.SelectUnit(GetOccupyingUnit());
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public Unit GetOccupyingUnit() {
        return UnitRoster.Units.FirstOrDefault(unit => unit.CurrentTiles.Contains(this));
    }
    
    //----------------------------------------------------------------------------------------------
    public void SetHighlightActive(bool isActive) {
        Highlight.SetActive(isActive);
    }
    
    //----------------------------------------------------------------------------------------------
    public Tile GetNeighborInDirection(BoardDirection dir) {
        return Neighbors.Where((t, i) => NeighborDirections[i] == dir).FirstOrDefault();
    }
}
