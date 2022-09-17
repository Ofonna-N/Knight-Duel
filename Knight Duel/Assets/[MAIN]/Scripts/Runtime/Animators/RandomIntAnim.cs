using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightDuel
{
    public class RandomIntAnim : StateMachineBehaviour
    {
        [SerializeField] string param;

        [SerializeField] int maxInt;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(param, Random.Range(0, maxInt));
        }
    }
}
