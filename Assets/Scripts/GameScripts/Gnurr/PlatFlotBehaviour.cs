using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFlotBehaviour : MonoBehaviour {

    public GameObject _player;

    void OnTriggerEnter(Collider _player)
    {
        _player.transform.Translate(0, 0, 0.4f);
    }

    private void OnTriggerExit(Collider _player)
    {
        _player.transform.Translate(0, 0, -0.4f);
    }

}
