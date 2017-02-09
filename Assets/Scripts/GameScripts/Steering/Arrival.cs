using UnityEngine;
using System.Collections;

public class Arrival : SteeringBehaviour {
	/// <summary>
	/// Distancia a la que comenzaremos a frenar
	/// </summary>
	public float m_arrivalDistance =1.0f;
	
	// Update is called once per frame
	public override Vector3 getSteering () {
		Debug.Log(gameObject.name+ ":Arrival");
		Vector3 arrivalAcel = Vector3.zero;
		Vector3 distance = transform.position - m_motor.m_target;
		if (distance.magnitude< m_arrivalDistance) {
			Debug.Log("Frenando!!!");
			arrivalAcel = distance * (distance.magnitude / m_arrivalDistance);
			arrivalAcel = arrivalAcel- m_motor.getVelocity();
		}
		return arrivalAcel * this.weight;
	}
}
