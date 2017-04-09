using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    private Transform cameraWindow;
    private CameraBoundary mCameraBoundary;
    private float _targetPositionY;
    private float _targetPositionX;


    public Transform InitialWorld;
    public Transform EndWorld;
    public int _offsetRun = 0;
    public float RotateX = 0;
    public float RotateY = 0;
    public float RotateZ = 0;
    public float RotateW = 0;

    //Nuevos valores de la posicion de la camara
    private float x, y, z;
    private Vector3 velocity = Vector3.zero;
    private SpriteRenderer gnurr;

    public float offSetX, offSetY, offSetZ;
    public Vector2 smoothing;


    void Awake()
    {

        z = transform.position.z;
        mCameraBoundary = GameObject.Find("Boundary").GetComponent<CameraBoundary>();
        gnurr = GameObject.Find("Gnurr").GetComponent<SpriteRenderer>();
        _targetPositionY = mCameraBoundary.y;
    }

    void Start()
    {
      
    }

    void LateUpdate()
    {
        if (EndWorld == null || InitialWorld == null)
        {
            Debug.LogWarning("Error al iniciar la camara, faltan los finales del mundo para la camara.");
        }
        x = transform.position.x;
        y = transform.position.y;

        _targetPositionX = mCameraBoundary.x;
        _targetPositionY = mCameraBoundary.y;

        // Debug.Log("Camara "+Mathf.Round(x)+" Boundary "+ Mathf.Round(_targetPositionX));

        if (_targetPositionX <= InitialWorld.position.x || _targetPositionX >= EndWorld.position.x)
        {
            y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocity.y, smoothing.y);
        }
        else
        {
            if (mCameraBoundary.lookAhead == 1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX)))
            {
                //Debug.Log("x de la camara: " + x + "Target: " + _targetPositionX);
                x = Mathf.SmoothDamp(x, _targetPositionX + _offsetRun, ref velocity.x, smoothing.x);

            }
            else if (mCameraBoundary.lookAhead == -1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX)))
            {
                x = Mathf.SmoothDamp(x, _targetPositionX - _offsetRun, ref velocity.x, smoothing.x);
            }
            else
            {
                if (gnurr.flipX)
                {
                    x = Mathf.SmoothDamp(x, _targetPositionX - offSetX, ref velocity.x, smoothing.x);
                }
                else
                {
                    x = Mathf.SmoothDamp(x, _targetPositionX + offSetX, ref velocity.x, smoothing.x);
                }
            }

            y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocity.y, smoothing.y);
            transform.position = new Vector3(x, y, z + offSetZ);
            transform.rotation = new Quaternion(RotateX, RotateY, RotateZ, RotateW);

        }

    }

 
}
