using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CBlackWhitePresenter : IScenarioPresenter
{
    CBlackWhiteView m_view;

    public CBlackWhitePresenter(CBlackWhiteView a_view)
    {
        m_view = a_view;
        CScManager.Instance.RegisterScenario(new CScBlackWhite(this));
    }

    public void ConditionChange(bool value)
    {
        string color = value ? "W": "B";
        if(value) //White
        {
            m_view.Invoke(() => { m_view.SetColor(Color.white); });
        }
        else // Black
        {
            m_view.Invoke(() => { m_view.SetColor(Color.black); });
        }
    }

    public void SetActive(bool a_bActive)
    {
        m_view.SetActive(a_bActive);
    }
}

