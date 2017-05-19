using System;
using System.IO;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
using System.Text;

public class XDebug
{

#if UNITY_EDITOR
    static public bool enableLog = true;
#else
	static public bool enableLog = true;
#endif

    static public StringBuilder errorBuffer = new StringBuilder();

    static public void Log(object message)
    {
        if ( enableLog )
        {
            string m = string.Format("[LOG]: {0}", message);
            Debug.Log(m);
        }
    }
    static public void Log(object message, Object context)
    {
        if ( enableLog )
        {
            Debug.Log(string.Format("[LOG]: {0}", message), context);
        }
    }
    static public void LogError(object message)
    {
        string msg = string.Format("[ERROR]: {0}", message);
        
        int len = errorBuffer.Length - 10000;
        if (len > 0)
        {
            errorBuffer.Remove(0, len);
        }
        errorBuffer.AppendLine(msg);

        if( enableLog )
        {
            Debug.LogError(msg);
        }

    }
    static public void LogError(object message, Object context)
    {
        string msg = string.Format("[ERROR]: {0}", message);
        int len = errorBuffer.Length - 10000;
        if (len > 0)
        {
            errorBuffer.Remove(0, len);
        }
        errorBuffer.AppendLine(msg);

        if (enableLog)
        {
            Debug.LogError(msg, context);
        }
    }


    static public void LogWarning(object message)
    {
        if( enableLog )
        {
            string war = string.Format("[WARNING]: {0}", message);
            Debug.LogWarning(war);

        }
    }
    static public void LogWarning(object message, Object context)
    {
        if (enableLog)
        {
            Debug.LogWarning(string.Format("[WARNING]: {0}", message), context);
        }
    }

    public static void LogException(System.Exception exception)
    {
        if (enableLog)
        {
            Debug.LogException(exception);
        }

    }
    public static void LogException(System.Exception exception, Object context)
    {
        if (enableLog)
        {
            Debug.LogException(exception, context);
        }
    }


    public static void DrawLine(Vector3 start, Vector3 end)
    {
        if( enableLog )
        {
            Debug.DrawLine(start, end);
        }

    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if (enableLog)
        {
            Debug.DrawLine(start, end, color);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        if (enableLog)
        {
            Debug.DrawLine(start, end, color, duration);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
    {
        if (enableLog)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
    }


    public static void DrawRay(Vector3 start, Vector3 dir )
    {
        if( enableLog )
        {
            Debug.DrawRay( start, dir );
        }
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        if (enableLog)
        {
            Debug.DrawRay(start, dir, color);
        }
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
    {
        if (enableLog)
        {
            Debug.DrawRay(start, dir, color, duration);
        }
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
    {
        if (enableLog)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }
    }

}
