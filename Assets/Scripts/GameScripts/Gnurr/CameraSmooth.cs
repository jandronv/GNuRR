using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    private Transform cameraWindow;
    private CameraBoundary mCameraBoundary;
    private float _targetPositionY;
    private float _targetPositionX;
  
    //Nuevos valores de la posicion de la camara
    private float x, y, z;
    private Vector3 velocity = Vector3.zero;

    public float offSetX, offSetY, offSetZ;
    public Vector2 smoothing;


    void Awake()
    {

        z = transform.position.z;
        mCameraBoundary = GameObject.Find("Boundary").GetComponent<CameraBoundary>();
        _targetPositionY = mCameraBoundary.y;
    }

    void Start()
    {
      
    }

    void Update()
    {
        x = transform.position.x;
        y = transform.position.y;

        _targetPositionX = mCameraBoundary.x;
        _targetPositionY = mCameraBoundary.y;

        Debug.Log(Mathf.Round(x)+" "+ Mathf.Round(_targetPositionX));
        if (mCameraBoundary.lookAhead == 1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX))) { //La camara no se esta moviendo
            x = Mathf.SmoothDamp(x, _targetPositionX + offSetX, ref velocity.x, smoothing.x);
        } else if (mCameraBoundary.lookAhead == -1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX))) {
            x = Mathf.SmoothDamp(x, _targetPositionX - offSetX, ref velocity.x, smoothing.x);
        } else {
            x = Mathf.SmoothDamp(x, _targetPositionX , ref velocity.x, smoothing.x); //La camara se está moviendo
        }
      
        y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocity.y, smoothing.y);

        transform.position = new Vector3(x, y , z + offSetZ);

    }

 
}
