using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace KnightDuel
{
    public class CharacterPoser : MonoBehaviour
    {
        //[ShowInInspector, ReadOnly, Title("STATIC DATA", TitleAlignment = TitleAlignments.Centered), 
            //LabelText("STATIC ANIMATOR")]
        //public static Animator poserAnimator;

        [Title("ANIMATOR", TitleAlignment = TitleAlignments.Centered), HideLabel]
        [SerializeField] Animator anim;

       // [Title("ANIMATIONS", TitleAlignment = TitleAlignments.Centered)]
        //[SerializeField, Range(0, 1f)] float playback = 0.5f;

        [Title("ANIMATIONS"), ListDrawerSettings(ShowIndexLabels = true)]
        [SerializeField] PoseAnimData[] anims;


        private void Start()
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].Init(anim);
            }
        }
    }

    [System.Serializable]
    public class PoseAnimData
    {
        [ShowInInspector, ReadOnly] private Animator mainAnim;
        [Space(10)]
        [SerializeField, InlineButton("Animate", Label = "Animate"), 
            LabelText("Animation Name")] string animName;


        public void Init(Animator a)
        {
            mainAnim = a;
        }

        public void Animate()
        {
            mainAnim.CrossFade(animName, .1f);
        }
    }
}
