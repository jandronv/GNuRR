using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Motor))]
public abstract class SteeringBehaviour : MonoBehaviour {

	protected Motor m_motor;
	
	public float weight = 1.0f;
	
	// Use this for initialization
	void Start () {
		m_motor = GetComponent<Motor>();
		m_motor.register(this);


	}
	

	public void setWeight(float newWeight) {
		weight = newWeight;
	}

	public abstract Vector3 getSteering();
}
