using SLua;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;


// 检测xlua的装载状态
public class XLuaBind : CustomYieldInstruction, XIProgress
{
    public LuaSvr luaSvr;
    public override bool keepWaiting
    {
        get { return !this.isComplete; }
    }
    public float progress
    {
        get
        {
            if( luaSvr == null )
            {
                return 0;
            }
            return luaSvr.bindProgress / 100f;
        }
    }
    public string info { get { return isComplete ? "绑定lua完成" : "绑定lua中"+ Mathf.Ceil( progress * 100) + "%"; } }
    public string tip { get { return isComplete ? "绑定lua完成" : "绑定lua中" + Mathf.Ceil(progress * 100) + "%"; } }

    public bool isComplete
    {
        get
        {
            if( luaSvr == null )
            {
                return false;
            }
            return luaSvr.bindProgress == 100;
        }
    }
}

public class XApp : MonoBehaviour
{
    public static XApp instance;
    public Transform uiRoot;
    public Transform uiProgress;
    public Transform uiMsgBox;
    public Transform uiWindow;
    public Transform uiBack;
    public XLoading xloading;
    public XPoolManager xpool;

    // luaDict
    Dictionary<string, UnityEngine.Object> luaDict;

    // luaSvr
    private LuaSvr luaSvr = null;
    float _lastGCTime = 0f;


    DebugInterface ldb = null;

    // 装载lua文件
    byte[] loadLuaFile(string fn )
    {
        TextAsset txob = null;

        if (luaDict != null)
        {
            string lfn = string.Format("assets/res/lua/" + fn.ToLower() + ".bytes");
            if (luaDict.ContainsKey(lfn))
            {
                txob = luaDict[lfn] as TextAsset;
            }
        }

        // XDebug.Log("装载" + fn + (txob == null ? " null":txob.ToString()) );
        if ( txob != null )
        {
            return txob.bytes;
        }

        else if ( !XLoad.instance.isLoadFromAB )
        {
            fn = Path.Combine("lua", fn) + ".lua";
            FileStream fp = File.OpenRead(fn);
            byte[] bytes = new byte[fp.Length];
            fp.Read(bytes, 0, (int)fp.Length);
            return bytes;
        }

        return null;
    }

    void Awake()
    {
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        GameObject goRoot = GameObject.Find("UIRoot");
        if ( !goRoot )
        {
            XDebug.LogError("UIRoot not exist, quit!!!");
            Application.Quit();
            return;
        }

        uiRoot = goRoot.transform;
        uiProgress = uiRoot.transform.Find("progress");
        if (!uiProgress)
        {
            XDebug.LogError("uiProgress not exist, quit!!!");
            Application.Quit();
            return;
        }
        uiMsgBox = uiRoot.transform.Find("msgbox");
        if (!uiMsgBox)
        {
            XDebug.LogError("uiMsgBox not exist, quit!!!");
            Application.Quit();
            return;
        }
        uiWindow = uiRoot.transform.Find("window");
        if (!uiWindow)
        {
            XDebug.LogError("uiWindow not exist, quit!!!");
            Application.Quit();
            return;
        }

        uiBack = uiRoot.transform.Find("back");
        if (!uiBack)
        {
            XDebug.LogError("uiBack not exist, quit!!!");
            Application.Quit();
            return;
        }

    }

    void Start()
    {

        StartCoroutine( init() );
    }

    // XApp 启动
    IEnumerator init()
    {
        XDebug.Log("init:" + Time.realtimeSinceStartup);
        // 先启动装载服务器
        yield return StartCoroutine( XLoad.instance.init() );

        if( !XLoad.instance.isOK )
        {
            // 弹对话框
            XDebug.LogError("致命错误XLoad没有装载成功");
        }
        else
        {
            // 先加载进度条
            XLoadDesc desc = XLoad.load("Assets/Res/progress/style1.prefab");
            yield return desc;

            if (desc.ob == null)
            {
                XDebug.LogError("Assets/Res/progress/style1.prefab not exist, quit!!!");
                yield break;
            }

            XDebug.Log("onLoadProgress:" + Time.realtimeSinceStartup);
            xloading = this.gameObject.AddComponent<XLoading>();
            xpool = this.gameObject.AddComponent<XPoolManager>();

            GameObject loadingStyle0 = GameObject.Instantiate(desc.ob, uiProgress) as GameObject;
            xloading.addStyle(0, loadingStyle0, loadingStyle0.transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>(), null,
                loadingStyle0.transform.Find("Info").GetComponent<UnityEngine.UI.Text>(), null);


            // 切换进度条
            XLoading.instance.beginShow(0);

            bool wantUpdateVersion = XConf.isUpdateVersion;

            // 是否更新
            if( !XLoad.instance.isLoadFromAB )
            {
                // editor状态下并不更新版本
                wantUpdateVersion = false;
            }

            
            if ( wantUpdateVersion )
            {
                // 创建更新对象
                XUpdate xup = new XUpdate();

                // 开启更新协程
                yield return StartCoroutine( xup.updateVersion(XConf.publishUrl) );

                if( xup.isFail )
                {
                    // 失败了MSGBOX，重新更新，下载链接？都是从头走，清除已装载的东西
                    yield break;
                }
                else
                {
                    // 成功了，那么走正式流程，再次来一发
                    // 清理操作-------
                    // StartCoroutine(init());
                    // yield break;
                }
            }


            XLoadDesc descBack = XLoad.load("Assets/Res/ui/uiBack.prefab");
            XLoading.instance.beginShow(0, descBack);

            yield return descBack;
            if (descBack.ob != null)
            {
                GameObject.Instantiate(descBack.ob, this.uiBack);
            }

            #region 装载lua
            // 开始装载lua文件
            XLoadDesc descLua = loadLua();
            XLoading.instance.beginShow(0, descLua);

            yield return descLua;

            var luaAsInfo = descLua.assetInfo;
            if ( luaAsInfo.isSuccess )
            {
                luaDict = descLua.obDict;
            }

            // 装载lua函数
            LuaState.loaderDelegate += loadLuaFile;

            // 创建一个luaSvr
            luaSvr = new LuaSvr();
            luaSvr.init();

            XLuaBind luaBind = new XLuaBind { luaSvr = luaSvr };
            XLoading.instance.beginShow( 0, luaBind );

            // 全局装载函数

            // 绑定的时间
            yield return luaBind;

            // 绑定完成后从Main开始执行
            luaSvr.onBindComplete( () =>
           {
               // 初始化调试器
               ldb = new DebugInterface( luaSvr.luaState );
               ldb.init();


               XDebug.Log( "luaBindComplete start Main" + Time.realtimeSinceStartup );
               luaSvr.start( XConf.luastart );
               XDebug.Log( "luaBindComplete end Main" + Time.realtimeSinceStartup );
           });
            #endregion

        }
    }



    void Update()
    {
        luaGC();

        if (ldb != null)
        {
            ldb.update();
        }
    }

    public void closeldb()
    {
        if (ldb != null)
        {
            ldb.close();
            ldb = null;
        }
    }
    void OnDestroy()
    {
        closeldb();
    }

    void OnApplicationQuit()
    {
        closeldb();
    }

    void luaGC()
    {
        if ( luaSvr == null)
        {
            return;
        }

        if ( !luaSvr.inited )
        {
            return;
        }

        IntPtr L = luaSvr.luaState.L;
        float escapeTime = Time.unscaledTime - _lastGCTime;
        if (escapeTime >= 5f)
        {
            _lastGCTime = Time.unscaledTime;
            if (LuaDLL.lua_gc(L, LuaGCOptions.LUA_GCSTEP, 1024) == 1)
            {
                LuaDLL.lua_gc(L, LuaGCOptions.LUA_GCRESTART, 0);
            }
        }
    }

    public XLoadDesc loadLua(Action<XLoadDesc> cb = null)
    {
        string path = "Assets/Res/lua/";
#if (UNITY_STANDALONE_WIN && UNITY_EDITOR)
        // 肯定是forceLoadAB
        if ( !XConf.forceLoadAB )
        {
            path = "lua";
        }
#endif
        return XLoad.loadAll(path, typeof(TextAsset), cb);
    }

}
