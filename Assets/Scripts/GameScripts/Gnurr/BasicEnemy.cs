using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    public Transform Pos1;
    public Transform Pos2;

    public int EnemyAttack;

    private Vector3 m_pos1;
    private Vector3 m_pos2;
    public float speed = 0.50f;

    void Start()
    {
        m_pos1 = Pos1.position;
        m_pos2 = Pos2.position;

    }
    void Update()
    {
        transform.position = Vector3.Lerp(m_pos1, m_pos2, Mathf.PingPong(Time.time * speed, 1.0f));
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {

            Debug.Log("Entra el proyectil.");
        }

    }

    
}
