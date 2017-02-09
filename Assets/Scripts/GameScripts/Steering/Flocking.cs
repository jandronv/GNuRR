using UnityEngine;
using System.Collections.Generic;
public class Flocking : MonoBehaviour {
	
	private List<Transform> m_Objects;	
	private float [,]m_distMatrix;
	private bool is_init = false;
	
	public float m_nearAreaRadius;
	public float m_separation_weight = 1.0f;
	public float m_alignment_weight = 1.0f;
	public float m_cohesion_weight = 1.0f;
	
	public Transform m_flock_leader;
	
	void Awake() {
		m_Objects = new List<Transform>();
	}
	// Use this for initialization
	void Start () {
		if (m_flock_leader!=null)
			gameObject.BroadcastMessage("newTarget", m_flock_leader.position, SendMessageOptions.DontRequireReceiver);
	}
	
	// Update is called once per frame
	void Update () {
		if(!is_init)
			init();
		updateDistances();
		gameObject.BroadcastMessage("newTarget", m_flock_leader.position, SendMessageOptions.DontRequireReceiver);
	}
	
	private Vector3 separation(Transform t) {
		Vector3 steering = Vector3.zero;
		List<Transform> neighbours = getNeighbours(t, m_nearAreaRadius);
		foreach(Transform tn in neighbours) {
			Vector3 dist = t.position - tn.position;
			float r = dist.magnitude;
			dist.Normalize();
			dist *=1/r;
	
			steering+=dist;
		}
		
		return steering;
	}
	
	private Vector3 cohesion(Transform t) {
		Vector3 steering = Vector3.zero;
		List<Transform> neighbours = getNeighbours(t, m_nearAreaRadius);
		foreach(Transform tn in neighbours) {
			steering+=tn.position;
		}
		if(neighbours.Count!=0) {
			steering /=neighbours.Count;
		}
			
		steering =steering-t.position;
		return steering;
	}


	private Vector3 alignment(Transform t) {
		Vector3 steering = Vector3.zero;
		List<Transform> neighbours = getNeighbours(t, m_nearAreaRadius);
		foreach(Transform tn in neighbours) {
			steering+=tn.forward;
		}
		if(neighbours.Count!=0) {
			steering*=neighbours.Count;
		}
		
		steering -= t.forward;
		
		return steering;
	}
	
	public Vector3 computeSteering(Transform t) {
		if(!is_init)
			init();
		Vector3 acel = Vector3.zero;
		acel+=separation(t) * m_separation_weight;
		acel+=cohesion(t) * m_cohesion_weight;
		acel+=alignment(t) * m_alignment_weight;
		return acel;
	}
	
	public void register(Transform neighbour) {
		m_Objects.Add(neighbour);
	}
	
	private void updateDistances() {
		int size = m_Objects.Count;
		for (int i=0 ; i < size ; i++)	{	
			for (int j=i+1 ; j < size; j++) {
				Vector3 v = m_Objects[i].position - m_Objects[j].position;
				m_distMatrix[i,j]=v.sqrMagnitude;
				m_distMatrix[j,i]=v.sqrMagnitude;
			}
		}
	}
	
	public List<Transform> getNeighbours(Transform v, float distance) {
		List<Transform> result = new List<Transform>();
		float sqrDist = distance * distance;
		int vIndex = m_Objects.IndexOf(v);
		int size = m_Objects.Count;
		for (int i=0 ; i < size ; i++) {
			if ((i!=vIndex) && (m_distMatrix[vIndex, i]<=sqrDist))
				result.Add(m_Objects[i]);
		}
		return result;
	}
	
	public void init() {
		int size = m_Objects.Count;
		m_distMatrix = new float[size,size];	
		for (int i=0; i<size; i++)
			for(int j=0; j<size; j++)
				m_distMatrix[i,j]=-1;
		is_init = true;
		updateDistances();
	}
}