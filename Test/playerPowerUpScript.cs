using UnityEngine;

public class playerPowerUpScript : MonoBehaviour
{
    [Header("Temporary or Permanent?")]
    public bool temporary_permanent;

    [Header("Temporary")]
    public float speedBoost;
    public float speedBoostDuration;

    [Header("Permanent Power Up")]
    public float addMaxHP;
    public float addMaxHPModifier;
    public float addRunSpeed;
    public float addRunSpeedModifier;
    public float addJumpForce;
    public float addJumpForceModifier;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Player pick up a power up!");

            // Temporary Speed Boost
            SimplePlayerMovement simplePlayerMovement = collider.GetComponent<SimplePlayerMovement>();
            if (speedBoost > 0)
            {
                simplePlayerMovement.startTempSpeedBoost(speedBoost, speedBoostDuration);
            }
            //

            // Permanent Power Ups, "IF" to make sure that all functions wouldn't run at the same time.
            characterClassStatsAbilities characterClassStatsAbilities = collider.GetComponent<characterClassStatsAbilities>();
            if (addMaxHP > 0)
            {
                characterClassStatsAbilities.addMaxHP(addMaxHP);
            }
            if (addMaxHPModifier > 0)
            {
                characterClassStatsAbilities.addMaxHPModifier(addMaxHPModifier);
            }
            if (addRunSpeed > 0)
            {
                characterClassStatsAbilities.addRunSpeed(addRunSpeed);
            }
            if (addRunSpeedModifier > 0)
            {
                characterClassStatsAbilities.addRunSpeedModifier(addRunSpeedModifier);
            }
            if (addJumpForce > 0)
            {
                characterClassStatsAbilities.addJumpForce(addJumpForce);
            }
            if (addJumpForceModifier > 0)
            {
                characterClassStatsAbilities.addJumpForceModifier(addJumpForceModifier);
            }

            //

            //Destroy after pickup
            //GameObject.Destroy(gameObject);
        }
    }
}
