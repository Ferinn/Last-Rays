using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    public List<Bullet> inactivePool;
    public List<Bullet> activePool;
    public void Initialise(int size)
    {
        inactivePool = new List<Bullet>(size);
        activePool = new List<Bullet>(size);
    }

    public void AddBullet(GameObject bulletGO)
    {
        Bullet bullet = new Bullet();
        bullet.Initialise(bulletGO, this);
        inactivePool.Add(bullet);
    }

    public void SetActive(Bullet bullet, bool active)
    {
        switch (active)
        {
            case true:
                inactivePool.Remove(bullet);
                activePool.Add(bullet);
                break;
            case false:
                activePool.Remove(bullet);
                inactivePool.Add(bullet);
                break;
        }
    }

    public Bullet GetVacant()
    {
        for (int i = 0; i < inactivePool.Count; i++)
        {
            if (inactivePool[i] != null)
            {
                return inactivePool[i];
            }
        }
        Managers.bulletManager.AddBulletGO();
        return GetVacant();
    }
}
