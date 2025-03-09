using System.Collections;
using UnityEngine;

public class GunAmmo
{
    private int maxAmmo;
    private int currentAmmo;
    private bool isReloading = false;

    public bool IsEmpty => currentAmmo <= 0;
    public bool IsReloading => isReloading;

    public GunAmmo(int maxAmmo)
    {
        this.maxAmmo = maxAmmo;
        this.currentAmmo = maxAmmo;
    }

    public bool UseBullet()
    {
        if (isReloading || currentAmmo <= 0) return false;
        currentAmmo--;
        return true;
    }

    public void Reload(System.Action onReloadComplete)
    {
        if (!isReloading)
        {
            isReloading = true;
            CoroutineRunner.Instance.StartCoroutine(ReloadCoroutine(onReloadComplete));
        }
    }

    private IEnumerator ReloadCoroutine(System.Action onReloadComplete)
    {
        yield return new WaitForSeconds(1.5f); // Replace with gunData.reloadDuration
        currentAmmo = maxAmmo;
        isReloading = false;
        onReloadComplete?.Invoke();
    }
}