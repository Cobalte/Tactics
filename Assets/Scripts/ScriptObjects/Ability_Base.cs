using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/Ability_Base", order = 1)]
public class Ability_Base : ScriptableObject {
    
    public string DisplayName;
    public Sprite Icon;
    public int Range;
    public int Damage;

    //----------------------------------------------------------------------------------------------
    public void Execute(Unit caster, Tile target) {
        Unit targetUnit = target.GetOccupyingUnit();
        
        if (targetUnit != null) {
            int damage = Damage +
                         caster.Body.GetInjuryModifiers(InjuryEffectType.OutgoingDamage) +
                         targetUnit.Body.GetInjuryModifiers(InjuryEffectType.IncomingDamage);

            damage = Mathf.Max(damage, 0);
            targetUnit.Body.TakeDamage(damage);
        }
    }
}
