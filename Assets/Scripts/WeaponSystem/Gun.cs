using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Gun : MonoBehaviour
{
    [SerializeField] GunData data;

    ParticleManager particleManager;
    AimHandler aimHandler;
    
    SpriteRenderer spriteRenderer;

    public float lastFired;
    float shotCooldown;
    public bool isReloading;
    public float shotsRemaining;

    int facing = 1;
    public bool globaliseSFX = false;

    public void SetAsPlayerGun()
    {
        globaliseSFX = true;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastFired = 0f;
        particleManager = Managers.particleManager;
        //temp, include this in gundata!
        aimHandler = new AimHandler(data.maxSpread, data.spreadIncrement, data.spreadDecayDur);
        shotCooldown = (1f / data.fireRate);
        shotsRemaining = data.clipSize;
        isReloading = false;

        if (data.bulletsPerShot % 2 == 0) { data.bulletsPerShot += 1; }
    }

    //if reloading was interupted by switching, reload again
    private void OnEnable()
    {
        if (isReloading || shotsRemaining <= 0)
        {
            isReloading = false;
            Reload();
        }
    }

    private void LateUpdate()
    {
        float timeElapsed = Time.time - lastFired;
        if (timeElapsed > (shotCooldown / 2))
        {
            aimHandler.SpreadDecay();
        }
    }

    public float AimAt(Vector2 targetDir)
    {
        //Calculates how much the gun would "naturally" realign to the targetDir
        float recoverStage = Mathf.Clamp01(aimHandler.SimulateRecoil());
        HandleFliping(targetDir);

        Vector2 recoveredAim = Vector2.Lerp(targetDir, targetDir + aimHandler.recoilVector, recoverStage);

        float recoveredAngle = Mathf.Atan2(recoveredAim.y, recoveredAim.x) * Mathf.Rad2Deg;
        this.transform.localRotation = Quaternion.Euler(0, 0, recoveredAngle);

        return aimHandler.currentSpread;
    }

    public void HandleFliping(Vector2 direction)
    {
        // Adjust facing
        if (facing != (int)Math.Ceiling(direction.x))
        {
            facing = (int)Math.Ceiling(direction.x);

            spriteRenderer.flipY = !spriteRenderer.flipY;
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
    }

    public bool Fire(Vector2 desiredDir)
    {
        if (!isReloading)
        {
            float timeElapsed = Time.time - lastFired;
            if (timeElapsed > shotCooldown)
            {
                shotsRemaining--;
                if (shotsRemaining < 0)
                {
                    Reload(null);
                    return false;
                }
                float angleOffset = aimHandler.OnShot();
                float angle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg + angleOffset;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                //Playing handgun shot sfx
                Managers.audioManger.PlaySFX(SFXSounds.handgun_shot, this.transform.parent, globaliseSFX);
                //Spread the bullets based on the weapon arc
                FireBullets(direction);

                particleManager.GetVacant().Activate(transform, data.barrelOffset, 0.1f, true);
                lastFired = Time.time;

                if (shotsRemaining == 0)
                {
                    Reload(null);
                }
                return true;
            }
        }
        
        return false;
    }

    public void Reload(System.Action callback = null)
    {
        if (!isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadIE(callback));
            Managers.audioManger.PlaySFX(SFXSounds.handgun_reload, this.transform.parent, globaliseSFX);
        }
    }

    private IEnumerator ReloadIE(System.Action callback = null)
    {
        yield return new WaitForSeconds(data.reloadDuration);
        shotsRemaining = data.clipSize;
        isReloading = false;
        if (callback != null) { callback.Invoke(); }
    }

    private void FireBullets(Vector2 direction)
    {
        float startAngle = -(data.spreadArc / 2);
        float angleStep = data.bulletsPerShot > 1 ? data.spreadArc / (data.bulletsPerShot - 1) : 0;

        for (int i = 0; i < data.bulletsPerShot; i++)
        {
            //Distribute gun "power" property among all its bullets
            float specificPower = data.power / data.bulletsPerShot;

            //Calculate angle offset based on bullet index
            float angleOffset = startAngle + (i * angleStep);

            //Adjust targetDir with arc offset
            Vector2 arcOffsetDirection = AddAngle2Vector(direction, angleOffset).normalized;

            //Passing info to bullet
            ShotInfo shotInfo = new ShotInfo(this.data, specificPower, this.data.bulletType, Time.time, arcOffsetDirection, this.transform.parent.position, transform.parent.tag);
            Managers.bulletManager.DelegateFire(shotInfo);
        }
    }

    public static Vector2 AddAngle2Vector(Vector2 origin, float angle)
    {
        if (angle == 0) return origin;

        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float newX = origin.x * cos - origin.y * sin;
        float newY = origin.x * sin + origin.y * cos;

        return new Vector2(newX, newY).normalized;
    }

    public float GetMaxRange()
    {
        return data.bulletType.speed * (data.bulletLife * 2);
    }

    public GunData GetGunData()
    {
        return data;
    }

    public float GetTravelTime(Vector2 target)
    {
        return ((target - (Vector2)this.transform.position).magnitude) / data.bulletType.speed;
    }

    public float GetSpreadPercentage()
    {
        return aimHandler.spreadPercentage;
    }
}
