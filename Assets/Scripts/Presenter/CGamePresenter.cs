using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Score / Game State Presenter
/// </summary>
class CGamePresenter
{
    IGameView m_view;
    CGameEngine m_engine;
    CScManager m_manager;

    public string Description
    {
        set
        {
            m_view.Description = value;
        }
    }
    
    public int[] ScoreTable
    {
        set
        {
            m_view.ScoreTable = value;
        }
    }

    public CGamePresenter(IGameView a_view)
    {
        m_engine = CGameEngine.Instance;
        m_manager = CScManager.Instance;
        m_view = a_view;
        m_engine.SetPresenter(this);
    }

    public void StartGame()
    {
        m_engine.StartGame();
    }

    public void RollScenario()
    {
        m_manager.RollScenario();
    }

    public void StartScenario()
    {
        m_manager.StartScenario();
    }

    public void NextScreen()
    {
        m_manager.NextScreen();
    }

    public void Configure(byte a_numOfPlayers)
    {
        m_engine.Configure(a_numOfPlayers);
    }

    public EResult TriggerPlayer(EPlayer a_id)
    {
        return m_engine.TriggerPlayer(a_id);
    }
    
    public void ScenarioEnd()
    {
        CLoger.LOG_FLOW(ELayer.Presenter);
        m_view.ScenarioEnd();
    }
    
    public void GameEnd()
    {
        m_view.GameEnd();
    }

    public void Destroy()
    {
        m_engine.Destroy();
    }
}


