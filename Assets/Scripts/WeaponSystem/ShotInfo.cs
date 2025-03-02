using UnityEngine;

public struct ShotInfo
{
    public ShotInfo(GunData gunData, float power, BulletType bulletData, float time, Vector2 direction, Vector2 originPos, string teamTag)
    {
        this.timeShot = time;
        this.power = power;
        this.bulletLife = gunData.bulletLife * 2 * (power / 2);
        this.maxDamage = bulletData.damage * power;
        this.minDamage = bulletData.damage;
        this.speed = Mathf.Ceil(bulletData.speed);
        this.piercing = bulletData.piercing;
        this.color = bulletData.color;
        this.trailLengthMultiplier = bulletData.trailLengthMultiplier;
        this.direction = direction;
        this.originPos = originPos;
        this.teamTag = teamTag;
        
    }

    public float timeShot { get; private set; }
    public float bulletLife { get; private set; }
    public float maxDamage { get; private set; }
    public float minDamage { get; private set; }
    public float power { get; private set; }
    public float speed { get; private set; }
    public bool piercing {get; private set;}
    public Color color { get; private set; }
    public float trailLengthMultiplier { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 originPos { get; private set; }
    public string teamTag { get; private set; }
}
