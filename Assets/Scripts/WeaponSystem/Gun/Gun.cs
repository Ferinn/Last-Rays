using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gun : MonoBehaviour
{
    [SerializeField] private GunData data;

    private GunAimer aimer;
    private GunShooter shooter;
    private GunAmmo ammo;
    private GunModulator gunModulator;
    private GunEffects effects;

    private float lastFired;
    private float shotCooldown;
    public bool IsReloading => ammo.IsReloading;
    public GunStats stats;
    
    private void Awake()
    {
        lastFired = 0f;
        shotCooldown = 1f / data.fireRate;

        stats = new GunStats(data); //gunData required for gunModulator to function
        effects = new GunEffects(data); //barrelOffset not contained in gunStats
        gunModulator = new GunModulator(data, stats); //gunData required for gunModulator to function

        aimer = new GunAimer(transform, stats);
        ammo = new GunAmmo((int)stats.clipSize);
        shooter = new GunShooter(stats, ammo, aimer, this.transform);
    }

    private void LateUpdate()
    {
        if (Time.time - lastFired > (shotCooldown / 2))
            aimer.DecaySpread();
    }

    public float AimAt(Vector2 targetDir) => aimer.AimAt(targetDir);
    
    public bool Fire(Vector2 targetDir)
    {
        if (ammo.IsReloading || !shooter.CanShoot(lastFired, shotCooldown) || !ammo.UseBullet())
        {
            if (ammo.IsEmpty)
            {
                Reload();
            }
            return false;
        }

        lastFired = Time.time;
        shooter.Fire(targetDir);
        effects.PlayGunshotSound(transform);
        effects.SpawnMuzzleFlash(transform);

        return true;
    }

    public float LastFired(float time) => this.lastFired = time;

    public void Reload()
    {
        if (!ammo.IsReloading)
        {
            effects.PlayReloadSound(transform);
            ammo.Reload();
        }
    }

    public GunData GetGunData() => stats;

    public float GetTravelTime(Vector2 target)
    {
        return ((target - (Vector2)this.transform.position).magnitude) / data.bulletType.speed;
    }

    public float GetMaxRange()
    {
        return data.bulletType.speed * (data.bulletLife * 2);
    }

    public void SetAsPlayerGun() => effects.GlobaliseSFX();

    public float GetSpreadPercentage() => aimer.GetSpreadPercentage();
}