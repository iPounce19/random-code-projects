using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth;
    private float currentHealth;
    public GameObject body;
    public bool canRegen;
    public bool isAlive;
    public float nextRegenTime;
    public float nextRegenTimeAfterDamage; // Time to wait after taking damage to start regen, nextRegenTime + nextRegenTimeAfterDamage = total time to wait
    private float _nextRegenTime;
    Vector3 originalScale;

    void Start()
    {
        currentHealth = maxHealth;
        originalScale = body.transform.localScale;
        _nextRegenTime = nextRegenTime;
        if (currentHealth > 0)
        {
            isAlive = true;
        }
        else
        {
            isAlive = false;
        }
    }

    private void FixedUpdate()
    {
        if (_nextRegenTime > 0)
        {
            _nextRegenTime -= Time.deltaTime;
        }
        if (canRegen && isAlive)
        {
            if (currentHealth < maxHealth && _nextRegenTime <= 0)
            {
                currentHealth += 0.1f;
                _nextRegenTime = nextRegenTime;
            }
        }
    }
    public void takeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            damageAnimation();
            _nextRegenTime = nextRegenTimeAfterDamage + nextRegenTime;
        }
        
        if (currentHealth <= 0)
        {
            isAlive = false;
            currentHealth = 0;
            notAlive();
        }
    }   
    private async void damageAnimation()
    {
        Transform enemyBody = body.GetComponent<Transform>();
        Material enemyMaterial = body.GetComponent<Renderer>().material;
        
        if (enemyBody != null)
        {
            enemyBody.transform.localScale = new Vector3(originalScale.x +0.1f, originalScale.y + 1f, originalScale.z +0.1f);
            enemyMaterial.color = Color.white;
            Debug.Log("Damage Animation");
            await Task.Delay(100); //Miliseconds
            enemyBody.transform.localScale = originalScale;
            enemyMaterial.color = Color.red;
        }
        else return;

    }

    private async void notAlive()
    {
        await Task.Delay(1000);
        GameObject.Destroy(gameObject);
    }

}
