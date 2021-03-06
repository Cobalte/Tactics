using System;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine;

public class TurnController : MonoBehaviour {

    public TurnBanner PlayerTurnBanner;
    public TurnBanner CpuTurnBanner;
    
    public int CurrentTurn { get; private set; }
    public Player CurrentPlayer { get; private set; }

    private AiController currentAi;
    
    //----------------------------------------------------------------------------------------------
    private void Awake() {
        CurrentTurn = 1;
        CurrentPlayer = Player.HumanPlayer;
        PlayerTurnBanner.gameObject.SetActive(true);
    }
    
    //----------------------------------------------------------------------------------------------
    private void Update() {
        // if there's a banner visible, do nothing
        if (PlayerTurnBanner.gameObject.activeSelf || CpuTurnBanner.gameObject.activeSelf) {
            return;
        }
        
        CheckForAi();
        CheckForTurnTransition();
    }
    
    //----------------------------------------------------------------------------------------------
    private void CheckForAi() {
        // do any units for the current player need to use ai?
        foreach (Unit unit in UnitRoster.Units) {
            if (unit.UnitData.Owner == CurrentPlayer && !unit.HasActed && !unit.HasMoved) {
                currentAi = unit.GetComponent<AiController>();
                if (currentAi != null) {
                    currentAi.ExecuteTurn();
                }
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private void CheckForTurnTransition() {
        // if any units for the current player have not yet moved or acted, do nothing
        foreach (Unit unit in UnitRoster.Units) {
            if (unit.UnitData.Owner == CurrentPlayer && !unit.IsDead && (!unit.HasMoved || !unit.HasActed)) {
                return;
            }
        }
        
        // end the turn for this player. if it's the player's turn next, also increment turn counter
        if (CurrentPlayer == Player.CpuPlayer) {
            PlayerTurnBanner.gameObject.SetActive(true);
            CurrentPlayer = Player.HumanPlayer;
            CurrentTurn++;
        }
        else {
            CpuTurnBanner.gameObject.SetActive(true);
            CurrentPlayer = Player.CpuPlayer;
        }

        // reset all units for the new player so they can move and act
        foreach (Unit unit in UnitRoster.Units.Where(unit => unit.UnitData.Owner == CurrentPlayer)) {
            unit.HasActed = false;
            unit.HasMoved = false;
        }
    }
}