using System;
using UnityEngine;
using States = PixelartAnimator.States;

[RequireComponent(typeof(PixelartAnimator), typeof(BoxCollider2D), typeof(SpriteRenderer)), RequireComponent(typeof(Rigidbody2D))]
public abstract class ICharacter : MonoBehaviour
{
    public float health { get; private set; }
    public bool alive { get; private set; }

    [NonSerialized] public int facing = 1;

    [NonSerialized] public PixelartAnimator animator;
    [NonSerialized] public Rigidbody2D rigidBody;
    public CharacterData charData;
    [NonSerialized] public Gun heldGun;
    [NonSerialized] public BoxCollider2D boxCollider;

    //used by AI's aim algorithm to predict where this character is headed
    [NonSerialized] public Vector3 lastframeDeltaPos;

    public void Initialise()
    {
        health = charData.maxHealth;
        animator = this.GetComponent<PixelartAnimator>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        heldGun = this.GetComponentInChildren<Gun>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        alive = true;
        lastframeDeltaPos = Vector3.zero;
    }

    void Awake()
    {
        Initialise();
    }

    public float Hit(float damage)
    {
        //Check for damage overflow
        if (health < damage)
        {
            Die();
            return damage - health;
        }

        health -= damage;
        if (health == 0) { Die(); }
        return 0f;
    }

    private void Die()
    {
        animator.SetState(States.dead, facing);
        alive = false;
        try
        {
            boxCollider.enabled = false;
            heldGun.gameObject.SetActive(false);
        }
        catch { }
    }
}
