using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightDuel
{
    public static class StaticStrings
    {
        #region Anim Strings
        //=================================================Anim Bool States
        public static readonly int IsHighAttack_Anim_State = Animator.StringToHash("IsHighAttack");

        public static readonly int IsMidAttack_Anim_State = Animator.StringToHash("IsMidAttack");
                                        
        public static readonly int IsLowAttack_Anim_State = Animator.StringToHash("IsLowAttack");

        public static readonly int IsParrying_Anim_State = Animator.StringToHash("IsParrying");

        //public const string IsDodging_Anim_Tag = "dodge";

        public static readonly int IsAttacking_Anim_State = Animator.StringToHash("IsAttacking");


        public const string IsIdle_Anim_State = "Sword Idle";

        public static int GameStart_Anim_State = Animator.StringToHash("GameStart");

        //================================================= Anim Parry CrossFade States
        static readonly int DodgeHigh = Animator.StringToHash("Sword1h_ShortDodge_1");

        static readonly int DodgeMid = Animator.StringToHash("Sword1h_ShortDodge_Mid2");

        static readonly int DodgeLow = Animator.StringToHash("Sword1h_ShortDodge_Low2");


        public static int GetDodge(Character enemy/*, out bool goodDodge*/)
        {
            int retVal;
            

            if (enemy.Anim.GetBool(IsHighAttack_Anim_State))
            {
                retVal = DodgeHigh;
                //goodDodge = true;
                //enemy.Weapon.Col.enabled = false;
            }
            else if (enemy.Anim.GetBool(IsMidAttack_Anim_State))
            {
                retVal = DodgeMid;
                //goodDodge = true;
                //enemy.Weapon.Col.enabled = false;
            }
            else if (enemy.Anim.GetBool(IsLowAttack_Anim_State))
            {
                retVal = DodgeLow;
                //goodDodge = true;
                //enemy.Weapon.Col.enabled = false;
            }
            else
            {
                retVal = DodgeHigh;
                //goodDodge = false;
            }

            return retVal;
        }

        //================================================Anim Damage States
        static readonly int HighHitReaction_Anim = Animator.StringToHash("Sword1h_Hit_Head_Front");

        static readonly int MidHitReaction_Anim = Animator.StringToHash("Sword1h_Hit_Torso_Right");

        static readonly int MidHitReactionL_Anim = Animator.StringToHash("Sword1h_Hit_Torso_Left");

        static readonly int LowHitReaction_Anim = Animator.StringToHash("Sword1h_Hit_Legs_Right");

        static readonly int LowHitReactionL_Anim = Animator.StringToHash("Sword1h_Hit_Legs_Left");

        static readonly int isRightParam_Anim = Animator.StringToHash("IsRight");

        public static int GetHitReaction(Animator anim)
        {
            int retVal;


            if (anim.GetBool(IsHighAttack_Anim_State))
            {
                retVal = HighHitReaction_Anim;
            }
            else if (anim.GetBool(IsMidAttack_Anim_State))
            {
                if (anim.GetBool(isRightParam_Anim))
                {
                    retVal = MidHitReaction_Anim;
                }
                else
                {
                    retVal = MidHitReactionL_Anim;
                }
            }
            else if (anim.GetBool(IsLowAttack_Anim_State))
            {
                if (anim.GetBool(isRightParam_Anim))
                {
                    retVal = LowHitReaction_Anim;
                }
                else
                {
                    retVal = LowHitReactionL_Anim;
                }
            }
            else
            {
                retVal = HighHitReaction_Anim;
            }

            return retVal;
        }

        //================================================Anim CrossFade Array States Names
        static readonly int[] SwipeUpAttacks            = { Animator.StringToHash("Sword_Attack_R_Whirlwind_fast") };

        static readonly int[] SwipeUpRightAttacks       = { Animator.StringToHash("Sword_Attack_Sp_R"),
                                                            Animator.StringToHash("Sword_Attack_Sp_RRound_fast")};

        static readonly int[] SwipeRightAttacks         = { Animator.StringToHash("Attack_Move_slow_Backtrick") };

        static readonly int[] SwipeDownRightAttacks     = { Animator.StringToHash("Attack_Move_slow_whirl_L_2") };


        static readonly int[] SwipeDownAttacks          = { Animator.StringToHash("Attack_Move_slow_Lup") };


        static readonly int[] SwipeDownLeftAttacks      = { Animator.StringToHash("Attack_Place_slow_Rdown_1") };


        static readonly int[] SwipeLeftAttacks          = { Animator.StringToHash("Attack_Move_slow_Lup") };


        static readonly int[] SwipeUpLeftAttacks        = { Animator.StringToHash("Attack_Move_slow_Rdown_1") };


        public static int GetSwipeUpAttack()
        {
            int rand = Random.Range(0, SwipeUpAttacks.Length);

            return SwipeUpAttacks[rand];
        }

        public static int GetSwipeUpRightAttack()
        {
            int rand = Random.Range(0, SwipeUpRightAttacks.Length);

            return SwipeUpRightAttacks[rand];
        }

        public static int GetSwipeRightAttack()
        {
            int rand = Random.Range(0, SwipeRightAttacks.Length);

            return SwipeRightAttacks[rand];
        }

        public static int GetSwipeDownRightAttack()
        {
            int rand = Random.Range(0, SwipeDownRightAttacks.Length);

            return SwipeDownRightAttacks[rand];
        }

        public static int GetSwipeDownAttack()
        {
            int rand = Random.Range(0, SwipeDownAttacks.Length);

            return SwipeDownAttacks[rand];
        }

        public static int GetSwipDownLeftAttack()
        {
            int rand = Random.Range(0, SwipeDownLeftAttacks.Length);

            return SwipeDownLeftAttacks[rand];
        }

        public static int GetSwipeLeftAttack()
        {
            int rand = Random.Range(0, SwipeLeftAttacks.Length);

            return SwipeLeftAttacks[rand];
        }

        public static int GetSwipeUpLeftAttack()
        {
            int rand = Random.Range(0, SwipeUpLeftAttacks.Length);

            return SwipeUpLeftAttacks[rand];
        }

        //================================================Anim CrossFade Game Over States Names
        static readonly int[] GameOverWinAnimParams = { Animator.StringToHash("Sword1h_Vicotry_1"), 
                                                        Animator.StringToHash("Sword1h_Victory_2"), 
                                                        Animator.StringToHash("Sword1h_Victory_3"), 
                                                        Animator.StringToHash("Sword1h_Victory_4") };

        public static int GetGameOverAnim()
        {
            int rand = Random.Range(0, GameOverWinAnimParams.Length);

            return GameOverWinAnimParams[rand];
        }

        #endregion

        //=========UI EVENTS
        public const string OnGameOverWin_Event = "GameOverWin";
        public const string OnGameOverLose_Event = "GameOverLose";
        public const string OnPlayerRevive_Event = "PlayerRevive";
        public const string VeiwTutorial_Event = "VeiwTutorial_Event";

        //==========UI FORMATING
        public const string TMP_CoinIconFormat = "<sprite=\"btn_icon_gold\", index=0>";
        public const string TMP_ADIconFormat = "<sprite=\"btn_icon_ad\", index=0>";

        //============ AUDIO MIXER PARAMS
        public const string SFXVolume_Param = "Sfx Volume";

        //============= GAME SAVE KEYS
        public const string Coins_Key = "Coins_Key";

        public const string SelectedSkin_Key = "SelectedSkin_Key";
        public const string Skin_Key_Suffix = "Skin_";

        public const string SelectedWeapon_Key = "SelectedWeapon_Key";
        public const string Weapon_Key_Suffix = "Weapon_";

        public const string SelectedAgent_Key = "SelectedAgent_Key";
        public const string Agent_Key_Suffix = "Agent_";

        public const string SelectedLevel_Key = "SelectedLevel_Key";
        public const string Level_Key_Suffix = "Level ";

        public const string levelPhaseNo_Key = "LevelPhaseNo_Key";

        public const string vibrateOn_Key = "VibrateOn_Key";

        public const string SfxOn_Key = "SfxOn_Key";

        public const string HasViewTutorial_key = "HasViewedTutorial_Key";

        public const string PlayerPower_Key = "PlayerPower_Key";

        public const string PlayerHealth_Key = "PlayerHealth_Key";

        public const string AgentPower_Key = "AgentPower_Key";

        public const string AgentHealth_Key = "AgentHealth_Key";

        public const string CoinsReward_Key = "CoinsReward_Key;";

        public const string Unlocked_Key_Prefix = "_IsUnlocked_Key";
        public const string Claimed_Key_Prefix = "_IsClaimed_Key";
        public const string UnlockType_Key_Prefix = "_UnlockType_Key";
        public const string UnlockLevel_Key_Prefix = "_UnlockLevel_Key";

        public const string PowerPrice_Key = "PowerPrice_Key";

        public const string HealthPrice_Key = "HealthPrice_Key";
    }
}
