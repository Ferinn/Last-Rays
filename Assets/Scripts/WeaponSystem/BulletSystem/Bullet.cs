using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet
{
    private const float constTrailLength = 10f;

    public GameObject bulletGO;
    private BulletPool pool;
    private TrailRenderer trailRenderer;

    private ShotInfo shotInfo;
    
    private Vector2 currentPosition;
    public Vector2 lastPos { get; private set; }

    private float timeActive;
    private float maxTime;
    private float damageOverflow;

    private int layerMask;

    public void Initialise(GameObject bulletGO, BulletPool pool)
    {
        this.bulletGO = bulletGO;
        this.trailRenderer = bulletGO.GetComponent<TrailRenderer>();
        this.pool = pool;

        this.layerMask = LayerMask.GetMask("Units");
        if (this.layerMask == 0)
        {
            Debug.LogError("Layer 'Units' not found. Please ensure it exists in the Project Settings.");
        }
    }

    public void Shoot(ShotInfo shotInfo)
    {
        InitBullet(shotInfo);
        InitTrail();
    }

    private void InitBullet(ShotInfo shotInfo)
    {
        this.shotInfo = shotInfo;
        currentPosition = shotInfo.originPos;
        lastPos = currentPosition;
        timeActive = 0f;
        damageOverflow = 0f;
    }

    private void InitTrail()
    {
        this.trailRenderer.Clear();
        InitTrailRenderer();

        bulletGO.transform.position = shotInfo.originPos;

        float trailLength = shotInfo.trailLengthMultiplier * constTrailLength;
        maxTime = trailLength / shotInfo.speed;
        trailRenderer.time = maxTime;

        float fixedDeltaTime = Time.fixedDeltaTime;
        trailRenderer.minVertexDistance = (shotInfo.speed * fixedDeltaTime) / 2f;

        pool.SetActive(this, true);
        this.bulletGO.SetActive(true);
    }

    private void InitTrailRenderer()
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = shotInfo.color;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = new Color(shotInfo.color.r, shotInfo.color.g, shotInfo.color.b, 0f);
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);

        trailRenderer.colorGradient = gradient;
    }

    public void Update(float deltaTime)
    {
        timeActive += deltaTime;
        if (timeActive > (shotInfo.bulletLife)) { Falloff(); return; }

        Vector2 deltaPos = (shotInfo.direction * deltaTime * shotInfo.speed);
        Vector2 newPosition = currentPosition + deltaPos;

        RaycastHit2D hit = Physics2D.Raycast(lastPos, shotInfo.direction, deltaPos.magnitude);

        if (hit.collider != null && hit.collider.tag != shotInfo.teamTag)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Door")
            {
                Falloff();
                return;
            }

            ICharacter character = hit.collider.GetComponent<ICharacter>();
            if (character != null && character.alive)
            {
                float rawDamage = shotInfo.maxDamage * (shotInfo.bulletLife - timeActive);
                float limitedDamage = rawDamage < shotInfo.minDamage ? shotInfo.minDamage : rawDamage;
                limitedDamage = damageOverflow != 0 ? damageOverflow : limitedDamage;

                damageOverflow = character.Hit(limitedDamage);
                if (damageOverflow == 0)
                {
                    Falloff();
                    return;
                }
            }
        }

        lastPos = currentPosition;
        currentPosition = newPosition;

        bulletGO.transform.position = currentPosition;
    }

    public void Falloff()
    {
        pool.SetActive(this, false);
        shotInfo = new ShotInfo();
        this.bulletGO.SetActive(false);
    }
}
