using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{

	public Transform Pos1;
	public Transform Pos2;

	public Transform cuerpo;
	public int EnemyAttack;
	public int EnemyLife;
	private Vector3 m_pos1;
	private Vector3 m_pos2;
	public float speed = 0.50f;
	private SpriteRenderer[] _sprite;
	public Color feedBack;
	public int NumTimes = 3;
	public float DelayFeedBack = 0.1f;
	public float attackTime;
	public bool sentido = true;
	public float delay = 0.3f;
	private bool move = true;
    public Animator _animacion;

	private float delayAttack=0;



	void Start()
	{
		m_pos1 = Pos1.position;
		m_pos2 = Pos2.position;
		_sprite = GetComponents<SpriteRenderer>();
	}
	void Update()
	{

		
		//TODO Hacer con corrutina
		if (move) {
			float aux = Mathf.PingPong(Time.time * speed, 1.0f);
			//print(aux);
			cuerpo.transform.position = Vector3.Lerp(m_pos1, m_pos2, aux);
			delay += Time.deltaTime;
			//TODO Flip
			if (aux > 0.95f && delay >= 0.5)
			{
				this.transform.rotation = new Quaternion(0.0f, 180.0f, 0f, 0f);

				delay = 0.0f;

			}

			if (aux < 0.09f && delay >= 0.5)
			{
				this.transform.rotation = new Quaternion(0.0f, 0.0f, 0f, 0f);
				delay = 0.0f;
			}
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
                StartCoroutine(FlashSprites(_sprite, NumTimes, DelayFeedBack, feedBack));
                //Debug.Log("Vida enemigo: " + EnemyLife);

            }
            else
            {
                _animacion.SetBool("Dead", true);
                move = false;
                Destroy(gameObject, 2);
            }
		}

		delayAttack += Time.deltaTime;
		if (other.tag == "Player" )
		{
			
			delayAttack = 0;
			other.GetComponent<Player>().RestaVidaEnemigo(EnemyAttack, false);

			//Debug.Log("Entra el personaje.");
		}

	}

	private void OnTriggerExit(Collider other)
	{
		move = true;
	}

	IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float delay, Color feedBack)
	{

		// number of times to loop
		for (int loop = 0; loop < numTimes; loop++)
		{
			// cycle through all sprites
			for (int i = 0; i < sprites.Length; i++)
			{

				// for changing the alpha
				sprites[i].color = new Color(feedBack.r, feedBack.g, feedBack.b, feedBack.a);
			}
			// delay specified amount
			yield return new WaitForSeconds(delay);
			// cycle through all sprites
			for (int i = 0; i < sprites.Length; i++)
			{
				sprites[i].color = new Color(255, 255, 255, 1);
			}

			// delay specified amount
			yield return new WaitForSeconds(delay);
		}
	}
}
