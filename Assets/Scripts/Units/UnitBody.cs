using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBody {
    
    private List<UnitBodyRegion> regions;

    //----------------------------------------------------------------------------------------------
    public void Initialize() {
        regions = new List<UnitBodyRegion> {
            new UnitBodyRegion {HitLocation = BodyHitLocation.Head, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Torso, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Waist, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Arms, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Legs, MaxHealth = 2, CurrentHealth = 2},
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
            region.CurrentHealth = Mathf.Max(region.CurrentHealth - amount, 0);
            Debug.Log(amount + " damage taken to " + location + " (" + region.CurrentHealth + " HP left).");
            if (region.CurrentHealth == 0) {
                ResolveInjury(location);
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------
    public void ResolveInjury(BodyHitLocation location) {
        Debug.Log("TODO: Resolve injury to " + location + ".");
    }
    
    //----------------------------------------------------------------------------------------------
    public int GetCurrentHealth(BodyHitLocation location) {
        return regions.Where(region => region.HitLocation == location).Select(region => region.CurrentHealth).FirstOrDefault();
    }
    
    //----------------------------------------------------------------------------------------------
    public int GetMaxtHealth(BodyHitLocation location) {
        return regions.Where(region => region.HitLocation == location).Select(region => region.MaxHealth).FirstOrDefault();
    }
}

//--------------------------------------------------------------------------------------------------
public class UnitBodyRegion {
    public BodyHitLocation HitLocation;
    public int MaxHealth;
    public int CurrentHealth;
}

//--------------------------------------------------------------------------------------------------
public enum BodyHitLocation {
    Torso,
    Waist,
    Head,
    Arms,
    Legs
}