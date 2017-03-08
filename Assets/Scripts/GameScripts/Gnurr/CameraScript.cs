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

    //TODO si saltamos tendriamos que ignorar el salto.
    private bool isGrounded = true;


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isGrounded)
        {
            this.transform.position = new Vector3(Target.position.x + offSetX, offSetY, Target.position.z + offSetZ);
        } else
        {
            this.transform.position = new Vector3(Target.position.x + offSetX, Target.position.y + offSetY, Target.position.z + offSetZ);
        }


        //TODO Rotation
        this.transform.rotation = new Quaternion(RotationX, RotationY, RotationY, Target.rotation.w);

    }

    void isJump(bool jump)
    {
        isGrounded = jump;
    }
}
