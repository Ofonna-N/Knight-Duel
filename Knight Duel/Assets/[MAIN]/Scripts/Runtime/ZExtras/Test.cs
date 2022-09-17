using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    [InfoBox("Testing dividing int values")]
    public class Test : MonoBehaviour
    {
        
        [SerializeField, HorizontalGroup("Hor"), Min(1)]
        private int num1, num2;

        [SerializeField, ReadOnly]
        private int ans;

        [Button("Calculate")]
        private void Calculate()
        {
            ans = num1 / num2;
        }
    }
}
