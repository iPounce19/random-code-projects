using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class newAttackSystem : MonoBehaviour
{

    public attackSkillList attackSkill;
    public KeyCode attackButton = KeyCode.Mouse0;
    


    private void Start()
    {
        disableAllSkill();
    }

    public void setAttackSkill()
    {
        disableAllSkill();
        

        if(attackSkill == attackSkillList.Kick)
        {  
            attackKick kick = GetComponent<attackKick>();
            kick.enabled = true;
            kick.kickButton = attackButton;
        }
        else if (attackSkill == attackSkillList.Fireball)
        {
            // Enable fireball skill
        }
        else if (attackSkill == attackSkillList.Flamethrower)
        {
            // Enable flamethrower skill
        }
    }
    public void disableAllSkill()
    {
        attackKick kick = GetComponent<attackKick>();
        kick.enabled = false;
    }

    public void pickUpAttack(attackSkillList skill)
    {
        attackSkill = skill;
        setAttackSkill();
    }
}
