using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainTimer : MonoBehaviour
{
    [SerializeField] private float maxTime;

    private Image img;
    private float currentTime;
    public bool Tick;

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
