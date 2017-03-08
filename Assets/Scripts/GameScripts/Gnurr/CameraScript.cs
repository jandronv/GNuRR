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
    private bool Ground = false;
    private float _lockY;

    void Start () {
        _lockY = transform.position.y + offSetY;

    }
	
	// Update is called once per frame
	void Update () {

       
        if (Ground)
        {
            this.transform.position = new Vector3(Target.position.x + offSetX, _lockY, Target.position.z + offSetZ);

        }
        else
        {
            this.transform.position = new Vector3(Target.position.x + offSetX, Target.position.y + offSetY, Target.position.z + offSetZ);
            _lockY = transform.position.y + offSetY;
            Debug.Log("Valor lock: " + _lockY + " Suma: " + Target.position.y + offSetY);

        }
        //TODO Rotation
        this.transform.rotation = new Quaternion(RotationX, RotationY, RotationY, Target.rotation.w);

    }

    public void isJump(bool jump)
    {

        Debug.Log("Llega mensaje");
        //Ground = jump;
    }
}
