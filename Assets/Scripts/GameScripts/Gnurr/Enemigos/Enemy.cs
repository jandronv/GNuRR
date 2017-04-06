using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : FSMExecutor<Enemy> {

    public int _ataque;
    public int _life;
    public float _speed;
    public Transform _initPoint;
    public Transform _endPoint;
    public float _stoppingDistance;
    public float _AttackTime;
    public float _visionDistance;
    public BoxCollider _ColliderEnemigo;

    private GameObject _target;
    private GameObject _oldTarget;
    private GameObject hit;
    private bool _playerVisto = false;

    protected override void CreateStates(FiniteStateMachine<Enemy> fsm)
    {

        fsm.AddState(new WanderState(this), true);
        fsm.AddState(new AttackState(this), false);
        fsm.AddTransition("WanderState", "AttackState", "PLAYER_VISTO");
        fsm.AddTransition("AttackState", "WanderState", "PLAYER_DEJADO_VER");
    }


    protected override void _Update(FiniteStateMachine<Enemy> fsm)
    {

        if (_ColliderEnemigo == null)
        {
            Debug.LogWarning("Collider de enemigo no asignado!!");
        }
        if (fsm.CurrentState == "WanderState")
        {
            if (_playerVisto)
            {
                OldTarget = Target;
                Target = hit;
                if (hit.tag == "Player")
                {
                    fsm.Emmit("PLAYER_VISTO");
                }
            }
        }
        else if (fsm.CurrentState == "AttackState")
        {
            if(!_playerVisto)
            {
                fsm.Emmit("PLAYER_DEJADO_VER");
                Target = OldTarget;
            }
        }
    }

    /// <summary>
    /// Pasamos al ataque
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        _playerVisto = true;
        hit = other.gameObject;
    }

    /// <summary>
    /// Volvemos al patrol
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Sale");
        _playerVisto = false;
        hit = other.gameObject;
    }

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public GameObject OldTarget
    {
        get { return _oldTarget; }
        set { _oldTarget = value; }
    }

}

public class WanderState : State<Enemy>
{
    private Transform _target;
    private int _point = 0;
    private SpriteRenderer[] _sprite;
    public float smoothLevel = 1.0f;

    public WanderState(Enemy executor) : base(executor)
    {

    }

    public override void Init()
    {
        base.Init();
        _sprite = Component.GetComponentsInChildren<SpriteRenderer>();
        _target = Component._initPoint;
        _point = 0;
       
    }

    public override void Update()
    {
        base.Update();

        Vector3 direction = _target.position - Component.transform.position;

        if (direction.sqrMagnitude < (Component._stoppingDistance * Component._stoppingDistance))
        {
            if (_point == 0)
            {
                _target = Component._endPoint;
                _point = 1;
                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = false;
                }
            }
            else
            {

                foreach (SpriteRenderer s in _sprite)
                {
                    s.flipX = true;
                }
                _target = Component._initPoint;
                _point = 0;
            }
        }

        direction = direction.normalized;

        MoveEnemy(_target.position);
        //_controller.Move(new Vector3(direction.x * Component._speed * Time.deltaTime, 0, 0));

    }


    public void MoveEnemy(Vector3 pos)
    {

        Component.transform.position = Vector3.Lerp(Component.transform.position, pos,Time.deltaTime * smoothLevel);

    }
}

public class AttackState : State<Enemy>
{
    private float _time;
    public AttackState(Enemy executor) : base(executor)
    {

    }

    public override void Init()
    {
        base.Init();
        Attack();
        _time = Component._AttackTime;
    }

    public override void Update()
    {
        base.Update();
        _time -= Time.deltaTime;
        if (_time <= 0f)
        {
            Attack();
            _time = Component._AttackTime;
        }
    }

    private void Attack()
    {
        //Component.GetComponent<Player>().RestaVidaEnemigo(Component._ataque);
        Debug.LogWarning("Atacando a " + Component.Target);
    }
}
