using UnityEngine;
using System.Collections;

public class Avoid : SteeringBehaviour {
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
	// Update is called once per frame
	public override Vector3 getSteering () {
		var hitInfo = new RaycastHit();
        var ray = new Ray(transform.position, transform.forward);
		Vector3 avoidAcel = Vector3.zero;
		if (Physics.Raycast(ray, out hitInfo, m_avoidDistance, m_layer)) {
			Debug.Log("Detectado obstaculo");
			avoidAcel = transform.right*m_avoidForce;
           	avoidAcel *= m_motor.maxAccel() / hitInfo.distance;
        }	
		return avoidAcel * this.weight;
	}
}
