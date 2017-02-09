using UnityEngine;
using System.Collections;

[System.Serializable]
public class StorageMgrConfig
{
    [SerializeField]
    private string m_storageFileName;

    public string StorageFileName { get { return m_storageFileName; } set { m_storageFileName = value; } }
}

[System.Serializable]
public class MemoryMgrConfig
{
    [SerializeField]
    private bool m_activeAutoRecolect;
    [SerializeField]
    private float m_maxFrameRateToRecolect;
    [SerializeField]
    private float m_timeSiceLastGarbage;
    [SerializeField]
    private bool m_recollectUnityAssets;

    public bool ActiveAutoRecolect { get { return m_activeAutoRecolect; } set { m_activeAutoRecolect = value; } }
    public float MaxFrameRateToRecolect { get { return m_maxFrameRateToRecolect; } set { m_maxFrameRateToRecolect = value; } }
    public float TimeSiceLastGarbage { get { return m_timeSiceLastGarbage; } set { m_timeSiceLastGarbage = value; } }
    public bool RecollectUnityAssets { get { return m_recollectUnityAssets; } set { m_recollectUnityAssets = value; } }
}

[System.Serializable]
public class InputMgrConfig
{
    [SerializeField]
    private InputMgr.TMouseButtonID m_buttonIdToPointAndClick;
    [SerializeField]
    private bool m_pointAndClickActive;

    public InputMgr.TMouseButtonID ButtonIdToPointAndClick { get { return m_buttonIdToPointAndClick; } set { m_buttonIdToPointAndClick = value; } }
    public bool PointAndClickActive { get { return m_pointAndClickActive; } set { m_pointAndClickActive = value; } }
}


public class GameMgrConfig : ScriptableObject
{
    public StorageMgrConfig m_storageMgrConfig;
    public MemoryMgrConfig m_memoryMgrConfig;
    public InputMgrConfig m_inputMgrConfig;

    public GameMgrConfig()
    {
        m_storageMgrConfig = new StorageMgrConfig();
        m_memoryMgrConfig = new MemoryMgrConfig();
        m_inputMgrConfig = new InputMgrConfig();
    }
}
