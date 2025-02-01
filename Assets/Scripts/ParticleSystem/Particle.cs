using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Particle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color color;

    float duration;
    public float deathTime { get; private set; }
    private float elapsedTime;
    public bool fadeOutActive { get; private set; }

    private bool isLightSource;
    private Light2D lightSource;
    private float lightSize;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

        try
        {
            lightSource = GetComponent<Light2D>();
            isLightSource = true;
            lightSize = lightSource.size;
        }
        catch
        {
            isLightSource = false;
        }
    }

    private void LateUpdate()
    {
        if (Time.time >= deathTime)
        {
            Deactivate();
            return;
        }

        if (fadeOutActive)
        {
            FadeOut();
        }
    }

    public void FadeOut()
    {
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / duration);

        float newAlpha = Mathf.Lerp(color.a, 0, progress);
        spriteRenderer.color = new Color(color.r, color.g, color.b, newAlpha);

        if (isLightSource)
        {
            float newSize = Mathf.Lerp(lightSource.size, 0, progress);
            lightSource.size = newSize;
        }
    }

    public void Activate(Transform parent, Vector2 barrelOffset, float duration, bool fadeOutActive)
    {
        transform.parent = parent;
        transform.localPosition = barrelOffset;
        transform.localRotation = Quaternion.Euler(0, 0, 270);
        this.duration = duration;
        deathTime = Time.time + duration;
        elapsedTime = 0;
        this.fadeOutActive = fadeOutActive;
        spriteRenderer.color = color;

        if (isLightSource)
        {
            lightSource.size = lightSize;
        }

        Managers.particleManager.SetActive(this, true);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Managers.particleManager.SetActive(this, false);
    }
}
