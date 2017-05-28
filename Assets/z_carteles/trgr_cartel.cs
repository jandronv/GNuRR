using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trgr_cartel : MonoBehaviour
{

    public GameObject holograma;
    public GameObject bocadillo;
    public GameObject bocadillo2;
    public GameObject player;

    private void Start()
    {
        holograma.SetActive(false);
        bocadillo.SetActive(false);
        bocadillo2.SetActive(false);

    }

    void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            holograma.SetActive(true);
            bocadillo.SetActive(true);
            bocadillo2.SetActive(true);

        }
    }

    private void OnTriggerStay(Collider player)
    {
        if (player.tag == "Player")
        {
            holograma.SetActive(true);
            bocadillo.SetActive(true);
            bocadillo2.SetActive(true);

        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.tag == "Player")
        {
            holograma.SetActive(false);
            bocadillo.SetActive(false);
            bocadillo2.SetActive(false);

        }
    }
}