using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject gameWonScreen;
    [SerializeField] private GameObject gameLostScreen;

    private GameObject currentScreen;

    void Start()
    {
        startScreen.SetActive(true);
        currentScreen = startScreen;
    }

    public void ChangeScreen(GameObject state)
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
            state.SetActive(true);
            currentScreen = state;
        }

    }
}
