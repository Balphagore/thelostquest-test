using System;
using UnityEngine;
using Zenject;

public class GameManagerSystem : MonoBehaviour, IGameManagerSystem
{
    public Action<Action> InitializeRoundAction { get; set; }
    public Action<int> StartRoundAction { get; set; }
    public Action RestartGameAction { get; set; }

    [SerializeField]
    private int roundCount = 1;

    [Inject]
    private void PostInject()
    {
        InitializeRoundAction?.Invoke(() => OnCallback());        
    }

    public void RestartGame()
    {
        RestartGameAction?.Invoke();
        InitializeRoundAction?.Invoke(() => OnCallback());
        roundCount = 1;
    }

    private void OnCallback()
    {
        StartRoundAction?.Invoke(roundCount);
    }

    public void NextRound()
    {
        roundCount++;
        StartRoundAction?.Invoke(roundCount);
    }
}
