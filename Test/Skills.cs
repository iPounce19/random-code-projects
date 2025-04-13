using UnityEngine;


[CreateAssetMenu(fileName = "NewSkillData", menuName ="Skill Data System/Skill Data")]
public class Skills : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public KeyCode assignedKey;
    public AnimationClip animationClip;
    public bool isEquipped;

    [Header("Base Data")]
    public float damage;
    public float projectileSpeed;
    public float projectileRange;
    public float cooldownTime;
    public float knockbackForce;

    [Header("Modifiers")]
    public float damageModifier;
    public float projectileRangeModifier;
    public float cooldownTimeModifier;
    public float knockbackForceModifier;

    [Header("Percentage Modifiers")]
    public float damageModifierPercentage;
    public float projectileRangeModifierPercentage;
    public float cooldownTimeModifierPercentage;
    public float knockbackForceModifierPercentage;


    [Header("Checks")]
    public bool canStun;
}
