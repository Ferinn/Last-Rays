using FunkyCode;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{
    private List<Door> doors = new List<Door>();
    
    [SerializeField] public int size;
    [SerializeField] public Sprite closedDoorSprite;
    [SerializeField] public Sprite openDoorSprite;
    public void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject childDoor = transform.GetChild(i).gameObject;
            try
            {
                doors.Add(childDoor.GetComponent<Door>());
            }
            catch
            {
                Debug.Log("Found an improperly configured door gameobject as DoorManager's children " + childDoor.name);
            }
        }
    }

    public void Update()
    {
        foreach (Door door in doors)
        {
            door.UpdateDoor();
        }
    }
}
