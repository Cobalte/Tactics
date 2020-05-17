using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Unit AssignedUnit;
    public float VerticalOffset;
    public GameObject HealthCellPrefab;
    public Color HealthyColor;
    public Color DamagedColor;

    private Transform cellParent;
    private List<GameObject> cellObjs;
    private List<Image> cellImages;

    //----------------------------------------------------------------------------------------------
    private void Awake() {
        cellParent = transform.GetChild(0);
        cellObjs = new List<GameObject>();
        cellImages = new List<Image>();
    }

    //----------------------------------------------------------------------------------------------
    private void Update() {
        if (AssignedUnit == null) {
            return;
        }

        UpdateCellCount();
        UpdateCellColors();
        UpdateScreenPosition();
    }

    //----------------------------------------------------------------------------------------------
    private void UpdateCellCount() {
        if (cellObjs.Count == AssignedUnit.UnitData.MaxHealth) {
            return;
        }
        
        // add as many as we need
        while (cellObjs.Count < AssignedUnit.UnitData.MaxHealth) {
            
            GameObject newCell = PrefabUtility.InstantiatePrefab(HealthCellPrefab) as GameObject;
            newCell.transform.parent = cellParent.transform;
            cellObjs.Add(newCell);
            cellImages.Add(newCell.GetComponent<Image>());
        }

        // destroy as many as we need
        while (cellObjs.Count > AssignedUnit.UnitData.MaxHealth) {
            cellImages.RemoveAt(cellImages.Count - 1);
            Destroy(cellObjs[cellObjs.Count - 1]);
        }
    }

    //----------------------------------------------------------------------------------------------
    private void UpdateCellColors() {
        for (int i = 0; i < cellImages.Count; i++) {
            cellImages[i].color = i <= AssignedUnit.CurrentHealth - 1 ? HealthyColor : DamagedColor;
        }
    }
    
    //----------------------------------------------------------------------------------------------
    private void UpdateScreenPosition() {
        transform.position = Camera.main.WorldToScreenPoint(AssignedUnit.transform.position) + Vector3.up * VerticalOffset;
    }
}
