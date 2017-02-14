using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CBlackWhiteView : ACInvokable
{
    public Image m_image;
    CBlackWhitePresenter m_presenter;

    void Start()
    {
        m_presenter = new CBlackWhitePresenter(this);
    }

    public void SetColor(Color c)
    {
        m_image.color = c;
    }

    public void SetActive(bool m_bActive)
    {
        gameObject.SetActive(m_bActive);
    }
}

