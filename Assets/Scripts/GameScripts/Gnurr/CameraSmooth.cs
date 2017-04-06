using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    private Transform cameraWindow;
    private CameraBoundary mCameraBoundary;
    private float _targetPositionY;
    private float _targetPositionX;


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
        x = transform.position.x;
        y = transform.position.y;

        _targetPositionX = mCameraBoundary.x;
        _targetPositionY = mCameraBoundary.y;

       // Debug.Log("Camara "+Mathf.Round(x)+" Boundary "+ Mathf.Round(_targetPositionX));
        if (mCameraBoundary.lookAhead == 1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX))) { 
            x = Mathf.SmoothDamp(x, _targetPositionX , ref velocity.x, smoothing.x);
        } else if (mCameraBoundary.lookAhead == -1 && (Mathf.Round(x) == Mathf.Round(_targetPositionX))) {
            x = Mathf.SmoothDamp(x, _targetPositionX, ref velocity.x, smoothing.x);
        } else {

            if (gnurr.flipX) {
                x = Mathf.SmoothDamp(x, _targetPositionX - offSetX, ref velocity.x, smoothing.x); 
            }else
            {
                x = Mathf.SmoothDamp(x, _targetPositionX + offSetX, ref velocity.x, smoothing.x);
            }
       }
      
        y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocity.y, smoothing.y);



        transform.position = new Vector3(x, y , z + offSetZ);
        transform.rotation = new Quaternion(RotateX, RotateY, RotateZ, RotateW);
    }

 
}
