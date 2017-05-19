using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


public enum XLoadStatus : int
{
    NONE = 0,       // 非装载状态， 有可能是人为取消也算
    LOADING = 1,    // 装载中
    SUCESS = 2,     // 成功
    FAIL = 3,       // 失败
}

// 代表所有的asset
public class XAssetInfo : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return this.status == XLoadStatus.LOADING;
        }
    }

    // bundle装载占整个进度0.9
    public static float bundleWeight = 0.9f;

    // 全名
    public string res;
    public bool all;
    // Asset类型
    public System.Type tp = typeof(UnityEngine.Object);

    // asset信息
    public UnityEngine.Object ob = null;
    // 全部资源
    public UnityEngine.Object[] obs = null;     // loadAll对应
    // 全部资源dict
    public Dictionary<string, UnityEngine.Object> obDict = null;

    // owner为空表示非bundle资源
    public XBundleInfo bundle = null;
    public AssetBundleRequest rq = null;
    public XLoadStatus status = XLoadStatus.NONE;
    public bool isComplete
    {
        get { return status >= XLoadStatus.SUCESS; }
    }
    public bool isFail
    {
        get { return status == XLoadStatus.FAIL; }
    }
    public bool isSuccess
    {
        get { return status == XLoadStatus.SUCESS; }
    }


    public float progress
    {
        get
        {
            switch ( status )
            {
                case XLoadStatus.LOADING:
                    {
                        float progress = 0;
                        if( bundle != null )
                        {
                            progress += bundle.progress * bundleWeight;
                        }
                        
                        if( rq != null )
                        {
                            progress += rq.progress * (1 - bundleWeight);
                        }
                        return progress;
                        
                    }
                // break;
                case XLoadStatus.FAIL:
                case XLoadStatus.SUCESS:
                    return 1;
                //break;
                // case XLoadStatus.NONE:
                default:
                    return 0;
            }
        }
    }

    // 不计算依赖包的
    public float selfProgress
    {
        get
        {
            switch (status)
            {
                case XLoadStatus.LOADING:
                    {
                        if ( rq != null )
                        {
                            return bundle.selfProgress * bundleWeight + rq.progress * (1 - bundleWeight);
                        }

                        return 0;
                    }
                // break;
                case XLoadStatus.FAIL:
                case XLoadStatus.SUCESS:
                    return 1;
                //break;
                // case XLoadStatus.NONE:
                default:
                    return 0;
            }
        }
    }

    // 装载中的对象
    // 引用计数，方便从列表中删除
    public int refcount = 1;
    public int addRef()
    {
        refcount += 1;
        return refcount;
    }
    public int delRef()
    {
        refcount -= 1;
        return refcount;
    }

    // 卸载
    public void unload()
    {
         ob = null;
         obs = null;
        
        if( status == XLoadStatus.LOADING || status == XLoadStatus.NONE )
        {
            status = XLoadStatus.FAIL;
        }

        XAssetKey key = new XAssetKey { res = res, all = all, tp = tp };
        // 一般情况下都是属于bundle的，另外Editor模式是属于别的
        if ( bundle == null )
        {
            XLoad.instance.unloadAssetInfoFromCache( key );
        }
        else
        {
            // 属于bundle的
            bundle.unloadAssetInfo( key );
        }
    }

}
public struct XAssetKey
{
    public string res;
    public bool all;
    // Asset类型
    public System.Type tp;
}


// 记录打包的结构,用来查找最合适的包
public class XBundleInfo : CustomYieldInstruction
{
    public static float EMPTY_UNLOAD_SECONDS = 10;
    public override bool keepWaiting
    {
        get
        {
            return status == XLoadStatus.LOADING;
        }
    }

    // 本身权重
    public float progress
    {
        get
        {
            switch (status)
            {
                case XLoadStatus.LOADING:
                    {
                        return loadSize * 1f / allSize;
                    }
                    // break;
                case XLoadStatus.FAIL:
                case XLoadStatus.SUCESS:
                    return 1;
                    //break;
                // case XLoadStatus.NONE:
                default:
                    return 0; 
            }
        }
    }

    // 不包含依赖的自身进度
    public float selfProgress
    {
        get
        {
            switch ( status )
            {
                case XLoadStatus.LOADING:
                    {
                        return selfLoadSize * 1f / fileSize;
                    }
                // break;
                case XLoadStatus.FAIL:
                case XLoadStatus.SUCESS:
                    return 1;
                //break;
                // case XLoadStatus.NONE:
                default:
                    return 0;
            }
        }
    }

    // 固定记录
    public string res;
    public bool isDirectory = false;
    public int fileSize = 0;
    public Hash128 hash;
    // 依赖列表
    public List<string> dependList = new List<string>();
    public List<XBundleInfo> depend = new List<XBundleInfo>();
    // 树状依赖展开
    public List<XBundleInfo> dependAll = new List<XBundleInfo>();

    // 影响到的
    public List<XBundleInfo> affect = new List<XBundleInfo>();
    public List<XBundleInfo> affectAll = new List<XBundleInfo>();

    // 装载的资源
    public Dictionary<XAssetKey, XAssetInfo> assets = new Dictionary<XAssetKey, XAssetInfo>();

    // 最后一次为空的时间
    private float lastAssetsEmpty = 0f;

    // 获取一个依赖包的资源
    public XAssetInfo getAssetInfo( XAssetKey key )
    {
        if( assets.ContainsKey(key) )
        {
            return assets[key];
        }
        XAssetInfo value = new XAssetInfo { res = key.res, all = key.all, tp = key.tp, status = XLoadStatus.NONE, bundle = this };
        assets[key] = value;

        return value;
    }

    public bool unloadAssetInfo(XAssetKey key)
    {
        if ( assets.ContainsKey(key) )
        {
            assets.Remove(key);

            if( assets.Count == 0 )
            {
                // 计时需要释放,说明曾经unload过，如果一次没有，那么不允许释放
                lastAssetsEmpty = Time.realtimeSinceStartup;
            }
            return true;
        }
        return false;
    }

    // 计算总大小
    public void calAllSize()
    {
        allSize = fileSize;
        for (int i = 0; i < depend.Count; i++)
        {
            allSize += depend[i].fileSize;
        }
    }

    // 包括依赖的所有的大小
    public int allSize = 0;
    
    // 包括依赖装载大小
    public int loadSize
    {
        get
        {
            int ret = 0;
            for( int i=0; i< depend.Count; i++ )
            {
                ret += depend[i].loadSize;
            }
            if( www!= null )
            {
                ret += www.bytesDownloaded;
            }
            return ret;
        }
    }

    public int selfLoadSize
    {
        get
        {
            int ret = 0;
            if (www != null)
            {
                ret += www.bytesDownloaded;
            }
            return ret;
        }

    }
    /// <summary>
    /// 存储在啥地方
    /// </summary>
    public bool persistentData = false;
    
    // 运行时
    public HashSet<string> resdepend;

    public AssetBundle ab;
    public delegate void AssetInfoCallBack( AssetBundle ab );
    public AssetInfoCallBack callback;
    public WWW www;

    public XLoadStatus status = XLoadStatus.NONE;
    public bool isComplete
    {
        get { return status >= XLoadStatus.SUCESS; }
    }

    public bool isFail
    {
        // 人为取消也算
        get { return status == XLoadStatus.FAIL || status == XLoadStatus.NONE; }
    }

    public bool isSuccess
    {
        get { return status == XLoadStatus.SUCESS; }
    }

    public Dictionary<string, object> save_dict()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["res"] = res;
        dict["isDirectory"] = isDirectory;

        dict["hash"] = hash.ToString();
        dict["dependList"] = dependList;
        dict["persistendData"] = persistentData;
        dict["fileSize"] = fileSize;

        return dict;
    }

    public void read_dict(Dictionary<string, object> dict)
    {
        res = dict["res"] as string;
        isDirectory = Convert.ToBoolean( dict["isDirectory"] );

        fileSize = Convert.ToInt32( dict["fileSize"] );

        hash = Hash128.Parse( Convert.ToString(dict["hash"]));
        if (dict.ContainsKey("persistendData"))
        {
            persistentData = Convert.ToBoolean(dict["persistendData"]);
        }
        dependList.Clear();

        List<object> dependOBList = dict["dependList"] as List< object>;
        foreach (var ob in dependOBList)
        {
            dependList.Add(Convert.ToString(ob));
        }
    }

    // 根据depend设置dependList
    public void setDependList()
    {
        dependList.Clear();
        foreach (var asinfo in depend)
        {
            dependList.Add(asinfo.res);
        }
    }

    // 根据dependList设置depend
    public void setDependAffect(Dictionary<string, XBundleInfo> info)
    {

        foreach( var str in dependList )
        {
            if( info.ContainsKey( str ) )
            {
                depend.Add(info[str]);

                // 增加依赖项
                info[str].affect.Add( this );
            }
            else
            {
                XDebug.LogError("setDepend 不存在assetInfo" + str);
            }
        }
    }

    public void calDependChild(XBundleInfo bundle)
    {
        foreach (var dp in bundle.depend)
        {
            calDependChild(dp);
        }

        dependAll.Add( bundle );
    }

    public void calDependAll( )
    {
        dependAll.Clear();
        // 多写一遍不加自己
        foreach (var dp in this.depend)
        {
            calDependChild(dp);
        }
    }

    public void calAffectChild(XBundleInfo bundle)
    {
        foreach (var af in bundle.affect)
        {
            calAffectChild( af);
        }

        affectAll.Add( bundle );
    }

    public void calAffectAll()
    {
        affectAll.Clear();
        // 多写一遍不加自己
        foreach (var af in this.affect)
        {
            calAffectChild( af );
        }
    }

    /// <summary>
    ///  依赖的都装载了
    /// </summary>
    /// <returns></returns>
    public bool isDependComplete()
    {
        foreach ( var v in this.depend )
        {
            if( !v.isComplete )
            {
                return false;
            }
        }
        return true;
    }

    // 没有被人依赖
    public bool isAffectUnload()
    {
        foreach (var v in this.affect )
        {
            if ( v.isComplete )
            {
                return false;
            }
        }
        return true;
    }

    public bool checkUnload()
    {
        if ( this.isSuccess && this.assets.Count == 0 && this.lastAssetsEmpty > 0f &&
            Time.realtimeSinceStartup - this.lastAssetsEmpty > EMPTY_UNLOAD_SECONDS  && isAffectUnload() )
        {
            unloadBundle();
            return true;
        }

        return false;
    }
    //
    public void unloadBundle()
    {
        // 释放函数

        if( ab != null )
        {
            if (this.assets.Count > 0)
            {
                XDebug.LogWarning(this.res + ",有还有" + this.assets.Count + "资源未释放");
            }

            ab.Unload( true );
            
            ab = null;
        }
        // 恢复到初始状态
        this.status = XLoadStatus.NONE;

    }



}

public class XPackInfo
{
    // 记录stack信息以便确定依赖包
    Dictionary<string, XBundleInfo> _info = new Dictionary<string, XBundleInfo>();

    public Dictionary<string, XBundleInfo> info
    {
        get
        {
            return _info;
        }
    }
    public string _firstVersion = "0.0";
    public string _version = "0.0";
    // 运行时
    public delegate void PackInfoCallBack( bool success );
    // XPackInfo装载委托
    public PackInfoCallBack _callback;
    public void clear()
    {
        _info = new Dictionary<string, XBundleInfo>();
    }

    public HashSet<string> toHashSet( string[] resDepends )
    {
        HashSet<string> ret = new HashSet<string>();
        ret.UnionWith(resDepends);
        return ret;
    }

    public string getResName( string abname, out bool isDirectory )
    {
        string ext = Path.GetExtension(abname);
        isDirectory = (ext == ".abd" ? true : false);

        string res = abname.Substring(0, abname.Length - Path.GetExtension(abname).Length);
        if ( isDirectory )
        {
            res += "/";
        }
        return res;

    }
    public void parseManifest( AssetBundleManifest manifest, string packDir )
    {
        clear();

        string[] abs = manifest.GetAllAssetBundles();

        // 这里已经是小写了，所以没有大小写问题
        for( int i=0; i<abs.Length; i++ )
        {
            bool isDirectory = false;
            string res = getResName(abs[i], out isDirectory);
            XBundleInfo ainfo = new XBundleInfo();
            ainfo.res = res;
            ainfo.isDirectory = isDirectory;
            ainfo.persistentData = false;
            ainfo.hash = manifest.GetAssetBundleHash(abs[i]);
            ainfo.dependList = new List<string>( manifest.GetAllDependencies(abs[i]) );

            // 获取文件大小
            ainfo.fileSize = XUtil.getFileSize( packDir + abs[i] );
            for( int j = 0; j < ainfo.dependList.Count; j++ )
            {
                ainfo.dependList[j] = getResName(ainfo.dependList[j], out isDirectory );
            }

            _info.Add(ainfo.res, ainfo);
        }

        this.callDependAllSize();
    }


    public XBundleInfo getAssetBundle(string res)
    {
        // 小写
        res = res.ToLower();

        // 首先搜索直接文件
        if( _info.ContainsKey(res) )
        {
            return _info[res];
        }
        else
        {
            // 搜素以文件对应的每个文件夹
            string dirname;
            for( int i= 0; i<res.Length; i++ )
            {
                if( res[i] == '/' )
                {
                    dirname = res.Substring(0, i + 1);
                    if( _info.ContainsKey( dirname) && _info[dirname].isDirectory )
                    {
                        return _info[dirname];
                    }
                }
            }
        }

        return null;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 读取
    public bool load(string str)
    {
        try
        {
            Dictionary<string, object> mapConf = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
            read_dict(mapConf);
        }
        catch (System.Exception e)
        {
            XDebug.LogError("装载版本文件失败:采取删除该文件进行修复" + e.Message);
            return false;
        }
        return true;
    }

    // 写入
    public string save()
    {
        Dictionary<string, object> conf = save_dict();
        return MiniJSON.Json.SerializePretty(conf);
    }

    public Dictionary<string, object> save_dict()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["version"] = _version;

        dict["firstVersion"] = _firstVersion;
        
        Dictionary<string, Dictionary<string, object>> infoDict = new Dictionary<string, Dictionary<string, object>>();
        dict["info"] = infoDict;

        foreach (var info in _info )
        {
            infoDict[info.Key] = info.Value.save_dict();
        }
        return dict;
    }

    void read_dict(Dictionary<string, object> dict)
    {
        _version = dict["version"] as string;

        try
        {
            _firstVersion = dict["firstVersion"] as string;
        }
        catch( Exception )
        {
            _firstVersion = _version;
        }

        _info.Clear();
        Dictionary<string, object> infoDict = dict["info"] as Dictionary<string, object>;
        foreach(var info in infoDict )
        {
            Dictionary<string, object> asInfoDict = info.Value as Dictionary<string, object>;
            XBundleInfo assetInfo = new XBundleInfo();
            assetInfo.read_dict( asInfoDict );
            _info[info.Key] = assetInfo;
        }

        callDependAllSize();
    }

    public void callDependAllSize()
    {
        // 清理一下老依赖
        foreach( var info in _info )
        {
            info.Value.depend.Clear();
            info.Value.affect.Clear();
        }

        // 递归遍历
        foreach (var info in _info)
        {
            info.Value.setDependAffect(_info);
        }

        // 为后面优化铺平递归调用
        foreach (var info in _info)
        {
            info.Value.calDependAll();
            info.Value.calAffectAll();
            info.Value.calAllSize();
        }

    }


    public bool saveFile(string fileName)
    {
        try
        {
            XDebug.Log("saveFile filename:" + fileName);
            string result = save();

            // 目录要先创建
            // 创建文件夹，拷贝文件
            XUtil.assureDirectory( fileName );
            FileStream fs = File.Open(fileName, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(result);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
        catch (Exception e)
        {
            XDebug.LogError(e.Message);
            return false;
        }

        return true;
    }

    public bool loadFile(string fileName)
    {
        try
        {
            var sr = File.OpenText(fileName);
            string content = sr.ReadToEnd();
            sr.Close();
            load(content);
        }
        catch (Exception e)
        {
            XDebug.LogError(e.Message);
            return false;
        }

        return true;
    }

    // 获取目录打包相对目录名，右边有/
    public string getRelDir( string res, string dir )
    {
        string name = res.Remove(0, dir.Length).TrimStart('/');
        // 相对目录
        string reldir;
        if( name == "" )
        {
            reldir = "";
        }
        else
        {
            reldir = Path.GetDirectoryName(name);
        }

        if (reldir != "" && reldir[reldir.Length-1] != '/')
        {
            reldir += "/";
        }

        return reldir;
    }

    // 获取目录打包相对资源名
    public string getRelName(string res, string dir)
    {
        return getRelDir( res, dir) + Path.GetFileNameWithoutExtension( res );
    }

    // 比较别的，返回other的列表
    public Dictionary<string, XBundleInfo> compareWebList( XPackInfo other )
    {
        Dictionary<string, XBundleInfo> ret = new Dictionary<string, XBundleInfo>();

        foreach(var bd in other.info )
        {
            // 只更新没有的
            if( ! ( _info.ContainsKey(bd.Key) && 
                _info[bd.Key].fileSize == bd.Value.fileSize && 
                _info[bd.Key].hash == bd.Value.hash ) )
            {
                ret[bd.Key] = bd.Value;
            }
        }

        return ret;
    }

    public Dictionary<string, XBundleInfo> getBoundsLoaded()
    {
        Dictionary<string, XBundleInfo> ret = new Dictionary<string, XBundleInfo>();

        foreach (var bd in this.info )
        {
            if( bd.Value.isSuccess && bd.Value.ab != null )
            {
                ret[bd.Key] = bd.Value;
            }
        }
        return ret;

    }

    public void addBundles(Dictionary<string, XBundleInfo> bundles )
    {
        foreach(var bd in bundles )
        {
            addBundle( bd.Key, bd.Value );
        }
    }

    // 添加一个bundle，如果有则修改
    public void addBundle(string res, XBundleInfo bundleInfo )
    {
        XBundleInfo localBundle;
        if (_info.ContainsKey(res))
        {
            localBundle = _info[res];
        }
        else
        {
            localBundle = new XBundleInfo();
            _info[res] = localBundle;
            
        }
        localBundle.res = res;
        localBundle.isDirectory = bundleInfo.isDirectory;
        localBundle.fileSize = bundleInfo.fileSize;
        localBundle.hash = bundleInfo.hash;
        localBundle.dependList = bundleInfo.dependList;
        localBundle.persistentData = true;       
    }

    public void checkBundles()
    {
        foreach( var bd in _info )
        {
            bd.Value.checkUnload();
        }
    }


}