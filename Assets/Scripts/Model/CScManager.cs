using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class CScManager
{
    public EGameState eGameState;
    static CScManager instance;
    CRandomNorep m_random;
    List<IScenario> m_scenarios;
    IScenario m_currentSenario;
    int m_iCurrentRoll;


    int m_iMaxCheck = 5,
        m_iCurrCheck = 0;

    public static CScManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CScManager();
            }
            return instance;
        }
    }

    string m_strDestription;
    string Description
    {
        set
        {
            m_strDestription = value;
            CGameEngine.Instance.m_presenter.Description = value;
        }
    }
    

    CScManager()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_scenarios = new List<IScenario>();
        init();
    }

    void init()
    {
        eGameState = EGameState.Stopped;
        m_iCurrentRoll = 0;
    }

    public void RegisterScenario(IScenario scenario)
    {
        CLoger.LOG_INF(ELayer.Model, scenario.Description);
        m_scenarios.Add(scenario);
        m_random = new CRandomNorep(new CRange(0, m_scenarios.Count - 1));
        scenario.SetActive(false);
    }

    public void NextScreen()
    {
        if (eGameState != EGameState.Stopped)
        {
            m_currentSenario.NextScreen();
        }
        else
        {
            CLoger.LOG_WRN(ELayer.Model ,"Ignored - game stopped");
        }
    }

    public void RollScenario()
    {
        CLoger.LOG_INF(ELayer.Model, "!!!ROLL " + m_iCurrentRoll + " " + m_scenarios.Count);
        if ((m_iCurrentRoll < m_scenarios.Count))
        {
            m_iCurrentRoll++;
            m_currentSenario = m_scenarios[m_random.Roll()];
            CLoger.LOG_INF(ELayer.Model, "Rolled: " + m_currentSenario.Description);
            Description = m_currentSenario.Description;
        }
        else
        {
            CGameEngine.Instance.GameEnd();
        }
    }

    public void PauseScenario()
    {
        eGameState = EGameState.Paused;
        m_currentSenario.PauseScenario();
    }

    public void StartScenario()
    {
        if (eGameState == EGameState.Stopped)
        {
            m_iCurrCheck = 0; // reset counter
            eGameState = EGameState.Started;
            m_currentSenario.StartScenario();
        }
        else
        {
            CLoger.LOG_ERR( ELayer.Model ,"Ignored stop prevoius first");
            CLoger.ThrowException(ELayer.Model, "Ignored stop prevoius first");
        }
    }

    public bool CheckCondition() // Engine protects call if not stated
    {
        CheckScenarioEnd();
        return m_currentSenario.CheckCondition();
    }

    void CheckScenarioEnd()
    {
        if (m_iCurrCheck <= m_iMaxCheck)
        {
            if (m_currentSenario.CheckCondition())
            {
                m_iCurrCheck++;
            }
        }
        else
        {
            StopScenario();
            CGameEngine.Instance.m_presenter.ScenarioEnd();
        }
    }

    public void StopScenario()
    {
        eGameState = EGameState.Stopped;
        m_currentSenario.StopScenario();
    }

    public void Destroy()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        foreach(IScenario scenario in m_scenarios)
        {
            scenario.Destroy();
        }
        m_currentSenario = null;
        m_scenarios.Clear();
        instance = null;
    }
}

