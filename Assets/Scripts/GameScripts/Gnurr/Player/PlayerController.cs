using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {
    

    private CharacterController m_CharacterController;
    private Player m_Player;
    private bool _planear;
	private bool _puedePlanear;
	private bool PlaneoActivo = false;

	private bool SentidoBullet = false;
    private bool downPlatform = false, upPlatform = false, InSlope = false;
    public float _speedInJump = 6.0F;
    public float _speed = 6.0F;
    public float _jumpSpeed = 8.0F;
    public float _gravity = 20.0F;
    public float _gravityScale = 0.5f;
    public float _gravityPlaning = 10.0F;
    public int _NumFire = 1;
    public int _NumRecarga = 1;
    public float distance = 10;
    public LayerMask layer;

    private GameObject ultimoTiled = null;

    private Animator _animations;
	public Animator _animationEyes;
    //private GameObject _camera;
    //TODO Atributos para el Fire()
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;

    public float _destroyBullet = 2.0f;
    public float _velocityBullet = 6.0f;

    public Transform RechargeParticles;
    public ParticleSystem Estela;
	public SpriteRenderer ojos;

	private ParticleSystem[] _ParticulasRecarga;

	public AudioClip clipRecharge;
	public AudioClip clipWalk;
	public AudioClip clipJump;
	public AudioClip clipFire;
	private AudioSource audio;

	private float ultimaPosY;




	// Use this for initialization
	void Start ()
    {
		ultimaPosY = transform.localPosition.y;
		//InvokeRepeating("PlaySound", time, RepeatRate);
		//_camera = GameObject.FindGameObjectWithTag("Camera");
		_planear = false;
		_puedePlanear = false;
        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GetComponent<Player>();
        // si tengo el poder => lo registro. TODO esta registrado de momento
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterPlanear = Planear;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterMove = Move;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterFire = Fire;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterRecargarPelusas = RecargaPelusas;
        GameMgr.GetInstance().GetServer<InputMgr>().RegisterCargaPelusas = CargaPelusas;
		GameMgr.GetInstance().GetServer<InputMgr>().RegisterSonidoRecarga = SonidoRecarga;


		_animations = GetComponentInChildren<Animator>();
        if (RechargeParticles == null)
        {
            Debug.LogWarning("Asigna el sistema de particulas de recarga en el PlayerController!!");

        }else
            _ParticulasRecarga = RechargeParticles.GetComponentsInChildren<ParticleSystem>();

        if (Estela == null)
        {
            Debug.LogWarning("Asigna la estela del jugador al PlayerController!!");
        }
		audio = GetComponent<AudioSource>();
		Debug.Log("Entra al Player");
		PlayerMngr p = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr();
		if (p.CambioEscena)
		{

			Debug.Log("Hay cambio de escena, pongo al player en la pos final y reseteo la posicion");
			p.CambioEscena = false;
			transform.localPosition = p.Position;
			GameObject cam = GameObject.Find("Camera");
			cam.transform.localPosition = p.Position;
		}

	}

    void FixedUpdate()
    {

    }
    
    void Update () {
 
        //TODO lo hacemos al reves si estas debajo de una plataforma se desactiva el collider
        //Logica plataforma flotante
        Vector3 PosRay = new Vector3(transform.position.x, (transform.position.y + 0.4f), transform.position.z);
        Vector3 PosRayDown = new Vector3(transform.position.x, (transform.position.y - 0.2f), transform.position.z);


        Debug.DrawRay(PosRay, Vector3.up, Color.green);
        Debug.DrawRay(PosRayDown, Vector3.down, Color.red);

        RaycastHit hit;
        RaycastHit hitDown;


        downPlatform = Physics.Raycast(PosRay, Vector3.up, out hit, distance, layer);
        upPlatform = Physics.Raycast(PosRayDown, Vector3.down, out hitDown, distance, layer);
        //Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        if (downPlatform)
        {
            
            if (hit.collider.gameObject != null)
            {
                ultimoTiled = hit.collider.gameObject;
                hit.collider.isTrigger = true;
            }
        }
		

		if (upPlatform)
        {
      
            if (hitDown.collider.gameObject != null)
            {
                hitDown.collider.isTrigger = false;
            }
     
        }
		if (transform.localPosition.y >= ultimaPosY) {//Comprobamos la altura de personaje para comprobar que esta en unsalto o ya cae,asi puede planear
			//Debug.Log ("Estoy saltando!! y puedes planear");
			_puedePlanear = false;
		} else
			_puedePlanear = true;

		if (transform.localPosition.z != 0.0f && !upPlatform && !downPlatform)
		{
			Debug.Log("Te has movido del eje Z");
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
		}
		ultimaPosY = transform.localPosition.y;
	}


    private void CargaPelusas(int numPelusas)
    {

		Debug.Log("Tamaño pelusa:" + numPelusas);
        //_animations.SetTrigger("Carga");
        if (numPelusas > (m_Player.Vida - m_Player._VidaMin))
        {
            numPelusas = (int)(m_Player.Vida - m_Player._VidaMin);
        }

        if (m_Player.Vida > m_Player._VidaMin )
        {
            _animations.SetTrigger("Fire");
			_animationEyes.SetTrigger("Attack");
            if (SentidoBullet)
            {

                //Hacia la izquierda
                var bullet2 = (GameObject)Instantiate(bulletPrefab, bulletSpawn2.position, bulletPrefab.transform.rotation);
                
                bullet2.transform.localScale = new Vector3(bullet2.transform.localScale.x * (numPelusas * 0.5f), bullet2.transform.localScale.y * (numPelusas * 0.5f), bullet2.transform.localScale.z * (numPelusas * 0.5f));
                bullet2.GetComponent<Bullet>().SetTamBullet(numPelusas);
                bullet2.GetComponent<Rigidbody>().velocity = -1*(bullet2.transform.right) * _velocityBullet;
                //_animations.SetTrigger("Fire");
                Destroy(bullet2, _destroyBullet);

            }
            else
            {
                var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletPrefab.transform.rotation);
               
                bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * (numPelusas * 0.5f), bullet.transform.localScale.y * (numPelusas * 0.5f), bullet.transform.localScale.z * (numPelusas * 0.5f));
                bullet.GetComponent<Bullet>().SetTamBullet(numPelusas);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * _velocityBullet;
                Destroy(bullet, _destroyBullet);
            }
			//Restamos vida
			//Debug.Log("Cargando ataque especial!! " + numPelusas);
			audio.PlayOneShot(clipFire, 0.7f);
			m_Player.RestarVida(numPelusas);
        }

    }

    private void RecargaPelusas()
    {

        if (m_CharacterController.isGrounded) {
			//TODO meter aqui un delay para empezar a cargar
				
				m_Player.AumentaVida(_NumRecarga);
				_animations.SetTrigger("Recargar");
			
				foreach (ParticleSystem ps in _ParticulasRecarga)
				{
					ps.Play();
				}
			
        }

    }
	public void PlayerInSlope(bool inSlope)
	{
		InSlope = inSlope;
	}

    public void Planear(bool planear)
    {
        _planear = planear;
    }

	public void ActivaPlanear()
	{
		//Debug.Log("Entra");
		PlaneoActivo = true;
	}
		

	private void Move(float directionX, bool jump, bool dobleJump,bool blockControl)
    {
        Vector3 direction = Vector3.zero;

		//Debug.Log("Is Jump: " + jump);
		if (directionX > 0 && !blockControl)
		{
			//TODO Lanzar animacion de los OJOS!!!!!
			_animations.SetTrigger("Run");
			_animationEyes.SetTrigger("Walk");
			Estela.Play();
			SentidoBullet = false;
			m_Player.FlipInX(false);


		}
		else if (directionX < 0 && !blockControl)
		{
			_animations.SetTrigger("Run");
			_animationEyes.SetTrigger("Walk");
			Estela.Play();
			SentidoBullet = true;
			m_Player.FlipInX(true);


		}
		else
		{
			_animations.SetTrigger("Idle");
			_animationEyes.SetTrigger("Idle");
			Estela.Stop();
		}
		

		//Estas saltando
		if (!m_CharacterController.isGrounded && !blockControl)
        {
            direction = new Vector3(directionX * _speedInJump, m_CharacterController.velocity.y, 0);
            
        }
        else if(m_CharacterController.isGrounded && !blockControl)
        {
			
            direction = new Vector3(directionX * _speed, m_CharacterController.velocity.y, 0);
            
        }

		if (m_CharacterController.isGrounded && directionX != 0)
		{
			
			if (!audio.isPlaying)
			{
				audio.PlayOneShot(clipWalk, 0.08f);
			}

		}
		//Estas en el suelo y vas a saltar
		if (IsGround && jump)
        {
			_animations.SetTrigger("Jump");
			_animationEyes.SetTrigger("Jump");
            direction.y = _jumpSpeed;
			
			audio.PlayOneShot(clipJump, 0.9f);
            //direction = new Vector3(directionX * _speedInJump, m_CharacterController.velocity.y, 0);
        }

		if (blockControl)
		{
			ojos.enabled = false;
			direction = new Vector3(0, m_CharacterController.velocity.y, 0);
		}
		else {
			
			
		}

        direction = transform.TransformDirection(direction);

	
		//TODO solo aplicar la gravedad del planeo cuando ya has saltado, cuando has pulsado el salto, esta casi, asi no aplica la gravidad si has pulsado de antes(mirar como lo hace shantae)
		if (_planear && _puedePlanear && PlaneoActivo)
		{
			Debug.Log("Planeando...");
			direction.y -= _gravityPlaning * Time.deltaTime;
		}
		else
		{
			direction.y -= _gravity * Time.deltaTime;
		}
		if (!InSlope) {
			Debug.DrawRay(transform.position, direction, Color.red);
			m_CharacterController.Move(direction * Time.deltaTime);

		} else {

			//Debug.Log("Direction ante de moverte: "+ direction.ToString());
			if (IsGround && !jump)
			{
				Vector3 point;
				Vector3 normal = GetSurfaceNormal(out point);

				Vector3 surface = Vector3.Cross(normal, Vector3.forward);

				surface = surface * direction.x;
				m_CharacterController.Move(surface * Time.deltaTime);
				Debug.DrawRay(transform.position, normal * 5f, Color.blue);
				Debug.DrawRay(transform.position, surface, Color.red);
				m_CharacterController.Move(Vector3.down * _gravity * _gravityScale * Time.deltaTime);
			}
			else
			{
				Debug.DrawRay(transform.position, direction, Color.red);
				m_CharacterController.Move(direction * Time.deltaTime);
			}
		}
		//_animations.SetBool("isGrounded", m_CharacterController.isGrounded);
        
    }

	

    public Vector3 GetSurfaceNormal(out Vector3 point )
    {
        //TODO else if con Ray1 y Ray2
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 20f))
        {
            point = hit.point;
            return hit.normal;
        }
        point = Vector3.zero;
        return Vector3.zero;
    }

    public bool IsGround
    {
        get {
            bool b = (m_CharacterController.collisionFlags & CollisionFlags.Below) != 0;
            return b;
        }
    }

	

	private void Fire()
    {

        if (m_Player.Vida > m_Player._VidaMin) {
           
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


	private void SonidoRecarga(bool play)
	{
		if (play)
		{
			if (!audio.isPlaying)
			{
				audio.PlayOneShot(clipRecharge, 0.9f);
			}
		}
		else
		{

			if (_animations.GetCurrentAnimatorStateInfo(0).IsName("Recargar"))
			{
				_animations.SetTrigger("Idle");
				ojos.enabled = true;
				audio.Stop();
			}
		}
	}

    private void OnDestroy()
    {
        GameMgr.GetInstance().GetServer<InputMgr>().UnregisterPlanear = Planear;
        GameMgr.GetInstance().GetServer<InputMgr>().UnRegisterMove = Move;
        GameMgr.GetInstance().GetServer<InputMgr>().UnRegisterFire = Fire;
        GameMgr.GetInstance().GetServer<InputMgr>().UnRegisterRecargarPelusas = RecargaPelusas;
        GameMgr.GetInstance().GetServer<InputMgr>().UnRegisterCargaPelusas = CargaPelusas;
		GameMgr.GetInstance().GetServer<InputMgr>().UnRegisterSonidoRecarga = SonidoRecarga;

	}

}
