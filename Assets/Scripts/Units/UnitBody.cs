using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitBody {

    private Unit hostUnit;
    public List<InjuryData> Injuries;
    private List<UnitBodyRegion> regions;
    private InjuryChart injuryChart;

    //--------------------------------------------------------------------------------------------------------
    public void Initialize(Unit host) {
        hostUnit = host;
        
        regions = new List<UnitBodyRegion> {
            new UnitBodyRegion {HitLocation = BodyHitLocation.Head, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Torso, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Waist, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Arms, MaxHealth = 2, CurrentHealth = 2},
            new UnitBodyRegion {HitLocation = BodyHitLocation.Legs, MaxHealth = 2, CurrentHealth = 2},
        };
        
        Injuries = new List<InjuryData>();
        injuryChart = GameObject.Find("Game Controller").GetComponent<InjuryChart>();

        if (injuryChart == null) {
            throw new Exception("Unit body on '" + hostUnit.UnitData.name + 
                                "' tried to Initialize() without an injury chart.");
        }
    }

    //--------------------------------------------------------------------------------------------------------
    public void TakeDamage(int amount) {
        BodyHitLocation randomHitLoc = regions[Random.Range(0, regions.Count)].HitLocation;
        TakeDamage(randomHitLoc, amount);
    }
    
    //--------------------------------------------------------------------------------------------------------
    public void TakeDamage(BodyHitLocation location, int amount) {
        foreach (var region in regions.Where(region => region.HitLocation == location)) {
            region.CurrentHealth = Mathf.Max(region.CurrentHealth - amount, 0);
            Debug.Log(hostUnit.UnitData.DisplayName + ": " +  amount + " damage taken to " + location);
            
            if (region.CurrentHealth == 0) {
                GainInjury(location);
            }
            
            // if we have 3 injuries, die
            if (Injuries.Count >= 3) {
                hostUnit.Die();
            }
        }
    }
    
    //--------------------------------------------------------------------------------------------------------
    public void GainInjury(BodyHitLocation location) {
        InjuryData injury = injuryChart.GetRandomInjury(location); 
        Injuries.Add(injury);
        
        Debug.Log(hostUnit.UnitData.DisplayName + ": New " + location + " injury gained: " + injury.Name + " (" +
                  injury.Description + ").");
    }
    
    //--------------------------------------------------------------------------------------------------------
    public int GetCurrentHealth(BodyHitLocation location) {
        return regions.Where(region => region.HitLocation == location).Select(region => region.CurrentHealth).FirstOrDefault();
    }
    
    //--------------------------------------------------------------------------------------------------------
    public int GetMaxtHealth(BodyHitLocation location) {
        return regions.Where(region => region.HitLocation == location).Select(region => region.MaxHealth).FirstOrDefault();
    }

    //--------------------------------------------------------------------------------------------------------
    public int GetInjuryModifiers(InjuryEffectType type) {
        return Injuries.Where(injury => injury.Type == type).Sum(injury => injury.Value);
    }
}

//------------------------------------------------------------------------------------------------------------
public class UnitBodyRegion {
    public BodyHitLocation HitLocation;
    public int MaxHealth;
    public int CurrentHealth;
}

//------------------------------------------------------------------------------------------------------------
public enum BodyHitLocation {
    Torso,
    Waist,
    Head,
    Arms,
    Legs
}