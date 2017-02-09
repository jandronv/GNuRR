using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileLevel {

    //Clase que contiene el nivel
    public int height;
    public Layers[] layers;
    public int nextobjectid;
    public string orientation;
    public string renderorder;
    public int tileheight;
    public TileSet[] tilesets;
    public int tilewidth;
    public string version;
    public int width;

    /// <summary>
    /// Funcion que devuelve el path del spritesheet que le pasamos por parametro
    /// </summary>
    /// <param name="spriteSheet">Numero de spritesheet que queremos</param>
    /// <returns></returns>
    public string getPathSpriteSheet(int spriteSheet)
    {
        return tilesets[spriteSheet].image;
    }

    /// <summary>
    /// Funcion que devuelve el array con la posición del sprite en la pantalla
    /// </summary>
    /// <param name="layer">Array con los sprites</param>
    public int[] getPositionSprites(int layer)
    {
        return layers[layer].data;
    }
}
