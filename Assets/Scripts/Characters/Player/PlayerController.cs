using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using States = PixelartAnimator.States;

public class PlayerController
{
    public PlayerController(PlayerCharacter character, GameObject crosshair, Rigidbody2D rigidbody, float crosshairMaxSize, float crosshairMinSize)
    {
        this.character = character;
        this.crosshair = crosshair;
        this.rigidbody = rigidbody;

        this.crosshairMaxSize = crosshairMaxSize;
        this.crosshairMinSize = crosshairMinSize;
    }

    PlayerCharacter character;
    GameObject crosshair;
    Rigidbody2D rigidbody;

    Vector2 mousePos;
    Vector2 lookDirection;

    Vector2 currentMoveVector;

    float crosshairMaxSize = 1f;
    float crosshairMinSize = 1f;

    public void HandleInput()
    {
        HandleEscape();
        HandleSwitching();
        GetMousePos();
        HandleAiming();
        HandleMovement();
        HandleMouseClick();
        HandleReload();
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !character.heldGun.isReloading)
        {
            Debug.Log("Reloading!");
            character.heldGun.Reload(ReportReloaded);
        }
    }
        public void ReportReloaded()
        {
            Debug.Log("Done!");
        }

    public void Move()
    {
        if (Mathf.Approximately(currentMoveVector.sqrMagnitude, 0f))
        {
            rigidbody.velocity = Vector2.zero;
        }
        else
        {
            rigidbody.MovePosition((Vector2)character.transform.position + (currentMoveVector * character.charData.speed * Time.fixedDeltaTime));
            currentMoveVector = Vector2.zero;
        }

        AdjustCrosshair();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.gameManager.ToggleUI();
        }
    }

    private void HandleSwitching()
    {
        if (!Mathf.Approximately(Mouse.current.scroll.y.value, 0))
        {
            if (Mouse.current.scroll.y.value > 0)
            {
                character.EquipWeapon(character.equippedIndex + 1);
            }
            else
            {
                character.EquipWeapon(character.equippedIndex - 1);
            }
            Managers.audioManger.PlaySFX(SFXSounds.handgun_pickup, character.transform, true);
        }
    }

    private void GetMousePos()
    {
        Vector2 screenMousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
        lookDirection = (mousePos - (Vector2)character.transform.position).normalized;
    }

    private void HandleAiming()
    {
        character.heldGun.AimAt(lookDirection);
    }

    public void AdjustCrosshair()
    {
        GetMousePos();
        crosshair.transform.localPosition = mousePos - (Vector2)character.camController.transform.position;

        //float minDeviation = 1f; // Scale at recoilStage 0
        //float maxDeviation = 2f; // Scale at recoilStage 1

        //float targetDeviation = Mathf.Lerp(minDeviation, maxDeviation, recoilStage);
        float crosshairSize = character.heldGun.GetSpreadPercentage() * crosshairMaxSize;
        crosshairSize = Math.Max(crosshairSize, crosshairMinSize);
        for (int i = 0; i < 4; i++)
        {
            GameObject lineGo = crosshair.transform.GetChild(i).gameObject;
            Vector2 newPos = new Vector2();
            switch (lineGo.name)
            {
                case "0":
                    newPos = new Vector2(0, +crosshairSize);
                    break;
                case "1":
                    newPos = new Vector2(+crosshairSize, 0);
                    break;
                case "2":
                    newPos = new Vector2(0, -crosshairSize);
                    break;
                case "3":
                    newPos = new Vector2(-crosshairSize, 0);
                    break;
            }
            lineGo.transform.localPosition = newPos;
        }

        //float smoothSpeed = 5f;
        //float currentDeviation = crosshair.transform.localScale.x;

        //float newDeviation = Mathf.Lerp(currentDeviation, targetDeviation, Time.deltaTime * smoothSpeed);

        //crosshair.transform.localScale = new Vector2(newDeviation, newDeviation);
    }

    private void HandleMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (character.heldGun.Fire(lookDirection))
            {
                character.camController.StartRecoil(character.heldGun.GetGunData().power, lookDirection);
            }
        }
    }

    private void HandleMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        bool isMoving = horizontalAxis != 0 || verticalAxis != 0;

        SetFacing(isMoving);

        Vector2 moveDirection = new Vector2(horizontalAxis, verticalAxis).normalized;

        currentMoveVector = moveDirection;
    }

    public void SetFacing(bool isMoving)
    {
        int facing = (int)Math.Ceiling(lookDirection.x);

        if (isMoving)
        {
            character.animator.SetState(States.walk, facing);
        }
        else
        {
            character.animator.SetState(States.idle, facing);
        }
    }
}
