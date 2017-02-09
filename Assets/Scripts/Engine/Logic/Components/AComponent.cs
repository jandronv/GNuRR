using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Security.Permissions;
/// <summary>
/// A component. Componente abstracto que nos permite encapsular comportamiento que queramos que sea comun para todos los componentes.
/// </summary>
public abstract class AComponent : MonoBehaviour {

	// Use this for initialization
	protected virtual void Awake(){
		//identificador unico de componente.
		m_id = m_idCount++;
	}
	
	//Obtiene un identificador unico de componente que identifica univocamente este componente del resto.
	public int GetID()
	{
		return m_id;
	}
	
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
	
	protected virtual void OnDestroy()
	{
		
	}
	
	private int m_id;
	
	static int m_idCount = 0;
	
	
}
