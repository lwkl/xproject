using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
// 一些通用的操作类，比如文件拷贝，创建等操作
public class XUtil
{
    // 获取文件的大小
    public static int getFileSize(string filePath)
    {
        //创建一个文件对象 
        FileInfo fi = new FileInfo(filePath);
        // 获取文件的大小 
        return (int)fi.Length;
    }

    // 文件拷贝
    public static bool copyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if ( !dir.Exists )
        {
            XDebug.LogError( "源目录不存在拷贝失败" + sourceDirName);
            return false;

        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                copyDirectory(subdir.FullName, temppath, copySubDirs);
            }
        }
        return true;
    }

    public static bool deleteFile( string filename )
    {
        try
        {
            File.Delete( filename );
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    #region 删除指定目录
    /// <summary> 
    /// 删除指定目录及其所有子目录 
    /// </summary> 
    /// <param name="directoryPath">指定目录的绝对路径</param> 
    public static bool deleteDirectory(string directoryPath )
    {
        if ( isExistDirectory(directoryPath))
        {
            string[] files = Directory.GetFiles(directoryPath);
            string[] dirs = Directory.GetDirectories(directoryPath);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                deleteDirectory(dir);
            }
            Directory.Delete(directoryPath, false);
            return true;
        }

        return false;
    }
    #endregion


    public static bool isExistFile(string filePath)
    {
        return File.Exists( filePath );
    }

    public static bool isExistDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }



    public static void clearDirectory(string directoryPath)
    {
        if ( isExistDirectory(directoryPath))
        {
            //删除目录中所有的文件 
            string[] fileNames = getFileNames(directoryPath);
            for (int i = 0; i < fileNames.Length; i++)
            {
                deleteFile(fileNames[i]);
            }
            //删除目录中所有的子目录 
            string[] directoryNames = getDirectories(directoryPath);
            for (int i = 0; i < directoryNames.Length; i++)
            {
                deleteDirectory(directoryNames[i]);
            }
        }
        else
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static string[] getFileNames(string directoryPath)
    {
        //如果目录不存在，则抛出异常 
        if (!isExistDirectory(directoryPath))
        {
            return new string[0];
        }
        //获取文件列表 
        return Directory.GetFiles(directoryPath);
    }

    /// <summary> 
    /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法. 
    /// </summary> 
    /// <param name="directoryPath">指定目录的绝对路径</param>         
    public static string[] getDirectories(string directoryPath)
    {
        try
        {
            return Directory.GetDirectories(directoryPath);
        }
        catch (IOException)
        {
            return new string[0];
        }
    }


    // 获取url路径
    public static string getUrl(string url, string filename )
    {
        if( ! url.StartsWith("http://"))
        {
            url = "http://" + url;
        }
        return url.TrimEnd('/') + '/' + filename;
    }

    // 格式化assetName
    public static string formatAssetName( string assetName, bool isDir = false )
    {
        assetName = assetName.Replace("\\", "/");
        assetName = assetName.TrimEnd('/');
        assetName = assetName.ToLower();
        if ( isDir )
        {
            assetName += "/";
        }
        return assetName;
    }
    


    public static bool writeFileAllBytes( string filename, byte[] bytes)
    {
        try
        {
            assureDirectory( filename );
            File.WriteAllBytes(filename, bytes);
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }

    public static string getDiretoryNameByPath(string filePath)
    {
        //获取文件的名称 
        FileInfo fi = new FileInfo(filePath);
        return fi.DirectoryName;
    }

    public static bool copyFile(string sourceFilePath, string destFilePath)
    {
        try
        {
            assureDirectory(destFilePath);

            File.Copy(sourceFilePath, destFilePath, true);
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }

    public static void createDirectory(string directoryPath)
    {
        //如果目录不存在则创建该目录 
        if ( !isExistDirectory(directoryPath) )
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void assureDirectory( string fileName )
    {
        string directory = XUtil.getDiretoryNameByPath(fileName);
        if (!XUtil.isExistDirectory(directory))
        {
            XUtil.createDirectory(directory);
        }
    }


    public static string getSizeString(uint fileSize)
    {
        if (fileSize < 0)
        {
            return "0B";
        }
        else if (fileSize >= 1024 * 1024 * 1024)
        {
            return string.Format("{0:########0.00} GB", ((System.Double)fileSize) / (1024 * 1024 * 1024));
        }
        else if (fileSize >= 1024 * 1024)
        {
            return string.Format("{0:####0.00} MB", ((System.Double)fileSize) / (1024 * 1024));
        }
        else if (fileSize >= 1024)
        {
            return string.Format("{0:####0.00} KB", ((System.Double)fileSize) / 1024);
        }
        else
        {
            return string.Format("{0} bytes", fileSize);
        }
    }

}
