using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


    public Transform Target;
    public float offSetX;
    public float offSetY;
    public float offSetZ;

    public float RotationX;
    public float RotationY;
    public float RotationZ;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(Target.position.x + offSetX , Target.position.y + offSetY, Target.position.z + offSetZ);

        //TODO Rotation
	}
}
