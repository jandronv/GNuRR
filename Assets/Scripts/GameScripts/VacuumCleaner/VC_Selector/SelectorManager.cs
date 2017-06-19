using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectorManager : MonoBehaviour {

    public Image fondo;
    public Sprite ima1, ima2, ima3;
    public Image mundos;
    private Quaternion mundoseleccionado;
    private bool enter;
    private string Dinos = "Lvl_1A";
    private string Ropa = "Lvl_0";

    // Use this for initialization
    void Start () {
        mundoseleccionado = mundos.transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        mundoseleccionado = mundos.transform.rotation;
        //Debug.Log(mundoseleccionado);
        enter = Input.GetButton("Interact");


        if (enter == true)
        {
            if (mundoseleccionado.z == 0) {
                SceneManager.LoadScene(Dinos);
            }
            else
            {
                SceneManager.LoadScene(Ropa);
            }
        }
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //_animations.SetBool("TriggerIzq", true);
        //Debug.LogWarning("Entra");

        if (other.gameObject.tag == "Player")
        {
            fondo.sprite = ima3;
            if (mundoseleccionado.z == 0)
            {
                mundos.transform.Rotate(0, 0, 180);
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            fondo.sprite = ima2;
        }

    }
}
