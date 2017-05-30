using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeNivel : MonoBehaviour {

    public string _name = "";
	public float offSetSpawn = 0.0f;


    public void OnTriggerEnter(Collider other)
    {
		Debug.Log("Vida Actual: ");
		if (other.tag == "Player")
        {
            PlayerMngr p = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr();
          
            p.Vida = other.GetComponent<Player>().Vida;

			
			if (!p.CambioEscena)
			{
				Debug.Log("No hay cambio de escena, me guardo la posicion");
				p.Position = new Vector3(other.GetComponent<PlayerController>().transform.localPosition.x + offSetSpawn, other.GetComponent<PlayerController>().transform.localPosition.y, other.GetComponent<PlayerController>().transform.localPosition.z);
			}
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(_name);
        }
    }
}
