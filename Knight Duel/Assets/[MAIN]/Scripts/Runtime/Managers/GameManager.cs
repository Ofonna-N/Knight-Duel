using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Doozy.Engine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;
using Cinemachine;
//using System;
using UnityEngine;

namespace KnightDuel
{
    public class GameManager : MonoBehaviour
    {

        #region Variables

        #region Game Manager
        public static GameManager instance;

        //[Header("===================GAME DATA==================")]

        [TabGroup("Tabs", "Game Manager")]
        [BoxGroup("Tabs/Game Manager/GAME DATA", CenterLabel = true),
            InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
        [SerializeField] private ResourcesManager resourcesManager;
        public ResourcesManager ResourcesManager => resourcesManager;

        [BoxGroup("Tabs/Game Manager/CONSTANT VARIABLES", CenterLabel = true), ShowInInspector, ReadOnly]
        public const int maxLevelPhases = 10;


        //[Header("===================CAMERA DATA==================")]

        [BoxGroup("Tabs/Game Manager/CAMERA DATA", CenterLabel = true)]
        [SerializeField] CinemachineVirtualCamera virtualCamera_main;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField] CinemachineVirtualCamera virtualCamera_player;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField] CinemachineVirtualCamera virtualCamera_agent;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField] CinemachineBasicMultiChannelPerlin mainCamNoise;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField] Transform camTarget;

        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField, Range(0, 2f)] float camShakeamplitude = .5f;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField, Range(0, 2f)] float camShakefrequency = 2f;
        [BoxGroup("Tabs/Game Manager/CAMERA DATA")]
        [SerializeField, Range(0, 2f)] float camShakeDuration = 2f;

        //[SerializeField] Cinemachine.CinemachineBasicMultiChannelPerlin mainCamNoiseChannel;

        //[Header("===================AGENT DATA==================")]
        [BoxGroup("Tabs/Game Manager/AGENT DATA", CenterLabel = true)]
        [SerializeField] Agent agent;
        public Agent Agent => agent;

        //[Header("===================PLAYER DATA==================")]
        [BoxGroup("Tabs/Game Manager/PLAYER DATA", CenterLabel = true)]
        [SerializeField] Player player;

        //========================= ENVIRONMENT =====================
        [BoxGroup("Tabs/Game Manager/ENVIRONMENT DATA", CenterLabel = true)]
        [SerializeField] Transform environmentHolder;
        [BoxGroup("Tabs/Game Manager/ENVIRONMENT DATA"),
            ToggleGroup(toggleMemberName: "Tabs/Game Manager/ENVIRONMENT DATA/slowMotion", groupTitle: "Enable Slow Motion")]
        [SerializeField] bool slowMotion;
        [BoxGroup("Tabs/Game Manager/ENVIRONMENT DATA"), Range(0f, 1f),
            ToggleGroup("Tabs/Game Manager/ENVIRONMENT DATA/slowMotion")]
        [SerializeField] float slowMoScale = 0.5f;
        [BoxGroup("Tabs/Game Manager/ENVIRONMENT DATA"), Range(0f, 1f),
            ToggleGroup("Tabs/Game Manager/ENVIRONMENT DATA/slowMotion")]
        [SerializeField] float slowmoDuration = 0.5f;


        //[Header("===================FX DATA==================")]
        [BoxGroup("Tabs/Game Manager/FX DATA", CenterLabel = true)]
        [SerializeField] private FX_Data[] inGameFX;

        //[Header("===================SFX DATA==================")]
        [BoxGroup("Tabs/Game Manager/SFX DATA", CenterLabel = true)]
        [SerializeField] private AudioSource sfxSource;
        [BoxGroup("Tabs/Game Manager/SFX DATA")]
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true)]
        private SFX_Data[] inGameSFX;
        #endregion

        #region UI Manager
        //============== UI DATA ========================================
        [TabGroup("Tabs", "UI Manager")]
        [BoxGroup("Tabs/UI Manager/UI DATA", CenterLabel = true)]
        [TitleGroup("Tabs/UI Manager/UI DATA/Coins", Alignment = TitleAlignments.Centered)]
        [SerializeField] TextMeshProUGUI cointext;

        //=============== GAME PLAY UI ===============================
        [TabGroup("Tabs/UI Manager/Tab", "Game Menu")]
        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Level UI Data", Alignment = TitleAlignments.Centered)]
        [SerializeField] TextMeshProUGUI levelText;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Phase UI Data", Alignment = TitleAlignments.Centered)]
        [SerializeField] Color PhaseReachedColor;//, PhaseUnReachedColor;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Phase UI Data")]
        [SerializeField] Image[] phaseIndicators;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Health", Alignment = TitleAlignments.Centered)]
        [SerializeField] GameObject healthPrice;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Health")]
        [SerializeField] GameObject healthWatchAD;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Health")]
        [SerializeField] TextMeshProUGUI healthPriceText;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Power", Alignment = TitleAlignments.Centered)]
        [SerializeField] GameObject powerPrice;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Power")]
        [SerializeField] GameObject powerWatchAD;

        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Power")]
        [SerializeField] TextMeshProUGUI powerPriceText;

        //============ARROW ANIM DATA
        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Arrow", Alignment = TitleAlignments.Centered)]
        [SerializeField] Image ArrowImg;
        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Arrow")]
        [SerializeField] float arrowFadDur = 0.35f;
        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Arrow")]
        [SerializeField] float arrowScaleEndValue = 1.35f;
        [TitleGroup("Tabs/UI Manager/Tab/Game Menu/Arrow")]
        [SerializeField] float arrowScaleDur = 0.35f;

        //==============UI SHOP DATA===================
        [BoxGroup("Tabs/UI Manager/SHOP DATA", CenterLabel = true)]
        [SerializeField] GameObject shopItemPrefab;

        [BoxGroup("Tabs/UI Manager/SHOP DATA"), Space]
        [SerializeField] Transform shopPreviewContentHolder;

        /// <summary>
        /// SKINS DATA
        /// </summary>
        [BoxGroup("Tabs/UI Manager/SHOP DATA"), Space]
        [SerializeField] Transform skinsContentHolder;
        [BoxGroup("Tabs/UI Manager/SHOP DATA")]
        [ShowInInspector, ReadOnly] ShopItem[] skinShopItems;
        [BoxGroup("Tabs/UI Manager/SHOP DATA")]
        [ShowInInspector, ReadOnly] GameObject[] skinShopPreviewItems;


        /// <summary>
        /// WEAPONS DATA
        /// </summary>
        [BoxGroup("Tabs/UI Manager/SHOP DATA"), Space]
        [SerializeField] Transform weaponsContentHolder;
        [BoxGroup("Tabs/UI Manager/SHOP DATA")]
        [ShowInInspector, ReadOnly] ShopItem[] weaponShopItems;
        [BoxGroup("Tabs/UI Manager/SHOP DATA")]
        [ShowInInspector, ReadOnly] GameObject[] weaponShopPreviewItems;

        //=========== UI DATA GAME OVER ============
        [TabGroup("Tabs/UI Manager/Tab", "Game Over")]
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Game Over Buttons", Alignment = TitleAlignments.Centered)]
        [SerializeField] private GameObject gameOverButton_continue;

        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Game Over Buttons")]
        [SerializeField] private GameObject gameOverButton_watchAd;

        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Level Phase Indicator", Alignment = TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI gameOverCurLevel_Text;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Level Phase Indicator")]
        [SerializeField] Image[] gameOverPhaseIndicators;


        //========= COIN UI 
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward", Alignment = TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI coinsRewardText;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] private TextMeshProUGUI winText;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] Image rewardItemBgImg, rewardItemFillImg;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] ParticleSystem rewardItemUnlockedFX;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] RewardItemClaimer rewardItemClaimbtn;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] GameObject rewardItemsView;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] private Ease rewardCoinAnimEase;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField, Range(.2f, 1f)] private float rewardCoinAnimDuration = 0.3f;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField, Range(0f, 1f)] private float rewardCoinAnimDelay = 0.3f;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField, Range(0f, 1f)] private float rewardCoinAnimDelayIncrement = 0.05f;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward"), MinMaxSlider(100f, 200f, showFields: true)]
        [SerializeField] private Vector2 rewardCoinsPosRadius = new Vector2(100f, 150f);
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] private float[] rewardCoinsAnimAngles;
        [TitleGroup("Tabs/UI Manager/Tab/Game Over/Reward")]
        [SerializeField] private RewardCoinImg[] rewardCoins_Objs;
        [ToggleGroup("Tabs/UI Manager/Tab/Game Over/Reward/animateCoinText", groupTitle: "Animate Coin Text")]
        [SerializeField] private bool animateCoinText;
        [ToggleGroup("Tabs/UI Manager/Tab/Game Over/Reward/animateCoinText"), Range(0f, 1.5f)]
        [SerializeField] private float coinTextAnimDuration = 0.35f;
        [ToggleGroup("Tabs/UI Manager/Tab/Game Over/Reward/animateCoinText"), Range(0f, 1.5f)]
        [SerializeField] private float coinTextAnimDelay = 0.15f;

        //========================= GAME STATE DATA ========================================
        [BoxGroup("Tabs/Game Manager/GAME STATE DATA", CenterLabel = true)]
        [ShowInInspector, ReadOnly] private bool gameStarted;
        public bool StartGame
        {
            get => gameStarted;
            set
            {
                if (value && !gameStarted)
                {
                    OnGameStart();
                    gameStarted = true;
                }
            }
        }

        [BoxGroup("Tabs/Game Manager/GAME STATE DATA")]
        [ShowInInspector, ReadOnly] bool hasRevived;
        public bool HasRevived => hasRevived;

        [BoxGroup("Tabs/Game Manager/GAME STATE DATA")]
        [ShowInInspector, ReadOnly] public bool GameOver { get; set; }

        public System.Action OnGameStart;
        #endregion

        #region Settings Manager
        //====================SETTINGS MANAGER
        [TabGroup("Tabs", "Settings Manager")]
        [BoxGroup("Tabs/Settings Manager/SETTINGS MANAGER", CenterLabel = true), HideLabel]
        [SerializeField] private SettingsManager settingsManager;
        #endregion

        #endregion



        void Start()
        {
            if (GameManager.instance == null)
            {
                GameManager.instance = this;
            }


            Init();
        }


        private void Init()
        {
            SetTimeScale(true);
            mainCamNoise = virtualCamera_main.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            InitLevel();
            //InitPlayer();
            InitAgent();
            settingsManager.Init();
            StartCoroutine(settingsManager.ShowTutorial());
            //Invoke()
            UpdateUpgrader();
            InitShop();
            InitRewardCoinsImg();
            mainCamNoise.m_AmplitudeGain = 0f;
            mainCamNoise.m_FrequencyGain = 0f;
            SetAgentsPowerSliders();
            // = resourcesManager.Levels[resourcesManager.SelectedLevel].PhaseNo;
            
        }

        /*private void InitPlayer()
        {
            SkinData skin = resourcesManager.Skins[resourcesManager.SelectedSkin];

            GameObject model = Instantiate(skin.Prefab, player.transform, false);

            player.Init(model, Agent, skin.Sex, resourcesManager.UpgradableData.Health_Player, resourcesManager.UpgradableData.Power_Player);

            virtualCamera_player.LookAt = player.Anim.transform;

        }*/

        private void InitAgent()
        {
            LevelData l = resourcesManager.Levels[resourcesManager.SelectedLevel];

            int index = Random.Range(l.AgentsIndexRange.x,
                l.AgentsIndexRange.y);

            AgentData a = l.GetAgent(resourcesManager.Agents, out int weaponIndex);

            GameObject model = Instantiate(a.Prefab, Agent.transform, false);

            Agent.Init(model, player, a.Sex, resourcesManager.UpgradableData.Health_Agent, weaponIndex,
                resourcesManager.UpgradableData.Power_Agent, a.Icon);

            virtualCamera_agent.LookAt = Agent.Anim.transform;
        }

        private void InitLevel()
        {
            LevelData l = resourcesManager.Levels[resourcesManager.SelectedLevel];

            levelText.text = "LEVEL " + (ResourcesManager.SelectedLevel + 1);

            SDK_Implementation.instance?.LevelStarted(resourcesManager.SelectedLevel + 1, l.PhaseNo);

            SDK_Implementation.instance?.RoundStarted(l.PhaseNo);

            GameObject model = Instantiate(l.Prefab, environmentHolder.transform, false);

            for (int i = 0; i < l.PhaseNo; i++)
            {
                phaseIndicators[i].color = PhaseReachedColor;
                /*if (i <= l.PhaseNo - 1)
                {
                    phaseIndicators[i].color = PhaseReachedColor;
                }
                else
                {
                    phaseIndicators[i].color = PhaseUnReachedColor;
                }*/
            }
        }

        private void SetPlayer()
        {
            if (player.Model != null) Destroy(player.Model);

            SkinData skin = resourcesManager.Skins[resourcesManager.SelectedSkin];

            GameObject model = Instantiate(skin.Prefab, player.transform, false);

            player.Init(model, Agent, skin.Sex, resourcesManager.UpgradableData.Health_Player,
                resourcesManager.UpgradableData.Power_Player, skin.Icon);
        }

        /// <summary>
        /// needed due to the fact that we keep upgrading power dependent on their individual powers
        /// power max value for both is determined by the agent with the max power
        /// </summary>
        private void SetAgentsPowerSliders()
        {
            if (player.Weapon.Power > agent.Weapon.Power)
            {
                player.PowerSlider.maxValue = player.Weapon.Power;
                agent.PowerSlider.maxValue = player.Weapon.Power;
            }
            else
            {
                player.PowerSlider.maxValue = agent.Weapon.Power;
                agent.PowerSlider.maxValue = agent.Weapon.Power;
            }
        }

        public void SetTimeScale(bool reset)
        {
            if (slowMotion)
            {
                if (reset)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    DOTween.To(() => Time.timeScale, (x) => Time.timeScale = x, slowMoScale, 0.25f).
                    OnComplete(() => DOTween.To(() => Time.timeScale, (x) => Time.timeScale = x, 1f, 0.25f).SetDelay(slowmoDuration));
                }
            }
        }

        public GameObject GetWeapon(Transform r_hand)
        {
            GameObject weapon = Instantiate(resourcesManager.Weapons[resourcesManager.SelectedWeapon].Prefab, r_hand, false);

            weapon.transform.localPosition = resourcesManager.Weapons[resourcesManager.SelectedWeapon].Postion;
            weapon.transform.localEulerAngles = resourcesManager.Weapons[resourcesManager.SelectedWeapon].Rotation;
            weapon.transform.localScale = resourcesManager.Weapons[resourcesManager.SelectedWeapon].Scale;

            return weapon;
        }

        public GameObject GetWeapon(Transform r_hand, int index)
        {
            GameObject weapon = Instantiate(resourcesManager.Weapons[index].Prefab, r_hand, false);

            weapon.transform.localPosition = resourcesManager.Weapons[index].Postion;
            weapon.transform.localEulerAngles = resourcesManager.Weapons[index].Rotation;
            weapon.transform.localScale = resourcesManager.Weapons[index].Scale;

            return weapon;
        }

        public void PlayFX(int index, Vector3 pos, Quaternion rot, Transform parent = null, bool setParent = false, int subIndex = -1, bool setLoc = true)
        {
            ParticleSystem p = inGameFX[index].Fx[Random.Range(0, inGameFX[index].Fx.Length)];

            if (subIndex != -1)
            {
                p = inGameFX[index].Fx[subIndex];
            }


            if (setLoc)
            {
                p.transform.localPosition = pos;
                p.transform.localRotation = rot;
            }

            if (setParent) p.transform.parent = parent;
            p.Play(true);
        }

        public void StopFX(int index, Vector3 pos, Quaternion rot, Transform parent = null, bool setParent = false)
        {
            ParticleSystem[] p = inGameFX[index].Fx;

            for (int i = 0; i < p.Length; i++)
            {
                p[i].transform.position = pos;
                p[i].transform.rotation = rot;
                if (setParent) p[i].transform.parent = parent;
                p[i].Stop(true);
            }

        }

        public void PlaySFX(int index, float pitch, AudioSource s = null, int subIndex = -1)
        {
            if (s == null) s = sfxSource;
            s.pitch = pitch;

            if (subIndex == -1)
            {
                s.PlayOneShot(inGameSFX[index].Clip[Random.Range(0, inGameSFX[index].Clip.Length)]);
            }
            else
            {
                s.PlayOneShot(inGameSFX[index].Clip[subIndex]);
            }
        }

        public void ShakeCam()
        {
            //camTarget.DOShakePosition(camShakeDuration, strength: camShakeStrength, vibrato:camShakevibrato).SetEase(camShakeEase);

            //mainCamNoise.m_AmplitudeGain.

            DOTween.To(() => mainCamNoise.m_AmplitudeGain,
                x => mainCamNoise.m_AmplitudeGain = x, camShakeamplitude, camShakeDuration).
                OnComplete(() => mainCamNoise.m_AmplitudeGain = 0);

            DOTween.To(() => mainCamNoise.m_FrequencyGain,
                x => mainCamNoise.m_FrequencyGain = x, camShakefrequency, camShakeDuration).
                OnComplete(() => mainCamNoise.m_FrequencyGain = 0);
        }

        //[ButtonGroup("Tabs/UI Manager/Tab/Game Menu/Arrow/ Animate Arrow")]
        public void AnimateArrow(float degrees, bool on = true)
        {
            ArrowImg.rectTransform.localEulerAngles = new Vector3(0f, 0f, degrees);

            int aplha = (on) ? 1 : 0;
            ArrowImg.CrossFadeAlpha(alpha: aplha, arrowFadDur, true);
            ArrowImg.rectTransform.DOPunchScale(Vector3.one * arrowScaleEndValue, arrowScaleDur)
                .OnComplete(() => ArrowImg.CrossFadeAlpha(0f, arrowFadDur, true));
        }

        public void OnGameOver(bool win)
        {
            GameOver = true;

            if (win)
            {
                //player.Anim.applyRootMotion = true;
                player.Anim.CrossFade(StaticStrings.GetGameOverAnim(), player.AnimCrossFadeTime);

                //SDK_Implementation.instance.LevelComplete(resourcesManager.SelectedLevel + 1);
                SDK_Implementation.instance?.RoundComplete(resourcesManager.Levels[resourcesManager.SelectedLevel].PhaseNo);
                virtualCamera_player.gameObject.SetActive(true);
                virtualCamera_agent.gameObject.SetActive(false);
                virtualCamera_main.gameObject.SetActive(false);

                UpdateLevel();

                GameEventMessage.SendEvent(StaticStrings.OnGameOverWin_Event);
            }
            else
            {
                Agent.OnGameEnd();
                SDK_Implementation.instance?.LevelFailed(resourcesManager.SelectedLevel + 1, resourcesManager.Levels[resourcesManager.SelectedLevel].PhaseNo); ;
                
                if (hasRevived)
                {
                    gameOverButton_continue.SetActive(true);
                    gameOverButton_watchAd.SetActive(false);
                }
                //Agent.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
                //Agent.Anim.applyRootMotion = true;
                Agent.Anim.CrossFade(StaticStrings.GetGameOverAnim(), player.AnimCrossFadeTime);

                resourcesManager.Coins += resourcesManager.Upgrader.CoinsIncrementLose;
                coinsRewardText.text = $"REWARD: {StaticStrings.TMP_CoinIconFormat}  {resourcesManager.UpgradableData.Coins_Reward}";

                virtualCamera_agent.gameObject.SetActive(true);
                virtualCamera_player.gameObject.SetActive(false);
                virtualCamera_main.gameObject.SetActive(false);
                GameEventMessage.SendEvent(StaticStrings.OnGameOverLose_Event);
            }

            cointext.text = $"{StaticStrings.TMP_CoinIconFormat}  {resourcesManager.Coins}";
        }

        public void LoadLevel(string level)
        {
            SceneManager.LoadScene(level);
        }

        public void RevivePlayer()
        {
            AdsManager.Instance.ShowAd(AdType.RewardedVideo, OnPlayerRevive);
        }

        private void OnPlayerRevive()
        {
            if (!hasRevived)
            {
                GameEventMessage.SendEvent(StaticStrings.OnPlayerRevive_Event);
                virtualCamera_player.gameObject.SetActive(false);
                virtualCamera_agent.gameObject.SetActive(false);
                virtualCamera_main.gameObject.SetActive(true);
                StopFX(1, Vector3.zero, Quaternion.identity, null, true);
                GameOver = false;
                player.OnRevive();
                Agent.OnRevive();
                hasRevived = true;
                SDK_Implementation.instance?.ACTIONPerformed("Player Revived");
            }
            else
            {
                SceneManager.LoadScene("SCENE 1");
            }


        }


        #region Game Over Functions

        private void InitRewardCoinsImg()
        {
            for (int i = 0; i < rewardCoins_Objs.Length; i++)
            {
                rewardCoins_Objs[i].gameObject.SetActive(false);
            }
        }

        private void UpdateLevel()
        {
            LevelData l = resourcesManager.Levels[resourcesManager.SelectedLevel];
            rewardItemFillImg.fillAmount = (1f / 10f) * l.PhaseNo;


            //resourcesManager.UpgradableData.Coins_power += resourcesManager.Upgrader.CoinsIncrementWin;
            resourcesManager.Coins += resourcesManager.UpgradableData.Coins_Reward;
            coinsRewardText.text = $"REWARD: {StaticStrings.TMP_CoinIconFormat}  {resourcesManager.UpgradableData.Coins_Reward}";
            resourcesManager.UpgradableData.Coins_Reward += resourcesManager.Upgrader.CoinsRewardIncrementWin;
            rewardItemClaimbtn.Button.SetActive(false);

            l.PhaseNo += 1;

            winText.text = "YOU WIN!";

            var runtimePhaseNo = l.PhaseNo;
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            resourcesManager.UpgradableData.Health_Agent += resourcesManager.Upgrader.HealthIncrement_Agent;
            resourcesManager.UpgradableData.Power_Agent += resourcesManager.Upgrader.PowerIncrememt_Agent;
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            gameOverCurLevel_Text.text = $"{resourcesManager.SelectedLevel + 1}";

            SetRewardedItemInfo(runtimePhaseNo);

            if (l.LevelComplete)
            {
                runtimePhaseNo = 10;
                //rewardItemClaimbtn.ItemIndex = resourcesManager.SelectedLevel;
                SetRewardedItemInfo(runtimePhaseNo);
                resourcesManager.SelectedLevel += 1;
                
                winText.text = "LEVEL CLEARED!";
                rewardItemClaimbtn.Button.SetActive(true);
                if (resourcesManager.SelectedLevel >= resourcesManager.Levels.Length)
                {
                    resourcesManager.SelectedLevel = 0;
                    for (int i = 0; i < resourcesManager.Levels.Length; i++)
                    {
                        resourcesManager.Levels[i].LevelComplete = false;
                    }
                }
            }


            for (int i = 0; i < runtimePhaseNo; i++)
            {
                gameOverPhaseIndicators[i].color = PhaseReachedColor;
            }

        }

        private void SetRewardedItemInfo(int phaseNo)
        {
            Sprite setVal = null;

            Debug.Log("Phase: " + phaseNo);

            Sprite skinSprite = null;
            Sprite weaponsSprite = null;

            for (int i = 0; i < skinShopItems.Length; i++)
            {
                if (skinShopItems[i].UnlockLevel == resourcesManager.SelectedLevel + 1)
                {
                    skinSprite = skinShopItems[i].Icon;
                    rewardItemClaimbtn.ItemIndex = i;
                    rewardItemClaimbtn.Type = ItemType.Skin;
                    rewardItemClaimbtn.shopItems = skinShopItems;
                    if (phaseNo == maxLevelPhases)
                    {
                        Debug.Log("Level Cleared, Unlock: " + resourcesManager.Skins[i].ID);
                        resourcesManager.Skins[i].IsUnlocked = true;
                        SDK_Implementation.instance?.ContentUnlocked(resourcesManager.Skins[i].ID);
                    }
                    Debug.Log("Skin is Sprite: " + i);
                }
            }

            for (int i = 0; i < weaponShopItems.Length; i++)
            {
                if (weaponShopItems[i].UnlockLevel == resourcesManager.SelectedLevel + 1)
                {
                    weaponsSprite = weaponShopItems[i].Icon;
                    rewardItemClaimbtn.ItemIndex = i;
                    rewardItemClaimbtn.Type = ItemType.Weapon;
                    rewardItemClaimbtn.shopItems = weaponShopItems;
                    if (phaseNo == maxLevelPhases)
                    {
                        Debug.Log("Level Cleared, Unlock: " + resourcesManager.Weapons[i].ID);
                        resourcesManager.Weapons[i].IsUnlocked = true;
                        SDK_Implementation.instance?.ContentUnlocked(resourcesManager.Weapons[i].ID);
                    }
                    Debug.Log("weapon is Sprite: " + i);
                }
            }


            if (skinSprite == null && weaponsSprite == null)
            {
                rewardItemsView.SetActive(false); //hide reward item parent on game over
                return;
            }
            else if(skinSprite != null)
            {
                setVal = skinSprite;    //if we have a skin sprite at the unlock level index of current level then activate
                rewardItemUnlockedFX.gameObject.SetActive(false);
            }
            else if (weaponsSprite != null)
            {
                setVal = weaponsSprite;
                rewardItemUnlockedFX.gameObject.SetActive(false);
            }

            if (phaseNo == 10)
            {
                rewardItemUnlockedFX.gameObject.SetActive(true); //if level cleared then activate reward item particle
                SDK_Implementation.instance?.LevelComplete(resourcesManager.SelectedLevel + 1);
            }

            float fillAmount = (1f / 10f) * phaseNo;

            
            rewardItemBgImg.sprite = setVal;
            rewardItemFillImg.sprite = setVal;
            DOTween.To(() => rewardItemFillImg.fillAmount, (x) => rewardItemFillImg.fillAmount = x, fillAmount, 3f);


            //return retVal;
        }

        public void AnimateRewardCoins()
        {
            float delay = rewardCoinAnimDelay;

            for (int i = 0; i < rewardCoins_Objs.Length; i++)
            {
                rewardCoins_Objs[i].AnimateCoin(rewardCoinsAnimAngles, rewardCoinsPosRadius, rewardCoinAnimDuration,
                    rewardCoinAnimEase, delay, cointext.rectTransform);
                delay += rewardCoinAnimDelayIncrement;

            }

            if (animateCoinText) cointext.DOText(cointext.text, coinTextAnimDuration, scrambleMode: ScrambleMode.Numerals).
                    SetDelay(coinTextAnimDelay);
        }

        #endregion

        #region Settings Functions

        public void SetSfxSettings()
        {
            settingsManager.SetSfxSettings();
        }

        public void SetVibrateSettings()
        {
            settingsManager.SetVibrateSettings();
        }
        #endregion

        #region UpgradeFunctions

        private void UpdateUpgrader()
        {
            UpgradableData ug_data = resourcesManager.UpgradableData;

            cointext.text = $"{StaticStrings.TMP_CoinIconFormat}  {resourcesManager.Coins}";
            healthPriceText.text = $"{StaticStrings.TMP_CoinIconFormat} {ug_data.Health_Price}";
            powerPriceText.text = $"{StaticStrings.TMP_CoinIconFormat} {ug_data.Power_Price}";

            if (resourcesManager.Coins >= ug_data.Health_Price)
            {
                healthPrice.SetActive(true);
                healthWatchAD.SetActive(false);
            }
            else
            {
                healthPrice.SetActive(false);
                healthWatchAD.SetActive(true);
            }

            if (resourcesManager.Coins >= ug_data.Power_Price)
            {
                powerPrice.SetActive(true);
                powerWatchAD.SetActive(false);
            }
            else
            {
                powerPrice.SetActive(false);
                powerWatchAD.SetActive(true);
            }
        }

        public void UpgradeHealth()
        {
            UpgradableData ug_data = resourcesManager.UpgradableData;

            if (resourcesManager.Coins >= ug_data.Health_Price)
            {
                //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
                resourcesManager.Coins -= ug_data.Health_Price;
                //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
                OnHealthUpgrade(ug_data);
                //inGameFX[2].Fx[1].Play(true);
            }
            else
            {
                Debug.Log("SHOWING AD");
                AdsManager.Instance.ShowAd(AdType.RewardedVideo, () => OnHealthUpgrade(ug_data));
            }

            UpdateUpgrader();
        }

        private void OnHealthUpgrade(UpgradableData ug_data)
        {
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            ug_data.Health_Player += resourcesManager.Upgrader.HealthIncrement_Player;
            ug_data.Health_Price += resourcesManager.Upgrader.UpgradeIncrementPrice;
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            player.UpdateHealthUI(ug_data.Health_Player);
            PlayFX(2, Vector3.zero, Quaternion.identity, subIndex: 1, setLoc: false);
            PlaySFX(6, 1, subIndex: 0);
            SDK_Implementation.instance?.UpgradePurchased("Health Upgrade");
        }

        public void UprgradePower()
        {
            UpgradableData ug_data = resourcesManager.UpgradableData;

            if (resourcesManager.Coins >= ug_data.Power_Price)
            {
                
                resourcesManager.Coins -= ug_data.Power_Price;
                
                OnPowerUpgrade(ug_data);
            }
            else
            {
                Debug.Log("SHOWING AD");
                AdsManager.Instance.ShowAd(AdType.RewardedVideo, () => OnPowerUpgrade(ug_data));
            }

            UpdateUpgrader();
        }

        private void OnPowerUpgrade(UpgradableData ug_data)
        {
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            ug_data.Power_Player += resourcesManager.Upgrader.PowerIncrememt_Player;
            ug_data.Power_Price += resourcesManager.Upgrader.UpgradeIncrementPrice;
            //============================================================================================================================ TEMPORARILY TURNED OFF FOR TESTING
            player.Weapon.UpdatePower(ug_data.Power_Player);
            SetAgentsPowerSliders();
            //inGameFX[2].Fx[0].Play(true);
            PlayFX(2, Vector3.zero, Quaternion.identity, subIndex: 0, setLoc: false);
            PlaySFX(6, 1, subIndex: 1);
            SDK_Implementation.instance?.UpgradePurchased("Power Upgrade");
        }

        #endregion

        #region Shop Functions

        bool shopIsInit;

        private void InitShop()
        {
            
            InitSkinsShopItems(resourcesManager.Skins);
            InitWeaponsShopItems(resourcesManager.Weapons);
            UpdateShopPreview();
            shopPreviewContentHolder.gameObject.SetActive(false); //set shop items holder inactive in order to get animator right hand to place sword
            shopIsInit = true;
        }

        private void InitSkinsShopItems(SkinData[] skinDatas)
        {
            skinShopItems = new ShopItem[skinDatas.Length];
            skinShopPreviewItems = new GameObject[skinDatas.Length];

            for (int i = 0; i < skinDatas.Length; i++)
            {
                ShopItem item = Instantiate(shopItemPrefab, skinsContentHolder, false).GetComponent<ShopItem>(); // instantiate shop item ui into grid
                GameObject previewItem = Instantiate(skinDatas[i].ShopPrefab, shopPreviewContentHolder, false); // instantiate prefab for shop view
                previewItem.SetActive(false);

                item.Init(skinDatas[i].Icon, skinDatas[i].UnlockType, skinDatas[i].Type, skinDatas[i].IsUnlocked, 
                    skinDatas[i].IsClaimed, skinDatas[i].UnlockLevel, resourcesManager.SelectedSkin == i, i);

                skinShopItems[i] = item;
                skinShopPreviewItems[i] = previewItem;
            }
        }

        private void InitWeaponsShopItems(WeaponData[] weaponDatas)
        {
            weaponShopItems = new ShopItem[weaponDatas.Length];
            weaponShopPreviewItems = new GameObject[weaponDatas.Length];

            for (int i = 0; i < weaponDatas.Length; i++)
            {
                ShopItem item = Instantiate(shopItemPrefab, weaponsContentHolder, false).GetComponent<ShopItem>();
                GameObject previewItem = Instantiate(weaponDatas[i].ShopPrefab, shopPreviewContentHolder, false);
                previewItem.SetActive(false);

                item.Init(weaponDatas[i].Icon, weaponDatas[i].UnlockType, weaponDatas[i].Type, weaponDatas[i].IsUnlocked,
                     weaponDatas[i].IsClaimed, weaponDatas[i].UnlockLevel, (resourcesManager.SelectedWeapon == i), i);

                weaponShopItems[i] = item;
                weaponShopPreviewItems[i] = previewItem;
            }
        }

        private void UpdateShopPreview()
        {
            //PositionPreviewWeapon(resourcesManager.SelectedWeapon);

            int selectedSkin = resourcesManager.SelectedSkin;
            int selectedWeapon = resourcesManager.SelectedWeapon;

            skinShopPreviewItems[selectedSkin].SetActive(true);
            weaponShopPreviewItems[selectedWeapon].SetActive(true);

            var anim = skinShopPreviewItems[selectedSkin].GetComponent<Animator>();
            var weapon = weaponShopPreviewItems[selectedWeapon].transform;
            var weapon_Data = resourcesManager.Weapons[selectedWeapon];
            weapon.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
            weapon.localPosition = weapon_Data.Postion;
            weapon.localEulerAngles = weapon_Data.Rotation;
            weapon.localScale = weapon_Data.Scale;

            for (int i = 0; i < skinShopPreviewItems.Length; i++)
            {
                if (i == selectedSkin) continue;

                skinShopPreviewItems[i].SetActive(false);
            }

            for (int i = 0; i < weaponShopPreviewItems.Length; i++)
            {
                if (i == selectedWeapon) continue;

                weaponShopPreviewItems[i].SetActive(false);
            }

        }

        public void SelectShopItem(ItemType type, int index)
        {
            switch (type)
            {
                case ItemType.Skin:
                    resourcesManager.SelectedSkin = index;
                    //Debug.Log("Change Player");
                    SetPlayer();

                    

                    break;
                case ItemType.Weapon:
                    resourcesManager.SelectedWeapon = index;
                    player.SetWeapon(resourcesManager.UpgradableData.Power_Player);

                    //PositionPreviewWeapon(index);
                    
                    break;
                default:
                    break;
            }

            if(shopIsInit) UpdateShopPreview();
        }

        public void DeselectOtherShopItems(ItemType type, int index )
        {
            switch (type)
            {
                case ItemType.Skin:
                    for (int i = 0; i < skinShopItems.Length; i++)
                    {
                        if (skinShopItems[i] == null) break;
                        /*if (i == index)
                        {
                            //skinShopPreviewItems[index].SetActive(true);
                            continue;
                        }*/
                        //if(shopIsInit) skinShopPreviewItems[index].SetActive(false);
                        skinShopItems[i].Deselect();
                    }
                    break;
                case ItemType.Weapon:
                    for (int i = 0; i < weaponShopItems.Length; i++)
                    {
                        if (weaponShopItems[i] == null) break;
                        /*if (i == index)
                        {
                            //weaponShopPreviewItems[index].SetActive(true);
                            continue;
                        }*/
                        //if(shopIsInit) weaponShopPreviewItems[index].SetActive(false);
                        weaponShopItems[i].Deselect();
                    }
                    break;
                default:
                    break;
            }
        }

        

        #endregion
    }

    public enum Sex { male, female }

    public enum UnlockType { Level, Ad }

    public enum ItemType { Skin, Weapon }

    public enum Platform { Android, IOS }

    public enum AdType { Banner, Interstitial, RewardedVideo }

    [System.Serializable]
    public class FX_Data
    {
        [SerializeField] string id;
        [SerializeField] ParticleSystem[] fx;
        public ParticleSystem[] Fx => fx;
    }

    [System.Serializable]
    public class SFX_Data
    {
        [SerializeField] string id;
        [SerializeField, InlineEditor(InlineEditorModes.SmallPreview)] AudioClip[] clip;
        public AudioClip[] Clip => clip;
    }

    [System.Serializable]
    public class SettingsManager
    {
        [SerializeField] AudioMixer sfxMixer;
        /*[ShowInInspector, PropertyRange(0, -80f), ReadOnly]
        private float sfxVolume;*/
        [SerializeField] Color onColor, offColor;
        [SerializeField] TextMeshProUGUI sfxText, vibrateText;
        [SerializeField] Image sfxImg, vibrateImg;

        public void Init()
        {
            UpdateSfxSettingsUI(GameManager.instance.ResourcesManager.SettingsData.SfxOn);
            UpdateVibrateSettingsUI(GameManager.instance.ResourcesManager.SettingsData.ViberateOn);
        }

        public IEnumerator ShowTutorial()
        {
            yield return null;

            if (!GameManager.instance.ResourcesManager.SettingsData.HasViewedTutorial)
            {
                GameManager.instance.ResourcesManager.SettingsData.HasViewedTutorial = true;
                GameEventMessage.SendEvent(StaticStrings.VeiwTutorial_Event);
                SDK_Implementation.instance?.TutorialComplete();
            }
        }

        public void SetSfxSettings()
        { 
            
            bool on = GameManager.instance.ResourcesManager.SettingsData.SfxOn = !GameManager.instance.ResourcesManager.SettingsData.SfxOn;

            UpdateSfxSettingsUI(on);
        }

        private void UpdateSfxSettingsUI(bool on)
        {
            if (on)
            {
                sfxText.color = onColor;
                sfxImg.color = onColor;
                sfxText.text = "SFX: ON";
                sfxMixer.SetFloat(StaticStrings.SFXVolume_Param, 0);
            }
            else
            {
                sfxText.color = offColor;
                sfxImg.color = offColor;
                sfxText.text = "SFX: OFF";
                sfxMixer.SetFloat(StaticStrings.SFXVolume_Param, -80);
            }
        }


        public void SetVibrateSettings()
        {
            bool on = GameManager.instance.ResourcesManager.SettingsData.ViberateOn = !GameManager.instance.ResourcesManager.SettingsData.ViberateOn;

            UpdateVibrateSettingsUI(on);
        }

        private void UpdateVibrateSettingsUI(bool on)
        {
            if (on)
            {
                vibrateText.color = onColor;
                vibrateImg.color = onColor;
                vibrateText.text = "VIBRATE: ON";
            }
            else
            {
                vibrateText.color = offColor;
                vibrateImg.color = offColor;
                vibrateText.text = "VIBRATE: OFF";
            }
        }

    }
}
