using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSystem", menuName = "GunData", order = 0)]
public class GunData : ScriptableObject
{
    public GunData Clone()
    {
        return Instantiate(this);
    }

    [Range(1, 60)] public float fireRate; //per second
    public float maxSpread = 15f; //in euler angles
    public float spreadIncrement = 2f; //per shot
    public float spreadDecayDur = 1f; //time till spread decays to zero from max
    public float bulletLife;
    public float power;
    public float clipSize;
    public float reloadDuration;
    public bool isAutomatic;
    public BulletType bulletType;
    public Vector2 barrelOffset;
    public int bulletsPerShot;
    public float spreadArc;
}