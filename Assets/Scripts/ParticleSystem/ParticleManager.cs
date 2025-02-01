using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] int defaultSize = 10;
    //[SerializeField] float updateFrequency;

    [NonSerialized] public List<Particle> inactivePool;
    [NonSerialized] public List<Particle> activePool;

    private void AddParticle(GameObject prefab)
    {
        GameObject particleGO = Instantiate(prefab, transform.position, Quaternion.identity, transform) as GameObject;
        Particle particle = particleGO.GetComponent<Particle>();
        inactivePool.Add(particle);
        particle.Deactivate();
    }

    public void SetActive(Particle particle, bool active)
    {
        switch (active)
        {
            case true:
                inactivePool.Remove(particle);
                activePool.Add(particle);
                break;
            case false:
                activePool.Remove(particle);
                inactivePool.Add(particle);
                break;
        }
    }

    public Particle GetVacant()
    {
        for (int i = 0; i < inactivePool.Count; i++)
        {
            if (inactivePool[i] != null)
            {
                return inactivePool[i];
            }
        }
        AddParticle(muzzleFlash);
        return GetVacant();
    }

    void Start()
    {
        inactivePool = new List<Particle>(defaultSize);
        activePool = new List<Particle>(defaultSize);
        for (int i = 0; i < defaultSize; i++)
        {
            AddParticle(muzzleFlash);
        }
    }
}
