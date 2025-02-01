using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSystem", menuName = "Bullet", order = 1)]
public class BulletType : ScriptableObject
{
    public float speed;
    public float trailLengthMultiplier;
    public float damage;
    public Color color;
}
