using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMngr {

    private float vida = 20;
    private bool planear = false;

    public float Vida
    {
        get
        {
            return vida;
        }

        set
        {
            vida = value;
        }
    }

    public bool Planear
    {
        get
        {
            return planear;
        }

        set
        {
            planear = value;
        }
    }
}
