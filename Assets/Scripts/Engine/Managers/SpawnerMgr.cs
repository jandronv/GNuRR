﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Spawner mgr. Manager que gestiona la creacion de entidades dle juego dinamicas. Es altamente recomendable siempre utilizar el
/// SpawnerMng y no crear objetos a mano directamente. De esta forma podemos gestionar de ofrma global la instanciacion dew entidades.
/// </summary>
public class SpawnerMgr
{
	public SpawnerMgr(SceneMgr sceneMgr)
	{
		m_sceneMgr = sceneMgr;
		Assert.AbortIfNot(m_sceneMgr != null, "Error: el Scene mgr debe ser distinto de null");
		//Queremos que el manager de escena nos avise cuando haya temrinado la escena para poder destruir nuestra cache de recursos.
		//TODO 1 Registarnos al calback de fin de escena con OnDestroyCurrentScene.
		m_sceneMgr.RegisterDestroyScene(OnDestroyCurrentScene);
	}
	
	protected void OnDestroyCurrentScene()
	{
		Debug.Log("OnDestroyCurrentScene");
		ClearCache();
	}
	//Crea un gameobject a aprtir de un prefab y otr ogameobject, haciendo un clon dle mismo. Permite ademas posicionarlo
	//y orientarlo.
	public GameObject CreateNewGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject instance = null;
		//Si lo tenemos en la cache, lo reciclamos. (OJO al crear los componentes, estos deben estar pensados para ser reiniciados)
		//TODO 2 Si está en cache, lo recoperamos, lo activamos y lo ponemos y lo sacamos de a lista.
		if(m_cache.ContainsKey(prefab.name))
		{
			List<GameObject> list = m_cache[prefab.name];
			if(list.Count > 0)
			{
				instance = list[0];
				list.RemoveAt(0);
				instance.SetActive(true);
				instance.transform.position = position;
				instance.transform.rotation = rotation;
			}
		}
		//si no lo teniamos en la cache, lo creamos.
		if(instance == null)
		{
			//No tenemos una instancia creado...
			//TODO 3: instanciamos el objeto.
			instance = Object.Instantiate(prefab,position,rotation) as GameObject;
			//Los objetos estan nombrados con le nombre del prefab original, seguido de @ y un numero.
			//Esto es asi para poder obtener luego el prefab original que los instancio.
			instance.name =  prefab.name + "@" + m_staticIDs++;
			GameObject root = GameMgr.GetInstance().GetServer<SceneMgr>().GetCurrentSceneRoot();
			instance.transform.parent = root.transform;
		}
		return instance;
	}
	//"destruimos" un objeto. Un objeto puede estar destruido o desactivado, dependiendo de lo que decidamos.
	//Si esta desactivado, lo meteremos en la cache.
	public void DestroyGameObject(GameObject prefab, bool clear = false)
	{
		if(clear)
			GameObject.Destroy(prefab);
		else
		{
			prefab.SetActive(false);
			string originalPrefabName  = prefab.name;
			//obtenemos el nombre del prefab original...
			if(prefab.name.IndexOf("@") >= 0)
			{
				//Obtengo el nombre dle prefab original
				originalPrefabName = prefab.name.Split('@')[0];
			}
			
			//Miro en la cache is el prefab original esta cacheado.
			if (!m_cache.ContainsKey(originalPrefabName))
			{
				//Si no existe lo creo y lo añado
				List<GameObject> list = new List<GameObject>();
				list.Add(prefab);
				m_cache.Add(originalPrefabName,list);
			}
			else
			{
				//Si existe lo añado.
				List<GameObject> list = m_cache[originalPrefabName];
				list.Add(prefab);
			}
		}
	}
	
	public void ClearCache()
	{
		foreach(List<GameObject> a_list in m_cache.Values)
		{
			foreach( GameObject go in a_list)
			{
				GameObject.Destroy(go);
			}
			a_list.Clear();
		}
		m_cache.Clear();
	}
	
	//Instanciamos los objetos precargados iniciales de la escena.
	public void InstanciateInitialObjects(PrebuildGameObjects gameObjects)
	{
		ClearCache();
		foreach(PrebuildGameObjects.CacheData cd in gameObjects.m_objectCache)
		{
			//Creo los objetos y los almaceno en la cache...
			List<GameObject> list = new List<GameObject>();
			for(int i = 0; i < cd.cacheSize; ++i)
			{
				GameObject newObject = Object.Instantiate(cd.prefab,Vector3.zero,Quaternion.identity) as GameObject;
				newObject.name = cd.prefab.name + "@" + m_staticIDs++;
				newObject.SetActive(false);
				list.Add(newObject);
				GameObject root = GameMgr.GetInstance().GetServer<SceneMgr>().GetCurrentSceneRoot();
				newObject.transform.parent = root.transform;
			}
			m_cache.Add(cd.prefab.name,list);
		}
	}
	
	//obtenemos le ultimo punto de spawn del player.
	public Transform GetPlayerSpawnerPoint()
	{
		return m_lastRespawnPoint;
	}
	//Alacenamos el ultimo punto de respawn del player.
	public void ChangeSpawnPoint(Transform spawnPoint)
	{
		m_lastRespawnPoint = spawnPoint;
	}
	
	private Transform m_lastRespawnPoint;
	private Dictionary<string,List<GameObject>> m_cache = new Dictionary<string, List<GameObject>>();
	private SceneMgr m_sceneMgr;
	private static int m_staticIDs = 0;
}
