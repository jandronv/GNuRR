using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    public Transform Pos1;
    public Transform Pos2;

    public int EnemyAttack;
    public int EnemyLife;
    private Vector3 m_pos1;
    private Vector3 m_pos2;
    public float speed = 0.50f;

    public bool sentido = true;
    public float delay = 0.3f;
  

    void Start()
    {
        m_pos1 = Pos1.position;
        m_pos2 = Pos2.position;
        
    }
    void Update()
    {

        float aux = Mathf.PingPong(Time.time * speed, 1.0f);
        //TODO Hacer con corrutina
        transform.position = Vector3.Lerp(m_pos1, m_pos2, aux);
       
        delay += Time.deltaTime;
        //TODO Flip
        if (aux > 0.95f && delay >= 0.3)
        {
            this.transform.rotation = new Quaternion(0.0f, 180.0f, 0f, 0f);
            delay = 0.0f;

        }

        if (aux < 0.09f && delay >= 0.3)
        {
            this.transform.rotation = new Quaternion(0.0f, 0.0f, 0f, 0f);
            delay = 0.0f;
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            
            if (EnemyLife > 0)
            {
                EnemyLife--;
                Debug.Log("Vida enemigo: " + EnemyLife);

            } else Destroy(gameObject);

          

            //Debug.Log("Entra el proyectil.");
        }
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().RestaVidaEnemigo(EnemyAttack);
            //Debug.Log("Entra el personaje.");
        }


    }


}
