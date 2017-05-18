using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : FSMExecutor<EnemyBat>
{
    
    public float offVolando;
    public int _life = 1;
    public int _ataque = 2;
    public Transform center;
    public float _speed;
    public float degreesPerSecond = -65.0f;
    public int NumTimes = 3;
    public float DelayFeedBack = 0.1f;


    private Vector3 v;
    private GameObject _target;
    public float smoothLevel = 1.0f;
    public float _AttackTime= 1.0f;
    public Color feedBack;
    private SpriteRenderer[] _spriteEnemy;
    public float _visionDistance = 5;
    public bool _attack = false;

    //TODO Crear el estado follow y modificar el atacar
    protected override void CreateStates(FiniteStateMachine<EnemyBat> fsm)
    {
        fsm.AddState(new FlyState(this), true);
        fsm.AddState(new FlyFollow(this), false);
        fsm.AddTransition("FlyState", "FlyFollow", "PLAYER_VISTO");
        fsm.AddTransition("FlyFollow", "FlyState", "PLAYER_DEJADO_VER");
  
        _spriteEnemy = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void _Update(FiniteStateMachine<EnemyBat> fsm)
    {


        RaycastHit hitInfo;
        Ray r1 = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * _visionDistance, Color.blue);

        if (fsm.CurrentState == "FlyState")
        {
            if (Physics.Raycast(r1, out hitInfo, _visionDistance))//El player pasa debajo del enemigo
            {
                if (hitInfo.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player detectado.");
                    Target = hitInfo.collider.gameObject;
                    fsm.Emmit("PLAYER_VISTO");
                }

            }

        } else if (fsm.CurrentState == "FlyAttack")
        {
            //Vector3 newPos = Vector3.Lerp(transform.position, Target.transform.position, Time.deltaTime * smoothLevel);

        }

        if (_life == 0)
        {
            //GetComponentInChildren<Animator>().SetBool("Dead", true);
            Destroy(gameObject, 1f);
        }
    }

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public Vector3 VectorRadius
    {
        get { return v;}
        set { v = value;}
    }

    public void RestaVida(int life)
    {
        _life -= life;

        StartCoroutine(FlashSprites(_spriteEnemy, NumTimes, DelayFeedBack, feedBack));
    }

    IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float delay, Color feedBack)
    {

        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++)
        {
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++)
            {

                // for changing the alpha
                sprites[i].color = new Color(feedBack.r, feedBack.g, feedBack.b, feedBack.a);
            }
            // delay specified amount
            yield return new WaitForSeconds(delay);
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = new Color(255, 255, 255, 1);
            }

            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _attack = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _attack = false;
        }

    }
}

public class FlyState : State<EnemyBat>
{


    public FlyState(EnemyBat component) : base(component)
    {

    }

    public override void Init()
    {
        base.Init();
        Component.VectorRadius = Component.transform.position - Component.center.position;
        
    }

    public override void Update()
    {
        base.Update();

        Component.VectorRadius = Quaternion.AngleAxis(Component.degreesPerSecond * Time.deltaTime, Vector3.forward) * Component.VectorRadius;
        Component.transform.position = Component.center.position + Component.VectorRadius;


    }
}

public class FlyFollow : State<EnemyBat>
{

    private SpriteRenderer[] _sprite;
    private CharacterController _controller;
    private float _time;



    public FlyFollow(EnemyBat component) : base(component)
    {

    }

    public override void Init()
    {
        base.Init();
        _sprite = Component.GetComponentsInChildren<SpriteRenderer>();
        _controller = Component.GetComponent<CharacterController>();
    }

    public override void Update()
    {
        base.Update();
        Vector3 direction = Component.Target.transform.position - Component.transform.position;
        // Vector3 newPos = Vector3.Lerp(Component.transform.position, Component.Target.transform.position, Time.deltaTime * Component.smoothLevel);
        if (Component.Target.transform.position.x > Component.transform.position.x)
        {
            foreach (SpriteRenderer s in _sprite)
            {
                s.flipX = true;
            }

        }else
        {
            foreach (SpriteRenderer s in _sprite)
            {
                s.flipX = false;
            }
        }
        _controller.Move(new Vector3(direction.x * Component._speed * Time.deltaTime, (direction.y + Component.offVolando )* Component._speed * Time.deltaTime,0));
        //Component.transform.position = newPos;

        _time -= Time.deltaTime;
        if (_time <= 0f && Component._attack)
        {
            Attack();
            _time = Component._AttackTime;
        }
    }

    private void Attack()
    {

        Player p = Component.Target.GetComponent<Player>();
        p.RestaVidaEnemigo(Component._ataque, false);


    }
}
