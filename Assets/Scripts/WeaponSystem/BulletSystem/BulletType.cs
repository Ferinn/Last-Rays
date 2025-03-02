using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSystem", menuName = "Bullet", order = 1)]
public class BulletType : ScriptableObject
{
    public float speed;
    public float trailLengthMultiplier;
    public float damage;
    
    //if it can shoot through target
    public bool piercing;
    public Color color;
}
