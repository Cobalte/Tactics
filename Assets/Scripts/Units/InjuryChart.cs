using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InjuryChart : MonoBehaviour {

    public List<InjuryData> Injuries;

    //----------------------------------------------------------------------------------------------
    public InjuryData GetRandomInjury(BodyHitLocation hitLocation) {
        List<InjuryData> locInjuries = Injuries.Where(injury => injury.HitLocation == hitLocation).ToList();
        return locInjuries[Random.Range(0, locInjuries.Count - 1)];
    }
    
}

//--------------------------------------------------------------------------------------------------
