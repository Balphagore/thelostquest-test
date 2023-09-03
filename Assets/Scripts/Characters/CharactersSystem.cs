using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TheLostQuestTest.Characters
{
    public class CharactersSystem : MonoBehaviour, ICharactersSystem
    {
        [SerializeField]
        private List<CharacterData> playerCharactersData;
        [SerializeField]
        private List<CharacterData> enemyCharactersData;
        [SerializeField]
        private List<Transform> playerCharacterSpawnPositions;
        [SerializeField]
        private List<Transform> enemyCharacterSpawnPositions;
        [SerializeField]
        private List<CharacterBuffData> characterBuffs;
        [SerializeField]
        private int maxBuffsCount = 2;

        private List<ICharacterController> playerCharacters = new();
        private List<ICharacterController> enemyCharacters = new();
        private int callbackCount;

        [Inject]
        private IGameManagerSystem gameManager;
        [Inject(Id = "Main")]
        private ICharacterControllersFactory characterControllersFactory;

        public Action DestroyCharactersAction { get; set; }
        public Action<bool, int> SpawnCharacterAction { get; set; }
        public Action<bool> EndTurnAction { get; set; }
        public Action<CharacterModelDataModel> UpdateCharacterDataAction { get; set; }

        [Inject]
        protected void PostInject()
        {
            gameManager.InitializeRoundAction += OnInitializeRoundAction;
            gameManager.StartRoundAction += OnStartRoundAction;
            gameManager.RestartGameAction += OnRestartGameAction;
        }

        protected void OnDisable()
        {
            gameManager.InitializeRoundAction -= OnInitializeRoundAction;
            gameManager.StartRoundAction -= OnStartRoundAction;
            gameManager.RestartGameAction -= OnRestartGameAction;
        }

        public virtual void CharacterAttack(bool isPlayerCharacter, int characterIndex)
        {
            var characterList = isPlayerCharacter ? playerCharacters : enemyCharacters;
            var targetList = !isPlayerCharacter ? playerCharacters : enemyCharacters;
            characterList[characterIndex].AttackCharacter(targetList[characterIndex]);

            EndTurnAction?.Invoke(isPlayerCharacter);
            if (!isPlayerCharacter)
            {
                gameManager.NextRound();
            }
        }

        public virtual void CharacterBuff(bool isPlayerCharacter, int characterIndex)
        {
            var charactersList = isPlayerCharacter ? playerCharacters : enemyCharacters;
            var targetsList = !isPlayerCharacter ? playerCharacters : enemyCharacters;
            if (charactersList[characterIndex].GetActiveBuffsCount() < maxBuffsCount)
            {
                List<string> possibleBuffIds = characterBuffs.Select(buff => buff.Id).ToList();
                List<string> currentBuffIds = charactersList[characterIndex].GetActiveBuffsList();
                string randomBuffId = possibleBuffIds.Except(currentBuffIds).OrderBy(x => UnityEngine.Random.Range(0, possibleBuffIds.Count)).FirstOrDefault();
                int targetIndex = characterBuffs.FindIndex(buff => buff.Id == randomBuffId);
                if (characterBuffs[targetIndex].IsTargetSelf)
                {
                    charactersList[characterIndex].ActivateBuff(characterBuffs[targetIndex]);
                }
                else
                {
                    charactersList[characterIndex].ActivateBuff(characterBuffs[targetIndex]);
                    targetsList[characterIndex].ActivateDebuff(characterBuffs[targetIndex]);
                }
            }
        }

        public virtual void StartNewGame()
        {
            gameManager.RestartGame();
        }

        public virtual void UpdateCharacterData(CharacterModelDataModel characterModelData)
        {
            UpdateCharacterDataAction?.Invoke(characterModelData);
        }

        protected virtual void OnRestartGameAction()
        {
            DestroyCharactersAction?.Invoke();
        }

        protected virtual void SpawnCharacters(List<CharacterData> characters, List<Transform> spawnPositions, Action callback, bool isPlayerCharacter)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                CharacterData characterData = characters[i];
                Transform spawnPosition = spawnPositions[i];
                CharacterModelDataModel characterModelData = new(isPlayerCharacter, i, characters[i].CharacterStatsData.Copy(), new List<CharacterModelDataModel.CharacterBuff>(), new List<CharacterModelDataModel.CharacterBuff>());
                ICharacterController character = characterControllersFactory.Create(characterData.CharacterView, characterModelData, spawnPosition, () => OnCallback(callback));
                var targetList = isPlayerCharacter ? playerCharacters : enemyCharacters;
                targetList.Add(character);
                callbackCount++;

                SpawnCharacterAction?.Invoke(isPlayerCharacter, i);
            }
        }

        protected virtual void OnCallback(Action callback)
        {
            callbackCount--;
            if (callbackCount == 0)
            {
                callback?.Invoke();
            }
        }

        protected virtual void OnStartRoundAction(int value)
        {
            playerCharacters.ForEach(character => character.UpdateBuffDurations());
            enemyCharacters.ForEach(character => character.UpdateBuffDurations());
        }

        protected virtual void OnInitializeRoundAction(Action callback)
        {
            playerCharacters.Clear();
            enemyCharacters.Clear();
            SpawnCharacters(playerCharactersData, playerCharacterSpawnPositions, callback, true);
            SpawnCharacters(enemyCharactersData, enemyCharacterSpawnPositions, callback, false);
        }
    }
}
