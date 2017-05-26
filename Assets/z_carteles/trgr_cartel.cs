using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trgr_cartel : MonoBehaviour
{

    public GameObject cartel;
    public GameObject player;

    private void Start()
    {
        cartel.SetActive(false);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            cartel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider player)
    {
        if (player.tag == "Player")
        {
            cartel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.tag == "Player")
        {
            cartel.SetActive(false);
        }
    }
}