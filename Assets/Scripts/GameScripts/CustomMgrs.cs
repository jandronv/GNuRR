using UnityEngine;
using System.Collections;

/// <summary>
/// Clase que se puede usar para crear los Managers especificos de neustro juego
/// Tiene acceso al GameMgr para poder registrar 
/// </summary>
public class CustomMgrs : GameMgr.ProjectSpecificMgrs
{
    public CustomMgrs(GameMgr gameMgr) : base (gameMgr)
    {
        //ejemplo para añadir un server que no sea MonoBehaviour.
        m_playerMgr = new PlayerMngr();
        //Ejemplo para añadir un server de unity MonoBehaviour.
        //this.AddServerInGameMgr<MyComponent>();
    }

    public PlayerMngr GetPlayerMgr()
    {
        return m_playerMgr;
    }


    private PlayerMngr m_playerMgr;

}
