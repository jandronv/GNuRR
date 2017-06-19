using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour {

    private BoxCollider _WidowsBoxCollider;
    public CharacterController _PlayerCollider;
    public GameObject _Player;
	public float offSet = 0.0f;
	public Vector2 _playerMin, _playerMax;
    public Vector2 _WindowMin, _WindowMax;
 
    public float x, y;

    
    public int lookAhead;

    private float Maxdistance;
   // private float distanceY1;

    void Awake() {

        //_PlayerCollider = GameObject.Find("Player").GetComponent<CharacterController>();
        _WidowsBoxCollider = GetComponent<BoxCollider>();
        //_Player = GameObject.Find("Player");
		
	}


    void Start()
    {
        //Se inicializa con la posicion inicial 
        x = transform.position.x;
        y = transform.position.y;
        lookAhead = 0;
        Maxdistance = ((_WidowsBoxCollider.bounds.size.x / 2) - (_PlayerCollider.bounds.size.x / 2));
        //distanceY1 = ((_WidowsBoxCollider.bounds.size.y / 2) - (_PlayerCollider.bounds.size.y / 2));
    }

    void Update()
    {

		_playerMin.x =_Player.transform.localPosition.x;
		_playerMin.y =_Player.transform.localPosition.y;
		_playerMax.x = _Player.transform.localPosition.x;
		_playerMax.y = _Player.transform.localPosition.y;
        _WindowMin = _WidowsBoxCollider.bounds.min;
        _WindowMax = _WidowsBoxCollider.bounds.max;

		//La pelusa llega al limite derecho 
		//TODO el problema esta en que siempre devuelve 0, por lo tanto, la camara nunca se adelanta porque el player simpre esta dentro del boundary.
		//Lo que hay q hacer es lograr que la camara no se mueva dentro del boundary y cuando llegue a los limetes hacer que se adelante con el offsetrun.
       if (_playerMax.x >= _WindowMax.x)
        {
			x = _Player.transform.position.x;
            lookAhead = 1;
        }
        else if (_playerMin.x <= _WindowMin.x)
        {
            x = _Player.transform.position.x ;
            lookAhead = -1;
        }

		else if (_playerMin.x < _WindowMax.x && _playerMin.x > _WindowMin.x) 
		{
            
		} else 
		{
			x = _Player.transform.position.x;
			lookAhead = 0;
		}
        if (_playerMax.y > _WindowMax.y)
        {
            y = _Player.transform.position.y;
        }
        if (_playerMin.y < _WindowMin.y)
        {
            y = _Player.transform.position.y;

        }
       /* if (_playerMin.y < _WindowMin.y && _playerMax.y > _WindowMax.y)
        {
            y = _Player.transform.position.y;
        }*/
        //Actualizamos la posicion de la camara
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
