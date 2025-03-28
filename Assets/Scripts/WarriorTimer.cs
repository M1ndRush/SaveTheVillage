using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Image img;
    private float currentTime;
    public bool Tick;
    public float maxTime;

    void Start()
    {
        img = GetComponent<Image>();
        currentTime = 0;
    }

    void Update()
    {
        Tick = false;
        if (gameManager.warriorIsTraining)
        {
            currentTime += Time.deltaTime;
        }
        if (currentTime >= maxTime)
        {
            Tick = true;
            currentTime = 0;
        }

        img.fillAmount = currentTime / maxTime;
    }
}
