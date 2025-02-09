using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "CharacterData",  order = 0)]
public class CharacterData : ScriptableObject
{
    public float maxHealth = 10;
    public float speed = 5;
    public float accuracyPercentage = 60;
}
