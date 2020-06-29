using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectResizer : MonoBehaviour {

    public List<RectTransform> Parents;
    public int BorderWidth;

    private RectTransform ourRect;

    private void Start() {
        ourRect = GetComponent<RectTransform>();
    }

    private void Update() {
        foreach (RectTransform parentRect in Parents) {
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ourRect.sizeDelta.y + BorderWidth * 2);
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ourRect.sizeDelta.x + BorderWidth * 2);
        }
    }
}
