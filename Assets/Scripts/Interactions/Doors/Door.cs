using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LightCollider2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    LightCollider2D lightCollider;
    BoxCollider2D boxCollider;
    Sprite closedDoorSprite;
    Sprite openDoorSprite;

    [SerializeField] Vector2 size;
    public bool isOpen;

    private float closeDelay = 1f;
    private float timeOpened;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lightCollider = GetComponent<LightCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        closedDoorSprite = Managers.doorManager.closedDoorSprite;
        openDoorSprite = Managers.doorManager.openDoorSprite;
        if (size == null)
        {
            size = new Vector2(Managers.doorManager.size, Managers.doorManager.size);
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject != null)
        {
            GameObject go = collider.gameObject;
            if (go.tag == "Player" || go.tag == "Unit")
            {
                OpenDoor();
            }
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openDoorSprite;
        lightCollider.enabled = false;
        boxCollider.enabled = false;
        timeOpened = Time.time;
        isOpen = true;
    }

    private void CloseDoor()
    {
        spriteRenderer.sprite = closedDoorSprite;
        lightCollider.enabled = true;
        boxCollider.enabled = true;
        isOpen = false;
    }

    public void UpdateDoor()
    {
        if (isOpen)
        {
            float timeElapsed = Time.time - timeOpened;
            if (timeElapsed > closeDelay)
            {
                Collider2D hitCollider = Physics2D.OverlapBox((Vector2)transform.position, size, transform.eulerAngles.z);
                if (hitCollider != null)
                {
                    if (hitCollider.tag == "Unit" || hitCollider.tag == "Player")
                    {
                        timeOpened = Time.time;
                        return;
                    }
                }
                CloseDoor();
            }
        }
    }
}
