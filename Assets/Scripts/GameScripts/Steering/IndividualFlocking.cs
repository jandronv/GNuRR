using UnityEngine;
using System.Collections;

public class IndividualFlocking : SteeringBehaviour {
	private Flocking m_flock;
	// Use this for initialization
	void Start () {
		m_motor = GetComponent<Motor>();
		m_motor.register(this);
		m_flock = transform.parent.GetComponent<Flocking>();
		m_flock.register(transform);
	}

	public override Vector3 getSteering () {
		Debug.Log(gameObject.name+ ":Flocking");
		return this.weight * m_flock.computeSteering(transform);
	}
}
