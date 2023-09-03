using System.Collections.Generic;
using UnityEngine;

namespace TheLostQuestTest.Characters
{
    [CreateAssetMenu(fileName = "StatTypes", menuName = "Game Data/Stat Types")]
    public class StatTypes : ScriptableObject
    {
        public List<string> Types;
    }
}
