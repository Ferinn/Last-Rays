using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantBackground : MonoBehaviour
{
    [SerializeField] ICharacter player;

    void LateUpdate()
    {
        /*Vector3 playerDeltaPos = player.lastframeDeltaPos;
        this.transform.position += playerDeltaPos;*/
    }
}
