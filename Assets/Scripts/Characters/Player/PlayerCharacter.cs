using FunkyCode.Rendering.Day;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : ICharacter
{
    [SerializeField] GameObject crosshair;
    [SerializeField] float crosshairMaxSize;
    [SerializeField] float crosshairMinSize;

    public PlayerController controller;
    public CamController camController;

    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();

    [SerializeField] private Vector2 defaultWeaponPos = Vector2.zero;
    private List<GameObject> weapons = new List<GameObject>();
    public int equippedIndex = 0;

    void Start()
    {
        //Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 1;
        Cursor.visible = false;
        controller = new PlayerController(this, crosshair, rigidBody, crosshairMaxSize, crosshairMinSize);
        camController = transform.GetComponentInChildren<CamController>();

        LoadWeapons();
        EquipWeapon(0);
    }

    void Update()
    {
        if (alive)
        {
            Vector3 prevPos = this.transform.position;
            controller.HandleInput();
            lastframeDeltaPos = this.transform.position - prevPos;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
        controller.Move();
    }

    private void LoadWeapons()
    {
        foreach (GameObject prefab in weaponPrefabs)
        {
            GameObject weapon = Instantiate(prefab, defaultWeaponPos, Quaternion.identity, this.transform);
            weapon.SetActive(false);
            weapons.Add(weapon);
            weapon.GetComponent<Gun>().SetAsPlayerGun();
        }
    }

    public void EquipWeapon(int index)
    {
        weapons[equippedIndex].SetActive(false);
        equippedIndex = index % weapons.Count;
        equippedIndex = equippedIndex < 0 ? weapons.Count - 1 : equippedIndex;
        heldGun = weapons[equippedIndex].GetComponent<Gun>();
        heldGun.gameObject.SetActive(true);
        heldGun.gameObject.transform.localPosition = defaultWeaponPos;
        heldGun.gameObject.transform.localRotation = Quaternion.identity;

        heldGun.lastFired = Time.time;
    }
}
