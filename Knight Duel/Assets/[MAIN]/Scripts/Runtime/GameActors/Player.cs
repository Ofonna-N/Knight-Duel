using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using RootMotion.Dynamics;

using UnityEngine;
using Lean.Touch;

namespace KnightDuel
{
    public class Player : Character
    {
        [Space]
        [SerializeField] Ease gpTextAnimEase;
        [SerializeField] protected TextMeshProUGUI gamePlayText;
        [SerializeField, Range(0, 2f)] protected float gpScaleTextDur;
        [SerializeField] float gdTextScaleEndValue;


        bool dodged;
        bool isCheckingDodged;
        bool inParryMode;

        public override void Init(GameObject m, Character e, Sex s, int h, int weapon_p, Sprite icon)
        {
            base.Init(m, e, s, h, weapon_p, icon);
            SetWeapon(weapon_p);
            gamePlayText.CrossFadeAlpha(0f, .1f, true);
            GameManager.instance.AnimateArrow(0, false);
            //debugText.text = "Player Health: " + health;
            //weaponHolder = m.GetComponentInChildren<PropMuscle>();
            //weaponHolder.currentProp = weapon;
            //print(gameObject.name + " " + sex);
        }





        #region Swipe Override Functions

        public override void SwipeNorth(LeanFinger finger)
        {
            base.SwipeNorth(finger);
            GameManager.instance.AnimateArrow(90f, true);
        }

        public override void SwipeNorthEast(LeanFinger finger)
        {
            base.SwipeNorthEast(finger);
            GameManager.instance.AnimateArrow(45f, true);
        }

        public override void SwipeEast(LeanFinger finger)
        {
            base.SwipeEast(finger);
            GameManager.instance.AnimateArrow(0f, true);
        }

        public override void SwipeSouthEast(LeanFinger finger)
        {
            base.SwipeSouthEast(finger);
            GameManager.instance.AnimateArrow(315f, true);
        }

        public override void SwipeSouth(LeanFinger finger)
        {
            base.SwipeSouth(finger);
            GameManager.instance.AnimateArrow(270f, true);
        }

        public override void SwipeSouthWest(LeanFinger finger)
        {
            base.SwipeSouthWest(finger);
            GameManager.instance.AnimateArrow(225f, true);
        }

        public override void SwipeWest(LeanFinger finger)
        {
            base.SwipeWest(finger);
            GameManager.instance.AnimateArrow(180f, true);
        }

        public override void SwipeNorthWest(LeanFinger finger)
        {
            base.SwipeNorthWest(finger);
            GameManager.instance.AnimateArrow(135f, true);
        }

        #endregion





        #region Player Functions
        public void SetWeapon(int weapon_p)
        {
            if (weapon != null)
            {
                Destroy(weapon.gameObject);
            }

            weapon = GameManager.instance.GetWeapon(Anim.GetBoneTransform(HumanBodyBones.RightHand)).GetComponent<Weapon>(); //get weapon to player hand
            weapon.Init(this, weapon_p);
        }

        public override void CanParry()
        {
            dodged = false;
            inParryMode = true;
            swipedDodge = false;

            if (isCheckingDodged)
            {
                isCheckingDodged = false;
                StopCoroutine("CheckDodgeSuccess");
            }
            StartCoroutine("CheckDodgeSuccess");

        }

        IEnumerator CheckDodgeSuccess()
        {
            int h = health;
            yield return new WaitUntil(() => !GetEnemyIsAttacking(h));
            inParryMode = false;

            if (dodged)
            {
                AnimateDodgeText();
                //dodged = false;
            }
        }

        private bool GetEnemyIsAttacking(int h)
        {
            if (h == health)
            {
                //Debug.Log("DODGED");
                if (swipedDodge || anim.GetBool(StaticStrings.IsParrying_Anim_State))
                {
                    dodged = true;
                }
            }
            else
            {
                dodged = false;
                return false;
            }


            return enemy.IsAttacking;
        }

        private void AnimateDodgeText()
        {
            gamePlayText.transform.DOScale(gdTextScaleEndValue, gpScaleTextDur).SetEase(gpTextAnimEase)
                                .OnStart(() => { gamePlayText.CrossFadeAlpha(1f, .1f, false); })
                                .OnComplete(() => { gamePlayText.CrossFadeAlpha(0f, 1f, false); });
        }

        public override void OnCollision(int damage, Collision c)
        {
            base.OnCollision(damage, c);

            //debugText.text = "Player Health: " + health;
        }

        public override void OnCharacterDead()
        {
            GameManager.instance.OnGameOver(false);
        }

        public override void OnRevive()
        {
            health = enemy.Weapon.Power * 2;
            UpdateHealthUI();
            puppet.state = PuppetMaster.State.Alive;
            gamePlayText.CrossFadeAlpha(0f, .1f, true);
            GameManager.instance.AnimateArrow(0, false);
            base.OnRevive();
        }

        #endregion
    }
}
