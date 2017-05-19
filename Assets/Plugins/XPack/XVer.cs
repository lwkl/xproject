using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

// 远程装载文件
public class XVer
{
    public string ver;
    public string ab;
    public string remoteLogUrl;
    public bool isRemoteLog = false;


    string readString( Dictionary<string, object> dict, string name )
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

        if ( str == null )
        {
            return "";
        }
        return str.Trim();
    }

    bool readBool(Dictionary<string, object> dict, string name)
    {
        if( dict.ContainsKey( name ) )
        {
            return Convert.ToBoolean(dict[name]);
        }
        return false;
    }

    int readInt(Dictionary<string, object> dict, string name)
    {
        if ( dict.ContainsKey(name) )
        {
            return Convert.ToInt32(dict[name]);
        }
        return 0;
    }

    public bool loadFile( string fileName )
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


    public bool load( string str )
    {
        try
        {
            Dictionary<string, object> mapConf = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
            return read_dict(mapConf);
        }
        catch ( Exception e )
        {
            XDebug.LogError("装载数据错误" + e.Message);
            return false;
        }
    }

    bool read_dict(Dictionary<string, object> dict)
    {
        ver = readString(dict, "ver");
        if ( ver == ""  )
        {
            XDebug.LogError("远程版本文件ver字段错误");
            return false;
        }

        ab = readString(dict, "ab");
        if ( ab == "")
        {
            XDebug.LogError("远程版本文件ab字段错误");
            return false;
        }

        remoteLogUrl = readString(dict, "remoteLogUrl");
        if ( remoteLogUrl == "")
        {
            XDebug.LogError("远程版本文件ab字段错误");
            return false;
        }

        isRemoteLog = readBool(dict, "isRemoteLog");
        return true;
    }

    public Dictionary<string, object> save_dict()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["ver"] = this.ver;

        dict["ab"] = ab;

        dict["remoteLogUrl"] = remoteLogUrl;

        dict["isRemoteLog"] = isRemoteLog;

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
            XUtil.assureDirectory(fileName);
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
