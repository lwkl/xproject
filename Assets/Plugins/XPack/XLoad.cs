using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class XLoadRes
{
    // 是否装载所有
    public string res = "";
    public bool all = false;
    public System.Type tp = typeof(UnityEngine.Object);

    // 指针指向资源
    public XAssetInfo asset;

    public UnityEngine.Object ob
    {
        get
        {
            if ( asset != null  )
            {
                return asset.ob;
            }
            return null;
        }
    }

    // 获取
    public UnityEngine.Object[] obs
    {
        get
        {
            if( asset != null )
            {
                return asset.obs;
            }

            return null;
        }
    }

    public Dictionary<string, UnityEngine.Object> obDict
    {
        get
        {
            if( asset != null )
            {
                return asset.obDict;
            }
            return null;
        }
    }

    public XLoadStatus status
    {
        get
        {
            if( asset != null )
            {
                return asset.status;
            }
            return XLoadStatus.LOADING;
        }
    }

    public float progress
    {
        get
        {
            if( asset != null )
            {
                return asset.progress;
            }
            return 1;
        }
    }

    public string info
    {
        get
        {
            return "装载" + res + "中:" + Mathf.Ceil(progress * 100) + "%";
        }

    }
}

public class XLoadDesc : CustomYieldInstruction, XIProgress
{
    public override bool keepWaiting
    {
        get
        {
            return !this.isComplete;
        }
    }

    public XLoadRes cursor;
    public string info
    {
        get
        {
            if( cursor != null )
            {
                return cursor.info;
            }
            else
            {
                return "装载准备中";
            }
        }
    }

    public string tip
    {
        get { return "装载中..."; }
    }

    public bool _isComplete = false;
    public bool isComplete { get { return _isComplete; } }


    public List<XLoadRes> reses = new List<XLoadRes>();
    // 总进度
    public float progress
    {
        get
        {
            if( reses == null )
            {
                return 1;
            }
            if( reses.Count == 0 )
            {
                return 1;
            }

            float ret = 0f;
            for (int i = 0; i < reses.Count; i++)
            {
                ret += reses[i].progress * 1f / reses.Count;
            }
            return ret;
        }
    }

    public UnityEngine.Object ob
    {
        get
        {
            if (reses != null && reses.Count > 0)
            {
                return reses[0].ob;
            }
            return null;
        }
    }

    public UnityEngine.Object[] obs
    {
        get
        {
            if (reses != null && reses.Count > 0)
            {
                return reses[0].obs;
            }
            return null;
        }
    }

    public Dictionary<string, UnityEngine.Object> obDict
    {
        get
        {
            if (reses != null && reses.Count > 0)
            {
                return reses[0].obDict;
            }
            return null;
        }
    }

    public XLoadStatus status
    {
        get
        {
            if (reses != null && reses.Count > 0)
            {
                return reses[0].status;
            }
            return XLoadStatus.NONE;
        }
    }

    public XAssetInfo assetInfo
    {
        get
        {
            if (reses != null && reses.Count > 0)
            {
                return reses[0].asset;
            }
            return null;
        }
    }




}

public class XLoad : MonoBehaviour
{
    // instance
    static XLoad _instance = null;
    public static XLoad instance
    {
        get
        {
            if ( _instance == null )
            {
                _instance = XApp.instance.gameObject.AddComponent<XLoad>();
            }
            return _instance;
        }
    }

    // 装载是否成功了
    protected bool _isOK = false;
    public bool isOK { get { return _isOK; } }

    // 是否从AB中laod
    public bool isLoadFromAB
    {
        get
        {
            bool ret = true;
#if UNITY_EDITOR
            if (!XConf.forceLoadAB)
            {
                ret = false;
            }
#endif     
            return ret;
        }

    }

    // 初始化操作
    public IEnumerator init()
    {
        if( !isLoadFromAB )
        {
            this._isOK = true;
            yield break;
        }

        string version = getABDataPath( "assets/version.json" );
        string fixVersion = getABPath( "assets/version.json" );

        // 固定文件发布专用
        WWW wwwfix = new WWW( fixVersion );
        yield return wwwfix;

        _fixInfo = new XPackInfo();
        if (!string.IsNullOrEmpty(wwwfix.error) || !_fixInfo.load(wwwfix.text))
        {
            XDebug.LogError("装载安装文件失败:" + fixVersion + wwwfix.error);
            wwwfix.Dispose();
            yield break;
        }

        string wwwfixtext = wwwfix.text;
        wwwfix.Dispose();
        wwwfix = null;



        // 新文件存在,读取应该可以读出来，有可能损坏了，读取出错则清空，当成没有
        do
        {
            string versionWrite = getABDataWritePath("assets/version.json");
            if ( XUtil.isExistFile(versionWrite) )
            {
                WWW wwwver = new WWW(version);
                yield return wwwver;

                _info = new XPackInfo();
                if ( !string.IsNullOrEmpty( wwwver.error ) || !_info.load(wwwver.text) )
                {
                    XDebug.LogError("装载版本文件失败:采取删除版本文件进行修复" + version + wwwver.error);
                    wwwver.Dispose();
                    wwwver = null;
                    _info = null;
                    XUtil.deleteFile( version );
                    break;
                }

                // 清理
                wwwver.Dispose();
                wwwver = null;

                // 查看
                // 被重新安装了
                if ( isReinstall )
                {
                    // 那么直接清空所有的更新文件夹
                    XDebug.LogError("版本被重新安装了!直接删除所有缓存目录");
                    XUtil.clearDirectory( getABDataWritePath("") );
                    _info = null;
                }

            }
        }
        while ( false );


        // _fixInfo肯定为好的
        if( _info == null )
        {
            _info = new XPackInfo();
            // 如果不存在fixInfo，那么 fix指向
            _info.load( wwwfixtext);
        }
        
        // 装载信息完毕
        this._isOK = true;
    }


    // 装载单个的
    public static XLoadDesc load( string res, System.Type tp, Action<XLoadDesc> cb = null)
    {
        List<XLoadRes> lassets = new List<XLoadRes>();
        lassets.Add( new XLoadRes { res = res, tp = tp, all = false });
        return load(lassets,  cb);
    }

    public static XLoadDesc load( string res, Action<XLoadDesc> cb = null)
    {
        return load( res, typeof( UnityEngine.Object ), cb);
    }

    public static XLoadDesc load( List<string> assets, Action<XLoadDesc> cb = null )
    {
        List<XLoadRes> reses = new List<XLoadRes>();
        for( int i = 0; i < assets.Count; i++ )
        {
            reses.Add(new XLoadRes { res = assets[i] });
        }

        return load(reses, cb );
    }

    // 额外函数，装载一个文件夹
    public static XLoadDesc loadAll(string res, System.Type tp, Action<XLoadDesc> cb = null)
    {
        List<XLoadRes> reses = new List<XLoadRes>();
        reses.Add( new XLoadRes { res = res, tp = tp, all = true } );
        return load(reses, cb);
    }

    public static XLoadDesc loadAll(string res, Action<XLoadDesc> cb = null)
    {
        return loadAll(res, typeof(UnityEngine.Object), cb);
    }

    // 主函数,所有装载函数会调用次函数
    public static XLoadDesc load( List<XLoadRes> reses, Action<XLoadDesc> cb = null)
    {
        return XLoad.instance.realLoad(reses, cb );
    }


    // 装载的资源
    public Dictionary<XAssetKey, XAssetInfo> assets = new Dictionary<XAssetKey, XAssetInfo>();
    public XAssetInfo getAssetInfoFromCache(XAssetKey key, XLoadStatus status )
    {
        if ( assets.ContainsKey( key) )
        {
            return assets[key];
        }

        XAssetInfo value = new XAssetInfo { res = key.res, all = key.all, tp = key.tp, status = status };
        assets[key] = value;
        return value;
    }

    public bool unloadAssetInfoFromCache( XAssetKey key )
    {
        if ( assets.ContainsKey( key ) )
        {
            assets.Remove( key );
            return true;
        }
        return false;
    }


    public XAssetInfo getAssetInfo( XLoadRes res )
    {
        // 如果有了则直接返回
        if( res.asset != null )
        {
            return res.asset;
        }

        XAssetKey key = new XAssetKey { res = res.res, all = res.all, tp = res.tp };
        if ( string.IsNullOrEmpty( res.res) )
        {
            res.asset = getAssetInfoFromCache( key, XLoadStatus.FAIL );
            return res.asset;
        }

        // 从AB里面获取一个
        if ( isLoadFromAB )
        {
            // 从AB包里面获取
            XBundleInfo info = getAssetBundle(res.res);
            if (info != null)
            {
                // 是否一定要在这里取？--1方便info做引用计数
                res.asset = info.getAssetInfo( key );
            }
            else
            {
                res.asset = getAssetInfoFromCache( key, XLoadStatus.FAIL );
            }
        }
        else
        {
            // 直接在上面在上面进行装载
            res.asset = getAssetInfoFromCache(key, XLoadStatus.NONE );
        }

        return res.asset;
    }

    // DESC不允许为空
    public XLoadDesc fillDesc( XLoadDesc desc )
    {
        // asset desc!= null
        if ( desc.reses == null || desc.reses.Count == 0 )
        {
            // 直接完成了
            desc._isComplete = true;
            return desc;
        }
        bool isComplete = true;
        
        for (int i = 0; i < desc.reses.Count; i++)
        {
            getAssetInfo( desc.reses[i]);
            // 优化直接完成的情况
            if (!desc.reses[i].asset.isComplete)
            {
                isComplete = false;
            }
        }

        desc._isComplete = isComplete;
        return desc;
    }

    // 真正函数地方
    public XLoadDesc realLoad(List<XLoadRes> reses,  Action<XLoadDesc> cb )
    {
        // asset reses!= null
        // XLoad默认模块必须装载成功
        if ( !_isOK )
        {
            XDebug.LogError("XLoad模块并没有初始化成功就进行装载");
            return null;
        }

        // 不装载任何资源，默认模式算完成状态
        XLoadDesc desc = new XLoadDesc { reses = reses };
        fillDesc( desc ) ;

        if( desc.isComplete )
        {
            if (cb != null) { cb(desc); }
            return desc;
        }

#if UNITY_EDITOR
        // 肯定是EDITOR状态
        if ( !isLoadFromAB )
        {
            // 因为是阻塞的，所以没其他问题
            for (int i = 0; i < reses.Count; i++)
            {
                XAssetInfo assetInfo = reses[i].asset;
                if (!assetInfo.isComplete)
                {

                    if (assetInfo.all)
                    {
                        assetInfo.obs = editorLoadAll(assetInfo.res, assetInfo.tp);
                        assetInfo.status = assetInfo.obs == null ? XLoadStatus.FAIL : XLoadStatus.SUCESS;
                    }
                    else
                    {
                        assetInfo.ob = editorLoad(assetInfo.res, assetInfo.tp);
                        assetInfo.status = assetInfo.obs == null ? XLoadStatus.FAIL : XLoadStatus.SUCESS;
                    }
                }
            }

            desc._isComplete = true;
            if (cb != null) { cb(desc); }
            return desc;
        }
        else
#endif
        {
            // 开启协程整流程去装载
            StartCoroutine( coLoad( desc, cb, false) );
        }
 
        return desc;
    }

    // 优化协程版本，不开启更多的协程
    IEnumerator coLoad(XLoadDesc desc, Action<XLoadDesc> cb, bool check)
    {
        // asset desc!= null
        if ( check )
        {
            fillDesc( desc );
            if ( desc.isComplete )
            {
                if (cb != null) { cb(desc); }
                yield break;
            }
        }
        // 一个一个的装载
        for (int i = 0; i < desc.reses.Count; i++)
        {
            XLoadRes res = desc.reses[i];

            desc.cursor = res;
            XAssetInfo asset = res.asset;
            if ( asset.status == XLoadStatus.NONE )
            {
                asset.status = XLoadStatus.LOADING;

                XBundleInfo bundle = res.asset.bundle;
                #region bundle 装载流程
                // 没装载好
                if ( bundle.status == XLoadStatus.NONE )
                {

                    bundle.status = XLoadStatus.LOADING;

                    bool childOK = true;
                    // 拍平了的依赖
                    for (int j = 0; j < bundle.dependAll.Count; j++)
                    {
                        XBundleInfo dpab = bundle.dependAll[j];
                        if ( dpab.status == XLoadStatus.NONE )
                        {
                            dpab.status = XLoadStatus.LOADING;
                            // 真正装载的路径
                            dpab.www = new WWW(getPath(dpab));

                            // XDebug.LogError("装载依赖"+ getPath(dpab));
                            yield return dpab.www;

                            if ( !string.IsNullOrEmpty(dpab.www.error) || dpab.www.assetBundle == null )
                            {
                                dpab.status = XLoadStatus.FAIL;
                            }
                            else
                            {
                                dpab.ab = dpab.www.assetBundle;
                                dpab.status = XLoadStatus.SUCESS;
                            }
                            dpab.www.Dispose();
                            dpab.www = null;
                        }

                        if( !dpab.isComplete )
                        {
                            yield return dpab;

                        }

                        if ( dpab.isFail )
                        {
                            bundle.status = XLoadStatus.FAIL;
                            childOK = false;
                            break;
                        }
                    }
                    // 装载自己
                    if ( childOK )
                    {
                        bundle.www = new WWW( getPath(bundle) );

                        // XDebug.LogError("装载自己" + getPath(bundle));
                        yield return bundle.www;
                        if (!string.IsNullOrEmpty(bundle.www.error) || bundle.www.assetBundle == null)
                        {
                            bundle.status = XLoadStatus.FAIL;
                        }
                        else
                        {
                            bundle.ab = bundle.www.assetBundle;
                            bundle.status = XLoadStatus.SUCESS;
                        }
                        bundle.www.Dispose();
                        bundle.www = null;
                    }
                }
                #endregion

                if( !bundle.isComplete )
                {
                    yield return bundle;
                }
                #region 装载asset
                // 失败了
                if (bundle.isFail)
                {
                    asset.status = XLoadStatus.FAIL;
                }
                else
                {
                    // bundle成功了
                    if ( asset.all )
                    {
                        asset.rq = bundle.ab.LoadAllAssetsAsync(asset.tp);
                        yield return asset.rq;
                        if (asset.rq.isDone && asset.rq.allAssets != null)
                        {
                            asset.status = XLoadStatus.SUCESS;
                            asset.obs = asset.rq.allAssets;
                            if( asset.obs != null )
                            {

                                asset.obDict = new Dictionary<string, UnityEngine.Object>();
                                string[] assetNames = bundle.ab.GetAllAssetNames();

                                if( assetNames.Length == asset.obs.Length )
                                {
                                    for (int k = 0; k < asset.obs.Length; k++)
                                    {
                                        asset.obDict[assetNames[k]] = asset.obs[k];
                                    }
                                }
                                else
                                {
                                   // 如果只有一项，本来名称需要相同的
                                   if( assetNames.Length >= 1 )
                                   {
                                        for (int k = 0; k < asset.obs.Length; k++ )
                                        {
                                            asset.obDict[assetNames[0]] = asset.obs[k];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            asset.status = XLoadStatus.FAIL;
                        }

                        asset.rq = null;
                    }
                    else
                    {
                        string relname = getRelPathName( asset.res, bundle );
                        asset.rq = bundle.ab.LoadAssetAsync( relname, asset.tp );
                        yield return asset.rq;
                        if ( asset.rq.isDone && asset.rq.asset != null )
                        {
                            asset.status = XLoadStatus.SUCESS;
                            asset.ob = asset.rq.asset;
                        }
                        else
                        {
                            asset.status = XLoadStatus.FAIL;
                        }

                        asset.rq = null;
                    }
                }
                #endregion

            }
            
            // 可以让自己直接返回
            if( !asset.isComplete )
            {
                yield return asset;
            }
        }
        desc._isComplete = true;
        if (cb != null) { cb(desc); }
    }


#if UNITY_EDITOR

    static UnityEngine.Object[] editorLoadAll(string aspath, System.Type tp)
    {
        UnityEngine.Object[] obs = UnityEditor.AssetDatabase.LoadAllAssetsAtPath( aspath );

        if( obs == null || obs.Length == 0 )
        {
            return obs;
        }

        List<UnityEngine.Object> obList = new List<UnityEngine.Object>();
        foreach (var ob in obs)
        {
            if ( tp.IsInstanceOfType(ob))
            {
                obList.Add(ob);
            }
        }
        return obList.ToArray();
    }

    static UnityEngine.Object editorLoad(string aspath, System.Type tp )
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath( aspath, tp );
    }
#endif

    string getRelPathName(string res, XBundleInfo bundle )
    {
        if( bundle.isDirectory )
        {
            return _info.getRelName(res, bundle.res);
        }
        else
        {
            return Path.GetFileNameWithoutExtension( res );
        }
    }
    
    string getPath( XBundleInfo bundle )
    {
        // 确定下在什么位置
        if( bundle.persistentData )
        {
            return getABDataPath( bundle.res, bundle.isDirectory );
        }
        else
        {
            return getABPath( bundle.res, bundle.isDirectory );
        }

    }
    public static string getABPath( string path, bool isDirectiory )
    {
        return getABPath(path, isDirectiory ? ".abd" : ".ab");
    }
    public static string getABPath( string path, string back = "" )
    {
        path = path.TrimEnd('/');
#if (!UNITY_EDITOR)
    #if UNITY_ANDROID
                return Application.streamingAssetsPath + "/" + path + back;
    #elif UNITY_IPHONE
                return "file://" + Application.streamingAssetsPath + "/" + path + back;
    #elif UNITY_STANDALONE_OSX
                return "file://" + Application.streamingAssetsPath + "/" + path + back;
    #elif UNITY_STANDALONE_WIN
                return "file:///" + Application.streamingAssetsPath + "/" + path + back;
    #elif UNITY_STANDALONE_LINUX
                return "file://" + Application.streamingAssetsPath + "/" + path + back;
    #else
               return "file://" + Application.streamingAssetsPath + "/" + path + back;
    #endif
#else
    #if UNITY_EDITOR_OSX
                return "file://" + Application.dataPath + "/../ab/mac/" + path + back;
    #else
            // 使用的资源
        return "file:///" + Application.dataPath + "/../ab/win/" + path + back;
    #endif
#endif
    }
    public static string getABDataPath(string path, bool isDirectiory)
    {
        return getABDataPath(path, isDirectiory ? ".abd" : ".ab");
    }
    public static string getABDataPath(string path, string back = "" )
    {

        path = path.TrimEnd('/');
#if (!UNITY_EDITOR)

    #if (UNITY_ANDROID)
            return "file://" + Application.persistentDataPath + "/" + path + back;
    #elif (UNITY_IPHONE)
            return "file://" + Application.persistentDataPath + "/" + path + back;
    #elif (UNITY_STANDALONE_OSX)
            return "file://" + Application.persistentDataPath + "/"+ path + back;
    #elif (UNITY_STANDALONE_WIN)
            return "file:///" + Application.persistentDataPath + "/"+ path + back;
    #elif (UNITY_STANDALONE_LINUX)
            return "file://" + Application.persistentDataPath + "/"+ path + back;
    #else
            return "file://" + Application.persistentDataPath + "/"+ path + back;
    #endif
#else
#if UNITY_EDITOR_OSX
        return "file://" + Application.persistentDataPath + "/mac/" + path + back;
#else// UNITY_EDITOR_WIN
        return "file:///" + Application.persistentDataPath + "/win/" + path + back;
    #endif
#endif

    }

    // 写专用
    public static string getABDataWritePath(string path, bool isDirectiory)
    {
        return getABDataWritePath(path, isDirectiory ? ".abd" : ".ab");
    }
    public static string getABDataWritePath(string path, string back = "")
    {
        path = path.TrimEnd('/');
#if (!UNITY_EDITOR)

#if (UNITY_ANDROID)
            return  Application.persistentDataPath + "/" + path + back;
#elif (UNITY_IPHONE)
            return Application.persistentDataPath + "/" + path + back;
#elif (UNITY_STANDALONE_OSX)
            return  Application.persistentDataPath + "/"+ path + back;
#elif (UNITY_STANDALONE_WIN)
            return  Application.persistentDataPath + "/"+ path + back;
#elif (UNITY_STANDALONE_LINUX)
            return Application.persistentDataPath + "/"+ path + back;
#else
            return  Application.persistentDataPath + "/"+ path + back;
#endif
#else
#if UNITY_EDITOR_OSX
        return  Application.persistentDataPath + "/mac/" + path + back;
#else// UNITY_EDITOR_WIN
        return  Application.persistentDataPath + "/win/" + path + back;
#endif

#endif
    }

    // info
    public XPackInfo _info = null;
    public XPackInfo _fixInfo = null;

    // 获取对象缩在的AssetBundle进行装载
    XBundleInfo getAssetBundle(string res)
    {
        return _info.getAssetBundle(res);
    }

    public string localver
    {
        get
        {
            if ( !_isOK )
            {
                XDebug.LogError("XLoad模块并没有初始化成功就进行读取版本文件");
                return null;
            }

            if ( _info != null )
            {
                return _info._version.Trim();
            }
            return "";
        }
    }

    public bool isReinstall
    {
        get
        {
            if( _info != null && _fixInfo != null )
            {
                if( _info._firstVersion != _fixInfo._version )
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void Update()
    {
        if( _info != null )
        {
            _info.checkBundles();
        }
    }
}