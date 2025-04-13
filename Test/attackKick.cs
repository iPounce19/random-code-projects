using UnityEngine;

public class attackKick : MonoBehaviour
{
    public GameObject parentGameObject;
    public GameObject kickProjectile;
    public unitComponent forcedMovementCheck;
    private unitComponent _unitComponent;
    private projectileScript projectileScript;
    public KeyCode kickButton;
    private Animator _animator;
    private bool _isKicking;
    private float _cooldownTime;

    private void Start()
    {
        _unitComponent = parentGameObject.GetComponent<unitComponent>();
        _animator = parentGameObject.GetComponent<Animator>();
        if (kickProjectile.GetComponent<projectileScript>() != null)
        {
            projectileScript = kickProjectile.GetComponent<projectileScript>();
        }
    }

    private void Update()
    {
        forcedMovementCheck.isForcedMovement = _isKicking;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Upper Body.Attack_Kick") && stateInfo.normalizedTime >= 0.8f)
        {
            _isKicking = false;
            _animator.SetBool("isKicking", false);
        }
        if (Input.GetKey(kickButton) && _cooldownTime <= 0)
        {
            kick();
            _cooldownTime = projectileScript.cooldown;
            _animator.Play("Upper Body.Attack_Kick");
            _animator.Play("Lower Body.Attack_Kick");
            _isKicking = true;
            _animator.SetBool("isKicking", true);
        }

        if (_cooldownTime > 0)
        {
            _cooldownTime -= Time.deltaTime;
        }
    }

    void kick()
    {
        if (kickProjectile == null) return;
        projectileScript kickData = kickProjectile.GetComponent<projectileScript>();
        GameObject projectileKick = Instantiate(kickProjectile, _unitComponent.attackPoint.transform.position, _unitComponent.attackPoint.transform.rotation);
        if(_unitComponent.type == unitTeam.player)
        {
            projectileKick.GetComponent<projectileScript>().isPlayerProjectile = true;
        }
        else
        {
            projectileKick.GetComponent<projectileScript>().isEnemyProjectile = true;
        }
        projectileKick.GetComponent<Rigidbody>().AddForce(_unitComponent.attackPoint.transform.forward * kickData.projectileSpeed, ForceMode.Impulse);
    }
}
