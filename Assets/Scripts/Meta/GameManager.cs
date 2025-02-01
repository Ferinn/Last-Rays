using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject UI;
    private bool uiState = false;

    private void Awake()
    {
        uiState = false;
        UI.SetActive(false);
    }

    public void ToggleUI()
    {
        uiState = !uiState;
        UI.SetActive(uiState);
    }

    public void Exitgame()
    {
        Application.Quit(0);
    }
}
