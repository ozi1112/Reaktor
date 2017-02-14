using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

static class CTime
{
    static int m_fStartTicks = -1;
    public static void Initialize()
    {
        m_fStartTicks = Environment.TickCount;
    }

    public static int GetTimeMs()
    {
        return Environment.TickCount - m_fStartTicks;
    }



    public static float GetTimeSec()
    {
        return (Environment.TickCount - m_fStartTicks) / 1000.0f;
    }
}

/// <summary>
/// enum to select Log layer
/// </summary>
public enum ELayer
{
    Model,
    View,
    Presenter,
    Default
}

/// <summary>
/// Loger class
/// </summary>
static class CLoger
{
    static bool 
        MODEL = true,
        VIEW = true,
        PRESENTER = true,
        EXCEPTIONS = true; // disable on release
    static bool IsInitialized = false;

    /// <summary>
    /// Initialize - set startup time. Second initialization - do nothing.
    /// </summary>
    public static void Initialize()
    {
        if (!IsInitialized)
        {
            CTime.Initialize();
            IsInitialized = true;
        }
    }

    static void LOG(object print = null, string pre = "", ELayer a_eLayer = ELayer.Default, int a_iFrame = 2)
    {
        if ((MODEL && (a_eLayer == ELayer.Model)) || ( VIEW && (a_eLayer == ELayer.View) ) || (PRESENTER && (a_eLayer == ELayer.Presenter)))
        {
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            string output = CTime.GetTimeSec() + "~:~" + a_eLayer.ToString() + "~:~" + pre + t.GetFrame(a_iFrame).GetMethod().ReflectedType.FullName + "." +
                t.GetFrame(a_iFrame).GetMethod().Name + "~:~" + print;
            Debug.Log(output);
        }
    }

    static void LOG(string pre = "", ELayer a_eLayer = ELayer.Default)
    {
        LOG(null, pre, a_eLayer, 3);
    }

    /// <summary>
    /// Information log
    /// </summary>
    /// <param name="a_eLayer">Layer enum</param>
    /// <param name="print">Object to print</param>
    public static void LOG_INF(ELayer a_eLayer, object print)
    {
        LOG(print, "INF~:~", a_eLayer);
    }

    /// <summary>
    /// Error log
    /// </summary>
    /// <param name="a_eLayer">Layer enum</param>
    /// <param name="print">Object to print</param>
    public static void LOG_ERR(ELayer a_eLayer, object print)
    {
        LOG(print, "ERR~:~", a_eLayer);
    }

    /// <summary>
    /// Warning log
    /// </summary>
    /// <param name="a_eLayer">Layer enum</param>
    /// <param name="print">Object to print</param>
    public static void LOG_WRN(ELayer a_eLayer, object print)
    {
        LOG(print, "WRN~:~", a_eLayer);
    }

    /// <summary>
    /// Logic flow log - print only 'class'.'method' name.
    /// </summary>
    /// <param name="a_eLayer">Layer enum</param>
    /// <param name="print">Object to print</param>
    public static void LOG_FLOW(ELayer a_eLayer)
    {
        LOG("FLW~:~", a_eLayer);
    }

    public static void ThrowException(ELayer a_eLayer, string message)
    {
        if(EXCEPTIONS)
        {
            StackTrace t = new StackTrace();
            StackFrame[] frames = t.GetFrames();
            string strCallStack = "Call stack\n";
            for(int i = frames.Length-1, it = 0; i >= 0; --i, ++it)
            {
                StackFrame frame = frames[i];
                strCallStack += "[" + it + "]. " + frame.GetMethod().ReflectedType.FullName + "." +
                frame.GetMethod().Name + "\n";
            }

            Debug.Log(strCallStack);

            throw new Exception("Layer: " + a_eLayer.ToString() + " " +t.GetFrame(1).GetMethod().ReflectedType.FullName + "." +
                t.GetFrame(1).GetMethod().Name + " Msg: " + message);
        }
    }
}
