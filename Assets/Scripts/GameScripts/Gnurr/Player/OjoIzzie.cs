using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OjoIzzie : MonoBehaviour {

	public Transform posCuerpo;
	public SpriteRenderer cuerpo;
	public float offsetX;
	public float offsetY;
	public float smooth;

	public Player pj;

	// Use this for initialization
	void Start () {
		if (posCuerpo == null)
		{
			Debug.LogWarning("Mete el cuerpo en el script de los ojos!!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//TODO comprobar la vida del player para mover los ojos hacia el centro
		if (pj.Vida <= 10)
		{
			//TODO meter un smooth
			//
			//transform.position = Vector3.Lerp(new Vector3(posCuerpo.position.x, posCuerpo.position.y + offsetY, posCuerpo.position.z), new Vector3(posCuerpo.position.x - 0.13f, posCuerpo.position.y + offsetY, posCuerpo.position.z), smooth);

			if (cuerpo.flipX) {
				transform.position = new Vector3(posCuerpo.position.x + 0.13f, posCuerpo.position.y + offsetY, posCuerpo.position.z);
			}else
				transform.position = new Vector3(posCuerpo.position.x - 0.13f, posCuerpo.position.y + offsetY, posCuerpo.position.z);
		}
		else
			transform.position = new Vector3(posCuerpo.position.x + offsetX, posCuerpo.position.y + offsetY, posCuerpo.position.z);

	}
}
