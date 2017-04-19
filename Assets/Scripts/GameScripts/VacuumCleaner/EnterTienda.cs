using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTienda : MonoBehaviour
{
    private bool enter;

    void OnTriggerStay(Collider _player)
    {
        enter = Input.GetKey(KeyCode.O);
        if (_player.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                Debug.Log("ENTRA EN LA TIENDA A COMPRARSE TRAPITOS");
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
