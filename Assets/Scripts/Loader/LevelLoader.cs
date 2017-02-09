using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;
using System.IO;

public class LevelLoader : MonoBehaviour
{


    public TextAsset level_JSON;
    public float scale = 1;
    public Dictionary<int, GameObject> tileSet; 
  
    private TileLevel level;
    private Sprite[] spriteSheet;
    private int[,] matrixLevel;
    



    public void Start()
    {
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
        // TODO  
        //Cargamos el tile map
        string path = level.getPathSpriteSheet(0);
        spriteSheet = Resources.LoadAll<Sprite>(path);

        
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
                            //TODO Crear el esprite bien, ahora solo se crea un cubo en las pos i j
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.position = new Vector3(i, j, 0);
                            //Crear prefabs

                        }
                    }
                }

            }
            if (l.type == "objectgroup")//Pos de los collider
            {
                print("Creando colliders...");
            }
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
}
