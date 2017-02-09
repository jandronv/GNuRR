using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Motor : AComponent {

	/// <summary>
	/// Referencia al objetivo al que queremos llegar
	/// </summary>
	public Vector3 m_target;
	
	/// <summary>
	/// Distancia a la que consideramos que hemos alcanzado el objetivo
	/// </summary>
	public float m_StopDistance = 2.0f;
	
	/// <summary>
	/// Velocidad máxima
	/// </summary>
	public float m_maxSpeed = 5.0f;
	
	/// <summary>
	/// Aceleración máxima
	/// </summary>
	public float m_maxAccel = 5.0f;
	
	/// <summary>
	/// Referencia al character controller para mover al personaje
	/// </summary>
	private CharacterController m_controller;
	private AnimationComponent m_animationComponent = null;
	
	/// <summary>
	/// ¿Nos estamos moviendo?
	/// </summary>
	private bool m_isMoving;
	private InputMgr m_inputMgr;
	
	public GameObject m_clickGizmo;

	/// <summary>
	/// Vector velocidad
	/// </summary>
	private Vector3 velocity;

	/// <summary>
	/// Vector de aceleración. Será actualizado por los steerings
	/// </summary>
	private Vector3 accel = Vector3.zero;

	private List<SteeringBehaviour> m_steerings;

	public bool ListenToClickEvents = false;


	private Vector2 To2D(Vector3 v) {
		return new Vector2(v.x, v.z);
	}
	
	protected override void Awake() {
		m_steerings = new List<SteeringBehaviour>();
	}

	// Use this for initialization
	protected override void Start () {
		// ## TO-DO 1 - Conseguir la referencia al characterController, inicializar a la velocidad máxima 
		// ## y poner la animación de caminar
		m_controller = GetComponent<CharacterController>();
		velocity = transform.forward * 0.0f;
		m_isMoving=true;
		if (ListenToClickEvents) {
			m_inputMgr = GameMgr.GetInstance().GetServer<InputMgr>();
			m_inputMgr.RegisterPointAndClickEvent(null,OnBeginClick, null);
			m_isMoving=false;
		}
		m_animationComponent = GetComponent<AnimationComponent>();

		if (m_animationComponent!=null)
			m_animationComponent.PlayAnim("idle",AnimationComponent.NORMAL,true);

		//gameObject.SendMessage("playAnimation", "idle", SendMessageOptions.DontRequireReceiver);
	}
	
	public void OnBeginClick(GameObject onCollision,Vector3 point, float distance)
	{
		Vector3 point2D = new Vector3(point.x, 0, point.z); 
		if (m_target!=point2D) {
			m_target = point2D;
			Instantiate(m_clickGizmo, point, Quaternion.identity);
			m_isMoving = true;
			if (m_animationComponent!=null)
				m_animationComponent.PlayAnim("run",AnimationComponent.NORMAL,true);
			gameObject.SendMessage("newTarget", point2D, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if (ListenToClickEvents && m_inputMgr!=null) {
			m_inputMgr.UnRegisterPointAndClickEvent(OnBeginClick,null,OnBeginClick);
		}
	}

	// Update is called once per frame
	protected override void Update () {				
		Vector2 distance = To2D(m_target - transform.position);
		if (distance.magnitude<m_StopDistance) {
			m_isMoving=false;
			if (m_animationComponent!=null)
				m_animationComponent.PlayAnim("idle",AnimationComponent.NORMAL,true);
		}
		if(m_isMoving) {
			Debug.Log(gameObject.name+ ":Motor");
			updatePosition();
		}
		
	}
	private void updatePosition() {
		foreach (SteeringBehaviour sb in m_steerings) {
			accel+= sb.getSteering();
		}
		if (accel.magnitude > m_maxAccel)
		{
			accel.Normalize();
			accel *= m_maxAccel;
		}
		Vector3 newVelocity = velocity;
		newVelocity += accel * Time.deltaTime;
		
		if (newVelocity.magnitude > m_maxSpeed)
		{
			newVelocity.Normalize();
			newVelocity *= m_maxSpeed;
		}
		velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
		m_controller.SimpleMove(velocity);
		transform.rotation = Quaternion.LookRotation(velocity, gameObject.transform.up);
	}

	public void updateSteering(Vector3 steering, float weight)
	{
		accel += weight*steering;
//		if (acel.magnitude > m_maxAccel)
//		{
//			acel.Normalize();
//			acel *= m_maxAccel;
//		}
//		Vector3 newVelocity = velocity;
//		newVelocity += weight * acel * Time.deltaTime;
//		
//		if (newVelocity.magnitude > m_maxSpeed)
//		{
//			newVelocity.Normalize();
//			newVelocity *= m_maxSpeed;
//		}
//		velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
	}

	public Vector3 target() {
		return m_target;
	}

	public Vector3 getVelocity() {
		return velocity;
	}

	public float maxSpeed() {
		return m_maxSpeed;
	}

	public float maxAccel() {
		return m_maxAccel;
	}

	public void register(SteeringBehaviour sb) {
		m_steerings.Add(sb);
	}

	public void newTarget(Vector3 targetPos) {
		m_target = targetPos;
		m_isMoving=true;
	}
}
