﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData UnitData;

    [HideInInspector] public bool HasMoved;
    [HideInInspector] public bool HasActed;
    [HideInInspector] public UnitBody Body;
    
    public List<Tile> Position { get; private set; }
    public bool IsDead { get; private set; } 
    
    private GameObject healthBar;
    public CanvasReferences canvasReferences;

    public int MoveRange => UnitData.MoveRange + Body.GetInjuryModifiers(InjuryEffectType.MovementRange);

    //----------------------------------------------------------------------------------------------
    private void Start() {
        if (UnitData == null) {
            throw new Exception("Unit '" + gameObject.name + "' tried to Start() with no unit data.");
        }
        
        UnitRoster.RegisterUnit(this);
        Body = new UnitBody();
        Body.Initialize(this);
    }
    
    //----------------------------------------------------------------------------------------------
    public void SetPosition(Tile destination) {
        SetPosition(new List<Tile> { destination });
    }
    
    //----------------------------------------------------------------------------------------------
    public void SetPosition(List<Tile> destination) {
        transform.position = GetCenterVector(from pos in destination select pos.transform.position);
        Position = destination;
        GameBoard.HideHighlights();
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
    
    //----------------------------------------------------------------------------------------------
    public void Die() {
        IsDead = true;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.material = canvasReferences.DeadUnitMaterial;
        Debug.Log(UnitData.DisplayName + " has taken fatal damage and died.");
    }
}
 