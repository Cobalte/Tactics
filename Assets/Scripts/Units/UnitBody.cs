using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBody {
    
    private List<UnitBodyRegion> regions;

    //----------------------------------------------------------------------------------------------
    public void Initialize() {
        regions = new List<UnitBodyRegion> {
            new UnitBodyRegion {HitLocation = BodyHitLocation.Head, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Torso, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Waist, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.LeftArm, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.LeftLeg, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.RightArm, HitPoints = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.RightLeg, HitPoints = 2}
        };
    }

    //----------------------------------------------------------------------------------------------
    public void TakeDamage(int amount) {
        BodyHitLocation randomHitLoc = regions[Random.Range(0, regions.Count)].HitLocation;
        TakeDamage(randomHitLoc, amount);
    }
    
    //----------------------------------------------------------------------------------------------
    public void TakeDamage(BodyHitLocation location, int amount) {
        foreach (var region in regions.Where(region => region.HitLocation == location)) {
            region.HitPoints = Mathf.Max(region.HitPoints - amount, 0);
            Debug.Log(amount + " damage taken to " + location + " (" + region.HitPoints + " HP left).");
            if (region.HitPoints == 0) {
                ResolveInjury(location);
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public void ResolveInjury(BodyHitLocation location) {
        Debug.Log("TODO: Resolve injury to " + location + ".");
    }
}

//--------------------------------------------------------------------------------------------------
public class UnitBodyRegion {

    public BodyHitLocation HitLocation;
    public int HitPoints;

}

//--------------------------------------------------------------------------------------------------
public enum BodyHitLocation {
    Torso,
    Waist,
    Head,
    RightArm,
    RightLeg,
    LeftArm,
    LeftLeg
}