using UnityEngine;

public class GunAimer
{
    private Transform gunTransform;
    private AimHandler aimHandler;
    private SpriteRenderer spriteRenderer;
    private int facing = 1;

    public GunAimer(Transform transform, GunData data)
    {
        this.gunTransform = transform;
        this.aimHandler = new AimHandler(data.maxSpread, data.spreadIncrement, data.spreadDecayDur);
        this.spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void DecaySpread() => aimHandler.SpreadDecay();

    public float AimAt(Vector2 targetDir)
    {
        HandleFlipping(targetDir);
        float recoverStage = Mathf.Clamp01(aimHandler.SimulateRecoil());

        Vector2 recoveredAim = Vector2.Lerp(targetDir, targetDir + aimHandler.recoilVector, recoverStage);
        float recoveredAngle = Mathf.Atan2(recoveredAim.y, recoveredAim.x) * Mathf.Rad2Deg;

        gunTransform.localRotation = Quaternion.Euler(0, 0, recoveredAngle);
        return aimHandler.currentSpread;
    }

    private void HandleFlipping(Vector2 direction)
    {
        int newFacing = (int)Mathf.Sign(direction.x);
        if (facing != newFacing)
        {
            facing = newFacing;
            spriteRenderer.flipY = !spriteRenderer.flipY;
            gunTransform.localPosition = new Vector3(-gunTransform.localPosition.x, gunTransform.localPosition.y, gunTransform.localPosition.z);
        }
    }

    public float GetSpreadPercentage() => aimHandler.spreadPercentage;
}