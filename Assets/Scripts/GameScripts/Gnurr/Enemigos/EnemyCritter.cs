using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCritter : FSMExecutor<EnemyCritter>  {

    public int _ataque;
    public int _life;
    public float _speed;

    public int _point = 0;

    public float _stoppingDistance;
    public float _AttackTime;
    public float _visionDistance;

    private GameObject _target;

    protected override void CreateStates(FiniteStateMachine<EnemyCritter> fsm)
    {
        fsm.AddState(new SleepState(this), true);
        fsm.AddState(new AttackCritter(this), false);
        fsm.AddTransition("SleepState", "AttackCritter", "PLAYER_VISTO");
        fsm.AddTransition("AttackCritter", "SleepState", "PLAYER_DEJADO_VER");
    }

    protected override void _Update(FiniteStateMachine<EnemyCritter> fsm)
    {
        RaycastHit hitInfo;
        Ray r1 = new Ray(transform.position, Vector3.left);
        Ray r2 = new Ray(transform.position, Vector3.right);

        Debug.DrawRay(transform.position, Vector3.right, Color.red);
        
        Debug.DrawRay(transform.position, Vector3.left, Color.red);
        if (fsm.CurrentState == "SleepState")
        {

            if (Physics.Raycast(r1, out hitInfo, _visionDistance)) {
                _point = 0;
            }
            if (Physics.Raycast(r2, out hitInfo, _visionDistance)) {
                _point = 1;
            }
 
            if (Physics.Raycast(r1, out hitInfo, _visionDistance) || Physics.Raycast(r2, out hitInfo, _visionDistance))
            {

                if (hitInfo.collider.gameObject.tag == "Player")
                {
                    Target = hitInfo.collider.gameObject;
                    fsm.Emmit("PLAYER_VISTO");
                }
            }
        }
        else if (fsm.CurrentState == "AttackCritter")
        {
            bool noVisto = false;
            if (Physics.Raycast(r1, out hitInfo, _visionDistance) || Physics.Raycast(r2, out hitInfo, _visionDistance))
            {
                if (hitInfo.collider.gameObject.tag != "Player")
                    noVisto = true;
            }
            else
                noVisto = true;

            if (noVisto)
            {
                fsm.Emmit("PLAYER_DEJADO_VER");
                Target = null;
            }
        }

        if (_life == 0)
        {

            GetComponentInChildren<Animator>().SetBool("Dead", true);
            Destroy(gameObject, 1f);
        }
    }

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.tag == "Bullet")
        {
            _life--;
        }
    }


}

public class SleepState : State<EnemyCritter>
{
    private Transform _target;
    private int _point = 0;
    private SpriteRenderer[] _sprite;
  

    public SleepState(EnemyCritter component) : base(component)
    {

    }

    public override void Init()
    {
        base.Init();
        _sprite = Component.GetComponentsInChildren<SpriteRenderer>();
        _target =null;
     
        
    }

    public override void Update()
    {

        base.Update();
        Component.GetComponentInChildren<Animator>().SetBool("PlayerDetect", false);
    }

 

}

public class AttackCritter : State<EnemyCritter>
{

    private float _time;
    public float smoothLevel = 1.0f;

    private SpriteRenderer[] _sprite;

    public AttackCritter(EnemyCritter component) : base(component)
    {

    }

    public override void Init()
    {
        base.Init();
        _sprite = Component.GetComponentsInChildren<SpriteRenderer>();
        Attack();
        _time = Component._AttackTime;
    }

    public override void Update()
    {
        base.Update();

        if (Component.Target != null) {


            if (Component._point == 0)
            {
                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = false;
                }
            }
            else {
                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = true;
                }
            }

            MoveEnemy(Component.Target.transform.position);
            _time -= Time.deltaTime;
            if (_time <= 0f)
            {
                Attack();
                _time = Component._AttackTime;
            }
        }

         
    }

    public void MoveEnemy(Vector3 pos)
    {
        Component.GetComponentInChildren<Animator>().SetBool("PlayerDetect", true);
        Component.transform.position = Vector3.Lerp(Component.transform.position, pos, Time.deltaTime * smoothLevel);

    }

    private void Attack()
    {
        if (Component.Target.GetComponent<Player>() != null) {
            Debug.Log("Atacando a " + Component.Target);
            Component.Target.GetComponent<Player>().RestaVidaEnemigo(Component._ataque);

        }
       
    }
}
