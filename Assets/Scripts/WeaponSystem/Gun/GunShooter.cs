using UnityEngine;

public class GunShooter
{
    private GunData data;
    private GunAmmo ammo;
    private GunAimer aimer;

    private Transform gunTransform;

    public GunShooter(GunData data, GunAmmo ammo, GunAimer aimer, Transform gunTransform)
    {
        this.data = data;
        this.ammo = ammo;
        this.aimer = aimer;
        this.gunTransform = gunTransform;
    }


    public bool CanShoot(float lastFired, float cooldown) => (Time.time - lastFired) > cooldown;

    public bool Fire(Vector2 direction)
    {
        if (!ammo.UseBullet()) return false;

        float angleOffset = aimer.AimAt(direction);
        FireBullets(direction, angleOffset);
        return true;
    }

    private void FireBullets(Vector2 direction, float spread)
    {
        float startAngle = -(data.spreadArc / 2);
        float angleStep = data.bulletsPerShot > 1 ? data.spreadArc / (data.bulletsPerShot - 1) : 0;

        for (int i = 0; i < data.bulletsPerShot; i++)
        {
            float angleOffset = startAngle + (i * angleStep);
            Vector2 adjustedDirection = GunHelper.AddAngle2Vector(direction, angleOffset).normalized;

            ShotInfo shotInfo = new ShotInfo(data, data.power / data.bulletsPerShot, data.bulletType, Time.time, adjustedDirection, gunTransform.parent.position, gunTransform.parent.tag);
            Managers.bulletManager.DelegateFire(shotInfo);
        }
    }
}