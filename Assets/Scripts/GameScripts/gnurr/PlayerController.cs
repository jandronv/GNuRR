﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    

    private CharacterController m_CharacterController;
    private Player m_Player;
    private bool SentidoBullet = false;
    private bool saltandoPlataformaFlotante;
    public float _speedInJump = 6.0F;
    public float _speed = 6.0F;
    public float _jumpSpeed = 8.0F;
    public float _gravity = 20.0F;
    public int _NumFire = 1;
    public int _NumRecarga = 1;
    public float distance = 800;
    public LayerMask layer;

    private GameObject ultimoTiled = null;

    private Animator _animations;
    //private GameObject _camera;
    //TODO Atributos para el Fire()
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;
    public Transform RayToGround;
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

    void FixedUpdate()
    {

    }

    
    void Update () {

        Ray ray = new Ray(transform.position, Vector3.down);

        Vector3 PosRay = new Vector3(transform.position.x, (transform.position.y - 0.2f), transform.position.z);

        Debug.DrawRay(PosRay, Vector3.down);
        RaycastHit hit;
        saltandoPlataformaFlotante = Physics.Raycast(PosRay, Vector3.down, out hit, distance, layer);

        //Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        if (saltandoPlataformaFlotante)
        {
            if (ultimoTiled != null)
            {
                ultimoTiled.GetComponent<Collider>().isTrigger = true;
            }
            ultimoTiled = hit.collider.gameObject;
            Debug.Log("Colisionamos con la plataforma!!: " + hit.transform.name);
            //SendMessage("CheckPlatforms", true, SendMessageOptions.RequireReceiver);
            hit.collider.isTrigger = false;
        }
        else
        {
            if (ultimoTiled != null)
            {
                ultimoTiled.GetComponent<Collider>().isTrigger = true;
            }
            //gameObject.SendMessage("CheckPlatforms", false, SendMessageOptions.RequireReceiver);
        }
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
            if (SentidoBullet)
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
            //Debug.Log("Cargando ataque especial!! " + numPelusas);
            m_Player.RestarVida(numPelusas);
        }

    }

    private void RecargaPelusas()
    {
        m_Player.AumentaVida(_NumRecarga);
        _animations.SetTrigger("Recargar");
    }

    private void Move(float directionX, bool jump)
    {
        Vector3 direction;

        Debug.Log("Is Ground: " + m_CharacterController.isGrounded.ToString());
        if (directionX > 0)
        {
            _animations.SetFloat("VelocidadX", directionX);

            SentidoBullet = false;
            m_Player.FlipInX(false);
        } else if (directionX < 0)
        {
            _animations.SetFloat("VelocidadX", -1 * directionX);

            SentidoBullet = true;
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
            if (SentidoBullet) {

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
