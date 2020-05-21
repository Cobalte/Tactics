using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBanner : MonoBehaviour {

    public float DisplayTime;
    
    //----------------------------------------------------------------------------------------------
    private void OnEnable() {
        StartCoroutine(DelayThenHide());
    }

    //----------------------------------------------------------------------------------------------
    IEnumerator DelayThenHide() {
        yield return new WaitForSeconds(DisplayTime);
        gameObject.SetActive(false);
    }
}
