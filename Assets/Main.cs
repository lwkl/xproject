using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    // forceLoadAB
    public bool forceLoadAB = true;
    // 开启更新
    public bool isUpdateVersion = false;
    // 是否发送远程远程日志
    public bool isRemoteLog = false;
    void Awake()
    {
        Application.runInBackground = true;
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
        // Screen.SetResolution(540, 960, false);
#endif

    }

    

    void Start()
    {
#if UNITY_EDITOR
        XConf.forceLoadAB = forceLoadAB;
        XConf.isUpdateVersion = isUpdateVersion;
        XConf.isRemoteLog = isRemoteLog;
#endif
        this.gameObject.AddComponent<XApp>();
    }
}
