using UnityEngine;
using System.Collections;

/// <summary>
/// comportamiento de un enemigo que ataca en mele
/// </summary>
public class MeleEnemy : AComponent {
 
	public float m_InitialVelocity = 0.5f;
	public float m_MaxVelocity = 20f;
	//public float m_gravity = 20f;
	public float m_accel = 20f;
	public string m_tagTarget = "Player";
	public bool HasWalAnim = true;
	public bool HasAttackAnim = true;
	public bool HasDeathAnim = true;
	
	public float m_stopDistance = 6f;
	public float m_runDistance = 10f;
	public float m_timeBetweenAttack = 2f;
	
	private float m_groundSpeed = 0f;
	private Quaternion m_deathRotationBlock;
	//private float m_airSpeed = 0f;
	private Vector3 m_direction = Vector3.zero;
	private GameObject m_target;
	private enum TState{ MOVE, ATTACK, DEATH};
	private TState m_state = TState.MOVE;
	private float m_attackTime;
	private PlayerAnimation m_animationcomponent;
    private bool m_started = false;


    // Call when component is create (only once)
    /*protected override void Create() {

    }*/

    void OnEnable()
    {
        if (m_started)
        {
            ReStart();
        }
    }

	protected override void Start () {
		base.Start();
        ReStart();
        m_started = true;
    }

    protected void ReStart()
    {
        m_target = GameObject.FindGameObjectWithTag(m_tagTarget) as GameObject;
        m_groundSpeed = m_InitialVelocity;
        m_attackTime = 0f;
        m_animationcomponent = GetComponent<PlayerAnimation>();
        m_state = TState.MOVE;
        if (m_animationcomponent != null)
            m_animationcomponent.RegisterOnAnimationEvent = OnAnimationFinish;
    }
 
 
 // Tick is called once per frame
 protected override void Update() {
 
		base.Update();
		if(m_state != TState.DEATH)
		{
			m_direction = m_target.transform.position;
			m_direction.y = transform.position.y;
			this.transform.LookAt(m_direction);
			m_direction = m_target.transform.position - this.transform.position;
		}
		Debug.DrawRay(transform.position,transform.forward*5f, Color.red);
		if(m_state == TState.MOVE)
		{
			//this.rigidbody.isKinematic = false;
			if(m_direction.sqrMagnitude > m_stopDistance*m_stopDistance)
			{
                //Debug.Log("Move");
                if (HasWalAnim)
                    m_animationcomponent.IsWalking = true;

				//Move
				float increment = Trajectories.UniformAccelerateMovement(m_groundSpeed,m_accel,Time.deltaTime);
				//m_direction.y = 0f;
				if(m_groundSpeed < m_MaxVelocity)
				{
					m_groundSpeed = Trajectories.UniformAccelerateVelocity(m_groundSpeed,m_accel,Time.deltaTime);
				}
				Vector3 fordward = transform.forward;
				fordward.y = 0f;
				this.GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + fordward * increment);
                //transform.position += transform.forward*increment;
                m_state = TState.MOVE;
            }
			else
			{
				m_state = TState.ATTACK;
				m_attackTime = m_timeBetweenAttack;
                if (HasAttackAnim)
                    m_animationcomponent.IsAttacking = true;
            }
		}
		else if(m_state == TState.ATTACK)
		{
			m_groundSpeed = m_InitialVelocity;
            //Debug.Log("Attack");

			//this.rigidbody.isKinematic = true;
			//Ejecuto el ataque....
			m_attackTime -= Time.deltaTime;
            bool attackAnim = true;
			if(m_attackTime <= 0f)
			{
				m_attackTime = m_timeBetweenAttack;
                attackAnim = false;
            }
			
			if(m_direction.sqrMagnitude > m_runDistance*m_runDistance)
			{
				m_state = TState.MOVE;
                attackAnim = false;
            }

            if (HasAttackAnim)
                m_animationcomponent.IsAttacking = attackAnim;
		}
		else
		{
			//Muerto...
			Debug.Log(name+" DEATH");
			transform.rotation = m_deathRotationBlock;
		}
 }
	
	void Death()
	{
        if (m_state != TState.DEATH)
        {
            m_state = TState.DEATH;
            m_target.SendMessage("EnemyDeath");
            m_deathRotationBlock = transform.rotation;
            if (HasDeathAnim)
                m_animationcomponent.EventDeath();
            else
                OnAnimationFinish("death");
        }
	}
	
	
	void OnAnimationFinish(string name)
	{
		if(name == "death" )
		{
			SendMessage("DestroyGameObject",SendMessageOptions.DontRequireReceiver);
		}
	}


    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (m_animationcomponent != null)
            m_animationcomponent.UnRegisterOnAnimationEvent=OnAnimationFinish;
    }

    // End is called when component is destroy
    /*protected override void End() {

    }*/
}
