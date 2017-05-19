using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;


public enum XUpdateStatus : int
{
    UPDATING = 1,    // 更新中
    SUCESS = 2,      // 成功
    FAIL = 3,        // 失败
}

// XUpdate
public class XUpdate : CustomYieldInstruction, XIProgress
{
    
    public override bool keepWaiting
    {
        get { return !this.isComplete; }
    }

    public float progress
    {
        get
        {
            if( _www != null )
            {
                return _www.progress;
            }
            return 1;
            
        }
    }

    WWW _www = null;
    string _wwwinfo = "";
    public string info { get { return isComplete ? "更新完成" : _wwwinfo + Mathf.Ceil(progress * 100) + "%"; } }
    public string tip { get { return isComplete ? "更新完成" : _wwwinfo + Mathf.Ceil(progress * 100) + "%"; } }

    XUpdateStatus _status = XUpdateStatus.UPDATING;
    public bool isComplete
    {
        get { return _status > XUpdateStatus.UPDATING; }
    }

    public bool isFail
    {
        get { return _status == XUpdateStatus.FAIL; }
    }
    public bool isSuccess
    {
        get { return _status == XUpdateStatus.SUCESS; }
    }


    // 旧新版本
    public string oldver;
    public string newver;
    public Dictionary<string, XBundleInfo> alreadyDownloadDict = new Dictionary<string, XBundleInfo>();
    

    public string getUpdateDirName()
    {
        return XLoad.getABDataWritePath("/" + oldver + "to" + newver + "/");
    }

    public string getUpdateFileName( string fileName, bool isDirectiory )
    {
        return getUpdateFileName(fileName, isDirectiory ? ".abd" : ".ab");
    }
    public string getUpdateFileName(string fileName, string back = "")
    {
        fileName = fileName.TrimEnd('/');
        return getUpdateDirName() + fileName + back;
    }

    public string getUpdateWebName(string aburl,  string fileName, bool isDirectiory)
    {
        return getUpdateWebName(aburl, fileName, isDirectiory ? ".abd" : ".ab");
    }
    public string getUpdateWebName(string aburl, string fileName, string back = "")
    {
        fileName = fileName.TrimEnd('/');
        return XUtil.getUrl(aburl, fileName + back);
    }

    public string getPlatformPublishUrl(string publishUrl)
    {
        publishUrl = publishUrl.TrimEnd('/');
#if (UNITY_ANDROID && !UNITY_EDITOR)
        return publishUrl + "/android/";
#elif (UNITY_IPHONE && !UNITY_EDITOR)
        return publishUrl + "/ios/";
#elif (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
        return publishUrl + "/win/";
#elif (UNITY_STANDALONE_OSX && !UNITY_EDITOR)
        return publishUrl + "/mac/";
#elif (UNITY_STANDALONE_LINUX && !UNITY_EDITOR)
        return publishUrl + "/linux/";
#elif (UNITY_STANDALONE_WIN && UNITY_EDITOR)
        return publishUrl + "/win/";
#elif (UNITY_STANDALONE_OSX && UNITY_EDITOR)
        return publishUrl + "/mac/";
#else 
        return publishUrl + "/linux/";
#endif
    }

    // 更新流程
    public IEnumerator updateVersion( string publishUrl )
    {
        // 转化下平台接口
        publishUrl = getPlatformPublishUrl( publishUrl );

        // 切换进度条
        XLoading.instance.beginShow( 0, this );

        string urlver = XUtil.getUrl(publishUrl, "ver.json");

        WWW wwwver = new WWW( urlver );

        _www = wwwver;
        _wwwinfo = "获取" +urlver + "中";

        yield return wwwver;

        XVer ver = new XVer();
        if (!string.IsNullOrEmpty(wwwver.error) || !ver.load(wwwver.text))
        {
            XDebug.LogError( "远程版本文件" + urlver + wwwver.error);
            wwwver.Dispose();
            _www = null;
            _status = XUpdateStatus.FAIL;
            yield break;
        }

        wwwver.Dispose();

        XDebug.Log("更新开始远程版本号:v" + ver.ver + "当前版本号:v" + XLoad.instance.localver );

        if ( ver.ver != XLoad.instance.localver )
        {
            string urlversion = XUtil.getUrl(ver.ab, "v" + ver.ver + "/assets/version.json");
            WWW wwwWebInfo = new WWW( urlversion );

            _www = wwwWebInfo;
            _wwwinfo = "获取" + urlversion + "中";

            yield return wwwWebInfo;

            XPackInfo webInfo = new XPackInfo();
            if (!string.IsNullOrEmpty(wwwWebInfo.error) || !webInfo.load(wwwWebInfo.text) )
            {
                XDebug.LogError("远程版本文件" + urlver + wwwWebInfo.error);
                wwwWebInfo.Dispose();
                _www = null;
                _status = XUpdateStatus.FAIL;
                yield break;
            }
            wwwWebInfo.Dispose();

            Dictionary<string, XBundleInfo> modify = XLoad.instance._info.compareWebList(webInfo);
            uint allsize = 0;
            int filecount = modify.Count;
            foreach ( var bd in modify )
            {
                allsize += (uint)(bd.Value.fileSize);
            }

            if( filecount <= 0 )
            {
                XDebug.Log("文件一样无需更新修改版本号");
                XLoad.instance._info._version = ver.ver;
                // 保存文件
                XLoad.instance._info.saveFile(XLoad.getABDataWritePath("assets/version.json"));
                _status = XUpdateStatus.SUCESS;
                // todo 修改版本号
                yield break;
            }

            XDebug.Log(string.Format("一共{0}个文件，大小需要更新{1}", filecount, XUtil.getSizeString(allsize)));

            // 更新目录
            this.oldver = XLoad.instance.localver;
            this.newver = ver.ver;

            string updateDirName = getUpdateDirName();
            string updateFileName = getUpdateFileName("update.json");
            if ( XUtil.isExistDirectory( updateDirName ) && XUtil.isExistFile( updateFileName ) )
            {
                if( !loadFile( updateFileName ) )
                {
                    alreadyDownloadDict.Clear();
                    XUtil.clearDirectory( updateDirName ); 
                }
            }

            int fileno = 0;
            foreach ( var bd in modify )
            {
                fileno++;
                string ufilename = getUpdateFileName(bd.Key, bd.Value.isDirectory);

                if ( alreadyDownloadDict.ContainsKey(bd.Key) )
                {    
                    XBundleInfo bundleInfo = alreadyDownloadDict[ bd.Key ];
                    if( bundleInfo.fileSize == bd.Value.fileSize && bundleInfo.hash == bd.Value.hash && XUtil.isExistFile( ufilename ) )
                    {
                        XDebug.Log("文件已下载完毕，跳过" + ufilename );
                        continue;
                    }
                }

                if ( XUtil.isExistFile(ufilename) )
                {
                    XUtil.deleteFile(ufilename);
                }
                string webufile = getUpdateWebName(ver.ab, "v" + ver.ver + "/" + bd.Key, bd.Value.isDirectory);
                WWW wwwufile = new WWW(webufile);

                _www = wwwufile;
                _wwwinfo = string.Format("下载({0}/{1}){2}", fileno, filecount, bd.Key);

                yield return wwwufile;

                if (!string.IsNullOrEmpty(wwwufile.error)  )
                {
                    XDebug.LogError("远程文件" + webufile + wwwufile.error );
                    wwwufile.Dispose();
                    _www = null;
                    _status = XUpdateStatus.FAIL;
                    yield break;
                }

                if( !XUtil.writeFileAllBytes( ufilename, wwwufile.bytes ) )
                {
                    XDebug.LogError("写入文件失败" + ufilename);
                    wwwufile.Dispose();
                    _www = null;
                    _status = XUpdateStatus.FAIL;
                    yield break;
                }

                alreadyDownloadDict[bd.Key] = bd.Value;
                // 保存一下记录
                saveFile( updateFileName );

                wwwufile.Dispose();
                _www = null;
            }

            // 已经成功了,拷贝文件到目标文件
            _wwwinfo = "拷贝文件中";
            foreach ( var bd in modify )
            {
                string ufilename = getUpdateFileName( bd.Key, bd.Value.isDirectory );

                string dfilename = XLoad.getABDataWritePath( bd.Key, bd.Value.isDirectory );
                if( ! XUtil.copyFile(ufilename, dfilename) )
                {
                    XDebug.LogError("拷贝文件失败" + ufilename);
                    _status = XUpdateStatus.FAIL;
                    break;
                }

                XLoad.instance._info.addBundle( bd.Key, bd.Value );
            }


            _wwwinfo = "重新生成版本文件";
            XLoad.instance._info.callDependAllSize();
            

            // 变更版本
            XLoad.instance._info._version = ver.ver;
            // 保存文件
            if( !XLoad.instance._info.saveFile(XLoad.getABDataWritePath("assets/version.json")) )
            {
                _status = XUpdateStatus.FAIL;
                yield break;
            }

            _status = XUpdateStatus.SUCESS;
        }
        else
        {
            // XDebug.Log("版本一致没有什么不同无需更新");
            _status = XUpdateStatus.SUCESS;
        }
    }

    string readString(Dictionary<string, object> dict, string name)
    {
        string str = "";
        try
        {
            str = dict[name] as string;
        }
        catch (Exception)
        {
            return str;
        }

        if (str == null)
        {
            return "";
        }
        return str.Trim();
    }

    bool readBool(Dictionary<string, object> dict, string name)
    {
        if (dict.ContainsKey(name))
        {
            return Convert.ToBoolean(dict[name]);
        }
        return false;
    }

    int readInt(Dictionary<string, object> dict, string name)
    {
        if (dict.ContainsKey(name))
        {
            return Convert.ToInt32(dict[name]);
        }
        return 0;
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


    public bool load(string str)
    {
        try
        {
            Dictionary<string, object> mapConf = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
            return read_dict(mapConf);
        }
        catch (Exception e)
        {
            XDebug.LogError("装载数据错误" + e.Message);
            return false;
        }
    }

    bool read_dict(Dictionary<string, object> dict)
    {
        
        oldver = readString(dict, "oldver");
        if ( oldver == "")
        {
            XDebug.LogError("配置文件oldver错误");
            return false;
        }

        newver = readString(dict, "newver");
        if ( newver == "")
        {
            XDebug.LogError("配置文件newver错误");
            return false;
        }

        alreadyDownloadDict.Clear();
        Dictionary<string, object> infoDict = dict["downloadDict"] as Dictionary<string, object>;
        foreach (var info in infoDict)
        {
            Dictionary<string, object> asInfoDict = info.Value as Dictionary<string, object>;
            XBundleInfo assetInfo = new XBundleInfo();
            assetInfo.read_dict(asInfoDict);
            alreadyDownloadDict[info.Key] = assetInfo;
        }

        return true;
    }

    public Dictionary<string, object> save_dict()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict["oldver"] = this.oldver;
        dict["newver"] = this.newver;

        Dictionary<string, Dictionary<string, object>> downloadDict = new Dictionary<string, Dictionary<string, object>>();
        dict["downloadDict"] = downloadDict;

        foreach (var info in alreadyDownloadDict)
        {
            downloadDict[info.Key] = info.Value.save_dict();
        }
        return dict;
    }

    // 写入
    public string save()
    {
        Dictionary<string, object> conf = save_dict();
        return MiniJSON.Json.SerializePretty(conf);
    }

    public bool saveFile(string fileName)
    {
        try
        {
            string result = save();
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

}
