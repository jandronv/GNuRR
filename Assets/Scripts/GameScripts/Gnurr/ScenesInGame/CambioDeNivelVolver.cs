using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeNivelVolver : MonoBehaviour {


	public string _name = "";

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("Vida Actual: ");
		if (other.tag == "Player")
		{
			PlayerMngr p = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr();

			p.Vida = other.GetComponent<Player>().Vida;


			if (!p.CambioEscena)
			{
				p.CambioEscena = true;
			}
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(_name);
		}
	}
}
