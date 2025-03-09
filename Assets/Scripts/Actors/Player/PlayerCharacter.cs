using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Reflection;

public class PlayerCharacter : ICharacter
{
    [Header("--- Crosshair ---")]
    [SerializeField] GameObject crosshair;
    [SerializeField] float crosshairMaxSize;
    [SerializeField] float crosshairMinSize;

    [Header("--- VFX ---")]
    public GameObject flashlight;
    [SerializeField] FullScreenPassRendererFeature dmgVignetteFeature;
    [Range(0f, 10f)] public float vignetteIntensityMax;
    private float vignetteIntensityMin = 5f;

    [Header("--- Controllers ---")]
    public PlayerController controller;
    public CamController camController;

    [Header("--- Weapon System ---")]
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
        UpdateVignette();
        controller.HandleEscape();
        if (alive)
        {
            Vector3 prevPos = this.transform.position;
            controller.HandleInput();
            lastframeDeltaPos = this.transform.position - prevPos;
        }
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            controller.Move();
        }
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

        heldGun.LastFired(Time.time);
    }

    private void UpdateVignette()
    {
        float healthStage = Mathf.Clamp01(health/maxHealth);

        if (healthStage <= 0.5f)
        {
            float VignetteIntensity = Mathf.Lerp(vignetteIntensityMax, 0.75f, healthStage);

            dmgVignetteFeature.passMaterial.SetFloat("_VignetteIntensity", VignetteIntensity);
        }
        else
        {
            dmgVignetteFeature.passMaterial.SetFloat("_VignetteIntensity", vignetteIntensityMin);
        }
    }

    public void ResetVignette()
    {
        dmgVignetteFeature.passMaterial.SetFloat("_VignetteIntensity", 0.75f);
    }
}
