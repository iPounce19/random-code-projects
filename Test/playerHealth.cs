using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    private characterClassStatsAbilities healthStats;
    public float maxHP;
    float currentHP;
    public TextMeshProUGUI text_HP;
    public bool isAlive;


    void Start()
    {
        healthStats = gameObject.GetComponent<characterClassStatsAbilities>();
        if(currentHP > 0)
        {
            isAlive = true;
        }
        else
        {
            isAlive = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void basedHPIni(float maxHPAdded)
    {
        maxHP = maxHPAdded;
        currentHP = maxHP;
        updateHPText();
    }

    public void updateMaxHP(float totalHealh)
    {
        // Updates the Max HP seen in the game from PowerUps
        maxHP = totalHealh;
        updateHPText();
    }

    public void playerTakeDamage(float damage)
    {
        currentHP -= damage;
        updateHPText();
        if (currentHP <= 0)
        {
            isAlive = false;
        }
    }
    private void updateHPText()
    {
       text_HP.SetText("HP: " + currentHP + " / " + maxHP);
    }
}
