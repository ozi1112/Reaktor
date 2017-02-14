using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Random = UnityEngine.Random;

class CHelper
{
}

/// <summary>
/// Range min / max val container.
/// </summary>
public class CRange
{
    public int m_iMaxVal, m_iMinVal, m_iDifference;

    public CRange(int a_iMinVal, int a_iMaxVal)
    {
        m_iMaxVal = a_iMaxVal;
        m_iMinVal = a_iMinVal;
        m_iDifference = m_iMaxVal - m_iMinVal;
    }
}

/// <summary>
/// Random witout repetitions number generator
/// </summary>
class CRandomNorep
{
    CRange m_Range;
    List<int> iList; 

    public CRandomNorep(CRange a_Range)
    {
        m_Range = a_Range;
        //Initialize
        Reset();
    }

    /// <summary>
    /// Reset
    /// Generate new numbers to random list.
    /// </summary>
    public void Reset()
    {
        int iItemsNum = m_Range.m_iDifference;
        if (iItemsNum >= 0)
        {
            iList = new List<int>(iItemsNum);
            for (int i = m_Range.m_iMinVal; i <= m_Range.m_iMaxVal; i++)
            {
                iList.Add(i);
            }
        }
        else
        {
            CLoger.LOG_ERR(ELayer.Default, "Incorrect range");
        }
    }

    /// <summary>
    /// Roll and remove next number from list. If list empty - autoreset.
    /// </summary>
    /// <returns>Rolled value.</returns>
    public int Roll()
    {
        int iRetVal = 0;
        if (iList.Count == 0)
        {
            Reset();
        }
        iRetVal = GetNextNum();
        CLoger.LOG_INF(ELayer.Default, "Value: " + iRetVal);
        return iRetVal;
    }

    /// <summary>
    /// Roll and remove next number from list.
    /// </summary>
    /// <returns>Rolled value.</returns>
    int GetNextNum()
    {
        int iRandIndex = Random.Range(0, iList.Count);
        int iRetVal = iList[iRandIndex];
        iList.RemoveAt(iRandIndex);
        return iRetVal;
    }
}

/// <summary>
/// Players ids
/// </summary>
enum EPlayer
{
    P1, P2, P3, P4, P_MAX
}

/// <summary>
/// 3-state result.
/// </summary>
enum EResult
{
    Positive, Negative, Neutral
}

/// <summary>
/// Game states
/// </summary>
enum EGameState
{
    Started, Paused, Stopped
}