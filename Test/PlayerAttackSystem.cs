using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public Camera thirdPersonCamera;
    public GameObject[] projectilePrefabs;

    public bool _isAttacking;
    private float[] cooldownTimers;

    private float damage;
    private float projectileRange;
    private float projectileSpeed;

    projectileScript checkProjectileScript;

    private int selectedProjectileIndex = 0;
    private float damageModifier = 0f;
    private float projectileSpeedModifier = 0f;
    private float projectileRangeModifier = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cooldownTimers = new float[projectilePrefabs.Length];
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedProjectileIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedProjectileIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedProjectileIndex = 2;
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0) && cooldownTimers[selectedProjectileIndex] <= 0)
        {
            animator.SetBool("isAttacking", true);
            testProjectile();
            _isAttacking = true;
            Debug.Log("Attack!");
            /*if ()
            {

            }
            else
            {
            d

            }*/

        }
        else
        {
            _isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
    }

    private void checkClass()
    {
        if(animator.GetBool("characterMage"))
        {   
            
        }
    }

    private void testProjectile()
    {
        if (selectedProjectileIndex >= 0 && selectedProjectileIndex < projectilePrefabs.Length)
        {
            if (projectilePrefabs[selectedProjectileIndex].GetComponent<projectileScript>() != null)
            {
                checkProjectileScript = projectilePrefabs[selectedProjectileIndex].GetComponent<projectileScript>();


                GameObject projectile = Instantiate(projectilePrefabs[selectedProjectileIndex], attackPoint.position, attackPoint.rotation);
                ////////// Convert prefab data into local data and put it in the instantiated projectile
                damage = checkProjectileScript.damage + (checkProjectileScript.damage * damageModifier);
                projectileRange = checkProjectileScript.projectileRange + (checkProjectileScript.projectileRange * projectileRangeModifier);
                projectileSpeed = checkProjectileScript.projectileSpeed + (checkProjectileScript.projectileSpeed * projectileSpeedModifier);
                ////////
                ///
                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                projectileScript projectileData = projectile.GetComponent<projectileScript>();
                projectileData.damage = damage;
                projectileData.projectileRange = projectileRange;

                if(transform.root.tag == "Player")
                {
                    projectileData.isPlayerProjectile = true;
                }


                /*if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Vector3 launchDirection = (hit.point - attackPoint.position).normalized;
                    Vector3 forceToAdd = launchDirection * (projectileSpeed + (projectileSpeed * projectileSpeedModifier));
                    projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
                }
                else*/ //Code for Third-person 
                //{
                    Vector3 launchDirection = attackPoint.transform.forward;
                    Vector3 forceToAdd = launchDirection * projectileSpeed;
                    projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
                //}
                Debug.Log("Damage: " + damage + " + Projectile Range: " + projectileRange + " + Projectile Speed: " + projectileSpeed);
                cooldownTimers[selectedProjectileIndex] = checkProjectileScript.cooldown;
            }
        }
        else
        {
            Debug.Log("Error");
        }

    }
}
