﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    

    private CharacterController m_CharacterController;
    private Player m_Player;
    private bool SentidoBulet = false;
    public float _speedInJump = 6.0F;
    public float _speed = 6.0F;
    public float _jumpSpeed = 8.0F;
    public float _gravity = 20.0F;
    public int _NumFire = 1;
    public int _NumRecarga = 1;

    private Animator _animations;
    //private GameObject _camera;
    //TODO Atributos para el Fire()
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;
    public float _destroyBullet = 2.0f;
    public float _velocityBullet = 6.0f;



    // Use this for initialization
    void Start ()
    {
        //_camera = GameObject.FindGameObjectWithTag("Camera");

        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GetComponent<Player>();
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterMove = Move;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterFire = Fire;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterRecargarPelusas = RecargaPelusas;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterCargaPelusas = CargaPelusas;

        _animations = GetComponentInChildren<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void CargaPelusas(int numPelusas)
    {

        

        if (numPelusas > (m_Player.GetVida() - m_Player._VidaMin))
        {
            numPelusas = (int)(m_Player.GetVida() - m_Player._VidaMin);
        }

        if (m_Player.GetVida() > m_Player._VidaMin )
        {
            _animations.SetTrigger("Fire");
            if (SentidoBulet)
            {

                //Hacia la izquierda
                var bullet2 = (GameObject)Instantiate(bulletPrefab, bulletSpawn2.position, bulletSpawn2.rotation);
                bullet2.transform.localScale = new Vector3(bullet2.transform.localScale.x * numPelusas, bullet2.transform.localScale.y * numPelusas, bullet2.transform.localScale.z * numPelusas);
                bullet2.GetComponent<Rigidbody>().velocity = (bullet2.transform.forward) * _velocityBullet;

                Destroy(bullet2, _destroyBullet);

            }
            else
            {
                var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * numPelusas, bullet.transform.localScale.y * numPelusas, bullet.transform.localScale.z * numPelusas);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _velocityBullet;
                Destroy(bullet, _destroyBullet);
            }
            //Restamos vida
            Debug.Log("Cargando ataque especial!! " + numPelusas);
            m_Player.RestarVida(numPelusas);
        }

    }

    private void RecargaPelusas()
    {
       
        Debug.Log("Recargando pelusas.");
        m_Player.AumentaVida(_NumRecarga);
        _animations.SetTrigger("Recargar");
    }

    private void Move(float directionX, bool jump)
    {
        Vector3 direction;
        //TODO Llamara a todas las animaciones
        
        if (directionX > 0)
        {
            _animations.SetFloat("VelocidadX", directionX);

            SentidoBulet = false;
            m_Player.FlipInX(false);
        } else if (directionX < 0)
        {
            _animations.SetFloat("VelocidadX", -1*directionX);

            SentidoBulet = true;
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

        _animations.SetBool("isGrounded", m_CharacterController.isGrounded);
        
    }

    //TODO Crear un numero fijo de balas. No crear balas continuamente.
    private void Fire()
    {

        if (m_Player.GetVida() > m_Player._VidaMin) {
           
            _animations.SetTrigger("Fire");
            // Create the Bullet from the Bullet Prefab

            // Add velocity to the bullet
            if (SentidoBulet) {

                //Hacia la izquierda
                var bullet2 = (GameObject)Instantiate(bulletPrefab, bulletSpawn2.position, bulletSpawn2.rotation);
                bullet2.GetComponent<Rigidbody>().velocity = (bullet2.transform.forward) * _velocityBullet;
                Destroy(bullet2, _destroyBullet);

            }
            else
            {
                var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _velocityBullet;
                Destroy(bullet, _destroyBullet);
            }
            // Destroy the bullet after 2 seconds


            //Restamos vida
            m_Player.RestarVida(_NumFire);
        }
    }

}
