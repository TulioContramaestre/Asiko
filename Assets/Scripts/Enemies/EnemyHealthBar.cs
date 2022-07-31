using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    //public static EnemyHealthBar instance;

    //public static EnemyHealthBar GetInstance()
    //{
    //    return instance;
    //}

    //void Start()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    instance = this;
    //}

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
