using UnityEngine;

public class GunStats
{
    private GunData baseData;

    public float fireRate;
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

    public GunStats(GunData data)
    {
        baseData = data;
        ResetToBaseStats(data);
    }

    public void ResetToBaseStats(GunData data)
    {
        fireRate = data.fireRate;
        maxSpread = data.maxSpread;
        spreadIncrement = data.spreadIncrement;
        spreadDecayDur = data.spreadDecayDur;
        bulletLife = data.bulletLife;
        power = data.power;
        clipSize = data.clipSize;
        reloadDuration = data.reloadDuration;
        isAutomatic = data.isAutomatic;
        bulletType = data.bulletType;
        bulletsPerShot = data.bulletsPerShot;
        spreadArc = data.spreadArc;
    }

    // Addition operator for GunStats
    public static GunStats operator +(GunStats stats, GunModule module)
    {
        stats.fireRate += module.fireRate;
        stats.maxSpread += module.maxSpread;
        stats.spreadIncrement += module.spreadIncrement;
        stats.spreadDecayDur += module.spreadDecayDur;
        stats.bulletLife += module.bulletLife;
        stats.power += module.power;
        stats.clipSize += module.clipSize;
        stats.reloadDuration += module.reloadDuration;
        stats.bulletsPerShot += module.bulletsPerShot;
        stats.spreadArc += module.spreadArc;

        stats.isAutomatic = module.isAutomatic;
        if (module.bulletType != null)
            stats.bulletType = module.bulletType;

        return stats;
    }

    // Subtraction operator for GunStats
    public static GunStats operator -(GunStats stats, GunModule module)
    {
        stats.fireRate -= module.fireRate;
        stats.maxSpread -= module.maxSpread;
        stats.spreadIncrement -= module.spreadIncrement;
        stats.spreadDecayDur -= module.spreadDecayDur;
        stats.bulletLife -= module.bulletLife;
        stats.power -= module.power;
        stats.clipSize -= module.clipSize;
        stats.reloadDuration -= module.reloadDuration;
        stats.bulletsPerShot -= module.bulletsPerShot;
        stats.spreadArc -= module.spreadArc;

        return stats;
    }

    // Implicit conversion operator to GunData
    public static implicit operator GunData(GunStats stats)
    {
        GunData newGunData = stats.baseData.Clone();

        newGunData.fireRate = stats.fireRate;
        newGunData.maxSpread = stats.maxSpread;
        newGunData.spreadIncrement = stats.spreadIncrement;
        newGunData.spreadDecayDur = stats.spreadDecayDur;
        newGunData.bulletLife = stats.bulletLife;
        newGunData.power = stats.power;
        newGunData.clipSize = stats.clipSize;
        newGunData.reloadDuration = stats.reloadDuration;
        newGunData.isAutomatic = stats.isAutomatic;
        newGunData.bulletType = stats.bulletType;
        newGunData.bulletsPerShot = stats.bulletsPerShot;
        newGunData.spreadArc = stats.spreadArc;

        return newGunData;
    }
}