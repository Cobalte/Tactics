using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Gun Cats Data/Unit_Base", order = 1)]
public class Unit_Base : ScriptableObject
{
    public string DisplayName = "New Unit";
    public int MaxHealth = 1;
    public int MoveRange;
    public int TileDiameter = 1;
    public Player Owner = Player.HumanPlayer;
    public Sprite BoardSprite;
    public Sprite PortraitSprite;
    public Ability_Base[] Abilities;
}
