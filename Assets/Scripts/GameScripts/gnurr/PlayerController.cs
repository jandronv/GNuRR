using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    

    private CharacterController m_CharacterController;
    private Player m_Player;
    public float _speedInJump = 6.0F;
    public float _speed = 6.0F;
    public float _jumpSpeed = 8.0F;
    public float _gravity = 20.0F;



    //TODO Atributos para el Fire()
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float _destroyBullet = 2.0f;
    public float _velocityBullet = 6.0f;



    // Use this for initialization
    void Start ()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GetComponent<Player>();
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterMove = Move;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterFire = Fire;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterRecargarPelusas = RecargaPelusas;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void RecargaPelusas()
    {

        m_Player.AumentaVida(1);
    }

    private void Move(float directionX, bool jump)
    {
        Vector3 direction;
        
        if (directionX > 0)
        {
            m_Player.FlipInX(true);
        } else if (directionX < 0)
        {
            m_Player.FlipInX(false);
        }

        if (!m_CharacterController.isGrounded)
        {
            direction = new Vector3(directionX * _speedInJump, m_CharacterController.velocity.y, 0);

        }
        else {
            direction = new Vector3(directionX * _speed, m_CharacterController.velocity.y, 0);
        }

        //Move del CharacterController de Unity
        if (m_CharacterController.isGrounded && jump)
        {
            direction.y = _jumpSpeed;
            //direction = new Vector3(directionX * _speedInJump, m_CharacterController.velocity.y, 0);
        }
        direction.y -= _gravity * Time.deltaTime;
        m_CharacterController.Move(direction * Time.deltaTime);

    }

    //TODO Crear un numero fijo de balas. No crear balas continuamente.
    private void Fire()
    {
        // Create the Bullet from the Bullet Prefab
       var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _velocityBullet;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, _destroyBullet);

        //Restamos vida
        m_Player.RestarVida(1);
    }

}
