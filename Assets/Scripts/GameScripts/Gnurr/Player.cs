using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float _VidaMax = 15;
    public float _VidaMin = 1;
    public float _PorcentajeRestarVida=0.90f;
    public float _PorcentajeAumetarVida = 1.2f;
    public float _Vida = 15;
    public int _BolaPelusas = 5;
    public SpriteRenderer _SpriteGnurr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RestarVida(int numVidas)
    {

        if (_VidaMin == _Vida - numVidas)
        {

        } else if ((_Vida - numVidas) > _VidaMin)
        {


        }
        _Vida -= numVidas;

        Vector3 scale = new Vector3(_Vida / _VidaMax, _Vida / _VidaMax, _Vida / _VidaMax);

        this.transform.localScale = scale;
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

    public float GetVida()
    {
        return _Vida;
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

}
