using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goto : Command {

    // Use this for initialization

    CharacterController _controller;
    GameObject _destino;
    
    public override bool run()
    {
        //
        Vector3 direccion = _destino.transform.position - transform.position;
        _controller.Move(direccion);
        if (direccion.magnitude < 1)
            return true;
        else
            return false;
       
    }
}
