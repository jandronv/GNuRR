using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFlotBehaviour : MonoBehaviour {

	public GameObject _player;

	private void OnTriggerStay(Collider other)
	{
		_player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, 0.4f);
	}
	void OnTriggerEnter(Collider _player)
	{
		_player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, 0.4f);
	}

	private void OnTriggerExit(Collider _player)
	{
		_player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, 0.0f);
	}
}
