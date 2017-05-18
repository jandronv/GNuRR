﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCritter : FSMExecutor<EnemyCritter>  {

    public int _ataque;
    public int _life;
    public float _speed;
    public float maxDistance = 0.1f;
    public Color feedBack;
    public int NumTimes = 3;
    public float DelayFeedBack = 0.1f;

    public bool firsTime = true;

    public int _point = 0;
    public float _gravity = 20.0F;
    public bool _attack = false;
    public float _stoppingDistance;
    public float _AttackTime;
    public float _visionDistance;

    private CharacterController _cController;
    private SpriteRenderer[] _spriteEnemy;
    private GameObject _target;




    protected override void CreateStates(FiniteStateMachine<EnemyCritter> fsm)
    {
        fsm.AddState(new SleepState(this), true);
        fsm.AddState(new AttackCritter(this), false);
        fsm.AddState(new FollowState(this), false);
        fsm.AddTransition("SleepState", "FollowState", "PLAYER_VISTO");
        fsm.AddTransition("SleepState", "AttackCritter", "PLAYER_DENTRO");
        fsm.AddTransition("FollowState", "AttackCritter", "PLAYER_DENTRO");
        fsm.AddTransition("FollowState", "SleepState", "PLAYER_DEJADO_VER");
        fsm.AddTransition("AttackCritter", "SleepState", "PLAYER_DEJADO_VER");
        fsm.AddTransition("AttackCritter", "FollowState", "PLAYER_ALEJANDOSE");


        _cController = GetComponent<CharacterController>();
        _spriteEnemy = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void _Update(FiniteStateMachine<EnemyCritter> fsm)
    {
        //Variable para la vision del enemigo
        RaycastHit hitInfo;
        RaycastHit hitInfo2;
        //Variables debug
        Ray r1 = new Ray(transform.position, Vector3.left);
        Ray r2 = new Ray(transform.position, Vector3.right);
        Debug.DrawRay(transform.position, Vector3.left * _visionDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector3.right * _visionDistance, Color.red);

        //Comprobamos en que estado estamos para el comportamiento
        if (fsm.CurrentState == "SleepState")//Esta quieto esperando a que entre el jugador en su campo de vision
        {
            if (Physics.Raycast(r1, out hitInfo, _visionDistance))//Lo detecta en la izq
            {

                if (hitInfo.collider.gameObject.tag == "Player")
                {
                    _point = 0;
                    Target = hitInfo.collider.gameObject;

                    fsm.Emmit("PLAYER_VISTO");
                }
            }

            if (Physics.Raycast(r2, out hitInfo2, _visionDistance))//derecha
            {
                if (hitInfo2.collider.gameObject.tag == "Player")
                {
                    _point = 1;
                    Target = hitInfo2.collider.gameObject;

                    fsm.Emmit("PLAYER_VISTO");
                }
            }
        }
        else if (fsm.CurrentState == "FollowState")//Se empieza a mover hacia el target(Player)
        {
            bool Visto = false;

            if (Physics.Raycast(r1, out hitInfo, _visionDistance))//Comprobamos si el player ha salido del rango de vision
            {
                if (hitInfo.collider.gameObject.tag == "Player")
                    Visto = true;
            }
            else if (Physics.Raycast(r2, out hitInfo2, _visionDistance))
            {
                if (hitInfo2.collider.gameObject.tag == "Player")
                    Visto = true;
            }
            if (!Visto)
            {
                fsm.Emmit("PLAYER_DEJADO_VER");

            }
     
            //cambiar a ataque
        }
        else if (fsm.CurrentState == "AttackCritter")
        {
            if (!_attack)
            {
                fsm.Emmit("PLAYER_ALEJANDOSE");
            }
        }
        if (_life == 0)
        {
            GetComponentInChildren<Animator>().SetBool("Dead", true);
            Destroy(gameObject, 1f);
        }
        //El player ha colisionado con el enemigo
        if (_attack)
        {
            fsm.Emmit("PLAYER_DENTRO");
        }
        //Aplicamos la gravedad por si se queda un poco en el aire y se cambia de estado
        Vector3 direction = Vector3.zero;
        direction.y -= _gravity * Time.deltaTime;
        _cController.Move(new Vector3(0, direction.y, 0));
    }

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
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

    public class SleepState : State<EnemyCritter>
{

    private SpriteRenderer[] _sprite;
  

    public SleepState(EnemyCritter component) : base(component)
    {
        
    }

    public override void Init()
    {
        base.Init();
        _sprite = Component.GetComponentsInChildren<SpriteRenderer>();
     
     
        
    }

    public override void Update()
    {

        base.Update();
        Component.GetComponentInChildren<Animator>().SetBool("PlayerDetect", false);
    }
}

public class FollowState : State<EnemyCritter>
{

    
    public float smoothLevel = 1.0f;
    private CharacterController _controller;
    private SpriteRenderer[] _sprite;

    public FollowState(EnemyCritter component) : base(component)
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

        if (Component.Target != null)
        {
            Vector3 direction = Component.Target.transform.position - Component.transform.position;
            if (Component._point == 0)//Segun por que lado este el personaje hacemos el flip en los sprite
            {
                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = true;
                }
            }
            else
            {
                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = false;
                }
            }
            Component.GetComponentInChildren<Animator>().SetBool("PlayerDetect", true);
            direction.y -= Component._gravity * Time.deltaTime;
            _controller.Move(new Vector3(direction.x * Component._speed * Time.deltaTime, direction.y, 0));     
        } 
    }
}

public class AttackCritter : State<EnemyCritter> {

    private float _time;

    public AttackCritter(EnemyCritter component) : base(component)
    {
    }

    public override void Init()
    {
        base.Init();
        _time = Component._AttackTime;
    }

    public override void Update()
    {
        _time -= Time.deltaTime;
        if (Component.firsTime) {
            Component.firsTime = false;
            Attack();
        }
        if (_time <= 0f)
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