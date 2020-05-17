using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HitLocationPanel : MonoBehaviour
{
    public GameObject[] BoxObjects;
    public int BaseGlanceRating;
    public int BaseCritRating;
    public Image casterImage;
    public Image targetImage;

    private List<HitType> hitLocations;
    private OperationContext hitOperationContext;

    //--------------------------------------------------------------------------------------------------------
    private void Awake() {
        hitLocations = new List<HitType>();
        foreach (GameObject boxObj in BoxObjects) {
            hitLocations.Add(HitType.Hit);
        }
    }

    //--------------------------------------------------------------------------------------------------------
    public void Show(OperationContext context) {
        hitOperationContext = context;        
        casterImage.sprite = context.Source.GetOccupyingUnit().UnitData.BoardSprite;
        targetImage.sprite = context.Target.GetOccupyingUnit().UnitData.BoardSprite;

        // reset boxes
        for (int i = 0; i < hitLocations.Count; i++) {
            hitLocations[i] = HitType.Hit;
        }
        
        // show glancing boxes
        for (int i = 0; i < BaseGlanceRating; i++) {
            BoxObjects[i].GetComponentInChildren<Text>().text = "G";
            hitLocations[i] = HitType.Glance;
        }
        
        // show critical boxes
        for (int i = 9; i >= 10 - BaseCritRating; i--) {
            BoxObjects[i].GetComponentInChildren<Text>().text = "C";
            hitLocations[i] = HitType.Critical;
        }
    }
    
    //--------------------------------------------------------------------------------------------------------
    public void Resolve() {
        int impactSite = Random.Range(0, hitLocations.Count);
        
        switch (hitLocations[impactSite]) {
            case HitType.Hit:
                Debug.Log("HIT on a roll of " + impactSite);
                break;
            case HitType.Glance:
                Debug.Log("GLANCE on a roll of " + impactSite);
                break;
            case HitType.Critical:
                Debug.Log("CRITICAL on a roll of " + impactSite);
                break;
        }
        
        UiController.ShowUnitPanel(hitOperationContext.Source.GetOccupyingUnit());
    }
}