using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/Injury Data", order = 1)]
public class InjuryData : ScriptableObject {

    public string Name;
    public string Description;
    public BodyHitLocation HitLocation;
    public InjuryEffectType Type;
    public int Value;

}

//--------------------------------------------------------------------------------------------------
public enum InjuryEffectType {
    MovementRange,
    OutgoingDamage,
    IncomingDamage,
    MissChance,
    DodgeChance
}
