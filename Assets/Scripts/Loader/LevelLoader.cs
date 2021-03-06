﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;


public enum Tiles{ TILE_0=0, TILE_1, TILE_2, TILE_3, TILE_4, TILE_5, TILE_6, TILE_7, TILE_8, TILE_9, TILE_10, TILE_11, TILE_12, TILE_13, TILE_14, TILE_15, TILE_16, TILE_17, TILE_18, TILE_19, TILE_20, TILE_21, TILE_22, TILE_23, TILE_24, TILE_25, TILE_26, TILE_27, TILE_28, TILE_29, TILE_30, TILE_31, TILE_32, TILE_33, TILE_34, TILE_35}

/// <summary>
/// Clase para asociar los tiles 2d con los prefabs 3d
/// </summary>
[Serializable]
public class TileTranslate
{
    public Tiles _tiles;
    public GameObject _prefab;
}


public class LevelLoader : MonoBehaviour
{
#if UNITY_EDITOR
	public string PREFAB_PATH = "Assets/Prefabs/Niveles/";
    public TextAsset level_JSON;
    public string _prefabName;
    public float scale = 1;
    public Dictionary<int, GameObject> tileSet; 
    public TileTranslate[] _arrayPrefabs;

    private TileLevel level;
    //public Sprite[] spriteSheetLevel;
    private int[,] matrixLevel;
    

    public void InicializarLevel()
    {
        //Inicializamos el diccionario, asociamos tiles con cubos
        tileSet = new Dictionary<int, GameObject>();
        foreach (var item in _arrayPrefabs)
        {
           if( !tileSet.ContainsKey((int)item._tiles))                
           {
            
                tileSet.Add((int)item._tiles, item._prefab);

            }

        }

        //Cargamos los datos del JSON en nuestra clase
        LoadFile(level_JSON);
        //Generamos el nivel con los datos del JSON
        CreateScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadFile(TextAsset levelFile)
    {

        if (levelFile == null)
        {
            throw new Exception("Level File is null!");
        }

        string levelFilePath = AssetDatabase.GetAssetPath(levelFile);

        if (levelFilePath.Substring(levelFilePath.Length - 5, 5) != ".json")
        {
            throw new Exception(levelFile.name + " is not a JSON file!");
        }
        //Cargamos los datos del JSON en nuestra clase
        level = JsonUtility.FromJson<TileLevel>(levelFile.text);
        if (level == null)
        {
            throw new Exception("Level not loaded!");
        }
    }
    private void CreateScene()
    {
  
        //Creamos el padre de todos los cubos del nivel
        GameObject ground = new GameObject("Ground");
        //Recorremos los layers
        foreach (Layers l in level.layers)
        {
            if (l.type == "imagelayer")//Imagenes de fondo
            {
                print("Editando el fondo...");
            }

            if (l.type == "tilelayer" && l.data != null)//Posicion de los tiles
            {
                print("Creando los tiles...");
                matrixLevel = ConvertMatrix(l.data, l.height, l.width);

                for (int i = 0;i < l.height; i++)
                {
                    for (int j = 0;j < l.width;j++)
                    {
                        if (matrixLevel[i,j] != 0) {

                            if (tileSet.ContainsKey(matrixLevel[i, j] - 1))
                            {
                                GameObject g = tileSet[matrixLevel[i, j] - 1];
                                //GameObject g = tileSet[0];
                                GameObject tile = Instantiate(g, new Vector3(i, j, 0), Quaternion.Euler(new Vector3(-180, 90, 0)) );
                                tile.transform.parent = ground.transform; //pone como padre del cubo el go ground.
                                tile.gameObject.tag = "Platforms";                                        
                                if ((matrixLevel[i, j] - 1) == (int)Tiles.TILE_10 || (matrixLevel[i, j] - 1) == (int)Tiles.TILE_11 || (matrixLevel[i, j] - 1) == (int)Tiles.TILE_12)//Si es una plataforma flotante le ponemos el tag
                                {
                                    
                                    tile.layer = LayerMask.NameToLayer("PlataformaFlotante");
                                    foreach (Transform trans in tile.GetComponentsInChildren<Transform>(true))
                                    {
                                        trans.gameObject.layer = LayerMask.NameToLayer("PlataformaFlotante");
                                    }
                                    if (tile.GetComponentInChildren<BoxCollider>() != null) {
                                        tile.GetComponentInChildren<BoxCollider>().isTrigger = true;
                                    }
                                }

                            }
                            else {
                                Debug.Log("Tiled Nº:"+ matrixLevel[i, j]+", no ha sido encontrado.");
                            }

                            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            //cube.transform.parent = ground.transform;
                            //cube.transform.position = new Vector3(i, j, 0);

                        }
                    }
                }

            }

            if (l.type == "objectgroup")//Pos de los collider
            {
                print("Creando colliders...");
            }
            //Giramos el nivel, ya que indicar 'generar de arriba izq' no funciona con el tiled
            ground.transform.Rotate(new Vector3(0f,0f,-90.0f));
            PrefabUtility.CreatePrefab(PREFAB_PATH+_prefabName+".prefab", ground);
            //Destroy(ground);
        }

        /**
         * 
         *      spriteSheet = Resources.LoadAll<Sprite>(level.getPathSpriteSheet(0));
         *      GameObject go = new GameObject("Test");
                go.transform.Translate(new Vector3(1,1,1));
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = spriteSheet[0];
         **/
    }

    static int[,] ConvertMatrix(int[] flat, int m, int n)
    {
        if (flat.Length != m * n)
        {
            throw new ArgumentException("Invalid length");
        }
        int[,] ret = new int[m, n];
        // BlockCopy uses byte lengths: a double is 8 bytes
        Buffer.BlockCopy(flat, 0, ret, 0, flat.Length * sizeof(int));
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="rotation"></param>
    public void MakeGameObject(int posX, int posY, Quaternion rotation, GameObject basicObject)
    {
        Instantiate(basicObject, new Vector3(posX, posY, 0), rotation);
    }

    public void Clear()
    {
        List<GameObject> allChildren = new List<GameObject>();
        foreach (Transform t in transform) allChildren.Add(t.gameObject);
        allChildren.ForEach(child => DestroyImmediate(child));
    }
#endif
}
