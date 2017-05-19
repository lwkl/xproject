using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XConf
{
    // 强制装载AB，该参数只在编辑器状态有用
    public static bool forceLoadAB = true;
    // 远程更新地址
    public static string publishUrl = "http://192.168.50.142/publish/";
    // 开启更新
    public static bool isUpdateVersion = false;

    // luastart函数
    public static string luastart = "Main";

    // 以下会根据 versionUrl + ver.json 更新
    // 远程日志端口 
    public static string remoteLogUrl = "http://192.168.50.142/log/";
    // 是否发送远程远程日志
    public static bool isRemoteLog = false;

}
