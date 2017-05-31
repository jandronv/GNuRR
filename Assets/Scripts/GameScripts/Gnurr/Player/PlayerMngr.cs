using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMngr {

    private float vida = 20;
    private bool planear = false;
	private Vector3 position;
	private bool cambioEscene = false;

	public Vector3 Position
	{
		get
		{
			return position;
		}

		set
		{
			position = value;
		}
	}

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
	public bool CambioEscena
	{
		get
		{
			return cambioEscene;
		}

		set
		{
			cambioEscene = value;
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
