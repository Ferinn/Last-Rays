using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class PixelartAnimator : MonoBehaviour
{
    Animator animator;

    [Header("--- VFX ---")]
    bool hasVFX = false;
    [SerializeField] Animator vfxAnimator;
    [SerializeField] SpriteRenderer vfxRenderer;


    SpriteRenderer spriteRenderer;
    private States currentState;

    //Animation States
    public enum States
    {
        idle = 0,
        walk = 1,
        attack = 2,
        death = 3,
    }

    Dictionary<States, string> animStates = new Dictionary<States, string>
    {
        {States.idle, "Idle" },
        {States.walk, "Walk" },
        {States.attack, "Attack" },
        {States.death, "Death" },
    };

    Dictionary<States, float> animLengths = new Dictionary<States, float>();

    private void Start()
    {
        hasVFX = vfxAnimator != null && vfxRenderer != null;

        InitialiseDependencies();
        UpdateAnimClipTimes();

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

        if (hasVFX)
        {
            vfxRenderer.flipX = _facing <= 0;
            vfxAnimator.Play(animStates[newState]);
        }

        animator.Play(animStates[newState]);
        currentState = newState;
    }

    public float GetClipLength(States state)
    {
        return animLengths[state];
    }

    private void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            foreach (States clipName in animStates.Keys)
            {
                if (clip.name == animStates[clipName])
                {
                    animLengths.Add(clipName, clip.length);
                }
            }
        }
    }
}
