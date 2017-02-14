using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IGameView
{
    int[] ScoreTable
    {
        set;
    }

    string Description
    {
        set;
    }

    void GameEnd();

    void ScenarioEnd();
}

