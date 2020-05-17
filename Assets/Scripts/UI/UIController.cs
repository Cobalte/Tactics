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

    private static bool showRouteDebug = true;

    private static HitLocationPanel hitPanel => canvasReferences.HitLocationPanel;

    //----------------------------------------------------------------------------------------------
    public static void Initialize() {
        canvasReferences = GameObject.Find("Canvas").GetComponent<CanvasReferences>();
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        hoverHighlight = PrefabUtility.InstantiatePrefab(gameController.HoverHighlightPrefab) as GameObject;
        debugRouteObjs = new List<GameObject>();
        
        canvasReferences.portraitBackground.SetActive(false);
        canvasReferences.lowerLeftFrame.SetActive(false);
        hitPanel.gameObject.SetActive(false);
        hoverHighlight.SetActive(false);

        if (canvasReferences.abilityFrames.Length != canvasReferences.abilityIcons.Length ||
            canvasReferences.abilityFrames.Length != canvasReferences.abilityFrameHighlights.Length) {

            Debug.LogWarning("There aren't an equal number of ability frames, ability icons, " +
                             "and ability frame highlights set up in CanvasReferences. Fix that."
            );
        }
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
        hitPanel.gameObject.SetActive(false);
        
        if (unit == null) {
            canvasReferences.portraitBackground.SetActive(false);
            canvasReferences.lowerLeftFrame.SetActive(false);
            return;
        }
        
        // show the portrait
        canvasReferences.portraitImage.sprite = unit.UnitData.PortraitSprite;
        canvasReferences.portraitBackground.SetActive(true);

        // show as many abilities as the unit has
        canvasReferences.lowerLeftFrame.SetActive(true);
        for (int i = 0; i < canvasReferences.abilityFrames.Length; i++) {
            if (i < unit.UnitData.Abilities.Length) {
                canvasReferences.abilityFrames[i].gameObject.SetActive(true);
                canvasReferences.abilityIcons[i].sprite = unit.UnitData.Abilities[i].Icon;
                canvasReferences.abilityFrameHighlights[i].enabled = false;
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
        canvasReferences.abilityFrameHighlights[frameIndex].enabled = true;
    }
    
    //----------------------------------------------------------------------------------------------
    public static void HideAbilityFrameHighlights() {
        foreach (var highlight in canvasReferences.abilityFrameHighlights) {
            highlight.enabled = false;
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
    
    //----------------------------------------------------------------------------------------------
    public static void ShowHitPanel(OperationContext context) {
        canvasReferences.lowerLeftFrame.SetActive(false);
        
        hitPanel.gameObject.SetActive(true);
        hitPanel.Show(context);
    }
}
