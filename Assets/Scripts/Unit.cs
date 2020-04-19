using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    const string animator_speed = "Speed",
        animator_alive = "Alive",
        animator_attack = "Attack";

    public static List<ISelectable> SelectableUnits { get { return selectableUnits; } }
    static List<ISelectable> selectableUnits = new List<ISelectable>();

    public float HealthPercent { get { return hp / hpMax; } }

    public Transform target;

    [SerializeField]
    float hp, hpMax = 100;

    [SerializeField]
    GameObject hpBarPrefab;

    protected HealthBar healthBar;

    NavMeshAgent nav;
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
