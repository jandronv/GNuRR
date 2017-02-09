using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// componente que maneja un Slider de unity y lo utiliza como una barra de carga.
/// </summary>
public class LoadingSlider : AComponent
{
    private SceneMgr m_gameMgr;
    private Slider m_slider;
    // Use this for initialization
    protected override void Start () {

        m_gameMgr = GameMgr.GetInstance().GetServer<SceneMgr>();
        m_gameMgr.RegisterLoadingSceneProgressCallback(ProgressLoading);
        m_slider = GetComponent<Slider>();
        m_slider.value = 0f;
    }

    // Update is called once per frame
    protected override void Update () {
	

	}

    protected void ProgressLoading(float progress, bool finish)
    {
        m_slider.value = progress;
    }

    protected override void OnDestroy()
    {
        if(m_gameMgr != null)
            m_gameMgr.UnRegisterLoadingSceneProgressCallback(ProgressLoading);
    }
}
