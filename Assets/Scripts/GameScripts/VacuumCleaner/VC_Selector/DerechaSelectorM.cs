using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DerechaSelectorM : MonoBehaviour
{

    public Image fondo;
    public Sprite ima1, ima2, ima3;
    public Image mundos;
    private Quaternion mundoseleccionado;

    // Use this for initialization
    void Start()
    {
        mundoseleccionado = mundos.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        mundoseleccionado = mundos.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            fondo.sprite = ima1;
            if (mundoseleccionado.z == 1)
            {
                mundos.transform.Rotate(0, 0, -180);
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
