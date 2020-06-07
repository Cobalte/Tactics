using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    public GameObject HoverHighlightPrefab;
    public CanvasReferences CanvasReferences;
    
    //----------------------------------------------------------------------------------------------
    public void Awake() {
        UiController.Initialize();
        GameBoard.Initialize();
        UnitRoster.Initialize();
    }

    //----------------------------------------------------------------------------------------------
    public void Update() {
        HandlePlayerInputs();
    }

    //----------------------------------------------------------------------------------------------
    private void HandlePlayerInputs() {
        // handle global escape key
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UnitRoster.SelectUnit(null);
        }
        
        // if the user pressed a valid ability hotkey, select that ability
        for (int i = 0; i < CanvasReferences.abilityFrames.Length; i++) {
            if (CanvasReferences.abilityFrames[i].gameObject.activeSelf &&
                Input.GetKeyDown(CanvasReferences.abilityFrames[i].Hotkey)) {
                
                UnitRoster.SelectAbilityForSelectedUnit(i);
                break;
            }
        }
        
        // if the player pressed 'skip turn' then skip their turn
        if (Input.GetKeyDown(KeyCode.X)) {
            foreach (Unit unit in UnitRoster.GetUnitsForOwner(Player.HumanPlayer)) {
                unit.HasActed = true;
                unit.HasMoved = true;
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------
}
