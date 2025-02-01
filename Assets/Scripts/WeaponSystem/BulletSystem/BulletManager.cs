using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int defaultPoolSize = 10;

    BulletPool pool;

    void Start()
    {
        pool = new BulletPool();
        pool.Initialise(defaultPoolSize);
        for (int i = 0; i < defaultPoolSize; i++)
        {
            AddBulletGO();
        }
    }

    void Update()
    {
        for (int i = 0; i < pool.activePool.Count; i++)
        {
            pool.activePool[i].Update(Time.deltaTime);
        }
    }

    public GameObject AddBulletGO()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, this.transform);
        bulletGO.SetActive(false);
        pool.AddBullet(bulletGO);
        return bulletGO;
    }

    public void DelegateFire(ShotInfo shotInfo)
    {
        Bullet bullet = pool.GetVacant();
        bullet.Shoot(shotInfo);
    }
}
