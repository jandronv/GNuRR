using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour {

    private BoxCollider _WidowsBoxCollider;
    public CharacterController _PlayerCollider;
    public GameObject _Player;

    private Vector2 _playerMin, _playerMax;
    private Vector2 _WindowMin, _WindowMax;
 
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

        _playerMin = _PlayerCollider.bounds.min;
        _playerMax = _PlayerCollider.bounds.max;
        _WindowMin = _WidowsBoxCollider.bounds.min;
        _WindowMax = _WidowsBoxCollider.bounds.max;

        //La pelusa llega al limite derecho
        if (_playerMax.x >= _WindowMax.x + 0.2)
        {
            x = _Player.transform.position.x - Maxdistance;
            lookAhead = 1;
        }
        else if (_playerMin.x <= _WindowMin.x + 0.1)
        {
            x = _Player.transform.position.x + Maxdistance;
            lookAhead = -1;
        }

        else
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
        if (_playerMin.y < _WindowMin.y && _playerMax.y > _WindowMax.y)
        {
            y = _Player.transform.position.y;
        }
        //Actualizamos la posicion de la camara
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
