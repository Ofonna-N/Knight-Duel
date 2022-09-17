using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//using System.Reflection;

namespace KnightDuel
{
    [InfoBox("Note that if event is sent, it will be sent to the Character component " +
        "better transfered to Game Manager but we do this for now", InfoMessageType = InfoMessageType.Info)]

    public class AnimBoolState : StateMachineBehaviour
    {
        //==========BOOL SETTER
        [BoxGroup("Box", CenterLabel = true, GroupName = "SET BOOL BEHAVIOR")]
        [Tooltip("animator parameter to set")]
        [SerializeField] private string param;

        [Tooltip("Value to set animator parameter"), BoxGroup("Box")]
        [SerializeField] private bool value;

        [Tooltip("set parameter on normalized time frame"),
            LabelText("Set on Normalized Time Frame"), BoxGroup("Box")]
        [SerializeField] private bool setOnTime;
        
        [Tooltip("should animator reset bool back to default state on exit"),
            BoxGroup("Box")]
        [SerializeField] private bool reset;
        
        [Tooltip("time to set parameter"), 
            LabelText("Normalized Time Action"), BoxGroup("Box"), ShowIf("@setOnTime")] //show if
        [SerializeField] [Range(0f, 1f)] private float time = .25f;


        //======= EVENT HANDLER
        [Tooltip("should we send event?"), ToggleGroup(toggleMemberName:"Box/sendEvent", groupTitle:"Send Event")]
        [SerializeField] private bool sendEvent = false;

        [Tooltip("Event Parameter to Send to Character component"),
            ToggleGroup("Box/sendEvent")]
        [SerializeField] private string _event;

        /*[Tooltip("Send Game Event on after parameter time"),
            ToggleGroup("Box/sendEvent"), LabelText("Send Event On Time Frame")]
        [SerializeField] bool sendEventOnTime;*/
        
      
        [Tooltip("should we set Direction of Anim Attack"),
            ToggleGroup(toggleMemberName:"Box/setHitDirection", groupTitle:"Set Hit Direction")]
        [SerializeField] bool setHitDirection = true;
        [Tooltip("Direction Param"), ToggleGroup("Box/setHitDirection")]
        [SerializeField] string dirParam = "IsRight";
        [Tooltip("Direction of Anim"), ToggleGroup("Box/setHitDirection")]
        [SerializeField] bool IsRight = true;

        //[ShowInInspector, ReadOnly, LabelText("Event Sent"), ToggleGroup("Box/sendEvent")] 
        bool hasSent = false;

        //bool onEnter;
        //int num;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //num += 1;
            if (!setOnTime)
            {
                //Debug.Log("Param Set");
                animator.SetBool(param, value);
                // hasSent = true;
            }

            if (sendEvent)
            {
                var reciever = animator.GetComponentInParent<Character>();
                var method = reciever.GetType().GetMethod(_event);
                method.Invoke(reciever, null);
            }

            if (setHitDirection)
            {
                animator.SetBool(dirParam, IsRight);
                //hasSent = true;
            }

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > time)
            {

                if (setOnTime && !hasSent) animator.SetBool(param, value);

                /*if (sendEvent && sendEventOnTime && !hasSent)
                {
                    //Debug.Log("Event Sent");
                    Debug.Log("Event Sent update");
                    var reciever = animator.GetComponentInParent<Character>();
                    var method = reciever.GetType().GetMethod(_event);
                    method.Invoke(reciever, null);

                    if (setHitDirection)
                    {
                        animator.SetBool(dirParam, IsRight);
                    }
                }*/

                hasSent = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (reset)
            {
                animator.SetBool(param, !value);
                //animator.SetBool(dirParam, !isRight);
            }
            hasSent = false;
            //num = 0;
            //onEnter = false;
        }
    }
}
