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
    public int _NumFire = 1;
    public int _NumRecarga = 1;

    private Animator _animations;

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

        _animations = GetComponentInChildren<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void RecargaPelusas(bool estado)
    {
        if (estado) {
            m_Player.AumentaVida(_NumRecarga);
        } 
    }

    private void Move(float directionX, bool jump)
    {
        Vector3 direction;
        //TODO Llamara a todas las animaciones
        //_animations.SetBool("isGrounded", m_CharacterController.isGrounded);
        //_animations.SetFloat("VelocidadX", directionX);

        if (directionX > 0)
        {
            m_Player.FlipInX(false);
        } else if (directionX < 0)
        {
            m_Player.FlipInX(true);
        }

        //Estas saltando
        if (!m_CharacterController.isGrounded)
        {
            direction = new Vector3(directionX * _speedInJump, m_CharacterController.velocity.y, 0);

        }
        else {
            direction = new Vector3(directionX * _speed, m_CharacterController.velocity.y, 0);
        }

        //Estas en el suelo y vas a saltar
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
        //Puedes cambiar al estado anterior si se ha terminado la animación??
        //_animations.SetBool("Fire", true);
        // Create the Bullet from the Bullet Prefab
       var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _velocityBullet;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, _destroyBullet);

        //Restamos vida
        m_Player.RestarVida(_NumFire);
    }

}
