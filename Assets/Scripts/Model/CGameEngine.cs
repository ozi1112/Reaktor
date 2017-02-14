using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CGameEngine
{
    public CGamePresenter m_presenter;
    public CScManager m_scManager;

    static CGameEngine instance;
    static public CGameEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CGameEngine();
            }
            return instance;
        }
    }

    int[] m_scoreTable;
    bool m_isRunning = false;
    
    CGameEngine()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_scManager = CScManager.Instance;
    }

    /// <summary>
    /// Set presenter
    /// </summary>
    /// <param name="a_presenter">Presenter object</param>
    public void SetPresenter(CGamePresenter a_presenter)
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_presenter = a_presenter;
    }

    /// <summary>
    /// Player trigger (update score). Call StartGame() first.
    /// Pauses scenario.
    /// </summary>
    /// <param name="a_id">Player id</param>
    /// <returns>3-state result</returns>
    public EResult TriggerPlayer(EPlayer a_id)
    {
        EResult eRetVal = EResult.Neutral;
        if(m_isRunning && EGameState.Started == m_scManager.eGameState)
        {
            if(m_scManager.CheckCondition())
            {
                m_scoreTable[(int)a_id] += 1;
                m_presenter.ScoreTable = m_scoreTable;
                eRetVal = EResult.Positive;
            }
            else
            {
                m_scoreTable[(int)a_id] -= 1;
                m_presenter.ScoreTable = m_scoreTable;
                eRetVal = EResult.Negative;
            }
            if(EGameState.Started == m_scManager.eGameState) // Fix scenario End
                m_scManager.PauseScenario();

            CLoger.LOG_INF(ELayer.Model, "Ret: " + eRetVal.ToString());
        }
        else
        {
            CLoger.LOG_INF(ELayer.Model, "Trigger blocked");
        }
        
        return eRetVal;
    }

    /// <summary>
    /// Initialize score table
    /// </summary>
    /// <param name="a_players"></param>
    public void Configure(byte a_players)
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_scoreTable = new int[a_players];
    }
    
    /// <summary>
    /// Notify presenter - scenario end.
    /// </summary>
    public void ScenarioEnd()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_presenter.ScenarioEnd();
    }

    /// <summary>
    /// Notify presenter - game end.
    /// </summary>
    public void GameEnd()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_presenter.GameEnd();
    }

    /// <summary>
    /// Unlocks triggers.
    /// </summary>
    public void StartGame()
    {
        CLoger.LOG_FLOW(ELayer.Model);
        m_isRunning = true;
    }

    public void Destroy()
    {
        m_scManager.Destroy();
        instance = null;
    }
}


