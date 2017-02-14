using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class CMathView : ACInvokable
{
    CMathPresenter m_presenter;
    public Text[] m_Text;

    void Start()
    {
        m_presenter = new CMathPresenter(this);
    }

    public void SetText(string a_strText)
    {
        foreach(Text t in m_Text)
        {
            t.text = a_strText;
        }
    }

    public void SetActive(bool m_bActive)
    {
        gameObject.SetActive(m_bActive);
    }
}

