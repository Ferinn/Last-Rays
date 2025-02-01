using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Managers
{
    public static BulletManager bulletManager = GameObject.Find("Managers").GetComponentInChildren<BulletManager>();
    public static DoorManager doorManager = GameObject.Find("Managers").GetComponentInChildren<DoorManager>();
    public static ParticleManager particleManager = GameObject.Find("Managers").GetComponentInChildren<ParticleManager>();
    public static AudioManager audioManger = GameObject.Find("Managers").GetComponentInChildren<AudioManager>();
    public static GameManager gameManager = GameObject.Find("Managers").GetComponentInChildren<GameManager>();
}