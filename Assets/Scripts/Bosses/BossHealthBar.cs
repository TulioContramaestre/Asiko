using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHealthBar : MonoBehaviour
{
   public Slider sliderCurrent;
   public Slider Damaged;
   private Image damagedImage;
   private Color damagedColor;
   private const float DAMAGE_FADE_TIMER = 1f;
   private float damagefade;
   private TextMeshProUGUI bossname;
   public string bossString;

   public static BossHealthBar instance { get; private set; }

    public static BossHealthBar GetInstance()
    {
        return instance;
    }

    public void setBossName(string name)
    {
        bossname = GameObject.Find("Bossname").GetComponent<TextMeshProUGUI>();
        bossname.SetText(name);
    }

    private void Start()
    {
        setBossName(bossString);
    }

    public void setMaxHealth(float health)
    {
        // damagedImage = transform.Find("Damaged").GetComponent<Image>();
        // damagedColor = damagedImage.color;
        // damagedColor.a = 0f;
        // damagedImage.color = damagedColor;

        sliderCurrent.maxValue = health;
        sliderCurrent.value = health;
        Damaged.maxValue = health;
        Damaged.value = health;

        // damagefade = DAMAGE_FADE_TIMER;
    }

    void Update()
    {
        if (Damaged.value > sliderCurrent.value)
        {
            damagefade -= Time.deltaTime;
            if (damagefade < 0)
            {
                Damaged.value -= 50f * Time.deltaTime;
                // damagedImage.color = damagedColor;
            }
        }

        // damagefade = DAMAGE_FADE_TIMER;
    }

    public void setHealth(float health)
    {
        sliderCurrent.value = health;
        if (Damaged.value <= sliderCurrent.value)
        {
            Damaged.value = health;
            // damagefade = DAMAGE_FADE_TIMER;
        }
        // damagedColor.a = 1f;
        // damagedImage.color = damagedColor;
        damagefade = DAMAGE_FADE_TIMER;
        
    }

    public void SetBossName(string name)
    {
        bossname.text = name;
    }
}
