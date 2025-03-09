using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gun : MonoBehaviour
{
    [SerializeField] private GunData data;

    private GunAimer aimer;
    private GunShooter shooter;
    private GunAmmo ammo;
    private GunEffects effects;

    private float lastFired;
    private float shotCooldown;
    public bool IsReloading => ammo.IsReloading;

    
    private void Awake()
    {
        lastFired = 0f;
        shotCooldown = 1f / data.fireRate;

        aimer = new GunAimer(transform, data);
        ammo = new GunAmmo((int)data.clipSize);
        shooter = new GunShooter(data, ammo, aimer, this.transform);
        effects = new GunEffects(data);
    }

    private void LateUpdate()
    {
        if (Time.time - lastFired > (shotCooldown / 2))
            aimer.DecaySpread();
    }

    public float AimAt(Vector2 targetDir) => aimer.AimAt(targetDir);
    
    public bool Fire(Vector2 targetDir)
    {
        if (ammo.IsReloading || !shooter.CanShoot(lastFired, shotCooldown))
            return false;

        lastFired = Time.time;
        bool success = shooter.Fire(targetDir);
        effects.PlayGunshotSound(transform);
        effects.SpawnMuzzleFlash(transform);

        if (ammo.IsEmpty)
            Reload();

        return success;
    }

    public float LastFired(float time) => this.lastFired = time;

    public void Reload() => ammo.Reload(() => effects.PlayReloadSound(transform));

    public GunData GetGunData() => data;

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