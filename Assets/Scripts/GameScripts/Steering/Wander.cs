using UnityEngine;
using System.Collections;

public class Wander : SteeringBehaviour {
	public float m_radius = 20;
	public float m_rate = 10;	

	public override Vector3 getSteering () {
		Debug.Log(gameObject.name+ ":Wander");
		Vector2 circle = m_radius * Random.insideUnitCircle;
		Vector3 v = new Vector3(circle.x, 0.0f, circle.y);

		Vector3 seekPoint = transform.position +m_rate*transform.forward;
		seekPoint+= v;
		
		Debug.Log("Position: "+ transform.position +"     GoTo: "+seekPoint);
		
		Vector3 desiredVelocity = seekPoint - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity*= m_motor.maxSpeed();
		Vector3 steering = desiredVelocity -m_motor.getVelocity();

		return this.weight * steering;
	}
}
