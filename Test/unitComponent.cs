using UnityEngine;


public enum unitTeam
{
    player,
    enemy
}

public enum playerRole
{
    mage,
}

public enum unitRole
{
    enemy_turret,

}

public class unitComponent : MonoBehaviour
{

    [Header("References")]
    public Animator animator;



    [Header("Basic Information")]
    public string unitName;
    public unitTeam type;
    public playerRole playerRole;
    public unitRole unitRole;

    [Header("Get Checks")]
    public bool isAlive;
    public bool isMoving;
    public bool isForcedMovement;
    public bool isJumping;
    public bool isPressingAttack1;
    public bool isPressingAttack2;

    [Header("For Enemy")]
    public bool isEnemyAlive;


    [Header("References if Player")]
    public Camera playerCamera;
    public Rigidbody playerRigidbody;
    public GameObject orientation;
    public characterClassStatsAbilities classStatsAbilities;
    public playerHealth playerHealth;
    public GameObject attackPoint;


    [Header("References if Enemy")]
    public GameObject enemyBody;


    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isPressingAttack1 = true;
        }
        else
        {
            isPressingAttack1 = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isPressingAttack2 = true;
        }
        else
        {
            isPressingAttack2 = false;
        }

    }

    private void FixedUpdate()
    {
        switch(type)
        {
            case unitTeam.player:
                {
                    playerHealth playerHealth = GetComponent<playerHealth>();
                    if (playerHealth != null)
                    {
                        isAlive = playerHealth.isAlive;
                    }
                    break;
                }
            case unitTeam.enemy:
                {
                    enemyHealth enemyHealth = GetComponent<enemyHealth>();
                    if (enemyHealth != null)
                    {
                        isEnemyAlive = enemyHealth.isAlive;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

}


