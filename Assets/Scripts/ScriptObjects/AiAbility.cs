using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/AiAbility", order = 1)]
public class AiAbility : ScriptableObject {

    public AiAbilityTargetType TargetType;
    public int MoveRange;
    public int Damage;

    //----------------------------------------------------------------------------------------------
    public void Execute(Tile target) {
        if (target.IsOccupied) {
            target.GetOccupyingUnit().Body.TakeDamage(Damage);
        }
    }
    
}

//--------------------------------------------------------------------------------------------------

public enum AiAbilityTargetType {
    ClosestThreat
}