using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using TMPro;
using RootMotion.Dynamics;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    public abstract class Character : MonoBehaviour, IRecieveCharacterAnimEvents
    {
        [Title("PUPPET DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected PuppetMaster puppet;
        public PuppetMaster Puppet => puppet;
        //[SerializeField] protected BehaviourPuppet behaviour;

        [Title("ANIMATION DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected GameObject model;
        public GameObject Model => model;
        [SerializeField] protected Animator anim;
        public Animator Anim => anim;
        [SerializeField] protected float animCrossFadeTime = 0.05f;
        public float AnimCrossFadeTime => animCrossFadeTime;

        [Title("ENEMY DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected Character enemy;

        [Title("WEAPON DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected Weapon weapon;
        public Weapon Weapon => weapon;
        [MinMaxSlider(0.5f, 1.5f, true)]
        [SerializeField] Vector2 pitchRange = new Vector2(0.7f, 1);
        //[SerializeField] protected PropMuscle weaponHolder;

        [Title("CHARACTER STATE DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected Sex sex;
        [MinMaxSlider(0.5f, 1f, true)]
        [SerializeField] protected Vector2 male_voice_pitchRange = new Vector2(0.8f, 1);
        [MinMaxSlider(1.5f, 2f, true)]
        [SerializeField] protected Vector2 female_voice_pitchRange = new Vector2(1.5f, 1.8f);
        [SerializeField] protected int health = 100;
        [SerializeField] protected int power = 10;
        [SerializeField] protected bool playerDead;
        public bool PlayerDead => playerDead;
        [Space]
        [SerializeField] protected AudioSource sfxSource;
        public AudioSource SfxSource => sfxSource;
        protected Collision hitInfo;

        [Title("UI DATA", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected TextMeshProUGUI healthText;
        [SerializeField] protected TextMeshProUGUI powerText;
        [SerializeField] protected Slider healthSlider;
        [SerializeField] protected Slider powerSlider;
        public Slider PowerSlider => powerSlider;
        [SerializeField] protected Image IconImg;


        protected bool swipedDodge;

        public bool IsAttacking
        {
            get
            {
                return anim.GetBool(StaticStrings.IsHighAttack_Anim_State) ||
                       anim.GetBool(StaticStrings.IsMidAttack_Anim_State) ||
                       anim.GetBool(StaticStrings.IsLowAttack_Anim_State);
        }
            
    }

        public bool IsAttackingFull => anim.GetBool(StaticStrings.IsAttacking_Anim_State); // using is attacking anim param

        //[ShowInInspector, ReadOnly] bool c_isRunning;

        public virtual void Init(GameObject m, Character e, Sex s, int h, int weapon_p, Sprite icon)
        {
            model = m;
            anim = m.GetComponentInChildren<Animator>();
            puppet = m.GetComponentInChildren<PuppetMaster>();
            sex = s;
            IconImg.sprite = icon;

            enemy = e;
            health = h;
            power = weapon_p;
            GameManager.instance.OnGameStart += OnGamestart;

            
            healthSlider.maxValue = health;
            healthSlider.value = health;
            healthText.text = health.ToString();
        }

        #region Swipe Gesture Functions

        public virtual void SwipeNorth(LeanFinger finger)
        {
            //debugText.text = "UP";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeUpAttack(), animCrossFadeTime);
        }

        public virtual void SwipeNorthEast(LeanFinger finger)
        {
            //debugText.text = "Up Right";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeUpRightAttack(), animCrossFadeTime);
        }

        public virtual void SwipeEast(LeanFinger finger)
        {
            //debugText.text = "Right";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeRightAttack(), animCrossFadeTime, 0, 0, .15f);
        }

        public virtual void SwipeSouthEast(LeanFinger finger)
        {
            //debugText.text = "Down Right";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeDownRightAttack(), animCrossFadeTime);
        }

        public virtual void SwipeSouth(LeanFinger finger)
        {
            //debugText.text = "Down";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeDownAttack(), animCrossFadeTime);
        }

        public virtual void SwipeSouthWest(LeanFinger finger)
        {
            //debugText.text = "Down Left";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipDownLeftAttack(), animCrossFadeTime);
        }


        public virtual void SwipeWest(LeanFinger finger)
        {
            //debugText.text = "Swipe Left - Parry";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || anim.GetBool(StaticStrings.IsParrying_Anim_State)) return;
            int hashNo = StaticStrings.GetDodge(enemy/*, out bool goodDodge*/);

            swipedDodge = true;
            anim.CrossFade(hashNo, animCrossFadeTime);

            /*if (GetEnemyIsAttacking())
            {
                Debug.Log("Lamba");
                enemy.IOnDeactivateWeapon();
            }*/

            GameManager.instance.PlaySFX(3, (sex == Sex.male) ?
                Random.Range(male_voice_pitchRange.x, male_voice_pitchRange.y) : Random.Range(female_voice_pitchRange.x, female_voice_pitchRange.y), sfxSource);

        }

        public virtual void SwipeNorthWest(LeanFinger finger)
        {
            //debugText.text = "Up Left";
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame || IsAttacking) return;
            anim.CrossFade(StaticStrings.GetSwipeUpLeftAttack(), animCrossFadeTime);
        }

        #endregion

        public virtual void OnGamestart()
        {
            anim.SetTrigger(StaticStrings.GameStart_Anim_State);
        }

        private void OnDisable()
        {
            GameManager.instance.OnGameStart -= OnGamestart;
        }

        public virtual void IOnActivateWeapon()
        {
            if (GameManager.instance.GameOver || !GameManager.instance.StartGame) return;
            //Debug.Log("Activating Weapon");
            weapon.Activate(true);
            enemy.CanParry();
            GameManager.instance.PlaySFX(3, (sex == Sex.male) ?
                Random.Range(male_voice_pitchRange.x, male_voice_pitchRange.y) : Random.Range(female_voice_pitchRange.x, female_voice_pitchRange.y), sfxSource);
        }

        public virtual void IOnDeactivateWeapon()
        {
            //Debug.Log("Weapon deactivated");
            weapon.Activate(false);
        }

        public virtual void IOCharacterDodge()
        {
            if (enemy.IsAttacking)
            {
                //Debug.Log("Dodging");
                enemy.IOnDeactivateWeapon();
            }
            //enemy.IOnDeactivateWeapon();
        }

        /*protected bool GetEnemyIsAttacking()
        {
            return (enemy.Anim.GetBool(StaticStrings.IsHighAttack_Anim_State) ||
                    enemy.Anim.GetBool(StaticStrings.IsMidAttack_Anim_State) ||
                    enemy.Anim.GetBool(StaticStrings.IsLowAttack_Anim_State));
        }*/

        public abstract void CanParry();

        public virtual void OnCollision(int damage, Collision c)
        {
            //if (GameManager.instance.GameOver) return;

            if (GameManager.instance.GameOver || !GameManager.instance.StartGame) return;
            health -= damage;
            anim.CrossFade(StaticStrings.GetHitReaction(enemy.anim), animCrossFadeTime);
            GameManager.instance.PlaySFX(1, Random.Range(pitchRange.x, pitchRange.y));
            //GameManager.instance.ShakeCam();
            hitInfo = c;
            if (health <= 0)
            {
                playerDead = true;
                health = 0;
                StartCoroutine(AwaitDie());
                GameManager.instance.SetTimeScale(false);
            }
            UpdateHealthUI();
        }

        IEnumerator AwaitDie()
        {
            yield return new WaitForSeconds(0.15f);
            //GameManager.instance.PlaySFX((sex == Sex.male) ? 4 : 5, 1f);
            GameManager.instance.PlaySFX(4, (sex == Sex.male) ?
                Random.Range(male_voice_pitchRange.x, male_voice_pitchRange.y) : Random.Range(female_voice_pitchRange.x, female_voice_pitchRange.y), sfxSource);
            puppet.Kill();
            GameManager.instance.PlayFX(1, hitInfo.GetContact(0).point, Quaternion.LookRotation(Vector3.up),hitInfo.transform, true);
            enemy.OnCharacterWin();
            OnCharacterDead();
            yield return new WaitForSeconds(1f);
            enemy.anim.applyRootMotion = true;
        }

        public abstract void OnCharacterDead();

        public virtual void OnCharacterWin()
        {
            GameManager.instance.PlaySFX(5, (sex == Sex.male) ?
                Random.Range(male_voice_pitchRange.x, male_voice_pitchRange.y) : Random.Range(female_voice_pitchRange.x, female_voice_pitchRange.y), sfxSource);
        }

      /*  protected IEnumerator AwaitDeactivateWeapon()
        {
            yield return new WaitForSeconds(.4f);
            weapon.Activate(false); // = false;
        }*/

        public void UpdateHealthUI()
        {
            healthText.text = health.ToString();
            healthSlider.value = health;
        }

        public void UpdateHealthUI(int h)
        {
            health = h;

            healthSlider.maxValue = health;
            healthSlider.value = health;
            healthText.text = health.ToString();
        }

        public void UpdatePowerUI(int p)
        {
            power = p;

            powerSlider.maxValue = power;

            powerSlider.value = power;
            powerText.text = power.ToString();
        }

        public virtual void OnRevive()
        {
            puppet.mode = PuppetMaster.Mode.Disabled;
            Anim.applyRootMotion = false;
            Anim.transform.localPosition = Vector3.Lerp(Anim.transform.localPosition, Vector3.zero, 1f);
            anim.CrossFade(StaticStrings.IsIdle_Anim_State, animCrossFadeTime);
            OnGamestart();
            playerDead = false;
            puppet.mode = PuppetMaster.Mode.Active;
        }

    }
}
