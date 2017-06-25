using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float _VidaMax = 15;
    public float _VidaMin = 5;
    public float _PorcentajeRestarVida=0.90f;
    public float _PorcentajeAumetarVida = 1.2f;
    public float _Vida = 15;
    public int _BolaPelusas = 5;
    public SpriteRenderer _SpriteGnurr;
    public float fadeTime = 0.1f;
    public bool initLoad;

    public Image fadeBlack;
    public Transform DamageParticles;
    private ParticleSystem[] _ParticulasDanio;
    private SpawnMgrs mSpawManager;


    // Use this for initialization
    void Start () {

        if (DamageParticles == null)
        {
            Debug.LogWarning("Asigna el sistema de particulas de daño en el Player!!");
        }
        else
        {
            _ParticulasDanio = DamageParticles.GetComponentsInChildren<ParticleSystem>();
        }

        mSpawManager = GetComponentInParent<SpawnMgrs>();

        if (mSpawManager == null)
        {
            Debug.Log("No se ha podido inicializar el SpawnManager!!");
        }
        if (!initLoad)
        {

            _Vida = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr().Vida;
			//Actualizamos el tamaño del sprite
			RestarVida(0);

		}
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// REstar vida disparando, nunca te podras matar
    /// </summary>
    /// <param name="numVidas"></param>
    public void RestarVida(int numVidas)
    {

        if (_VidaMin == _Vida - numVidas)
        {
            
            Vector3 scale = new Vector3(_VidaMin / _VidaMax, _VidaMin / _VidaMax, _VidaMin / _VidaMax);
            this.transform.localScale = scale;
            _Vida -= numVidas;
        } else if ((_Vida - numVidas) > _VidaMin)
        {
            _Vida -= numVidas;
            Vector3 scale = new Vector3(_Vida / _VidaMax, _Vida / _VidaMax, _Vida / _VidaMax);
            this.transform.localScale = scale;
        }      
    }
    

   /// <summary>
   /// Resta vida de un enemigo, si te mata te manda a la initial zone.
   /// </summary>
   /// <param name="numVidas"></param>
    public bool RestaVidaEnemigo(int numVidas, bool deadZone)
    {
        if (!deadZone) {
            foreach (ParticleSystem ps in _ParticulasDanio)
            {
                ps.Play();
            }
        }
        //TODO feedback sprite

        if ((_Vida - numVidas) < _VidaMin)//Te mata
        {
			//TODO Lanzar animacion de muerte
			StartCoroutine("FadeBlack");
			//Cuando funcione el carga de escena hay
			// Reiniciamos los valores del pj
			this.transform.position = mSpawManager.GetSpawPoint().position;
			this._Vida = _VidaMax;
			Vector3 scale = new Vector3(_Vida / _VidaMax, _Vida / _VidaMax, _Vida / _VidaMax);
			this.transform.localScale = scale;
			return true;
			//TODO Lanzar menu GAME OVER		

		}
		else if ((_Vida - numVidas) >= _VidaMin)
        {
            _Vida -= numVidas;
            Vector3 scale = new Vector3(_Vida / _VidaMax, _Vida / _VidaMax, _Vida / _VidaMax);
            this.transform.localScale = scale;
        }
		return false;
    }

    public void AumentaVida(int numVidas)
    {

        Vector3 scale = new Vector3(1, 1, 1); ;
        //TODO aumentar tamaño sprite
        if (_Vida + numVidas >= _VidaMax)
        {
            _Vida = _VidaMax;
        }
        else if (_Vida + numVidas <= _VidaMax)
        {
            _Vida += numVidas;
            scale = new Vector3(_Vida / _VidaMax, _Vida / _VidaMax, _Vida / _VidaMax);
        }

        this.transform.localScale = scale;
    }

    public void FallInDeathZone(int numVidas)
    {

		
		//Restamos la vida al caer por la zona de muerte
		if (RestaVidaEnemigo(numVidas, true))//Si devuelve true te has muerto, game over
		{
			Scene scene = SceneManager.GetActiveScene();
			//TODO mirar los nombres de las escenas y si estas en una del 1_ pues poner en la 1a
			if (scene.name == "Lvl_0")
			{
				GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr().UltimaEscena = "Lvl_0";
			} else if (scene.name == "Lvl_2")
			{
				GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr().UltimaEscena = "Lvl_2";
			} else
			{
				GameMgr.GetInstance ().GetCustomMgrs ().GetPlayerMgr ().UltimaEscena = "Lvl_1A";
			
			}
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene("Menu_GameOver2");
			//return;
		}
		GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = false;
		StartCoroutine(FadeBlack());
		//Llamamos al manager de spawn para volver a aparecer en el ultimo check point
		this.transform.position = mSpawManager.GetSpawPoint().position;
        GameObject cam = GameObject.Find("Camera");
        cam.transform.position = mSpawManager.GetSpawPoint().position;
	
	}

	IEnumerator WaitAseconds() {
		print(Time.time);
		yield return new WaitForSeconds(5);
		print(Time.time);
	}

	IEnumerator FadeBlack()
    {
        float amount = Time.deltaTime / fadeTime;
        Color c1 = fadeBlack.color;
        c1.a = 1;
        fadeBlack.color = c1;

        for (float f = 1f; f >= 0f; f = f - amount)
        {
            Color c = fadeBlack.color;
            c.a = f;
            fadeBlack.color = c;
            yield return new WaitForEndOfFrame();
        }

		GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = true;

		yield return null;
    }

    public float Vida
    {
       get
        {
            return _Vida;
        }
        set
        {
            _Vida = value;
        }
    }

    public int CargaPelusas()
    {
        return _BolaPelusas;
    }

    public void FlipInX(bool flip)
    {
        _SpriteGnurr.flipX = flip;
    }

    public void FlipinY(bool flip)
    {
        _SpriteGnurr.flipY = flip;
    }


    private void OnDestroy()
    {

        GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr().Vida = _Vida;
    }

}
