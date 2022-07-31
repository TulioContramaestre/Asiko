using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
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

    // death needs to be adjusted to fit bosses a bit better
    public void death()
    {
        foreach (Behaviour childCompnent in gameObject.GetComponents<Behaviour>())
            childCompnent.enabled = false;

        transform.Find("LeftLeg").GetComponent<Balance>().enabled = false;
        transform.Find("LeftFoot").GetComponent<Balance>().enabled = false;
        transform.Find("RightLeg").GetComponent<Balance>().enabled = false;
        transform.Find("RightFoot").GetComponent<Balance>().enabled = false;
        transform.Find("Hips").GetComponent<Balance>().enabled = false;
        transform.Find("Torso").GetComponent<Balance>().enabled = false;
        transform.Find("Head").GetComponent<Balance>().enabled = false;

        // If the gameObject that died was a MeleeAI then disable their scripts
        //try
        //{
            slider.gameObject.SetActive(false);
        //}
        //catch
        //{
        //    Debug.Log("That wasn't an enemy. Try catch statement in death()");
        //}

        //turnoff health bar

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
