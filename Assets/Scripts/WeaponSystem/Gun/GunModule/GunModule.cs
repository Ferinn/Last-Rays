using UnityEngine;

public enum ModuleType
{
    Stacking = 0, //simply modifies gunData, can stack
    Modifier = 1, //modifies the effect of other modules
    Unique = 2, //special module, only one per weapon allowed
}

[CreateAssetMenu(fileName = "GunModule", menuName = "GunModule", order = 1)]
public class GunModule : ScriptableObject
{
    public ModuleType type;

    [Header("--- GunStats Additions ---")]
    [Range(0, 60)] public float fireRate;
    public float maxSpread;
    public float spreadIncrement;
    public float spreadDecayDur;
    public float bulletLife;
    public float power;
    public float clipSize;
    public float reloadDuration;
    public bool isAutomatic;
    public BulletType bulletType;
    public int bulletsPerShot;
    public float spreadArc;

    [Header("--- GunModule Modifiers ---")]
    public float modPower;
}