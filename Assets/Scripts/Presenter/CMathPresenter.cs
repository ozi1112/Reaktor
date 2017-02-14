using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class CMathPresenter : IScenarioPresenter
{
    CMathView _view;

    public CMathPresenter(CMathView a_view)
    {
        _view = a_view;
        CScManager.Instance.RegisterScenario(new CScMath(this));
    }

    public void SetActive(bool isActive)
    {
        _view.SetActive(isActive);
    }

    public void SetText(string a_strText)
    {
        CLoger.LOG_FLOW(ELayer.Presenter);
        _view.Invoke(() => { _view.SetText(a_strText); });
    }
}
