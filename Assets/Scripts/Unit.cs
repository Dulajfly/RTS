using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum Task
    {
        idle, move, follow, chase, attack
    }
    const string animator_speed = "Speed",
        animator_alive = "Alive",
        animator_attack = "Attack";

    public static List<ISelectable> SelectableUnits { get { return selectableUnits; } }
    static List<ISelectable> selectableUnits = new List<ISelectable>();
    
    public float HealthPercent { get { return hp / hpMax; } }
    public bool IsAlive { get { return hp > 0; } }
    public Transform target;

    [SerializeField]
    float hp, hpMax = 100;

    [SerializeField]
    GameObject hpBarPrefab;

    [SerializeField]
    float stoppingDistance = 1;

    protected HealthBar healthBar;
    protected Task task = Task.idle;
    protected NavMeshAgent nav;

    Animator animator;
    

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hp = hpMax;
        healthBar = Instantiate(hpBarPrefab, transform).GetComponent<HealthBar>();
    }

    private void Start()
    {
        if(this is ISelectable)
        {
            selectableUnits.Add(this as ISelectable);
            (this as ISelectable).SetSelected(false);
        }
    }

    private void OnDestroy()
    {
        if (this is ISelectable)
        {
            selectableUnits.Remove(this as ISelectable);
        }
    }

    // Update is called once per frame
    void Update() 
    {
        //if (target)
        //{
        //    nav.SetDestination(target.position); // obieranie targetu
        //}
        if (IsAlive)
        {
            switch (task)
            {
                case Task.idle:
                    Idling();
                    break;
                case Task.move:
                    Moving();
                    break;
                case Task.follow:
                    Following();
                    break;
                case Task.chase:
                    Chasing();
                    break;
                case Task.attack:
                    Attacking();
                    break;
            }
        }

        Animate();
    }

    protected virtual void Idling() 
    {
        nav.velocity = Vector3.zero;
    }
    protected virtual void Moving() 
    {
        float distance = Vector3.Magnitude(nav.destination - transform.position);
        if (distance <= stoppingDistance)
        {
            task = Task.idle;
        }
    }
    protected virtual void Following()
    {
        if (target)
        {
            nav.SetDestination(target.position); // obieranie targetu
        }
        else
        {
            task = Task.idle;
        }
    }
    protected virtual void Chasing() 
    { 
        //todo
    }
    protected virtual void Attacking()
    {
        nav.velocity = Vector3.zero;
    }

    protected virtual void Animate() // nadawanie szybkosci dla animatora 
    {
        var speedVector = nav.velocity; // magnitude mozna tu dac ale jebie sie os y, miec na uwadze

        speedVector.y = 0;
        float speed = speedVector.magnitude;

        animator.SetFloat(animator_speed, speed);

        animator.SetBool(animator_alive, IsAlive);
    }


}
