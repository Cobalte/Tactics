using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Unit_Base UnitData;

    public List<Tile> CurrentTiles { get; private set; }
    public int CurrentHealth { get; private set; }

    private GameObject healthBar;
    private int pushDamageTaken = 0;

    //----------------------------------------------------------------------------------------------
    private void Start() {
        if (UnitData == null) {
            throw new Exception("Unit '" + gameObject.name + "'tried to Start() with no unit data.");
        }
        
        UnitRoster.RegisterUnit(this);
        CurrentHealth = UnitData.MaxHealth;
    }
    
    //----------------------------------------------------------------------------------------------
    private void Update() {
        if (pushDamageTaken != 0) {
            // take push damage only once per tick
            ActuallyTakeDamage(pushDamageTaken);
            pushDamageTaken = 0;
        }
    }

    //----------------------------------------------------------------------------------------------
    public void SetPosition(Tile destination) {
        SetPosition(new List<Tile> { destination });
    }
    
    //----------------------------------------------------------------------------------------------
    public void SetPosition(List<Tile> destination) {
        transform.position = GetCenterVector(from pos in destination
                                             select pos.transform.position);
        CurrentTiles = destination;
        GameBoard.HideHighlights();
    }
    
    //----------------------------------------------------------------------------------------------
    public void TakeDamage(int amount, DamageType type) {
        if (type == DamageType.Pushed) {
            pushDamageTaken = Math.Max(pushDamageTaken, amount);
        }
        else {
            ActuallyTakeDamage(amount);
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private void ActuallyTakeDamage(int amount) {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        Debug.Log(UnitData.DisplayName + " took " + amount + " damage.");

        if (CurrentHealth == 0) {
            Debug.Log(UnitData.DisplayName + " falls.");
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private void OnValidate() {
        // update the board sprite
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        if (UnitData != null && UnitData.BoardSprite != null) {
            sr.sprite = UnitData.BoardSprite;
            sr.enabled = true;
        }
        else {
            sr.enabled = false;
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private Vector3 GetCenterVector(IEnumerable<Vector3> points) {
        Vector3 result = new Vector3(0f, 0f, 0f);
        foreach (Vector3 point in points) {
            result += point;
        }
        return result / points.Count();
    }
}
 