using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    const string animator_speed = "Speed",
        animator_alive = "Alive",
        animator_attack = "Attack";

    public float HealthPercent { get { return hp / hpMax; } }

    [SerializeField]
    float hp, hpMax = 100;

    [SerializeField]
    GameObject hpBarPrefab;

    public Transform target;

    NavMeshAgent nav;
    Animator animator;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hp = hpMax;
        Instantiate(hpBarPrefab, transform);
    }

    // Update is called once per frame
    void Update() 
    {
        if (target)
        {
            nav.SetDestination(target.position); // obieranie targetu
        }
        Animate();
    }

    protected virtual void Animate() // nadawanie szybkosci dla animatora 
    {
        var speedVector = nav.velocity; // magnitude mozna tu dac ale jebie sie os y, miec na uwadze

        speedVector.y = 0;
        float speed = speedVector.magnitude;

        animator.SetFloat(animator_speed, speed);

        animator.SetBool(animator_alive, hp > 0);
    }
}
