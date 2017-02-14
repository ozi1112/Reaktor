using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

enum EGameDifficulty
{
    Easy,
    Medium,
    Hard
}
/// <summary>
/// Question type
/// </summary>
public static class EQuestionType
{
    /// <summary>
    /// 3 element question (Example a1 + a2 = a3)
    /// </summary>
    public enum elem3
    {
        addition = '+',
        subtraction = '-',
        multiplication = '*',
        iAddition = 0,
        iSubtraction = 1,
        iMultiplication = 2
    }

    /// <summary>
    /// 2 element question (Example a1 < a2)
    /// </summary>
    public enum elem2
    {
        lower = '<',
        bigger = '>'
    }
}

/// <summary>
/// Question
/// </summary>
class CQuestion
{
    const int RESULT_EMPTY_VAL = 0xFFFFFFF;
    int m_iElement1;
    int m_iElement2;
    int m_iResult = 0xFFFFFFF;
    bool m_IsTrue;
    int m_eQType;
    static Random m_random;

    /// <summary>
    /// Check question correct
    /// </summary>
    public bool IsTrue
    {
        get { return m_IsTrue; }
        private set { m_IsTrue = value; }
    }

    public CQuestion(int a_iElement1, EQuestionType.elem3 a_eQType, int a_iElement2, bool m_isTrue)
    {
        m_iElement1 = a_iElement1;
        m_eQType = (int)a_eQType;
        m_iElement2 = a_iElement2;
        IsTrue = m_isTrue;
        m_iResult = generateSolve();

        CLoger.LOG_INF(ELayer.Model, m_iElement1 + " " + a_eQType.ToString() + " " + m_iElement2 + "=" + m_iResult + " is: " + IsTrue);
        
    }

    /// <summary>
    /// Generate question (WARN: division not supported yet)
    /// </summary>
    public static CQuestion GenerateQuestion(bool a_isTrue, EQuestionType.elem3 a_eQType, EGameDifficulty eDiff)
    {
        if (m_random == null)
            m_random = new Random();
        int iElem1 = 0;
        int iElem2 = 0;
        
        switch (eDiff)
        {
            case EGameDifficulty.Easy:
                iElem1 = m_random.Next(1, 10);
                iElem2 = m_random.Next(1, 10);
                break;
            case EGameDifficulty.Medium:
                iElem1 = m_random.Next(5, 30);
                iElem2 = m_random.Next(5, 30);
                break;
            case EGameDifficulty.Hard:
                iElem1 = m_random.Next(15, 99);
                iElem2 = m_random.Next(15, 99);
                break;
        }

        return new CQuestion(iElem1, a_eQType, iElem2, a_isTrue);
    }

    int generateSolve()
    {
        //Calculate true
        switch (m_eQType)
        {
            case (int)EQuestionType.elem3.addition:
                m_iResult = m_iElement1 + m_iElement2;
                break;
            case (int)EQuestionType.elem3.subtraction:
                m_iResult = m_iElement1 - m_iElement2;
                break;
            case (int)EQuestionType.elem3.multiplication:
                m_iResult = m_iElement1 * m_iElement2;
                break;
            default:
                CLoger.LOG_ERR(ELayer.Model, "Wrong operation");
                break;
        }

        //Generate false
        if (!IsTrue)
        {
            int diffToTrue = Math.Abs(m_iResult) / 10;
            diffToTrue = m_random.Next(-diffToTrue, diffToTrue);
            if (diffToTrue == 0)
                diffToTrue = 1;

            m_iResult += diffToTrue;
            CLoger.LOG_INF(ELayer.Model, "DIFF: " + diffToTrue);
        }

        return m_iResult;
    }

    public CQuestion(int a_iElement1, EQuestionType.elem2 a_eQType, int a_iElement2)
    {
        m_iElement1 = a_iElement1;
        m_eQType = (int)a_eQType;
        m_iElement2 = a_iElement2;
        m_IsTrue = checkIsTrue();
    }

    /// <summary>
    /// Checks question kind
    /// </summary>
    /// <returns>True if elem3</returns>
    bool isElem3()
    {
        return m_iResult != RESULT_EMPTY_VAL;
    }

    bool checkIsTrue()
    {
        bool bRetVal = false;
        switch (m_eQType)
        {
            case (int)EQuestionType.elem3.addition:
                {
                    bRetVal = ((m_iElement1 + m_iElement2) == m_iResult);
                    break;
                }
            case (int)EQuestionType.elem3.subtraction:
                {
                    bRetVal = ((m_iElement1 - m_iElement2) == m_iResult);
                    break;
                }
            case (int)EQuestionType.elem3.multiplication:
                {
                    bRetVal = ((m_iElement1 * m_iElement2) == m_iResult);
                    break;
                }
            case (int)EQuestionType.elem2.bigger:
                {
                    bRetVal = (m_iElement1 > m_iElement2);
                    break;
                }
            case (int)EQuestionType.elem2.lower:
                {
                    bRetVal = (m_iElement1 < m_iElement2);
                    break;
                }
            default:
                {
                    CLoger.LOG_INF(ELayer.Model, "Wrong question type");
                    break;
                }
        }
        return bRetVal;
    }

    public string toString()
    {
        string strRetVal = string.Empty;

        if (isElem3())
        {
            strRetVal = m_iElement1 + " " + (char)m_eQType + " " + m_iElement2 + " = " + m_iResult;
        }
        else
        {
            strRetVal = m_iElement1 + " " + (char)m_eQType + " " + m_iElement2;
        }

        return strRetVal;
    }
}

class CQuestionManager
{
    List<CQuestion> m_lQuestions;
    CQuestion current;

    public CQuestionManager(uint a_iNumOfQuestions = 10)
    {
        generateNewList(a_iNumOfQuestions);
    }

    void generateNewList(uint a_iNumOfQuestions)
    {
        m_lQuestions = new List<CQuestion>();
        int numOfTrue = (int)(a_iNumOfQuestions * 0.5f);
        if (numOfTrue == 0)
            numOfTrue = 1;

        for (uint i = 0; i < a_iNumOfQuestions; ++i)
        {
            if (numOfTrue > 0)
            {
                numOfTrue--;
                CQuestion c = CQuestion.GenerateQuestion(true, EQuestionType.elem3.addition, EGameDifficulty.Easy);
                m_lQuestions.Add(c);

            }
            else
            {
                CQuestion c = CQuestion.GenerateQuestion(false, EQuestionType.elem3.addition, EGameDifficulty.Easy);
                m_lQuestions.Add(c);
            }
        }
    }

    public CQuestion getNext()
    {
        Random r = new Random();

        if (current != null)
        {
            m_lQuestions.Remove(current);
        }
        if (m_lQuestions.Count == 0)
        {
            generateNewList(10);
        }

        current = m_lQuestions[r.Next(0, m_lQuestions.Count - 1)];

        return current;
    }


}




class CScMath : IScenario
{
    CMathPresenter m_presenter;
    CQuestionManager m_qmanager;

    bool m_bCondition;
    Timer timer;
    public bool Condition
    {
        get { return m_bCondition; }
        set
        {
            CLoger.LOG_INF(ELayer.Model, value);
            m_bCondition = value;
        }
    }

    public string Description { get; set; }

    public CScMath(CMathPresenter a_presenter)
    {
        Description = "Press if True";
        m_presenter = a_presenter;
        m_qmanager = new CQuestionManager();
        timer = new Timer(3000);
        timer.Elapsed += (a, e) => {
            CLoger.LOG_INF(ELayer.Model, "Timer TRIGGER");
            NextQuestion();
        };
    }

    public void NextScreen()
    {
        CScManager.Instance.eGameState = EGameState.Started;
        StartTimer();
    }

    protected void StartTimer()
    {
        if (timer != null)
            if (timer.Enabled)
                CLoger.ThrowException(ELayer.Model, "Incorrect flow. Create second timer.");
        CLoger.LOG_FLOW(ELayer.Model);
        NextQuestion();
        timer.Enabled = true;
    }

    private void NextQuestion()
    {
        CQuestion q = m_qmanager.getNext();
        Condition = q.IsTrue;
        m_presenter.SetText(q.toString());
        CLoger.LOG_INF(ELayer.Model, q.toString() + " is: " +Condition);
    }
    

    public bool CheckCondition()
    {
        return m_bCondition;
    }
    

    public virtual void StartScenario()
    {
        m_presenter.SetActive(true);
        StartTimer();
    }
    

    protected void StopTimer()
    {
        if (timer != null)
        {
            CLoger.LOG_INF(ELayer.Model, "STOP");
            timer.Enabled = false;
        }
        else
        {
            CLoger.LOG_ERR(ELayer.Model, "NULL timer");
            //CLoger.ThrowException(ELayer.Model, "Incorrect flow. NULL timer.");
        }
    }

    public void StopScenario()
    {
        StopTimer();
        SetActive(false);
    }

    public void PauseScenario()
    {
        StopTimer();
    }

    public void SetActive(bool a_IsActive)
    {
        m_presenter.SetActive(a_IsActive);
    }

    public void Destroy()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        if (null != timer)
            if (timer.Enabled)
                StopTimer();
    }

    ~CScMath()
    {
        CLoger.LOG_FLOW(ELayer.Model);
    }
}
    

