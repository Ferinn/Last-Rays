using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHandler
{
    public float currentSpread = 0f; //in euler angles
    public float spreadPercentage = 0f;
    float maxSpread = 0f; //in euler angles
    float spreadIncrement = 0f; //per shot
    float spreadDecayRate = 0f;

    public Vector2 recoilVector = Vector2.zero;

    public AimHandler(float maxSpread, float spreadIncrement, float spreadDecayDur)
    {
        this.maxSpread = maxSpread;
        this.spreadIncrement = spreadIncrement;
        this.spreadDecayRate = maxSpread / spreadDecayDur;

        recoilVector = new Vector2(spreadIncrement, spreadIncrement);
    }

    public float OnShot()
    {
        float randomAngle = Random.Range(-currentSpread, currentSpread);
        currentSpread = Mathf.Clamp(currentSpread + spreadIncrement, 0f, maxSpread);
        spreadPercentage = Mathf.Clamp01(currentSpread / maxSpread);
        return randomAngle;
    }
    public void SpreadDecay()
    {
        currentSpread = Mathf.Max(currentSpread - (spreadDecayRate * Time.deltaTime), 0);
        spreadPercentage = Mathf.Clamp01(currentSpread / maxSpread);
    }

    public float SimulateRecoil()
    {
        return (spreadPercentage / 100) * 2;
    }
}
