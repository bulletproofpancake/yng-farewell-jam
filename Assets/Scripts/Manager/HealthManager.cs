using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Sprite heartFill, heartEmpty;
    [SerializeField] private Image[] hearts;

    public void SetHealth(int healthValue)
    {
        foreach (var heart in hearts)
        {
            heart.sprite = heartEmpty;
        }

        for (int i = 0; i < healthValue; i++)
        {
            hearts[i].sprite = heartFill;
        }
    }

}
