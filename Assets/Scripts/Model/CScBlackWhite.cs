using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

class CScBlackWhite : IScenario
{
    public string Description { get; set; }
    CBlackWhitePresenter m_presenter;
    Timer timer;
    bool m_bCondition;
    public bool Condition
    {
        get { return m_bCondition; }
        set
        {
            if (m_presenter != null)
            {
                m_presenter.ConditionChange(value);
            }
            m_bCondition = value;
        }
    }

    public CScBlackWhite(CBlackWhitePresenter a_presenter)
    {
        m_presenter = a_presenter;
        Description = "Press when the screen goes white";
        Condition = false;
    }

    public bool CheckCondition()
    {
        return m_bCondition;
    }

    public virtual void NextScreen()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        if (CScManager.Instance.eGameState == EGameState.Paused)
        {
            CScManager.Instance.eGameState = EGameState.Started;
            StartTimer();
        }
        else
        {
            CLoger.LOG_ERR(ELayer.Model, "Scenario stopped. Expected paused scenario.");
            CLoger.ThrowException(ELayer.Model, "Scenario stopped. Expected paused scenario.");
        }
    }

    public virtual void StartScenario()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_presenter.SetActive(true);
        StartTimer();
    }

    protected virtual void StartTimer()
    {
        if (timer != null)
            if (timer.Enabled)
            {
                timer.Enabled = false;
                CLoger.ThrowException(ELayer.Model, "Incorrect flow. Create second timer.");
            }
        
        Condition = false;
        timer = new Timer(Random.Range(3000, 6000));
        timer.Elapsed += (a, e) => {
            Condition = !Condition;
            CLoger.LOG_INF(ELayer.Model, "Timer TRIGGER " + Condition);
        };
        timer.AutoReset = false;
        timer.Enabled = true;
    }

    protected void StopTimer()
    {
        CLoger.LOG_FLOW(ELayer.Model);
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

    ~CScBlackWhite()
    {
        CLoger.LOG_FLOW(ELayer.Model);
    }
}

