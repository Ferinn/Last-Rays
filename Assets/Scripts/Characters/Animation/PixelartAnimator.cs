using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class PixelartAnimator : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    private States currentState;

    //Animation States
    public enum States
    {
        idle = 0,
        walk = 1,
        dead = 2,
    }

    Dictionary<States, string> animStates = new Dictionary<States, string>
    {
        {States.idle, "Idle" },
        {States.walk, "Walk" },
        {States.dead, "Dead" },
    };

    private void Start()
    {
        InitialiseDependencies();

        SetState(States.idle, 1);
    }

    private void InitialiseDependencies()
    {
        animator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetState(States newState, int _facing)
    {
        spriteRenderer.flipX = _facing <= 0;
        if (currentState == newState) return;

        animator.Play(animStates[newState]);
        currentState = newState;
    }
}
