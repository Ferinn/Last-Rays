using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using AnimState = PixelartAnimator.States;
using States = NonPlayerCharacter.FSM_States;

public class NonPlayerCharacter : ICharacter
{
    private ICharacter player;

    bool attacking = false;
    FSM_States state = FSM_States.Idle;

    [Header("--- AI Detection Parameters ---")]
    [SerializeField] LayerMask sightMask;
    [SerializeField] float viewRadius;
    [SerializeField] float reactionTime;
    [SerializeField] float memoryDuration;
    // private AI Detection variables
    private float detectionTimer = 0;
    private float memoryTimer = 0;

    [Header("--- AI Combat Parameters ---")]
    [SerializeField] float attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackKnockback;


    public enum FSM_States
    {
        Idle = 0,
        Chasing = 1,
        Searching = 2,
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Managers.gameManager.GetPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            Percieve();
            Act();
            UpdateAnimation();
        }
    }

    IEnumerator Attack(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);
        Vector2 thisToPlayer = player.transform.position - this.transform.position;
        if (thisToPlayer.magnitude <= attackRange)
        {
            Vector2 attackVector = thisToPlayer.normalized;
            ICharacter character = player;
            character.Hit(attackDamage, attackVector * attackKnockback);
            /*Vector2 attackVector = thisToPlayer.normalized;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, attackVector, attackRange);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Door")
                {
                    break;
                }
                else if (hit.collider.tag == "Player")
                {
                    ICharacter character = hit.collider.GetComponent<ICharacter>();

                    if (character != null && character.alive)
                    {
                        Debug.Log("PlayerHit!");
                        character.Hit(attackDamage, attackVector * attackKnockback);
                    }
                    break;
                }
            }*/
        }
        attacking = false;
    }

    void Chase()
    {
        Vector2 thisToPlayer = player.transform.position - this.transform.position;
        Vector2 currentMoveVector = thisToPlayer.normalized;

        if (thisToPlayer.magnitude > attackRange)
        {
            this.rigidBody.AddForce(currentMoveVector * charData.speed, ForceMode2D.Force);
            if (rigidBody.velocity.magnitude > charData.speed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * charData.speed;
            }
        }
        // start attacking
        else
        {
            if (!attacking)
            {
                attacking = true;
                StartCoroutine(Attack(animator.GetClipLength(AnimState.attack)));
            }

            if (thisToPlayer.magnitude > 0.75f)
            {
                this.rigidBody.AddForce(currentMoveVector * (charData.speed / 1.25f), ForceMode2D.Force);
                if (rigidBody.velocity.magnitude > charData.speed)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * charData.speed;
                }
            }
        }

        // set facing for animation
        facing = Mathf.CeilToInt(-currentMoveVector.x);
    }

    void Act()
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.Searching:
                break;
            case States.Chasing:
                Chase();
                break;
        }
    }

    void Percieve()
    {
        switch (state)
        {
            case States.Idle:
                if (DetectPlayer())
                {
                    detectionTimer += Time.fixedDeltaTime;
                    if (detectionTimer >= reactionTime)
                    {
                        state = States.Chasing;
                        memoryTimer = 0f;
                    }
                }
                else
                {
                    detectionTimer = 0f;
                }
                break;
            case States.Searching:
                if (!DetectPlayer())
                {
                    memoryTimer += Time.fixedDeltaTime;
                    if (memoryTimer >= (memoryDuration * 2))
                    {
                        state = States.Idle;
                    }
                }
                else
                {
                    state = States.Chasing;
                }
                break;
            case States.Chasing:
                if (!DetectPlayer())
                {
                    memoryTimer += Time.fixedDeltaTime;
                    if (memoryTimer >= memoryDuration)
                    {
                        state = States.Searching;
                    }
                }
                else
                {
                    memoryTimer = 0f;
                }
                break;
        }
    }

    //Checks if the player is close enough to be detected
    //then raycasts towards them to check if the LOS is clear
    private bool DetectPlayer(float overrideViewRadius = 0f)
    {
        Vector2 toPlayer = player.transform.position - this.transform.position;

        if (toPlayer.magnitude <= (viewRadius + overrideViewRadius))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, toPlayer.normalized, (viewRadius + overrideViewRadius), sightMask);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Door")
                {
                    break;
                }
                else if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Change animation clip and facing based on movement
    void UpdateAnimation()
    {
        if (attacking)
        {
            animator.SetState(AnimState.attack, facing);
        }
        else
        {
            if (Mathf.Approximately(rigidBody.velocity.sqrMagnitude, 0))
            {
                animator.SetState(AnimState.idle, facing);
            }
            else
            {
                animator.SetState(AnimState.walk, facing);
            }
        }
    }
}
