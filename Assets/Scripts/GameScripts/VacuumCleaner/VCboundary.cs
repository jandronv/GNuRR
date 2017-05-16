using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCboundary : MonoBehaviour
{
    private BoxCollider _WidowsBoxCollider; //el boundary USADO
    public CharacterController _PlayerCollider; //el collider de Izzie USADO
    public GameObject _Player; //Izzie
    public float _VCvelocity; //velocidad de rotacion
    public GameObject m_VCController;
    private float move;

    private Vector2 _playerMin, _playerMax; //los límites del playercollider USADO
    private Vector2 _WindowMin, _WindowMax; //los límites del boundary USADO

   
    void Start()
    {
        _WidowsBoxCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        //definimos los valores de esos vectores que hemos metido, y se va actualizando
        _playerMin = _PlayerCollider.bounds.min;
        _playerMax = _PlayerCollider.bounds.max;
        _WindowMin = _WidowsBoxCollider.bounds.min;
        _WindowMax = _WidowsBoxCollider.bounds.max;

        move = Input.GetAxis("Horizontal");
        Debug.Log(move);

        if (move != 0)
        {
            //La pelusa llega al limite derecho

            if (_playerMax.x >= (_WindowMax.x))
            {
                if (move >= 0.9)
                {
                    m_VCController.transform.Rotate(0, _VCvelocity * Time.deltaTime, 0);
                }
            }

            //la pelusa llega al límite izquierdo
            else if (_playerMin.x <= _WindowMin.x)
            {
                if (move <= -0.9)
                {
                    m_VCController.transform.Rotate(0, -_VCvelocity * Time.deltaTime, 0);
                }
            }
        }
    }
}
