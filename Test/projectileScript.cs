using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    public Skills skillData;
    public float damage;
    public float knockbackForce;
    public float projectileRange;
    public float projectileSpeed;
    public float cooldown;
    public bool isDestroyedOnImpact;
    public bool isPlayerProjectile;
    public bool isEnemyProjectile;


    private float baseDamage;
    private float baseKnockbackForce;
    private float baseProjectileRange;
    private float baseProjectileSpeed;
    private float baseCooldownTime;


    private float damageModifier;
    private float damageModifierPercentage;
    private float projectileRangeModifier;
    private float projectileRangeModifierPercentage;
    private float cooldownTimeModifier;
    private float cooldownTimeModifierPercentage;
    private float knockbackForceModifier;
    private float knockbackForceModifierPercentage;

    private Vector3 lastposition;
    private float totalDistance = 0f;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    private float totalDistanceTraveled;

    private void Start()
    {
        lastposition = transform.position;
        damage = skillData.damage;
        knockbackForce = skillData.knockbackForce;
        projectileRange = skillData.projectileRange;
        projectileSpeed = skillData.projectileSpeed;
        cooldown = skillData.cooldownTime;
        baseDamage = skillData.damage;
        baseKnockbackForce = skillData.knockbackForce;
        baseProjectileRange = skillData.projectileRange;
        baseProjectileSpeed = skillData.projectileSpeed;
        baseCooldownTime = skillData.cooldownTime;
        damageModifier = skillData.damageModifier;
        damageModifierPercentage = skillData.damageModifierPercentage;
        projectileRangeModifier = skillData.projectileRangeModifier;
        projectileRangeModifierPercentage = skillData.projectileRangeModifierPercentage;
        cooldownTimeModifier = skillData.cooldownTimeModifier;
        cooldownTimeModifierPercentage = skillData.cooldownTimeModifierPercentage;
        knockbackForceModifier = skillData.knockbackForceModifier;
        knockbackForceModifierPercentage = skillData.knockbackForceModifierPercentage;

        updateDamage();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(lastposition, transform.position);

        totalDistance += distance;

        lastposition = transform.position;

        if (totalDistance >= projectileRange)
        {
            GameObject.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitObject = collider.gameObject;
        if(collider.gameObject.tag == "Player" && isEnemyProjectile)
        {
            
            if(collider.gameObject.GetComponent<playerHealth>() != null && !hitEnemies.Contains(hitObject))
            {
                Debug.Log("You hit a player!");
                playerHealth playerHP = collider.gameObject.GetComponent<playerHealth>();
                playerHP.playerTakeDamage(damage);
                hitEnemies.Add(hitObject);
                if (isDestroyedOnImpact == true)
                {
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("This Player doesn't have a script called 'playerHealth' or this gameObject has already been hit!");
            }
        }
        else if (collider.gameObject.tag == "Enemy" && isPlayerProjectile)
        {
            Rigidbody enemyRB = collider.gameObject.GetComponent<Rigidbody>();
            if (collider.gameObject.GetComponent<enemyHealth>() != null && !hitEnemies.Contains(hitObject))
            {
                enemyHealth enemyhealth = collider.gameObject.GetComponent<enemyHealth>();
                if (enemyhealth != null)
                {
                    enemyhealth.takeDamage(damage);
                    hitEnemies.Add(hitObject);
                    if (isDestroyedOnImpact == true)
                    {
                        GameObject.Destroy(gameObject);
                    }
                }
            }
            else
            {
                Debug.Log("This Enemy doesn't have a script called 'enemyHealth' or this gameObject has already been hit!");
            }
            if (enemyRB != null)
            {
                Vector3 pushDirection = (collider.transform.position - transform.position).normalized;
                enemyRB.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
            }

        }
        else if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "Wall" || collider.gameObject.tag == "Object")
        {
            GameObject.Destroy (gameObject);
        }


    }

    void updateDamage()
    {
        damage = baseDamage + damageModifier + (baseDamage + damageModifier * damageModifierPercentage);
        knockbackForce = baseKnockbackForce + knockbackForceModifier + (baseKnockbackForce + knockbackForceModifier * knockbackForceModifierPercentage);
        projectileRange = baseProjectileRange + projectileRangeModifier + (baseProjectileRange + projectileRangeModifier * projectileRangeModifierPercentage);
        if (cooldown > 0.1f)
        {
            float difference = cooldown - (baseCooldownTime - cooldownTimeModifier - (baseCooldownTime - cooldownTimeModifier * cooldownTimeModifierPercentage));
            if (difference <= 0.1f)
            {
                cooldown = 0.1f;
            }
            else
            {
                cooldown = baseCooldownTime - cooldownTimeModifier - (baseCooldownTime - cooldownTimeModifier * cooldownTimeModifierPercentage);
            }
        }
    }
}
//enemyHealth enemyhealth = collider.gameObject.GetComponent<enemyHealth>();
//enemyhealth.takeDamage(damage);