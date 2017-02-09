using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]
public class SimpleMotor : AComponent {
	
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
	private AnimationComponent m_animationComponent;
	
	/// <summary>
    /// ¿Nos estamos moviendo?
    /// </summary>
	private bool m_isMoving;
	private InputMgr m_inputMgr;

	public bool ListenToClickEvents = false;


	public GameObject m_clickGizmo;
	
	/// <summary>
    /// Vector velocidad
    /// </summary>
	private Vector3 velocity;
	
	public float m_weightSeek = 1.0f;
	public float m_weightArrival = 1.0f;
	public float m_weightAvoid = 1.0f;
	public float m_weightWander = 1.0f;
	
	/// <summary>
	/// Esfera que representa la distancia a la que comenzaremos a frenar
	/// ## TODO 4: Añadir la esfera (SphereCollider) como atributo público
	/// </summary>
	public float m_arrivalDistance;

	/// <summary>
	/// Distancia a la que comenzamos a desviarnos del obstáculo
	/// ## TODO 6: Insertar el atributo público
	/// </summary>
	public float m_avoidDistance;
	public float m_avoidForce = 1.0f;
	
	/// <summary>
	/// Capa del obstáculo con la que calcularemos las colisiones
	/// ## TODO 6: Insertar el atributo público
	/// </summary>
	public LayerMask m_layer;

	/// <summary>
	/// Radio de la esfera para Wander
	/// </summary>
	public float m_radius =1.0f;

	/// <summary>
	/// Distancia de la esfera para Wander
	/// </summary>
	public float m_rate =1.0f;

	/// <summary>
	/// Vector de aceleración. Será actualizado por los steerings
	/// </summary>
	private Vector3 accel = Vector3.zero;

	private Vector2 To2D(Vector3 v) {
		return new Vector2(v.x, v.z);
	}

	// Use this for initialization
	protected override void Start () {
		// ## TO-DO 1 - Conseguir la referencia al characterController, inicializar a la velocidad máxima 
		// ## y poner la animación de caminar
		m_controller = GetComponent<CharacterController>();
		velocity = transform.forward * 0.0f;
		m_isMoving=false;
		if (ListenToClickEvents) {
			m_inputMgr = GameMgr.GetInstance().GetServer<InputMgr>();
			m_inputMgr.RegisterPointAndClickEvent(null,OnBeginClick, null);
		}
		m_animationComponent = GetComponent<AnimationComponent>();
		m_animationComponent.PlayAnim("idle",AnimationComponent.NORMAL,true);
	}

	public void OnBeginClick(GameObject onCollision,Vector3 point, float distance)
	{
		Vector3 point2D = new Vector3(point.x, 0, point.z); 
		if (m_target!=point2D) {
			m_target = point2D;
			Instantiate(m_clickGizmo, point, Quaternion.identity);
			m_isMoving = true;
			velocity = transform.forward * m_maxSpeed;
			m_animationComponent.PlayAnim("run",AnimationComponent.NORMAL,true);
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if (ListenToClickEvents&& m_inputMgr!=null) {
			m_inputMgr.UnRegisterPointAndClickEvent(OnBeginClick,null,OnBeginClick);
		}
	}
	// Update is called once per frame
	protected override void Update () {
		// ## TO-DO 3 - Si nos estamos moviendo: ##
		// ## 1. Comprobar si hemos llegado al destino (target - miPosicion < distanciaObjetivo) 
		// ## Si es así nos detenemos
		// ## 2. Comprobar si nos hemos detenido
		// ## Si no es así entonces ejecutamos el seek y actualizamos la posición


		// Wander
//		updateSteering(wander(), m_weightWander);
//		updatePosition();

		Vector2 distance = To2D(m_target - transform.position);
		Debug.Log (distance.magnitude);
		if (distance.magnitude<m_StopDistance) {
			m_isMoving=false;
			m_animationComponent.PlayAnim("idle",AnimationComponent.NORMAL,true);
		}
		if(m_isMoving) {
			// Arrival
			updateSteering(arrival(),m_weightArrival);

			// Seek
			updateSteering(seek(), m_weightSeek);

			// Avoid
			updateSteering(avoid(), m_weightAvoid);

			// Wander
			//updateSteering(avoid(), m_weightAvoid);

			updatePosition();

		}


	}
	
	public void updateSteering(Vector3 steering, float weight)
    {
		accel += weight*steering;
//        if (acel.magnitude > m_maxAccel)
//        {
//            acel.Normalize();
//            acel *= m_maxAccel;
//        }
//		Vector3 newVelocity = velocity;
//        newVelocity += weight * acel * Time.deltaTime;
//
//        if (newVelocity.magnitude > m_maxSpeed)
//        {
//            newVelocity.Normalize();
//            newVelocity *= m_maxSpeed;
//        }
//		velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
    }

	private void updatePosition() {
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
	
	/// <summary>
    /// Devuelve el vector de aceleración generado usando el steering behaviour Wander
    /// </summary>
	public Vector3 wander() {
		Vector2 circle = m_radius * Random.insideUnitCircle;
		Vector3 v = new Vector3(circle.x, 0.0f, circle.y);

//		Vector3 v = new Vector3(Random.value*100, 0.0f, Random.value*100);
//
//		if (Random.value >0.5)
//			v.Scale(new Vector3(-1f,1f,1f));
//		if (Random.value >0.5)
//			v.Scale(new Vector3(1f, 1f, -1f));	
//		
//
//		if (v.magnitude > m_radius) {
//			v.Normalize();
//			v*=m_radius;
//		}
		
		Vector3 seekPoint = transform.position +m_rate*transform.forward;
		seekPoint+= v;
		
		Debug.Log("Position: "+ transform.position +"     GoTo: "+seekPoint);
		
		Vector3 desiredVelocity = seekPoint - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity*= m_maxSpeed;
		Vector3 steering = desiredVelocity -velocity;
		Debug.Log("Steering: "+steering);
		return steering;
	}
	
	/// <summary>
    /// Devuelve el vector de aceleración generado usando el steering behaviour Seek
    /// </summary>
	public Vector3 seek() {
		// ## TO-DO 2 - Implementar el steering behaviour de Seek ##
		Vector3 desiredVelocity = m_target - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity*=m_maxSpeed;
		Vector3 steering = desiredVelocity -velocity;
		return steering;
	}
	
	/// <summary>
    /// Devuelve el vector de aceleración generado usando el steering behaviour Arrival
    /// </summary>
	public Vector3 arrival() {
		// ## TO-DO 5 - Implementar el steering behaviour de Arrival ##
		Vector3 arrivalAcel = Vector3.zero;
		Vector3 distance = transform.position - m_target;
		if (distance.magnitude< m_arrivalDistance) {
			Debug.Log("Frenando!!!");
			arrivalAcel = distance* (distance.magnitude / m_arrivalDistance);
			arrivalAcel = arrivalAcel-velocity;
		}
		return arrivalAcel;
	}
	
	/// <summary>
    /// Devuelve el vector de aceleración generado usando el steering behaviour Avoid
    /// </summary>
	public Vector3 avoid() {
		// ## TO-DO 6 - Implementar el steering behaviour de Avoid ##
		var hitInfo = new RaycastHit();
        var ray = new Ray(transform.position, transform.forward);
		Vector3 avoidAcel = Vector3.zero;
        if (Physics.Raycast(ray, out hitInfo, m_avoidDistance, m_layer)) {
			Debug.Log("Detectado obstaculo");
           	avoidAcel = transform.right * m_avoidForce;
           	avoidAcel *= m_maxAccel / hitInfo.distance;
        }
		return avoidAcel;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, m_StopDistance);
		Gizmos.DrawRay(transform.position, velocity);


	}

}
