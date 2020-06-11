using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoBox : MonoBehaviour {
    
    public Image PortraitImage;
    public Text HeadHpLabel;
    public Text TorsoHpLabel;
    public Text WaistHpLabel;
    public Text ArmsHpLabel;
    public Text LegsHpLabel;
    
    //----------------------------------------------------------------------------------------------
    public void ShowInfo(Unit unit) {

        PortraitImage.sprite = unit.UnitData.PortraitSprite;
        HeadHpLabel.text = unit.Body.GetHealthOfRegion(BodyHitLocation.Head).ToString();
        TorsoHpLabel.text = unit.Body.GetHealthOfRegion(BodyHitLocation.Torso).ToString();
        WaistHpLabel.text = unit.Body.GetHealthOfRegion(BodyHitLocation.Waist).ToString();
        ArmsHpLabel.text = unit.Body.GetHealthOfRegion(BodyHitLocation.Arms).ToString();
        LegsHpLabel.text = unit.Body.GetHealthOfRegion(BodyHitLocation.Legs).ToString();

    }
}
