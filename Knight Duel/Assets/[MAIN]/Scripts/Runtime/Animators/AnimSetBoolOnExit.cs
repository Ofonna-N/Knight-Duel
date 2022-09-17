using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    public class AnimSetBoolOnExit : StateMachineBehaviour
    {
        [BoxGroup(GroupID = "Box", ShowLabel = true, LabelText = "SET BOOL ON EXIT", CenterLabel = true)]
        [SerializeField, LabelText("Parameter")]
        private string param;

        [SerializeField, BoxGroup("Box")]
        private bool value;


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(param, value);
        }
    }
}
