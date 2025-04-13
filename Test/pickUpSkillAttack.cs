using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class pickUpSkillAttack : MonoBehaviour
{
    public enum checkAttackSkillSlot
    {
        attack1,
        attack2,
        skill1,
        skill2,
        skill3,

    }

    public attackSkillList attackSkill;
    public checkAttackSkillSlot attackSkillSlot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform parentTransform = other.transform;
            Transform childTransform1 = parentTransform.Find("attackSkillHolder");

            switch (attackSkillSlot)
            {
                case checkAttackSkillSlot.attack1:
                    Transform childTransform2 = childTransform1.Find("attack1");
                    newAttackSystem attackSystem1 = childTransform2.GetComponent<newAttackSystem>();
                    if (attackSystem1 != null)
                    {
                        attackSystem1.pickUpAttack(attackSkill);
                        //Destroy(gameObject);
                    }
                    break;
                default:
                    Debug.Log("No attack slot found");
                    break;

            }
        }
    }
}
