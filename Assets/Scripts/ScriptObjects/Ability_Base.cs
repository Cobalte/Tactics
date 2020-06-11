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
    public void Execute(Tile source, Tile target) {
        if (target.IsOccupied) {
            target.GetOccupyingUnit().Body.TakeDamage(Damage);
        }
    }
}
