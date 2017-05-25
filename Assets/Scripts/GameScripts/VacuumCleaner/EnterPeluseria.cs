using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPeluseria : MonoBehaviour
{
    private bool enter;
    private string Peluseria = "Lvl_VC_Peluseria";

    void OnTriggerStay(Collider _player)
    {
        enter = Input.GetButton("Interact");
        if (_player.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                SceneManager.LoadScene(Peluseria);
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