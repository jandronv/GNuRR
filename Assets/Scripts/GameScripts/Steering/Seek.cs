using UnityEngine;
using System.Collections;

public class Seek : SteeringBehaviour {
	
	// Update is called once per frame
	public override Vector3 getSteering () {
		Debug.Log(gameObject.name+ ":Seek");
		Vector3 desiredVelocity = m_motor.m_target - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity*=m_motor.maxSpeed();
		Vector3 steering = desiredVelocity -m_motor.getVelocity();
		return this.weight * steering;
	}
}
