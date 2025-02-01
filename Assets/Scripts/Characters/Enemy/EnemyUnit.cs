using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using States = PixelartAnimator.States;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(PixelartAnimator))]
public class EnemyUnit : MonoBehaviour
{
    public bool alive { get; private set; }
    float health = 10;
    PixelartAnimator animator;

    public void Awake()
    {
        animator = GetComponent<PixelartAnimator>();
        alive = true;
    }

    public void Hit(float damage)
    {
        health -= damage;
        Debug.Log($"{name} has {health} HP");
        if (health <= 0)
        {
            this.Die();
        }
    }

    private void Die()
    {
        animator.SetState(States.dead, 1);
        alive = false;
        try
        {
            Gun gun = this.gameObject.GetComponentInChildren<Gun>();
            gun.gameObject.SetActive(false);
        }
        catch { }
    }
}
