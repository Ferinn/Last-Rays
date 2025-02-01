using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using States = PixelartAnimator.States;
using Random = UnityEngine.Random;

public enum AIStates
{
    idle = 0,
    chasing = 1,
}

[RequireComponent(typeof(SensorManager))]
public class EnemyController : ICharacter
{
    [SerializeField] ICharacter player;
    AICombatDelegate combatDelegate;
    public AIMoveDelegate moveDelegate;

    AIStates state;
    Vector3 prevPos;

    float viewRadius;
    private float reactionTime = 0.5f;
    private float currentDetectionTime = 0f;
    private float memoryDuration = 2f;
    private float memoryTimer = 0f;

    private LayerMask sightMask;

    void Start()
    {
        prevPos = this.transform.position;
        ChangeState(AIStates.idle);
        viewRadius = Camera.main.orthographicSize * Camera.main.aspect;

        combatDelegate = new AICombatDelegate(player, this);
        moveDelegate = new AIMoveDelegate(player, this);
        sightMask = LayerMask.GetMask("Units", "Solid");
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            Vector3 prevPos = this.transform.position;
            lastframeDeltaPos = this.transform.position - prevPos;
            Detect();
            combatDelegate.Chase(state);
        }
    }

    public void SetMoveAnim()
    {
        if (transform.position != prevPos)
        {
            animator.SetState(States.walk, facing);
        }
        else
        {
            animator.SetState(States.idle, facing);
        }
        prevPos = transform.position;
    }

    private void Detect()
    {
        if (state == AIStates.idle)
        {
            if (DetectPlayer())
            {
                currentDetectionTime += Time.fixedDeltaTime;
                if (currentDetectionTime >= reactionTime)
                {
                    ChangeState(AIStates.chasing);
                    memoryTimer = 0f;
                }
            }
            else
            {
                currentDetectionTime = 0f;
            }
        }
        else if (state == AIStates.chasing)
        {
            if (!DetectPlayer())
            {
                memoryTimer += Time.fixedDeltaTime;
                if (memoryTimer >= memoryDuration)
                {
                    ChangeState(AIStates.idle);
                }
            }
            else
            {
                memoryTimer = 0f;
            }
        }
    }

    private void ChangeState(AIStates newState)
    {
        state = newState;
    }

    //Checks if the player is close enough to be detected
    //then raycasts towards them to check if the LOS is clear
    private bool DetectPlayer()
    {
        Vector2 toPlayer = player.transform.position - this.transform.position;

        if (toPlayer.magnitude <= viewRadius)
        {
            Vector2 rayStart = (Vector2)this.transform.position + toPlayer.normalized;
            
            RaycastHit2D hit = Physics2D.Raycast(rayStart, toPlayer.normalized, viewRadius, sightMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }
}
