using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AICombatDelegate
{
    ICharacter player;
    EnemyController controller;

    public float skillPercentage;
    public float maxDeviationAngle = 50f;
    const float shotFrequencyAmplitude = 0.33f;
    float lastFired;

    float fireCooldown;

    public AICombatDelegate(ICharacter player, EnemyController controller)
    {
        this.player = player;
        this.controller = controller;
        this.skillPercentage = controller.charData.accuracyPercentage / 100;
        fireCooldown = 1 / controller.heldGun.GetGunData().fireRate;
    }

    public void Chase(AIStates state)
    {
        if (state == AIStates.chasing)
        {
            controller.moveDelegate.Follow();

            Attack(player);
        }
    }

    //TEMP!!! The AI fireCooldown directly depends on the variable "maxRecoilSeconds" from RecoilMath
    //Introduce randomness to its fireCooldown, sometimes it should fire once the recoil's gone
    //and sometimes while its still active
    private void Attack(ICharacter target)
    {
        if (lastFired > Time.time)
        {
            return;
        }

        float fireTimeElapsed = Time.time - lastFired;
        if (fireTimeElapsed > fireCooldown)
        {
            Vector3 targetVelocity = target.lastframeDeltaPos / Time.deltaTime;
            Vector3 predictedPosition = target.transform.position + (targetVelocity * controller.heldGun.GetTravelTime(target.transform.position));

            Vector3 toTarget = predictedPosition - controller.transform.position;

            if (toTarget.magnitude <= controller.heldGun.GetMaxRange())
            {
                Vector3 normalizedDirection = toTarget.normalized;

                float currentDeviationAngle = maxDeviationAngle * (1f - Mathf.Clamp01(skillPercentage));
                float randomAngle = Random.Range(-currentDeviationAngle, currentDeviationAngle);

                Vector3 deviatedDirection = Quaternion.Euler(0, 0, randomAngle) * normalizedDirection;
                controller.facing = (int)Math.Ceiling(deviatedDirection.normalized.x);

                controller.heldGun.Fire(deviatedDirection);
                controller.heldGun.AimAt(deviatedDirection);

                lastFired = Time.time + Random.Range(-shotFrequencyAmplitude, shotFrequencyAmplitude);
            }
        }
    }
}
