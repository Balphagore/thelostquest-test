using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheLostQuestTest.UI
{
    public class StatBar : MonoBehaviour, IStatBar
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TextMeshProUGUI statValueText;

        public void SetValue(float value, float maxValue)
        {
            statValueText.text = value.ToString();
            slider.value = value / maxValue;
        }
    }
}
