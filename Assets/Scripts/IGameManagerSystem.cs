using System;

public interface IGameManagerSystem
{
    Action<Action> InitializeRoundAction { get; set; }

    Action<int> StartRoundAction { get; set; }

    Action RestartGameAction { get; set; }

    void RestartGame();

    void NextRound();
}
