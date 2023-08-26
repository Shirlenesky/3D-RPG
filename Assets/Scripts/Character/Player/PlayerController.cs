using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private Animator animator;

    private GameObject attackTarget;
    private float lastAttackTime;

    private CharacterStats characterStats;

    private bool isDead;

    private float stopDistance;

    private void Awake()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();

        stopDistance = navAgent.stoppingDistance;

    }

    private void Start()
    {
        GameManager.Instance.RigisterPlayer(characterStats);
    }


    private void OnEnable()
    {
        EventHandler.ClickToMove += OnClickToMove;
        EventHandler.ClickEnemyAttack += OnClickEnemyAttack;
    }

    private void OnDisable()
    {
        EventHandler.ClickToMove -= OnClickToMove;
        EventHandler.ClickEnemyAttack -= OnClickEnemyAttack;
    }

    private void Update()
    {
        isDead = characterStats.CurrHealth == 0 ? true : false;

        if(isDead)
        {
            GameManager.Instance.NotifyObservers();
        }

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", navAgent.velocity.sqrMagnitude);

        animator.SetBool("Die", isDead);
    }

    private void OnClickToMove(Vector3 position)
    {
        StopAllCoroutines();
        if (isDead) return;

        navAgent.stoppingDistance = stopDistance;
        navAgent.isStopped = false;
        navAgent.destination = position;
    }

    private void OnClickEnemyAttack(GameObject target)
    {
        if (isDead) return;
        if (target == null) return;
        attackTarget = target;
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
        StartCoroutine(MoveToEnemy(target));
    }

    IEnumerator MoveToEnemy(GameObject target)
    {
        navAgent.isStopped = false;
        navAgent.stoppingDistance = characterStats.attackData.attackRange;

        transform.LookAt(target.transform);

        while (Vector3.Distance(target.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            navAgent.destination = target.transform.position;
            yield return null;
        }

        navAgent.isStopped = true;

        if (lastAttackTime < 0)
        {
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //animation event
    private void Hit()
    {
        if(attackTarget.CompareTag("Attackable"))
        {
            if(attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockState == Rock.RockState.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockState = Rock.RockState.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
