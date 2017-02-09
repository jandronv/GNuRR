using UnityEngine;
using System.Collections;

/// <summary>
/// Un GameObject Es destruible si tiene el Componente Destroyable y escucha el mensaje DestroyGameObject.
/// </summary>
public class Destroyable : AComponent {
 
	public bool m_desactive = true;

	//Mensaje
	void DestroyGameObject()
	{
		GameMgr.GetInstance().GetSpawnerMgr().DestroyGameObject(this.gameObject,!m_desactive);
	}
 
}
