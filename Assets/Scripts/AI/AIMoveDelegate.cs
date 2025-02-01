using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AIMoveDelegate
{
    EnemyController controller;
    LayerMask wallsMask;
    ICharacter player;

    public AIMoveDelegate(ICharacter player, EnemyController controller)
    {
        this.controller = controller;
        this.player = player;
        wallsMask = LayerMask.GetMask("Walls");
    }

    public void Follow()
    {
        Vector3 toPlayer = player.transform.position - controller.transform.position;
        Vector3 moveVector = PathFind(toPlayer);

        if (Mathf.Approximately(moveVector.magnitude, 0))
        {
            return;
        }

        controller.SetMoveAnim();
        controller.rigidBody.MovePosition(controller.transform.position + (moveVector * Time.fixedDeltaTime * controller.charData.speed));
    }

    private Vector3 PathFind(Vector3 toPlayer)
    {
        float approachDistance = controller.heldGun.GetGunData().bulletLife;

        if (toPlayer.magnitude <= approachDistance)
        {
            return Vector3.zero;
        }

        Vector3 fromPlayer = (controller.transform.position - player.transform.position).normalized;
        Vector3 deltaMove = (fromPlayer * approachDistance) - controller.transform.position;
        if (deltaMove.magnitude <= controller.charData.speed * 0.25f)
        {
            return Vector3.zero;
        }

        return toPlayer.normalized;
    }
}
