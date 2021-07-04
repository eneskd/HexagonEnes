using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int counter = 6;
    [SerializeField] private TextMeshProUGUI text;


    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.SetText(counter.ToString());
        HexDropper.OnActionCompleted += CountDown;
    }

    private void Update()
    {
        text.rectTransform.rotation = Quaternion.identity;
    }

    private void CountDown()
    {
        counter--;
        text.SetText(counter.ToString());

        if (counter == 0)
        {
            GameLoop.I.EndGame();
        }
    }

    private void OnDestroy()
    {
        HexDropper.OnActionCompleted -= CountDown;
    }
}