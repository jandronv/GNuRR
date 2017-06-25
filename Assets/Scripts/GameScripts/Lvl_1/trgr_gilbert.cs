using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trgr_gilbert : MonoBehaviour {

    public GameObject y_button;
    private bool enter = false;
    public GameObject txt_gilbert;
	public int num_bocadillos = 0;
	public bool entraPrimera = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
			
			y_button.SetActive(true);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enter = Input.GetButtonDown("Interact");
            
            if (enter == true)
            {
				if (entraPrimera)
				{
					entraPrimera = false;
					GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = false;
				}

				num_bocadillos++;
                txt_gilbert.SetActive(true);
                y_button.SetActive(false);
            }

			if (num_bocadillos == 10)
			{
				GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = true;
			}

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //y_button.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
        y_button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
