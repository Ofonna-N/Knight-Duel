using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KnightDuel
{
    public class CharacterAnimEventReciever : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private Character character;
        [ShowInInspector, ReadOnly]
        private Animator anim;


        private void Start()
        {
            //if (anim == null) return;
            character = GetComponentInParent<Character>();
            anim = GetComponent<Animator>();
        }

        public void IOnActivateWeapon(string state)
        {
            //if (character == null) return;
            anim.SetBool(Animator.StringToHash(state), true);
            character.IOnActivateWeapon();
        }

        public void IOnDeactivateWeapon(string state)
        {
            //if (character == null) return;
            anim.SetBool(Animator.StringToHash(state), false);
            character.IOnDeactivateWeapon();
        }

        public void IOCharacterDodge(string state)
        {
            //if (character == null) return;
            anim.SetBool(Animator.StringToHash(state), true);
            character.IOCharacterDodge();
        }
    }
}