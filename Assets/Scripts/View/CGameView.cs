using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Player
{
    public Button m_buttons;
    public Text m_txtDescriptions;
    public Text m_txtScores;
}


/// <summary>
/// Buttons / Scoretable view
/// </summary>
class CGameView : MonoBehaviour, IGameView
{
    CGamePresenter m_presenter;

    public List<Player> m_players;

    void Awake()
    {
        CLoger.Initialize();
    }

    void Start()
    {
        m_presenter = new CGamePresenter(this);
        m_presenter.Configure(2);
        m_presenter.StartGame();
        m_presenter.RollScenario();
        //m_presenter.StartScenario();
    }

    void OnDestroy()
    {
        CLoger.LOG_FLOW(ELayer.View);
        if(null != m_presenter)
            m_presenter.Destroy();
    }

    public int[] ScoreTable
    {
        set
        {
            int it = 0;
            foreach (Player p in m_players)
            {
                p.m_txtScores.text = value[it].ToString();
                it++;
            }
        }
    }

    public string Description
    {
        set
        {
            CLoger.LOG_FLOW(ELayer.View);
            bool OneSet = false;
            foreach (Player player in m_players)
            {
                if (!OneSet)
                {
                    player.m_buttons.GetComponentInChildren<CButtonSlide>()
                        .SetText(value, () => {
                            CLoger.LOG_INF(ELayer.View, "Callback");
                            m_presenter.StartScenario();
                        });
                    OneSet = true;
                }
                else
                {
                    player.m_buttons.GetComponentInChildren<CButtonSlide>().SetText(value);
                }
            }
        }
    }

    public void GameEnd()
    {
        CLoger.LOG_FLOW(ELayer.View);
    }
    
    public void ScenarioEnd()
    {
        m_presenter.RollScenario();
    }

    public void Player1Trigger()
    {
        PlayerTrigger((int)EPlayer.P1);
    }

    public void Player2Trigger()
    {
        PlayerTrigger((int)EPlayer.P2);
    }

    public void PlayerTrigger(int id)
    {
        EResult result = m_presenter.TriggerPlayer((EPlayer)id);
        //Animate + NextScreen
        if (result == EResult.Positive)
        {
            m_players[id].m_buttons.GetComponent<Animator>().Play("GreenBlink", 0);
        }
        else if (result == EResult.Negative)
        {
            m_players[id].m_buttons.GetComponent<Animator>().Play("RedBlink", 0);
        }
        m_presenter.NextScreen();
    }
}

