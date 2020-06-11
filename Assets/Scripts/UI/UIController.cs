using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public static class UiController {
    
    private static GameController gameController;
    private static GameObject hoverHighlight;
    private static CanvasReferences canvasReferences;
    private static List<GameObject> debugRouteObjs;
    private static UnitInfoBox unitInfoBox;

    private static bool showRouteDebug = true;

    //----------------------------------------------------------------------------------------------
    public static void Initialize() {
        unitInfoBox = GameObject.Find("Unit Info Box").GetComponent<UnitInfoBox>();
        canvasReferences = GameObject.Find("Canvas").GetComponent<CanvasReferences>();
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        hoverHighlight = PrefabUtility.InstantiatePrefab(gameController.HoverHighlightPrefab) as GameObject;
        debugRouteObjs = new List<GameObject>();
        
        canvasReferences.lowerLeftFrame.SetActive(false);
        hoverHighlight.SetActive(false);
    }

    //----------------------------------------------------------------------------------------------
    public static void HighlightTile(Tile tile) {
        if (tile != null) {
            hoverHighlight.SetActive(true);
            hoverHighlight.transform.position = tile.transform.position;
        }
        else {
            hoverHighlight.SetActive(false);
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void ShowUnitPanel(Unit unit) {
        if (unit == null) {
            canvasReferences.lowerLeftFrame.SetActive(false);
            return;
        }
        
        canvasReferences.lowerLeftFrame.SetActive(true);
        unitInfoBox.ShowInfo(unit);

        // show as many abilities as the unit has
        for (int i = 0; i < canvasReferences.abilityFrames.Length; i++) {
            if (i < unit.UnitData.Abilities.Length) {
                canvasReferences.abilityFrames[i].gameObject.SetActive(true);
                canvasReferences.abilityFrames[i].Highlight.enabled = false;
                canvasReferences.abilityFrames[i].Icon.sprite = unit.UnitData.Abilities[i].Icon;
                canvasReferences.abilityFrames[i].NameText.text = unit.UnitData.Abilities[i].DisplayName;
                canvasReferences.abilityFrames[i].RangeText.text = unit.UnitData.Abilities[i].Range.ToString();
                canvasReferences.abilityFrames[i].DamageText.text = unit.UnitData.Abilities[i].Damage.ToString();
            }
        }

        // hide the rest of the ability frames
        for (int i = unit.UnitData.Abilities.Length; i < canvasReferences.abilityFrames.Length; i++) {
            canvasReferences.abilityFrames[i].gameObject.SetActive(false);
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void ShowAbilityFrameHighlight(int frameIndex) {
        HideAbilityFrameHighlights();
        canvasReferences.abilityFrames[frameIndex].Highlight.enabled = true;
    }
    
    //----------------------------------------------------------------------------------------------
    public static void HideAbilityFrameHighlights() {
        foreach (var frame in canvasReferences.abilityFrames) {
            frame.Highlight.enabled = false;
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public static void CreateDebugText(string text, Vector3 position) {
        if (!showRouteDebug) {
            return;
        }
        
        GameObject newObj = PrefabUtility.InstantiatePrefab(canvasReferences.DebugTextPrefab) as GameObject;
        debugRouteObjs.Add(newObj);
        newObj.name = "Debug Text: " + text;
        newObj.transform.parent = canvasReferences.transform;
        newObj.transform.position = Camera.main.WorldToScreenPoint(position);
        newObj.GetComponent<Text>().text = text;
    }
    
    //----------------------------------------------------------------------------------------------
    public static void ClearDebugText() {
        for (int i = 0; i < debugRouteObjs.Count; i++) {
            MonoBehaviour.DestroyImmediate(debugRouteObjs[i]);
        }
    }
}
