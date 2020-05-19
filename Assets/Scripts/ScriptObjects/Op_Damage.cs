using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/Op_Damage", order = 1)]
public class Op_Damage : Op_Base
{
    public int Amount;

    public override void Resolve(OperationContext context) {
        Unit affectedUnit = context.Target.GetOccupyingUnit();
        if (affectedUnit != null) {
            affectedUnit.TakeDamage(Amount, DamageType.Ability);
        }
    }
}
