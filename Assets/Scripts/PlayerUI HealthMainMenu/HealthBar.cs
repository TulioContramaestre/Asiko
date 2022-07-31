using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public static HealthBar instance { get; private set; }

    public static HealthBar GetInstance()
    {
        return instance;
    }

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        slider.value = PlayerHealth.curHealth;
    }

    public void setMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    } 

    
    public void setHealth(float health)
    {
        slider.value = health;
    }

}
