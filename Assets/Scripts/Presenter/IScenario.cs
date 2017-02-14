using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IScenario
{
    string Description { get; set; }
    void StartScenario();
    void PauseScenario();
    void StopScenario();
    void NextScreen();
    bool CheckCondition();
    void SetActive(bool a_IsActive);
    void Destroy();
}

