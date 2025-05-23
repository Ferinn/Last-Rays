using UnityEngine;

public class GunShooter
{
    private GunStats stats;
    private GunAmmo ammo;
    private GunAimer aimer;

    private Transform gunTransform;

    public GunShooter(GunStats stats, GunAmmo ammo, GunAimer aimer, Transform gunTransform)
    {
        this.stats = stats;
        this.ammo = ammo;
        this.aimer = aimer;
        this.gunTransform = gunTransform;
    }


    public bool CanShoot(float lastFired, float cooldown) => (Time.time - lastFired) > cooldown;

    public void Fire(Vector2 direction)
    {
        float angleOffset = aimer.AimAt(direction);
        FireBullets(direction, angleOffset);
    }

    private void FireBullets(Vector2 direction, float spread)
    {
        float startAngle = -(stats.spreadArc / 2);
        float angleStep = stats.bulletsPerShot > 1 ? stats.spreadArc / (stats.bulletsPerShot - 1) : 0;

        for (int i = 0; i < stats.bulletsPerShot; i++)
        {
            float angleOffset = startAngle + (i * angleStep);
            Vector2 adjustedDirection = GunHelper.AddAngle2Vector(direction, angleOffset).normalized;

            ShotInfo shotInfo = new ShotInfo(stats, stats.power / stats.bulletsPerShot, stats.bulletType, Time.time, adjustedDirection, gunTransform.parent.position, gunTransform.parent.tag);
            Managers.bulletManager.DelegateFire(shotInfo);
        }
    }
}