using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Layers {

    //Clase que almacena la posicion donde se dibujan el fondo, los sprites y colliders
    public int[] data;
    public int height;
    public string image;
    public string name;
    public ColliderObject[] objects;
    public float opacity;
    public string type;
    public bool visible;
    public int width;
    public int x;
    public int y;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
