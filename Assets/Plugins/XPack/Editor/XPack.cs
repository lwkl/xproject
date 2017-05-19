using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;


public class XPack
{
    public string abpathname = "ab";

    public static Dictionary<BuildTarget, string> builddict = new Dictionary<BuildTarget, string>()
        {
            { BuildTarget.Android,              "/android/" },
            { BuildTarget.iOS,                  "/ios/" },
            { BuildTarget.StandaloneWindows,    "/win/" },
            { BuildTarget.StandaloneWindows64,  "/win64/" },
            { BuildTarget.StandaloneOSXIntel,   "/mac32/" },
            { BuildTarget.StandaloneOSXIntel64, "/mac64/"},
            { BuildTarget.StandaloneOSXUniversal,"/mac/"},
            { BuildTarget.StandaloneLinux,      "/linux32/"},
            { BuildTarget.StandaloneLinux64,    "/linux64/"},
            { BuildTarget.StandaloneLinuxUniversal,"/linux/"},
        };

    public enum BUILDLUATYPE : int
    {
        WIN32 = 0,
        WIN64 = 1,
        GC64 = 2,
    }
    public static Dictionary<BuildTarget, BUILDLUATYPE> buildluddict = new Dictionary<BuildTarget, BUILDLUATYPE>()
        {
            { BuildTarget.Android,              BUILDLUATYPE.WIN32 },
            { BuildTarget.iOS,                  BUILDLUATYPE.GC64 },
            { BuildTarget.StandaloneWindows,    BUILDLUATYPE.WIN32 },
            { BuildTarget.StandaloneWindows64,  BUILDLUATYPE.WIN64 },
            { BuildTarget.StandaloneOSXIntel,   BUILDLUATYPE.WIN32 },
            { BuildTarget.StandaloneOSXIntel64, BUILDLUATYPE.GC64},
            { BuildTarget.StandaloneOSXUniversal,BUILDLUATYPE.GC64},
            { BuildTarget.StandaloneLinux,      BUILDLUATYPE.WIN32},
            { BuildTarget.StandaloneLinux64,    BUILDLUATYPE.WIN64},
            { BuildTarget.StandaloneLinuxUniversal,BUILDLUATYPE.WIN64},
        };

    // 发版平台
    private BuildTarget _target = BuildTarget.StandaloneWindows;
    private string _targetDir = "../ab/win/";

    public string targetDir { get { return _targetDir; } }
    public BuildTarget target { get { return _target; } }

    // 根据当前的目录打包
    public void setInfo()
    {
        setInfo(getCurTarget());
    }

    public void setInfo( BuildTarget target )
    {
        _target = target;
        _targetDir = Application.dataPath + "/../" + abpathname + builddict[_target];
        XUtil.assureDirectory(_targetDir);
        if ( !Directory.Exists(_targetDir) )
        {
            Directory.CreateDirectory( _targetDir );
        }
    }

    public string getPublishDir( string publishDir )
    {
        // 标准格式化
        publishDir = publishDir.Replace("\\", "/").TrimEnd('/');

        return Application.dataPath + "/../" + publishDir + builddict[_target];

    }

    public string getVersionDir(string publishDir, string ver)
    {
        return getPublishDir( publishDir) + ver + "/";
    }

    public string getWebDir(string webPublishUrl  )
    {
        // 标准格式化
        webPublishUrl = webPublishUrl.Replace("\\", "/").TrimEnd('/');
        return webPublishUrl + builddict[_target];
    }


    public static BuildTarget getCurTarget()
    {
#if UNITY_ANDROID
        return BuildTarget.Android;
#elif UNITY_IPHONE
        return BuildTarget.iOS;
#elif UNITY_OSX
        return BuildTarget.StandaloneOSXIntel
#elif UNITY_LINUX
        return BuildTarget.StandaloneLinux
#else // UNITY_EDITOR
        return BuildTarget.StandaloneWindows;
#endif
    }


    public bool pack_file( string res )
    {
        AssetImporter importer = AssetImporter.GetAtPath( res );
        if( importer == null )
        {
            XDebug.LogError("打包错误，地址不存在" + res);
            return false;
        }
        importer.assetBundleName = res + ".ab";

        return true;
        
    }

    // 获取统配
    public string[] getDirFile(string dir, string tp, SearchOption searchOption )
    {
        if( tp == null )
        {
            return new string[0];
        }
        
        tp.Trim();
        string []tps = tp.Split(';');

        List<string> tpList = new List<string>();
        foreach( var v in tps )
        {
            v.Trim();
            if( v != "" )
            {
                string[] files = Directory.GetFiles(dir, v, searchOption );
                foreach(var f in files )
                {
                    tpList.Add(f.Replace("\\", "/"));
                }
            }
        }
        return tpList.ToArray();
    }


    public void compileLua(string[] src, string[] dst)
    {
        for ( int i = 0; i < src.Length; i++ )
        {
            string srcLua = Application.dataPath + "/../" + src[i];
            string dstLua = Application.dataPath + "/../" + dst[i];

            XUtil.deleteFile( dstLua );
            File.Copy(srcLua, dstLua);
        }
    }



    // 编译lua到固定目录
    public void compileLuaJit(string[] src, string[] dst)
    {
        XDebug.Log("compileLuajit");
#if !UNITY_EDITOR_OSX
        string workDir = Application.dataPath + "/../jit/";
        Dictionary<BUILDLUATYPE, string> build = new Dictionary<BUILDLUATYPE, string>
        {
            { BUILDLUATYPE.WIN32, Application.dataPath + "/../jit/win/x86/luajit.exe" },
            { BUILDLUATYPE.WIN64, Application.dataPath + "/../jit/win/x64/luajit.exe" },
            { BUILDLUATYPE.GC64, Application.dataPath + "/../jit/win/gc64/luajit.exe" },
        };
        string exePath = build[buildluddict[_target]];
        Process[] psList = new Process[ src.Length ];

#else
        string workDir = Application.dataPath + "/../jit/";
        Dictionary<BUILDLUATYPE, string> build = new Dictionary<BUILDLUATYPE, string>
        {
            { BUILDLUATYPE.WIN32, Application.dataPath + "/../jit/mac/x86/luajit" },
            { BUILDLUATYPE.WIN64, Application.dataPath + "/../jit/mac/x64/luajit" },
            { BUILDLUATYPE.GC64, Application.dataPath + "/../jit/mac/gc64/luajit" },
        };

        string exePath = build[buildluddict[_target]];
        Process[] psList = new Process[ src.Length ];
#endif

        for ( int i=0; i<src.Length; i++)
        {
            string srcLua = Application.dataPath + "/../" + src[i];
            string dstLua = Application.dataPath + "/../" + dst[i];
            string cmd = " -b " + srcLua + " " + dstLua;

           
#if !UNITY_EDITOR_OSX
            psList[i] = ProcessHelper.StartProcess(exePath, cmd, workDir );
#else
            var ps = ProcessHelper.StartProcess(exePath, cmd, workDir );
            ps.WaitForExit();
#endif
        }


#if !UNITY_EDITOR_OSX
        foreach (var ps in psList)
        {
            if (ps != null && !ps.HasExited)
            {
                ps.WaitForExit();
            }
        }
#endif
    }



    // 直接打包
    public bool pack( string res )
    {
        AssetImporter importer = AssetImporter.GetAtPath(res);
        if (importer == null)
        {
            XDebug.LogError("打包错误，地址不存在" + res);
            return false;
        }
        importer.assetBundleName = res + ".ab";

        return true;
    }
    
    // 目录打包
    public bool packdir(string res, string tp, SearchOption searchOption )
    {
        XDebug.Log("打包目录" + res + "类型" + tp);
        string[] files = getDirFile(res, tp, searchOption);
        if (files.Length == 0)
        {
            return false;
        }

        // 去掉末尾的/ 创建目录
        string fn = res.TrimEnd('/') + ".abd";


        // 看有没有子目录判定是那种方式去包装他        
        // List<Object> obs = new List<Object>();
        foreach (var file in files)
        {
            AssetImporter importer = AssetImporter.GetAtPath(file);
            importer.assetBundleName = fn;

            XDebug.Log("打包目录加入" + file);
        }

        return true;
    }

    // 打包目录中所有资源
    public bool packdirfiles(string res, string tp, SearchOption searchOption)
    {
        string[] files = getDirFile(res, tp, searchOption);
        if (files.Length == 0)
        {
            return false;
        }

        foreach (var file in files)
        {
            pack_file(file);
        }

        return true;
    }


    // 打包目录中所有资源
    public bool packdirdir(string res, string tp)
    {
        string[] dirs = Directory.GetDirectories(res);
        if (dirs.Length == 0)
        {
            return false;
        }

        foreach (var d in dirs)
        {
            packdir(d.Replace("\\", "/"), tp, SearchOption.AllDirectories);
        }

        return true;
    }

    // 打包lua目录
    public bool packluadir(string res, bool useJit = true )
    {
        string fixluadir = "Assets/Res/lua/";
        // 删除目录
        AssetDatabase.DeleteAsset(fixluadir);

        var files = Directory.GetFiles(res, "*.lua", SearchOption.AllDirectories);
        var dests = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i].Replace("\\", "/");
            string dest = "Assets/Res/" + file;
            string destName = dest.Substring(0, dest.Length - 3) + "bytes";

            string destDir = Path.GetDirectoryName(destName);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            files[i] = file;
            dests[i] = destName;
            XDebug.Log(file + ":" + destName);
        }

        if ( useJit )
        {
            compileLuaJit(files, dests);
        }
        else
        {
            compileLua(files, dests);
        }


        AssetDatabase.Refresh();

        packdir(fixluadir, "*.bytes", SearchOption.AllDirectories);

        // 删除目录
        // AssetDatabase.DeleteAsset(fixluadir);
        // AssetDatabase.Refresh();

        return true;
    }

    public AssetBundleManifest packAll()
    {
        if ( !Directory.Exists( _targetDir) )
        {
            Directory.CreateDirectory( _targetDir );
        }
        return BuildPipeline.BuildAssetBundles( _targetDir, BuildAssetBundleOptions.None, _target );
    }


    // 目录打包
    public bool clearBundleName()
    {
        string res = "Assets/Res";
        string tp = "*.*";
        SearchOption searchOption = SearchOption.AllDirectories;
        string[] files = getDirFile(res, tp, searchOption);
        if (files.Length == 0)
        {
            return false;
        }

        // 去掉末尾的/ 创建目录
        // string fn = res.TrimEnd('/') + ".abd";


        // 看有没有子目录判定是那种方式去包装他        
        // List<Object> obs = new List<Object>();
        foreach (var file in files)
        {
            AssetImporter importer = AssetImporter.GetAtPath(file);
            if( importer )
            {
                importer.assetBundleName = string.Empty;
            }

        }

        AssetDatabase.RemoveUnusedAssetBundleNames();

        return true;
    }
}
