using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CButtonSlide : MonoBehaviour {


    VoidDelegate m_DAnimationFinished;
    public Text m_text;
    Animator m_animator;
    int m_iVisibleHash;
    string m_strText = string.Empty;

	// Use this for initialization
	void Awake () {
        m_animator = m_text.GetComponent<Animator>();
        m_iVisibleHash = Animator.StringToHash("IsVisible");
    }

    void Start()
    {
        m_text.text = m_strText;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Text out of screen now set new text.
    /// </summary>
    void ReadyToShow()
    {
        m_animator.SetBool(m_iVisibleHash, true);
        m_text.text = m_strText;
    }

    void Finished()
    {
        if (null != m_DAnimationFinished)
        {
            m_DAnimationFinished();
            m_DAnimationFinished = null;
        }
    }

    void ReadyToSet()
    {
        
    }

    /// <summary>
    /// Start new text animation
    /// </summary>
    /// <param name="a_strText">text</param>
    public void SetText(string a_strText, VoidDelegate a_DFinishCallback = null)
    {
        m_strText = a_strText;
        m_DAnimationFinished = a_DFinishCallback;
        //Hide text and wait for ReadyToShow()
        m_animator.SetBool(m_iVisibleHash, false);
    }
}
