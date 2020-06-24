using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/UnitData", order = 1)]
public class UnitData : ScriptableObject {
    
    public string DisplayName = "New Unit";
    public int MoveRange;
    public int TileDiameter = 1;
    public Player Owner = Player.HumanPlayer;
    public Sprite BoardSprite;
    public Sprite PortraitSprite;
    public Ability_Base[] Abilities;
    public AiAbility[] AiAbilities;

}
