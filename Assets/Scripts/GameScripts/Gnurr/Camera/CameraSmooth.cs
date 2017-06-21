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
   
    public float RotateX = 0;
    public float RotateY = 0;
    public float RotateZ = 0;
    public float RotateW = 0;

    //Nuevos valores de la posicion de la camara
    private float x, y, z;
    public float velocityX = 0.0F, velocityY = 0.0F;
    private SpriteRenderer gnurr;

    public float offSetX, offSetY, offSetZ;
    public float smoothingX;
	public float smoothingYUp, smoothingYDown;



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
		//Debug.Log("Pos de la camara: "+ x);
		

		_targetPositionX = mCameraBoundary.x ;
		//Debug.Log("Pos del target: " + _targetPositionX);
		_targetPositionY = mCameraBoundary.y;

        // Debug.Log("Camara "+Mathf.Round(x)+" Boundary "+ Mathf.Round(_targetPositionX));

        if (_targetPositionX <= InitialWorld.position.x || _targetPositionX >= EndWorld.position.x)
        {
            y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocityY, smoothingX);
            transform.position = new Vector3(x, y, z + offSetZ);
            transform.rotation = new Quaternion(RotateX, RotateY, RotateZ, RotateW);
        }
        else
        {
            if (mCameraBoundary.lookAhead == 1)
            {
                Debug.Log("1");
				x = Mathf.SmoothDamp(x , _targetPositionX + offSetX, ref velocityX, smoothingX);

            }//TODO
            else if (mCameraBoundary.lookAhead == -1)
            {
				Debug.Log("-1");
				x = Mathf.SmoothDamp(x , _targetPositionX - offSetX, ref velocityX, smoothingX);
            }
            else
            {                  
				x = Mathf.SmoothDamp(x, _targetPositionX, ref velocityX, smoothingX);
 
            }
			if (y < _targetPositionY)
			{
				y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocityY, smoothingYUp);
			} else
				y = Mathf.SmoothDamp(y, _targetPositionY + offSetY, ref velocityY, smoothingYDown);

			transform.position = new Vector3(x, y, z + offSetZ);
            transform.rotation = new Quaternion(RotateX, RotateY, RotateZ, RotateW);

        }

    }

    public void FadeBlack() {



    }

 
}
