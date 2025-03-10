using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerCharacter player;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject lightPreview;
    private bool uiState = false;

    object padlock = new object();

    private void Awake()
    {
        uiState = false;
        UI.SetActive(false);

        for (int i = 0; i < lightPreview.transform.childCount; i++)
        {
            GameObject previewSource = lightPreview.transform.GetChild(i).gameObject;

            previewSource.SetActive(false);
        }
    }

    public ICharacter GetPlayer()
    {
        lock (padlock)
        {
            return player;
        }
    }

    public void ToggleUI()
    {
        uiState = !uiState;
        UI.SetActive(uiState);
        Cursor.visible = uiState;
    }

    public void Exitgame()
    {
        Application.Quit(0);
    }
}
