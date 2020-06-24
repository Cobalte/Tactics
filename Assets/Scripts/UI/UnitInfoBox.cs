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
        HeadHpLabel.text = unit.Body.GetCurrentHealth(BodyHitLocation.Head) + "/" +
                           unit.Body.GetMaxtHealth(BodyHitLocation.Head);
        TorsoHpLabel.text = unit.Body.GetCurrentHealth(BodyHitLocation.Torso) + "/" +
                           unit.Body.GetMaxtHealth(BodyHitLocation.Torso);
        WaistHpLabel.text = unit.Body.GetCurrentHealth(BodyHitLocation.Waist) + "/" +
                            unit.Body.GetMaxtHealth(BodyHitLocation.Waist);
        ArmsHpLabel.text = unit.Body.GetCurrentHealth(BodyHitLocation.Arms) + "/" +
                           unit.Body.GetMaxtHealth(BodyHitLocation.Arms);
        LegsHpLabel.text = unit.Body.GetCurrentHealth(BodyHitLocation.Legs) + "/" +
                           unit.Body.GetMaxtHealth(BodyHitLocation.Legs);

    }
}
