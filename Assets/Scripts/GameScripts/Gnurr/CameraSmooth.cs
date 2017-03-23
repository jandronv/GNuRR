using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    private Transform cameraWindow;
    private CameraBoundary mCameraBoundary;
    public float _targetPositionY;
    private float _targetPositionX;
  
    //Nuevos valores de la posicion de la camara
    private float x, y;
    public float smoothTime = 0.5F;
    private Vector3 velocity = Vector3.zero;
   


    void Awake()
    {
        
        mCameraBoundary = GameObject.Find("Boundary").GetComponent<CameraBoundary>();
        _targetPositionY = mCameraBoundary.y;
    }

    void Start()
    {
      
    }

    void Update()
    {

        _targetPositionX = mCameraBoundary.x;
        _targetPositionY = mCameraBoundary.y;


        //TODO regular el +3 ese muy gincho
        if (mCameraBoundary.lookAhead == 1) {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_targetPositionX + 2, _targetPositionY +2, transform.position.z), ref velocity, smoothTime);
        }

        else if (mCameraBoundary.lookAhead == -1) {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_targetPositionX - 2, _targetPositionY + 2, transform.position.z), ref velocity, smoothTime);
        }
        else
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_targetPositionX, _targetPositionY + 2, transform.position.z), ref velocity, smoothTime);
    }

 
}
