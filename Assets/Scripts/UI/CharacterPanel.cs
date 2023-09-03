using AYellowpaper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TheLostQuestTest.UI
{
    public class CharacterPanel : MonoBehaviour, ICharacterPanel
    {
        [SerializeField]
        private TextMeshProUGUI buffsListText;
        [SerializeField]
        private InterfaceReference<IStatBar, MonoBehaviour> healthBar;
        [SerializeField]
        private InterfaceReference<IStatBar, MonoBehaviour> armorBar;
        [SerializeField]
        private InterfaceReference<IStatBar, MonoBehaviour> vampirismBar;
        [SerializeField]
        private Button attackButton;
        [SerializeField]
        private Button buffButton;

        [Inject]
        private IUISystem uI;

        private bool isPlayerCharacter;
        private int characterIndex = -1;

        public void Activate(bool isPlayerCharacter, int characterIndex)
        {
            attackButton.onClick.AddListener(OnAttackButtonClick);
            buffButton.onClick.AddListener(OnBuffButtonClick);
            this.isPlayerCharacter = isPlayerCharacter;
            this.characterIndex = characterIndex;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            attackButton.onClick.RemoveAllListeners();
            buffButton.onClick.RemoveAllListeners();
            characterIndex = -1;
            gameObject.SetActive(false);
        }

        public void OnAttackButtonClick()
        {
            uI.Attack(isPlayerCharacter, characterIndex);
        }

        public void OnBuffButtonClick()
        {
            uI.Buff(isPlayerCharacter, characterIndex);
            buffButton.interactable = false;
        }

        public void SetInteractible(bool isInteractable)
        {
            attackButton.interactable = isInteractable;
            buffButton.interactable = isInteractable;
        }

        public void UpdateStatsUI(CharacterStatsUIUpdateDataModel characterStatsUIUpdateData)
        {
            string buffsString = string.Join("\n", characterStatsUIUpdateData.BuffsStatuses);
            buffsListText.text = buffsString;
            healthBar.Value.SetValue(characterStatsUIUpdateData.Health, characterStatsUIUpdateData.MaximumHealth);
            armorBar.Value.SetValue(characterStatsUIUpdateData.Armor, characterStatsUIUpdateData.MaximumArmor);
            vampirismBar.Value.SetValue(characterStatsUIUpdateData.Vampirism, characterStatsUIUpdateData.MaximumVampirism);
        }
    }
}
