using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    const string animator_speed = "Speed",
        animator_alive = "Alive",
        animator_attack = "Attack";

    public Transform target;
    NavMeshAgent nav;
    Animator animator;
    
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
    }
}
