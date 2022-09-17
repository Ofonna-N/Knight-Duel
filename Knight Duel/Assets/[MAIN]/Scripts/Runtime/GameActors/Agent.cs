using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;

namespace KnightDuel
{
    public class Agent : Character
    {
        /*[Header("==================AGENT ATTACK DATA==================")]
        [SerializeField] Vector2 attackInterval = new Vector2(.5f, 1f);*/

        Lean.Touch.LeanFinger emptyFingerData;

        bool parrying;

        public void Init(GameObject m, Character e, Sex s, int h, int weapon_i, int weapon_p, Sprite icon)
        {
            base.Init(m, e, s, h, weapon_p, icon);
            weapon = GameManager.instance.GetWeapon(Anim.GetBoneTransform(HumanBodyBones.RightHand), weapon_i).GetComponent<Weapon>(); //get weapon to hand
            weapon.Init(this, weapon_p);
            //GameManager.instance.OnGameStart += StartAgentAttack;
            //debugText.text = "Agent Health: " + health;
            //weaponHolder = m.GetComponentInChildren<PropMuscle>();
            //weaponHolder.currentProp = weapon;
        }


        public override void OnGamestart()
        {
            base.OnGamestart();
            //StartCoroutine("AwaitAgentAttack");
        }

        public void OnGameEnd()
        {
            //StopCoroutine("AwaitAgentAttack");
        }


        /// <summary>
        /// recieves an animatoin can attack method call from an
        /// animator state behavior. to enable agent's next attack
        /// </summary>
        public void CanAttack_AnimEvent()
        {
            if (!GameManager.instance.GameOver && !playerDead && !enemy.PlayerDead)
            {
                int rand = Random.Range(0, 7);

                switch (rand)
                {
                    case 0:
                        SwipeNorth(emptyFingerData);
                        break;
                    case 1:
                        SwipeNorthEast(emptyFingerData);
                        break;
                    case 2:
                        SwipeEast(emptyFingerData);
                        break;
                    case 3:
                        SwipeSouthEast(emptyFingerData);
                        break;
                    case 4:
                        SwipeSouth(emptyFingerData);
                        break;
                    case 5:
                        SwipeSouthWest(emptyFingerData);
                        break;
                    case 6:
                        SwipeNorthWest(emptyFingerData);
                        break;
                    default:
                        break;
                }

            }
        }

        


        public override void CanParry()
        {
            int rand = Random.Range(0, 2);
            //int rand = 0;

            if (rand == 0)
            {
                //anim.CrossFade(StaticStrings.GetDodge(enemy, out bool goodDodge), animCrossFadeTime);
                SwipeWest(emptyFingerData);

                /*if (parrying) StopCoroutine("CheckIfDodge");
                StartCoroutine("CheckIfDodge");*/
            }
        }

        /*IEnumerator CheckIfDodge()
        {
            parrying = true;
            yield return new WaitUntil(() => !IsDodging());
            
            // = false;
            parrying = false;
            
        }

        private bool IsDodging()
        {
            if (anim.GetBool(StaticStrings.IsParrying_Anim_State))
            {
                enemy.IOnDeactivateWeapon();
                for (int i = 0; i < anim.GetCurrentAnimatorClipInfo(0).Length; i++)
                {
                    Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[i].clip.name);
                }
            }
            return !anim.GetBool(StaticStrings.IsParrying_Anim_State);
        }*/

        public override void OnCollision(int damage, Collision c)
        {
            base.OnCollision(damage, c);
            //debugText.text = "Agent Health: " + health;
        }

        

        public override void OnCharacterDead()
        {
            GameManager.instance.OnGameOver(true);
        }
    }
}
