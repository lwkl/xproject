using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Diagnostics;


[System.Serializable]
public class XPackItem
{
    public enum XPackType { File, DirFiles, Dir }
    public XPackType type;
    public string path;
    public string searchPattern = "*.*";
    public SearchOption searchOption;
}


// 记录XPackConf
[System.Serializable]
public class XPackConf : ScriptableObject
{
    // 版本号
    [SerializeField]
    private string _version = "1.0";
    public static string version { get { return instance._version; } }

    // 游戏名称
    [SerializeField]
    private string _exe = "xproject";
    public static string exe { get { return instance._exe; } }

    // 游戏发布目录
    [SerializeField]
    private string _publishDir = "publish/";
    public static string publishDir { get { return instance._publishDir; } }

    // 游戏WEB发布目录
    [SerializeField]
    private string _webPublishUrl = "http://192.168.50.142/publish/";
    public static string webPublishUrl { get { return instance._webPublishUrl; } }

    // 游戏WEB发布目录
    [SerializeField]
    private string _remoteLogURL = "http://192.168.50.142/log/";
    public static string remoteLogURL { get { return instance._remoteLogURL; } }

    [SerializeField]
    private bool _isRemoteLog = false;
    public static bool isRemoteLog { get { return instance._isRemoteLog; } }

    // 是否使用jit
    [SerializeField]
    private bool _luaJit = true;
    public static bool luaJit { get { return instance._luaJit; } }

    // debugBuild
    [SerializeField]
    private bool _debugBuild = true;
    public static bool debugBuild { get { return instance._debugBuild; } }

    // 自动开启
    [SerializeField]
    private bool _autoRunPlayer = false;
    public static bool autoRunPlayer { get { return instance._autoRunPlayer; } }

    [SerializeField]
    private List<XPackItem> _packItems = new List<XPackItem>();
    public static List<XPackItem> packItems { get { return instance._packItems; } }



    private static XPackConf _instance;
    private static XPackConf instance
    {
        get
        {
            _instance = AssetDatabase.LoadAssetAtPath<XPackConf>("Assets/Plugins/XPack/Editor/conf.asset");
            if ( _instance == null )
            {
                XDebug.LogError("创建conf.asset失败重新创建");
                _instance = CreateInstance<XPackConf>();
                AssetDatabase.CreateAsset(_instance, "Assets/Plugins/XPack/Editor/conf.asset");
            }
            return _instance;
        }
    }

    // 获取build
    public static BuildOptions options
    {
        get
        {
            if ( debugBuild )
            {
                BuildOptions op = BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler | BuildOptions.Development;
                if ( autoRunPlayer )
                {
                    return op | BuildOptions.AutoRunPlayer;
                }
                return op;
            }
            else
            {
                return BuildOptions.None;
            }
        }
    }

    // 各种配置文件
    [MenuItem("Assets/发布设定")]
    public static void open()
    {
        Selection.activeObject = instance;
    }

    // 获取当前目录
    static string getCurPath()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }


    [MenuItem("Assets/发布/打包/打包当前平台lua")]
    static void menuPackLua()
    {
        XDebug.Log("打包lua结束");
        XPack pack = new XPack();
        pack.setInfo();
        XPackInfo info = new XPackInfo();
        if (!info.loadFile(pack.targetDir + "Assets/version.json") )
        {
            XDebug.LogError("打包lua失败，应当先打包");
            return;
        }
        XDebug.Log("打包lua");
        pack.packluadir("lua/", luaJit );
        info.saveFile( pack.targetDir + "Assets/version.json");
        XDebug.Log("打包lua结束");
    }

    static void pack_all(BuildTarget buildTarget )
    {
        XPack pack = new XPack();
        pack.setInfo( buildTarget );
        try
        {
            System.IO.Directory.Delete(pack.targetDir, true);
        }
        catch (System.Exception e)
        {
            XDebug.LogError(e.Message);
        }

        XDebug.Log("开始打包");

        foreach (var item in instance._packItems )
        {
            if (item.type == XPackItem.XPackType.File)
            {
                string path = item.path;
                pack.pack( path );
            }
            else if (item.type == XPackItem.XPackType.DirFiles)
            {
                pack.packdirfiles(item.path, item.searchPattern, item.searchOption);
            }
            else if (item.type == XPackItem.XPackType.Dir)
            {
                pack.packdir(item.path, item.searchPattern, item.searchOption);
            }
        }

        pack.packluadir("lua/", luaJit );

        AssetBundleManifest manifest = pack.packAll();
        XPackInfo info = new XPackInfo();

        info.parseManifest(manifest, pack.targetDir );
        info._version = version;
        info._firstVersion = version;
        info.saveFile(pack.targetDir + "Assets/version.json");
        
        XVer ver = new XVer { ver = version, ab = pack.getWebDir( webPublishUrl), remoteLogUrl = remoteLogURL , isRemoteLog = isRemoteLog };
        ver.saveFile( pack.targetDir + "Assets/ver.json");

       

        XDebug.Log("打包结束");
    }

    [MenuItem("Assets/发布/打包/打包当前平台")]
    static void menuPackCur()
    {
        pack_all( XPack.getCurTarget() );
    }

    [MenuItem("Assets/发布/打包/打包安卓")]
    static void menuPackAndroid()
    {
        pack_all(BuildTarget.Android);
    }

    [MenuItem("Assets/发布/打包/打包IOS")]
    static void menuPackIOS()
    {
        pack_all( BuildTarget.iOS );
    }

    [MenuItem("Assets/发布/打包/打包win")]
    static void menuPackWin()
    {
        pack_all(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Assets/发布/打包/打包win64")]
    static void menuPackWin64()
    {
        pack_all(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Assets/发布/打包/打包mac")]
    static void menuPackMac()
    {
        pack_all(BuildTarget.StandaloneOSXUniversal);
    }
    [MenuItem("Assets/发布/打包/打包mac32")]
    static void menuPackMac32()
    {
        pack_all(BuildTarget.StandaloneOSXIntel);
    }
    [MenuItem("Assets/发布/打包/打包mac64")]
    static void menuPackMac64()
    {
        pack_all(BuildTarget.StandaloneOSXIntel64);
    }

    [MenuItem("Assets/发布/打包/打包linux")]
    static void menuPackLinux()
    {
        pack_all(BuildTarget.StandaloneLinuxUniversal);
    }

    [MenuItem("Assets/发布/打包/打包linux32")]
    static void menuPackLinux32()
    {
        pack_all(BuildTarget.StandaloneLinux);
    }

    [MenuItem("Assets/发布/打包/打包linux64")]
    static void menuPackLinux64()
    {
        pack_all(BuildTarget.StandaloneLinux64);
    }


    [MenuItem("Assets/发布/打包/打包所有平台")]
    static void menuPackAll()
    {
        menuPackAndroid();
        menuPackIOS();
        menuPackWin();
        menuPackWin64();
        menuPackMac();
        menuPackLinux();
    }



    static void delStreamingAssets()
    {
        try
        {
            if (Directory.Exists("Assets/StreamingAssets/Assets"))
            {
                Directory.Delete("Assets/StreamingAssets/Assets", true);
            }

            string[] delfiles = new string[]
            {
                "Assets/StreamingAssets/android",
                "Assets/StreamingAssets/android.manifest",
                "Assets/StreamingAssets/win",
                "Assets/StreamingAssets/win.manifest",
                "Assets/StreamingAssets/win64",
                "Assets/StreamingAssets/win64.manifest",
                "Assets/StreamingAssets/mac",
                "Assets/StreamingAssets/mac.manifest",
                "Assets/StreamingAssets/mac64",
                "Assets/StreamingAssets/mac64.manifest",
                "Assets/StreamingAssets/mac32",
                "Assets/StreamingAssets/mac32.manifest",
                "Assets/StreamingAssets/linux",
                "Assets/StreamingAssets/linux.manifest",
                "Assets/StreamingAssets/linux64",
                "Assets/StreamingAssets/linux64.manifest",
                "Assets/StreamingAssets/linux32",
                "Assets/StreamingAssets/linux32.manifest",
                "Assets/StreamingAssets/ios",
                "Assets/StreamingAssets/ios.manifest",
            };

            for (int i = 0; i < delfiles.Length; i++)
            {
                if (File.Exists(delfiles[i]))
                {
                    File.Delete(delfiles[i]);
                }
            }

        }
        catch (System.Exception e)
        {
            XDebug.LogError(e.Message);
        }
    }

    [MenuItem("Assets/发布/移动/当前平台")]
    static void menuMoveCur()
    {
        move_all( XPack.getCurTarget() );
    }

    [MenuItem("Assets/发布/移动/安卓")]
    static void menuMoveAndroid()
    {
        move_all(BuildTarget.Android);

    }

    [MenuItem("Assets/发布/移动/ios")]
    static void menuMoveIOS()
    {
        move_all(BuildTarget.iOS);
    }

    [MenuItem("Assets/发布/移动/win")]
    static void menuMoveWin()
    {
        move_all(BuildTarget.StandaloneWindows);
    }


    [MenuItem("Assets/发布/移动/win64")]
    static void menuMoveWin64()
    {
        move_all(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Assets/发布/移动/mac")]
    static void menuMoveMac()
    {
        move_all(BuildTarget.StandaloneOSXUniversal);
    }

    [MenuItem("Assets/发布/移动/mac32")]
    static void menuMoveMac32()
    {
        move_all(BuildTarget.StandaloneOSXIntel);
    }

    [MenuItem("Assets/发布/移动/mac64")]
    static void menuMoveMac64()
    {
        move_all(BuildTarget.StandaloneOSXIntel64);
    }

    [MenuItem("Assets/发布/移动/linux")]
    static void menuMoveLinux()
    {
        move_all(BuildTarget.StandaloneLinuxUniversal);
    }

    [MenuItem("Assets/发布/移动/linux32")]
    static void menuMoveLinux32()
    {
        move_all(BuildTarget.StandaloneLinux);
    }

    [MenuItem("Assets/发布/移动/linux64")]
    static void menuMoveLinux64()
    {
        move_all(BuildTarget.StandaloneLinux64);
    }

    static void move_all( BuildTarget buildTarget )
    {
        delStreamingAssets();
        XPack pack = new XPack();
        pack.setInfo( buildTarget );
        XUtil.copyDirectory(pack.targetDir, "Assets/StreamingAssets", true);
        AssetDatabase.Refresh();

        // publish目录 
        string versionDir = pack.getVersionDir(publishDir, "v" + version);

        // 清除所有东西
        XUtil.clearDirectory( versionDir );

        XUtil.copyDirectory(pack.targetDir, versionDir, true);

        
        build(pack, versionDir);

        // 拷贝单个文件
        XDebug.Log(versionDir + "assets/ver.json" + "=>" + pack.getPublishDir(publishDir) + "ver.json");
        XUtil.copyFile( versionDir + "assets/ver.json", pack.getPublishDir(publishDir) + "ver.json");

    }
    static string getTargetName( XPack pack, string versionDir )
    {
        if ( !Directory.Exists(versionDir) )
        {
            Directory.CreateDirectory(versionDir);
        }
        string builddir;

        if ( pack.target == BuildTarget.Android )
        {
            builddir = versionDir + "client/" + exe + ".apk";
            if ( File.Exists(builddir) )
            {
                File.Delete(builddir);
            }
        }
        else if (pack.target == BuildTarget.iOS)
        {
            // 不允许有/斜杠，！！，否则拷贝不过去，汗~
            builddir = versionDir + "client/" + exe + "_ios";

            if ( Directory.Exists(builddir) )
            {
                Directory.Delete(builddir, true);
            }
        }
        else if ( pack.target == BuildTarget.StandaloneWindows )
        {
            builddir = versionDir + "client/" +exe + ".exe";
            if ( File.Exists(builddir) )
            {
                File.Delete(builddir);
            }
        }
        else
        {
            builddir = versionDir + "client/" + exe + ".exe";
            if (File.Exists(builddir))
            {
                File.Delete(builddir);
            }

        }

        return builddir;

    }

    // 发布平台
    static void build(XPack pack, string versionDir )
    {
        List<string> levels = new List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            levels.Add(EditorBuildSettings.scenes[i].path);
        }

        string builddir = getTargetName(pack, versionDir);

        BuildPipeline.BuildPlayer( levels.ToArray(), builddir, pack.target, options );

        string output = Path.GetDirectoryName( Path.GetFullPath(builddir) );
        XDebug.Log( output );
        // 打开目录
        openOutputDir( output );

    }

    [MenuItem("Assets/一键发布/当前平台")]
    static void menuAutoCur()
    {
        menuPackCur();
        menuMoveCur();
    }

    [MenuItem("Assets/一键发布/安卓")]
    static void menuAutoAndroid()
    {
        menuPackAndroid();
        menuMoveAndroid();
    }

    [MenuItem("Assets/一键发布/ios")]
    static void menuAutoIOS()
    {
        menuPackIOS();
        menuMoveIOS();
    }

    [MenuItem("Assets/一键发布/win")]
    static void menuAutoWin()
    {
        menuPackWin();
        menuMoveWin();
    }

    [MenuItem("Assets/一键发布/win64")]
    static void menuAutoWin64()
    {
        menuPackWin64();
        menuMoveWin64();
    }
    [MenuItem("Assets/一键发布/mac")]
    static void menuAutoMac()
    {
        menuPackMac();
        menuMoveMac();
    }

    [MenuItem("Assets/一键发布/mac32")]
    static void menuAutoMac32()
    {
        menuPackMac32();
        menuMoveMac32();
    }
    [MenuItem("Assets/一键发布/mac64")]
    static void menuAutoMac64()
    {
        menuPackMac64();
        menuMoveMac64();
    }

    [MenuItem("Assets/一键发布/linux")]
    static void menuAutoLinux()
    {
        menuPackLinux();
        menuMoveLinux();
    }

    [MenuItem("Assets/一键发布/linux32")]
    static void menuAutoLinux32()
    {
        menuPackLinux32();
        menuMoveLinux32();
    }
    [MenuItem("Assets/一键发布/linux64")]
    static void menuAutoLinux64()
    {
        menuPackLinux64();
        menuMoveLinux64();
    }

    // 打包好打开目录
    protected static void openOutputDir(string path)
    {
#if UNITY_EDITOR_WIN
        Process ps = ProcessHelper.StartProcess("explorer", path, "");
#else
        Process ps = ProcessHelper.StartProcess("open", path );
#endif
        ps.WaitForExit();
    }

    [MenuItem("Assets/清理名称")]
    public static void clearBundleName()
    {
        XPack pack = new XPack();
        pack.clearBundleName();
    }
}
