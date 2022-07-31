using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
    public EnemyHealthBar enemyHealthBar;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        setHealth(curHealth);
    }

    void Update()
    {
        if (curHealth <= 0)
            death();
    }

    public void takeDamage(float damage)
    {
        curHealth -= damage;
        setHealth(curHealth);
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

    public void death()
    {
        foreach (Behaviour childCompnent in gameObject.GetComponents<Behaviour>())
            childCompnent.enabled = false;

        // Disable all body parts from balancing.
        transform.Find("LeftLeg").GetComponent<Balance>().enabled = false;
        transform.Find("LeftFoot").GetComponent<Balance>().enabled = false;
        transform.Find("RightLeg").GetComponent<Balance>().enabled = false;
        transform.Find("RightFoot").GetComponent<Balance>().enabled = false;
        transform.Find("Hips").GetComponent<Balance>().enabled = false;
        transform.Find("Torso").GetComponent<Balance>().enabled = false;
        transform.Find("Head").GetComponent<Balance>().enabled = false;

        // Disable all weapon-related scripts
        Transform weaponScript = transform.Find("RightHand");
        if (weaponScript.GetComponent<MeleeAI_Attack>() != null)
            weaponScript.GetComponent<MeleeAI_Attack>().enabled = false;
        else if (weaponScript.GetComponent<RangedAI_Attack>() != null)
            weaponScript.GetComponent<RangedAI_Attack>().enabled = false;
        slider.gameObject.SetActive(false);

    }
}
