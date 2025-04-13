using UnityEngine;


public enum ChoosingClass
{
    swordAndShield,
    bow,
    axe,
    mage
}
public class characterClassStatsAbilities : MonoBehaviour
{
    [SerializeField] private ChoosingClass choosingClass;
    private playerHealth playerHealth;
    public Animator animator;

    [Header("Stats")]
    public float totalHealth;
    public float runSpeed;
    public float jumpForce;
    public float jumpCooldown;

    [Header("Permanent Stats Modifier")]
    private float maxHealthModifier;
    private float maxHealthAdd;
    private float maxHealthTotalAdded;
    private float runSpeedModifier;
    private float runSpeedAdd;
    private float runSpeedTotalAdded;
    private float jumpForceModifier;
    private float jumpForceAdd;
    private float jumpForceTotalAdded;
    private float jumpCooldownModifier;

    private bool addNewMaxHP;
    private bool addNewMaxHPModifier;
    private bool addNewRunSpeed;
    private bool addNewRunSpeedModifier;
    private bool addNewJumpForce;
    private bool addNewJumpModifier;

    private float baseHealth = 100f;
    private float baseRunSpeed = 6f;
    private float baseJumpForce = 1f;
    private float baseJumpCooldown = 1f;

    bool saS;
    bool bow;
    bool axe;
    bool mage;

    private void Start()
    {
        animator = GetComponent<Animator>();
        totalHealth = baseHealth;
        runSpeed = baseRunSpeed;
        jumpForce = baseJumpForce;
        jumpCooldown = baseJumpCooldown;
        playerHealth = GetComponent<playerHealth>();
        playerHealth.basedHPIni(baseHealth);
    }


    private void Update()
    {
        updateStats();

        switch (choosingClass)
        {
            case ChoosingClass.swordAndShield:
                {
                    Debug.Log("Sword and Shield");
                    SaS();
                    break;
                }
            case ChoosingClass.bow:
                {
                    Debug.Log("Bow");
                    SaS();
                    break;
                }
            case ChoosingClass.axe:
                {
                    Debug.Log("Axe");
                    Axe();
                    break;
                }
            case ChoosingClass.mage:
                {
                    Debug.Log("Mage");
                    Mage();
                    break;
                }
            default:
                {
                    Debug.Log("You should not be seeing this");
                    break;
                }
        }
    }

    private void SaS()
    {
        animator.SetBool("characterSaS", true);
    }
    private void Axe()
    {
        animator.SetBool("characterAxe", true);
    }
    private void Mage()
    {
        animator.SetBool("characterMage", true);
        saS = false;
        bow = false;
        axe = false;
        mage = true;
    }


    ///////////////// PERMANENT STAT UPS
    public void addMaxHP(float add)
    {
        maxHealthAdd += add;
        maxHealthTotalAdded += add;
        addNewMaxHP = true;
    }

    public void addMaxHPModifier(float add)
    {
        maxHealthModifier += add;
        addNewMaxHPModifier = true;
    }

    public void addRunSpeed(float add)
    {
        runSpeedAdd += add;
        addNewRunSpeed = true;
        runSpeedTotalAdded += add;
    }
    public void addRunSpeedModifier(float add)
    {
        runSpeedModifier += add;
        addNewRunSpeedModifier = true;
    }
    public void addJumpForce(float add)
    {
        jumpForceAdd += add;
        jumpForceTotalAdded += add;
        addNewJumpForce = true;
    }
    public void addJumpForceModifier(float add)
    {
        jumpForceModifier += add;
        addNewJumpModifier = true;
    }

    private void updateStats()
    {
        if (addNewMaxHP)
        {
            totalHealth += maxHealthAdd;
            addNewMaxHP = false;
            maxHealthAdd = 0;
            playerHealth.updateMaxHP(totalHealth);
        }
        else if (addNewMaxHPModifier)
        {
            totalHealth = (baseHealth + maxHealthTotalAdded) + ((baseHealth + maxHealthTotalAdded) * maxHealthModifier);
            addNewMaxHPModifier = false;
            playerHealth.updateMaxHP(totalHealth);
        }
        else if (addNewRunSpeed)
        {
            runSpeed += runSpeedAdd;
            addNewRunSpeed = false;
            runSpeedAdd = 0;
        }
        else if (addNewRunSpeedModifier)
        {
            runSpeed = (baseRunSpeed + runSpeedTotalAdded) + ((baseRunSpeed + runSpeedTotalAdded) * runSpeedModifier);
            addNewRunSpeedModifier = false;
        }
        else if (addNewJumpForce)
        {
            jumpForce += jumpForceAdd;
            addNewJumpForce = false;
        }
        else if (addNewJumpModifier)
        {
            jumpForce = (baseJumpForce + jumpForceTotalAdded) + ((baseJumpForce + jumpForceTotalAdded) * jumpForceModifier);
            addNewJumpModifier = false;
        }
    }

    ///////////////// PERMANENT STAT UPS
}

