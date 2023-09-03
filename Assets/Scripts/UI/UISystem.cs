using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using AYellowpaper;

using TheLostQuestTest.Characters;

namespace TheLostQuestTest.UI
{

    public class UISystem : MonoBehaviour, IUISystem
    {
        [SerializeField]
        private TextMeshProUGUI roundsCounterText;
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private List<InterfaceReference<ICharacterPanel, MonoBehaviour>> playerCharacterPanels;
        [SerializeField]
        private List<InterfaceReference<ICharacterPanel, MonoBehaviour>> enemyCharacterPanels;

        [SerializeField]
        private string roundsCounterTextValue;

        [Inject]
        private IGameManagerSystem gameManager;
        [Inject]
        private ICharactersSystem characters;

        [Inject]
        private void PostInject()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);

            gameManager.RestartGameAction += OnRestartGameAction;
            gameManager.StartRoundAction += OnStartRoundAction;
            characters.SpawnCharacterAction += OnSpawnCharacterAction;
            characters.EndTurnAction += OnEndTurnAction;
            characters.UpdateCharacterDataAction += OnUpdateCharacterDataAction;
        }

        private void OnDisable()
        {
            gameManager.RestartGameAction -= OnRestartGameAction;
            gameManager.StartRoundAction -= OnStartRoundAction;
            characters.SpawnCharacterAction -= OnSpawnCharacterAction;
            characters.EndTurnAction -= OnEndTurnAction;
            characters.UpdateCharacterDataAction -= OnUpdateCharacterDataAction;
        }

        public void Attack(bool isPlayerCharacter, int characterIndex)
        {
            characters.CharacterAttack(isPlayerCharacter, characterIndex);
        }

        public void Buff(bool isPlayerCharacter, int characterIndex)
        {
            characters.CharacterBuff(isPlayerCharacter, characterIndex);
        }

        private void OnRestartButtonClick()
        {
            gameManager.RestartGame();
        }

        private void OnRestartGameAction()
        {
            playerCharacterPanels.ForEach(x => x.Value.Deactivate());
            enemyCharacterPanels.ForEach(x => x.Value.Deactivate());
        }

        private void OnStartRoundAction(int value)
        {
            roundsCounterText.text = roundsCounterTextValue + " " + value;
            SwitchTurn(true);
        }

        private void OnEndTurnAction(bool isPlayerTurn)
        {
            SwitchTurn(!isPlayerTurn);
        }

        private void OnSpawnCharacterAction(bool isPlayerCharacter, int characterIndex)
        {
            var targetPanels = isPlayerCharacter ? playerCharacterPanels : enemyCharacterPanels;
            targetPanels[characterIndex].Value.Activate(isPlayerCharacter, characterIndex);
        }

        private void SwitchTurn(bool isPlayerCharacter)
        {
            playerCharacterPanels.ForEach(x => x.Value.SetInteractible(isPlayerCharacter));
            enemyCharacterPanels.ForEach(x => x.Value.SetInteractible(!isPlayerCharacter));
        }

        private void OnUpdateCharacterDataAction(CharacterModelDataModel characterData)
        {
            var targetPanels = characterData.IsPlayerCharacter ? playerCharacterPanels : enemyCharacterPanels;
            targetPanels[characterData.CharacterIndex].Value.UpdateStatsUI(new CharacterStatsUIUpdateDataModel(characterData));
        }
    }
}
