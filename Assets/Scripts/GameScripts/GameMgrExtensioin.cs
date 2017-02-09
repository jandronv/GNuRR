using UnityEngine;
using System.Collections;

/// <summary>
/// Método de extensión que nos permite añadir al GameMgr nuestros Managers específicos par el juego.
/// CustomMgrs, no debería ir en una dll y ahí podremos crear nuestros propios managers y manejarlos directamente desde el GameMgr
/// </summary>
public static class GameMgrExtensioin
{
    public static CustomMgrs GetCustomMgrs(this GameMgr gameMgr)
    {
        if(!gameMgr.IsCustomMgrInit())
        {
            CustomMgrs customMgrs = new CustomMgrs(gameMgr);
            gameMgr.CustomMgr = customMgrs;
        }
        return (CustomMgrs)gameMgr.CustomMgr;
    }
     
}


