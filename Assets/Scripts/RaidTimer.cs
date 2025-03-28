using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Image img;
    private float currentTime;
    public bool Tick;
    public float maxTime;

    void Start()
    {
        img = GetComponent<Image>();
        currentTime = maxTime;
    }

    void Update()
    {
        Tick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Tick = true;
            currentTime = maxTime;
        }

        img.fillAmount = currentTime / maxTime;
    }
}
