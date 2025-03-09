using UnityEngine;

public class GunEffects
{
    private GunData data;
    private ParticleManager particleManager;
    private bool globaliseSFX = false;
    public void GlobaliseSFX() => globaliseSFX = true;

    public GunEffects(GunData data)
    {
        this.data = data;
        this.particleManager = Managers.particleManager;
    }

    public void PlayGunshotSound(Transform transform)
    {
        Managers.audioManager.PlaySFX(SFXSounds.handgun_shot, transform.parent, globaliseSFX);
    }

    public void PlayReloadSound(Transform transform)
    {
        Managers.audioManager.PlaySFX(SFXSounds.handgun_reload, transform.parent, globaliseSFX);
    }

    public void SpawnMuzzleFlash(Transform gunTransform)
    {
        particleManager.GetVacant().Activate(gunTransform, data.barrelOffset, 0.1f, true);
    }
}