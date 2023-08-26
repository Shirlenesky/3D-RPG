using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockState { HitPlayer, HitEnemy, HitNothing}

    public RockState rockState;

    private Rigidbody rb;
    [Header("---- Basic Setting ----")]

    public float force;

    public int damage;

    public GameObject target;

    Vector3 dir;

    public GameObject breakEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;

        rockState = RockState.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockState = RockState.HitNothing;
        }
    }

    private void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;

        dir = (target.transform.position - transform.position + Vector3.up).normalized;

        rb.AddForce(dir * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(rockState)
        {
            case RockState.HitPlayer:
                if(collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = dir * force;

                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, collision.gameObject.GetComponent<CharacterStats>());

                    rockState = RockState.HitNothing;
                }
                break;
            case RockState.HitEnemy:
                if(collision.gameObject.GetComponent<Golem>())
                {
                    var otherStats = collision.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);


                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case RockState.HitNothing:
                break;
        }
    }
}
