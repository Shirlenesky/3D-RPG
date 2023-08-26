using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates{
    GUARD, PATROL, CHASE, DEAD
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private EnemyStates enemyState;

    private NavMeshAgent agent;

    protected GameObject attackTarget;

    private Animator animator;

    private float speed;

    private Vector3 guardPos;

    protected CharacterStats characterStats;

    private float lastAttackTime;

    private Quaternion guradRotation;

    private new Collider collider;

    [Header("---- Basic Setting ----")]
    public float sightRadius;

    public float lookAtTime;

    private float remainLookAtTime;

    [Header("---- Patrol Setting ----")]
    public float patrolRange;

    public bool isGuard;

    private Vector3 wayPoint;

    private bool playerDead = false;

    //anim bool set
    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        collider = GetComponent<Collider>();

        
        guradRotation = transform.rotation;

        speed = agent.speed;
        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        guardPos = transform.position;
        if (isGuard)
        {
            enemyState = EnemyStates.GUARD;
        }
        else
        {
            enemyState = EnemyStates.PATROL;
            RandomlyGetNewWayPoint();
        }
        GameManager.Instance.AddObserver(this);
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);

        if(GetComponent<LootSpawner>() && isDead)
        {
            GetComponent<LootSpawner>().SpawnLoot();
        }

        if(QuestManager.IsInitialized && isDead)
        {
            QuestManager.Instance.UpdateQuestProgress(this.name, 1);
        }
    }

    

    private void Update()
    {
        if(characterStats.CurrHealth <= 0)
        {
            isDead = true;
        }

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnim();
            lastAttackTime -= Time.deltaTime;
        }
    }

    public void SwitchAnim()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Critical", characterStats.isCritical);
        animator.SetBool("Die", isDead);
    }

    public void SwitchStates()
    {
        if (isDead)
        {
            enemyState = EnemyStates.DEAD;
        }

        else if (FoundPlayer())
        {
            enemyState = EnemyStates.CHASE;
        }

        switch (enemyState)
        {
            case EnemyStates.GUARD:
                EnemyGuardState();
                break;
            case EnemyStates.PATROL:
                EnemyPatrolState();
                break;
            case EnemyStates.CHASE:
                EnemyChaseState();
                break;
            case EnemyStates.DEAD:
                EnemyDeadState();
                break;
        }
    }

    private void EnemyDeadState()
    {
        collider.enabled = false;
        agent.radius = 0 ;
        Destroy(gameObject, 2f);
    }

    private void EnemyGuardState()
    {
        isChase = false;

        //if (Vector3.Distance(guardPos, transform.position) > 1)
        if (guardPos != transform.position)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;

            if(Vector3.Distance(guardPos , transform.position) <= agent.stoppingDistance)
            {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guradRotation, 0.01f);
            }
        }
    }

    private void EnemyPatrolState()
    {
        isChase = false;

        agent.speed = speed * 0.5f;

        if(Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance)
        {
            isWalk = false;
            if(remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                RandomlyGetNewWayPoint();
            }
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    private void EnemyChaseState()
    {
        isWalk = false;
        isChase = true;
        if(!FoundPlayer())
        {
            isFollow = false;
            if (remainLookAtTime > 0)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                if (isGuard) enemyState = EnemyStates.GUARD;
                else
                {
                    enemyState = EnemyStates.PATROL;
                }

            }
        }
        else
        {
            isFollow = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }

        if(TargetInAttackRange() || TargetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;

            if(lastAttackTime < 0)
            {
                lastAttackTime = characterStats.attackData.coolDown;

                //ÊÇ·ñ±©»÷
                characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                Attack();
            }
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if(TargetInAttackRange())
        {
            animator.SetTrigger("Attack");
        }
        if(TargetInSkillRange())
        {
            animator.SetTrigger("Skill");
        }

    }

    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    private bool TargetInAttackRange()
    {
        if(attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange; 
        }
        else
        {
            return false;
        }
    }

    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }


    private void RandomlyGetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    private void Hit()
    {
        if(attackTarget!= null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        animator.SetBool("Win", true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
