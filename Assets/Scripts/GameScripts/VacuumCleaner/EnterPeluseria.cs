﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPeluseria : MonoBehaviour
{
    private bool enter;

    void OnTriggerStay(Collider _player)
    {
        enter = Input.GetKey(KeyCode.O);
        if (_player.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                //SceneManager.LoadScene("PELUSERIA");
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