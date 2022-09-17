using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    [InfoBox("This class is specific to this game and sends event to agent component for attack action", InfoMessageType = InfoMessageType.Warning)]
    public class AnimSendEvent : StateMachineBehaviour
    {
        [BoxGroup("Box", LabelText = "Events Sender", CenterLabel = true, ShowLabel = true),
        SerializeField, ToggleGroup(toggleMemberName:"Box/sendEventOnUpdate", groupTitle:"Send on Update")] 
        private bool sendEventOnUpdate;

        [BoxGroup("Box"), SerializeField, ToggleGroup(toggleMemberName: "Box/sendEventOnUpdate"),
            MinMaxSlider(0f, 1f, ShowFields = true)]
        private Vector2 sendEventTimeRange;
        //[SerializeField, BoxGroup("On Update Sender"), PropertyRange(0, 1f)] float minEventNormalizedPoint = 0.2f, maxEventNormalizedPoint = .4f;



        [BoxGroup("Box"), SerializeField, ToggleGroup(toggleMemberName: "Box/sendEventOnExit", groupTitle: "Send on Exit")]
        private bool sendEventOnExit;
        [SerializeField, BoxGroup("Box"), MinMaxSlider(0, 3, ShowFields = true),
            ToggleGroup(toggleMemberName: "Box/sendEventOnExit")]
        Vector2Int comboProbability = new Vector2Int(0, 2);

        [ShowInInspector, BoxGroup("RUNTIME VALUES", CenterLabel = true), ReadOnly] float randValue;
        [ShowInInspector, BoxGroup("RUNTIME VALUES"), ReadOnly]                     int r; 
        [ShowInInspector, BoxGroup("RUNTIME VALUES"), ReadOnly]                     bool hasSent;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator != GameManager.instance.Agent.Anim) return;
            randValue = Random.Range(sendEventTimeRange.x, sendEventTimeRange.y);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator != GameManager.instance.Agent.Anim) return;

            if (sendEventOnUpdate && !hasSent)
            {
                if (stateInfo.normalizedTime >= randValue)
                {
                    hasSent = true;
                    GameManager.instance.Agent.CanAttack_AnimEvent();
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator != GameManager.instance.Agent.Anim) return;

            hasSent = false;
           

            if (sendEventOnExit)
            {
                if ((r = Random.Range(comboProbability.x, comboProbability.y)) == 1)
                {
                    //Debug.Log("COMBO");
                    GameManager.instance.Agent.CanAttack_AnimEvent();
                }
            }
        }
    }
}
