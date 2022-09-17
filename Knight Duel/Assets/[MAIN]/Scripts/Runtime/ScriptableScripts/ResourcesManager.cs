using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KnightDuel
{
    [CreateAssetMenu(fileName = "Resources Manager", menuName = "SmallyGames/Managers/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        [BoxGroup("COINS DATA", CenterLabel = true), GUIColor(1f, 0.8714468f, 0f, 1f), 
            LabelText("COINS"), InlineButton("RCoins", "Reset")]
        [SerializeField] int coins;
        public int Coins 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.Coins_Key))
                {
                    coins = ES3.Load<int>(StaticStrings.Coins_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.Coins_Key, coins);
                }
                return coins;
            }
            set => ES3.Save(StaticStrings.Coins_Key, coins = value); 
        }

        [BoxGroup("SKINS DATA", CenterLabel = true), MinValue(0), GUIColor(0.213f, 1f, 0.468f, 1), 
            LabelText("SELECTED SKIN"), PropertySpace(SpaceAfter = 10), InlineButton("RSkin", "Reset")]
        [SerializeField] private int selectedSkin;
        public int SelectedSkin 
        {
            get
            {
                if (ES3.KeyExists(StaticStrings.SelectedSkin_Key))
                {
                    selectedSkin = ES3.Load<int>(StaticStrings.SelectedSkin_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.SelectedSkin_Key, selectedSkin);
                }
                return selectedSkin;
            } 
            set => ES3.Save(StaticStrings.SelectedSkin_Key, selectedSkin = value); 
        }

        [BoxGroup("SKINS DATA"), ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true)]
        [SerializeField] SkinData[] skins;
        public SkinData[] Skins => skins;


        [BoxGroup("AGENTS DATA", CenterLabel = true), InlineButton("RAgents", "Reset"), 
            ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true)]
        [SerializeField] private AgentData[] agents;
        public AgentData[] Agents => agents;

        [BoxGroup("WEAPON DATA", CenterLabel = true), MinValue(0), GUIColor(0.213f, 1f, 0.468f, 1), 
            LabelText("SELECTED WEAPON"), PropertySpace(SpaceAfter = 10), InlineButton("RWeapon", "Reset")]
        [SerializeField] private int selectedWeapon;
        public int SelectedWeapon 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.SelectedWeapon_Key))
                {
                    selectedWeapon = ES3.Load<int>(StaticStrings.SelectedWeapon_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.SelectedWeapon_Key, selectedWeapon);
                }

                return selectedWeapon;
            }
            set => ES3.Save(StaticStrings.SelectedWeapon_Key, selectedWeapon = value);
        }

        [BoxGroup("WEAPON DATA"), ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true)]
        [SerializeField] private WeaponData[] weapons;
        public WeaponData[] Weapons => weapons;

        [BoxGroup("LEVELS DATA", CenterLabel = true), MinValue(0), GUIColor(0.213f, 1f, 0.468f, 1), 
            LabelText("SELECTED LEVEL"), PropertySpace(SpaceAfter = 10), InlineButton("RLevel", "Reset")]
        [SerializeField] private int selectedLevel;
        public int SelectedLevel 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.SelectedLevel_Key))
                {
                    selectedLevel = ES3.Load<int>(StaticStrings.SelectedLevel_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.SelectedLevel_Key, selectedLevel);
                }
                return selectedLevel;
            }
            set
            {
                /*for (int i = 0; i < agents.Length; i++)
                {
                    agents[i].IsUsed = false;
                }*/
                ES3.Save(StaticStrings.SelectedLevel_Key, selectedLevel = value);
            }
        }
        
        [BoxGroup("LEVELS DATA"), ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true)]
        [SerializeField] private LevelData[] levels;
        public LevelData[] Levels => levels;


        [BoxGroup("UPGRADABLE DATA", CenterLabel = true), HideLabel]
        [SerializeField] UpgradableData upgradableData;
        public UpgradableData UpgradableData => upgradableData;

        [ButtonGroup("UPGRADABLE DATA/Reset Data"), GUIColor(0.163f,0.567f,1,1)]
        private void ResetUpgradableData()
        {
            upgradableData.Health_Player = 100;
            upgradableData.Power_Player = 16;
            upgradableData.Health_Agent = 100;
            upgradableData.Power_Agent = 16;
            upgradableData.Health_Price = 100;
            upgradableData.Power_Price = 100;
            upgradableData.Coins_Reward = 200;
            //coins = 1000;
        }

        [BoxGroup("STATIC UPGRADER DATA", CenterLabel = true), HideLabel]
        [SerializeField] UpgraderData upgrader;
        public UpgraderData Upgrader => upgrader;


        [BoxGroup("SETTINGS DATA", CenterLabel = true), HideLabel]
        [SerializeField] SettingsData settingsData;
        public SettingsData SettingsData => settingsData;

        [Button("RESET RESOURCES MANAGER", ButtonHeight = 30), GUIColor(0.85f, 0, 0, 1)]
        private void ResetResources()
        {
            RCoins();
            RLevel();
            RWeapon();
            RSkin();
            RAgents();
            ResetUpgradableData();
        }

        private void RCoins()
        {
            coins = 100;
        }

        private void RLevel()
        {
            selectedLevel = 0;

            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].ID = StaticStrings.Level_Key_Suffix + i;
                levels[i].PhaseNo = 1;
                levels[i].LevelComplete = false;
            }
        }

        private void RWeapon()
        {
            SelectedWeapon = 0;
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].ID = StaticStrings.Weapon_Key_Suffix + i;
                if (i == 0) continue;
                weapons[i].IsUnlocked = false;
                weapons[i].IsClaimed = false;
            }
        }

        private void RSkin()
        {
            selectedSkin = 0;
            for (int i = 0; i < skins.Length; i++)
            {
                skins[i].ID = StaticStrings.Skin_Key_Suffix + i;
                if (i == 0) continue;
                skins[i].IsUnlocked = false;
                skins[i].IsClaimed = false;
            }
        }

        private void RAgents()
        {
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].ID = StaticStrings.Agent_Key_Suffix + i;
            }
        }
    }


    [System.Serializable]
    public class SkinData
    {
        //[HorizontalGroup("Main Char Data")]
        //es
        [HorizontalGroup("Left", Width = 120, PaddingLeft = 20), HideLabel]
        [SerializeField, InlineEditor(InlineEditorModes.SmallPreview, PreviewHeight = 60)] private Sprite icon;
        public Sprite Icon => icon;

        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField] private string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField] private ItemType type = ItemType.Skin;
        public ItemType Type => type;

        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField, EnumToggleButtons] private Sex sex;
        public Sex Sex => sex;

        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField] private GameObject prefab;
        public GameObject Prefab => prefab;

        [Title("Shop Data", TitleAlignment = TitleAlignments.Centered)]
        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField] private UnlockType unlockType;
        //public UnlockType UnlockType => unlockType;
        public UnlockType UnlockType
        {
            get
            {
                string key = ID + StaticStrings.UnlockType_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    unlockType = ES3.Load<UnlockType>(key);
                }
                else
                {
                    ES3.Save(key, unlockType);
                }
                return unlockType;
            }
            set
            {
                ES3.Save(ID + StaticStrings.UnlockType_Key_Prefix, unlockType = value);
            }
        }

        [VerticalGroup("Left/Right"), LabelWidth(100f),
            ShowIf("@unlockType == level")]
        [SerializeField] int unlockLevel = -1;
        public int UnlockLevel
        {
            get
            {
                string key = ID + StaticStrings.UnlockLevel_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    unlockLevel = ES3.Load<int>(key);
                }
                else
                {
                    ES3.Save(key, unlockLevel);
                }
                return unlockLevel;
            }
            set
            {
                ES3.Save(ID + StaticStrings.UnlockLevel_Key_Prefix, unlockLevel = value);
            }
        }
        //public int UnlockLevel => unlockLevel;

        [VerticalGroup("Left/Right"), LabelWidth(100f)]
        [SerializeField] private GameObject storePrefab;
        public GameObject ShopPrefab => storePrefab;


        [VerticalGroup("Left/Right"), LabelWidth(100f), OnValueChanged("SetUnlocked")]
        [SerializeField] private bool isUnlocked;
        public bool IsUnlocked 
        {
            get
            {
                string key = ID + StaticStrings.Unlocked_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    isUnlocked = ES3.Load<bool>(key);
                }
                else
                {
                    ES3.Save(key, isUnlocked);
                }
                return isUnlocked;
            }

            set
            {
                ES3.Save(ID + StaticStrings.Unlocked_Key_Prefix, isUnlocked = value);
                if (isUnlocked)
                {
                    UnlockType = UnlockType.Ad;
                   // unlockLevel = -1;
                }
                else
                {
                    UnlockType = UnlockType.Level;
                }
            }
        }

        [VerticalGroup("Left/Right"), LabelWidth(100f), OnValueChanged("SetUnlocked")]
        [SerializeField] bool isClaimed;
        public bool IsClaimed
        {
            get
            {
                string key = ID + StaticStrings.Claimed_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    isClaimed = ES3.Load<bool>(key);
                }
                else
                {
                    ES3.Save(key, isClaimed);
                }
                return isClaimed;
            }
            set
            {
                ES3.Save(ID + StaticStrings.Claimed_Key_Prefix, isClaimed = value);
                if (isClaimed)
                {
                    IsUnlocked = true;
                }
            }
        }

        //==============================================Odin implementation
        UnlockType level = UnlockType.Level;
        //UnlockType ad = UnlockType.Ad;

        void SetUnlocked()
        {
            IsUnlocked = isUnlocked;
            IsClaimed = isClaimed;
        }
    }

    [System.Serializable]
    public class LevelData
    {
        [BoxGroup("CONSTANTS", CenterLabel = true)]
        [SerializeField] private string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [BoxGroup("CONSTANTS", CenterLabel = true)]
        [SerializeField] private GameObject prefab;
        public GameObject Prefab => prefab;
        [BoxGroup("CONSTANTS", CenterLabel = true)]
        [MinMaxSlider(0, 8, ShowFields = true)]
        [SerializeField] private Vector2Int agentsIndexRange;
        [BoxGroup("CONSTANTS", CenterLabel = true)]
        [MinMaxSlider(0, 10, ShowFields = true)]
        [SerializeField] private Vector2Int weaponIndexRange;
        public Vector2Int AgentsIndexRange => agentsIndexRange;

        //[SerializeField] private AgentData[] agents;

        [BoxGroup("CONSTANTS/DYNAMIC", centerLabel:true)]
        [Tooltip("number of sublevels or phases in a level")]
        [SerializeField, Range(1, 10)] private int phaseNo;

        public int PhaseNo 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.levelPhaseNo_Key))
                {
                    phaseNo = ES3.Load<int>(StaticStrings.levelPhaseNo_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.levelPhaseNo_Key, phaseNo);
                }
                return phaseNo;
            }
            set
            {
                ES3.Save(StaticStrings.levelPhaseNo_Key, phaseNo = value);
                if (phaseNo >= GameManager.maxLevelPhases)
                {
                    PhaseNo = 1;
                    LevelComplete = true;
                }
            }
        }


        //[SerializeField] bool hasLevelInit;
        [BoxGroup("CONSTANTS/DYNAMIC")]
        [SerializeField] bool levelComplete = false;
        public bool LevelComplete { get => levelComplete; set => levelComplete = value; }



        public AgentData GetAgent(AgentData[] agentDatas, out int weaponIndex)
        {
            AgentData retVal = null;
            //sex = Sex.male;

            weaponIndex = Random.Range(weaponIndexRange.x, weaponIndexRange.y);

            /*for (int i = agentsIndexRange.x; i < agentsIndexRange.y; i++)
            {
                if (!agentDatas[i].IsUsed)
                {
                    agentDatas[i].IsUsed = true;
                    //sex = agentDatas[i].Sex;
                    retVal = agentDatas[i];
                    break;
                }
            }*/

            if (retVal == null)
            {
                int rand = Random.Range(agentsIndexRange.x, agentsIndexRange.y);
                retVal = agentDatas[rand];
                //sex = agentDatas[rand].Sex;
            }

            return retVal;
        }

        
    }

    [System.Serializable]
    public class AgentData
    {
        [HorizontalGroup("Left", Width = 120, PaddingLeft = 20), HideLabel]
        [SerializeField, InlineEditor(InlineEditorModes.SmallPreview
            , PreviewHeight = 60)]
        private Sprite icon;
        public Sprite Icon => icon;

        [HorizontalGroup("Left", PaddingLeft = 30)]
        [VerticalGroup("Left/Right"), LabelWidth(100)]
        [SerializeField] string id;
        public string ID 
        { 
            get { return id; }
            set { id = value; }
        }

        [VerticalGroup("Left/Right"), LabelWidth(100)]
        [SerializeField][EnumToggleButtons] private Sex sex;
        public Sex Sex => sex;

        [VerticalGroup("Left/Right"), LabelWidth(100)]
        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;


        //[VerticalGroup("Left/Right"), LabelWidth(100)]
        //[SerializeField] bool isUsed;
        //public bool IsUsed { get => isUsed; set => isUsed = value; }
        
    }

    [System.Serializable]
    public class WeaponData
    {
        [HorizontalGroup("WEAPON DATA", Width = 150, PaddingRight = 5)]
        [VerticalGroup("WEAPON DATA/Left")]
        [BoxGroup("WEAPON DATA/Left/Views", CenterLabel = true), 
            InlineEditor(InlineEditorModes.SmallPreview, PreviewHeight = 100), HideLabel]
        [SerializeField] Sprite icon;
        public Sprite Icon => icon;
        
        
        [VerticalGroup("WEAPON DATA/Right")]
        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(60)]
        [SerializeField] string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(60)]
        [SerializeField] private Vector3 position;
        public Vector3 Postion => position;

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(60)]
        [SerializeField] private Vector3 rotation;
        public Vector3 Rotation => rotation;

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(60)]
        [SerializeField] private Vector3 scale = Vector3.one;
        public Vector3 Scale => scale;

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(60)]
        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80f)]
        [Title("Shop Data", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private ItemType type = ItemType.Weapon;
        public ItemType Type => type;

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80)]
        [SerializeField] private UnlockType unlockType;
        //public UnlockType UnlockType => unlockType;
        public UnlockType UnlockType
        {
            get 
            {
                string key = ID + StaticStrings.UnlockType_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    unlockType = ES3.Load<UnlockType>(key);
                }
                else
                {
                    ES3.Save(key, unlockType);
                }
                return unlockType;
            }
            set
            {
                ES3.Save(ID + StaticStrings.UnlockType_Key_Prefix, unlockType = value);
            }
        }

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80), 
            ShowIf("@unlockType == level")]
        [SerializeField] int unlockLevel = -1;
        public int UnlockLevel
        {
            get
            {
                string key = ID + StaticStrings.UnlockLevel_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    unlockLevel = ES3.Load<int>(key);
                }
                else
                {
                    ES3.Save(key, unlockLevel);
                }
                return unlockLevel;
            }
            set
            {
                ES3.Save(ID + StaticStrings.UnlockLevel_Key_Prefix, unlockLevel = value);
            }
        }

        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80)]
        [SerializeField] GameObject shopPrefab;
        public GameObject ShopPrefab => shopPrefab;


        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80), OnValueChanged("SetUnlocked")]
        [SerializeField] bool isUnlocked;
        public bool IsUnlocked 
        {
            get
            {
                string key = ID + StaticStrings.Unlocked_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    isUnlocked = ES3.Load<bool>(key);
                }
                else
                {
                    ES3.Save(key, isUnlocked);
                }
                return isUnlocked;
            }

            set
            {
                ES3.Save(ID + StaticStrings.Unlocked_Key_Prefix, isUnlocked = value);
                if (isUnlocked)
                {
                    UnlockType = UnlockType.Ad;
                    //UnlockLevel = -1;
                    //level = UnlockType.
                }
                else
                {
                    unlockType = UnlockType.Level;
                }
            }
        }


        [BoxGroup("WEAPON DATA/Right/Stats"), LabelWidth(80), OnValueChanged("SetUnlocked")]
        [SerializeField] bool isClaimed;
        public bool IsClaimed 
        {
            get
            {
                string key = ID + StaticStrings.Claimed_Key_Prefix;
                if (ES3.KeyExists(key))
                {
                    isClaimed = ES3.Load<bool>(key);
                }
                else
                {
                    ES3.Save(key, isClaimed);
                }
                return isClaimed;
            }
            set
            {
                ES3.Save(ID + StaticStrings.Claimed_Key_Prefix, isClaimed = value);
                if (isClaimed)
                {
                    IsUnlocked = true;
                }
                
            }
        }

        //-----------------------------------------------------public bool IsUnlocked => isUnlocked;

        [BoxGroup("EDITOR FUNC")]
        [ShowInInspector] bool updateTransfromEdit;
        [ShowIf("@updateTransfromEdit")]
        [ShowInInspector, InlineButton("T", "update"), LabelWidth(80)] Transform reference;

        private void T()
        {
            if (reference != null)
            {
                position = reference.localPosition;
                rotation = reference.localEulerAngles;
                scale = reference.localScale;
            }
            reference = null;
        }

        //for attribute purposes
        UnlockType level = UnlockType.Level;

        void SetUnlocked()
        {
            IsUnlocked = isUnlocked;
            IsClaimed = isClaimed;
        }
    }

    [System.Serializable]
    public class UpgradableData
    {
        [BoxGroup("COINS DATA", CenterLabel = true)]
        [SerializeField] int coins_reward = 100;
        public int Coins_Reward 
        {
            get
            {
                if (ES3.KeyExists(StaticStrings.CoinsReward_Key))
                {
                    coins_reward = ES3.Load<int>(StaticStrings.CoinsReward_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.CoinsReward_Key, coins_reward);
                }

                return coins_reward;
            }
            set => ES3.Save(StaticStrings.CoinsReward_Key, coins_reward = value); 
        }
        

        [VerticalGroup("Vertical")]
        [HorizontalGroup("Vertical/Upgradable")]
        [BoxGroup("Vertical/Upgradable/Player")]
        [SerializeField] private int power_Player = 16;
        public int Power_Player 
        {
            get
            {
                if (ES3.KeyExists(StaticStrings.PlayerPower_Key))
                {
                    power_Player = ES3.Load<int>(StaticStrings.PlayerPower_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.PlayerPower_Key, power_Player);
                }
                return power_Player;
            }
            set => ES3.Save(StaticStrings.PlayerPower_Key, power_Player = value); 
        }


        [BoxGroup("Vertical/Upgradable/Player")]
        [SerializeField] private int health_Player = 100;
        public int Health_Player 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.PlayerHealth_Key))
                {
                    health_Player =  ES3.Load<int>(StaticStrings.PlayerHealth_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.PlayerHealth_Key, health_Player);
                }

                return health_Player;
            }
            set => ES3.Save(StaticStrings.PlayerHealth_Key, health_Player = value); 
        }


        [BoxGroup("Vertical/Upgradable/Agent")]
        [SerializeField] private int power_Agent = 16;
        public int Power_Agent 
        { 
            get
            {
                if(ES3.KeyExists(StaticStrings.AgentPower_Key))
                {
                    power_Agent = ES3.Load<int>(StaticStrings.AgentPower_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.AgentPower_Key, power_Agent);
                }
                return power_Agent;
            }
            set => ES3.Save(StaticStrings.AgentPower_Key, power_Agent = value); 
        }


        [BoxGroup("Vertical/Upgradable/Agent")]
        [SerializeField] private int health_Agent = 100;
        public int Health_Agent 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.AgentHealth_Key))
                {
                    health_Agent = ES3.Load<int>(StaticStrings.AgentHealth_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.AgentHealth_Key, health_Agent);
                }
                return health_Agent;
            }
            set => ES3.Save(StaticStrings.AgentHealth_Key, health_Agent = value); 
        }

        [BoxGroup("Vertical/Price", CenterLabel = true)]
        
        [BoxGroup("Vertical/Price")]
        [SerializeField] private int power_Price = 100;
        public int Power_Price 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.PowerPrice_Key))
                {
                    power_Price = ES3.Load<int>(StaticStrings.PowerPrice_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.PowerPrice_Key, power_Price);
                }

                return power_Price;
            }
            set => ES3.Save(StaticStrings.PowerPrice_Key, power_Price = value);
        }
        [BoxGroup("Vertical/Price")]
        [SerializeField] private int health_Price = 100;
        public int Health_Price 
        {
            get 
            {
                if (ES3.KeyExists(StaticStrings.HealthPrice_Key))
                {
                    health_Price = ES3.Load<int>(StaticStrings.HealthPrice_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.HealthPrice_Key, health_Price);
                }
                return health_Price;
            } 
            set => ES3.Save(StaticStrings.HealthPrice_Key, health_Price = value); 
        }
    }

    [System.Serializable]
    public class UpgraderData
    {
        [BoxGroup("COINS DATA", CenterLabel = true)]
        [SerializeField] int coinsRewardIncrementWin = 200;
        public int CoinsRewardIncrementWin => coinsRewardIncrementWin;
        [BoxGroup("COINS DATA")]
        [SerializeField] int coinsIncrementLose = 20;
        public int CoinsIncrementLose => coinsIncrementLose;
        [BoxGroup("COINS DATA")]
        [SerializeField] private int upgradeIncrementPrice = 100;
        public int UpgradeIncrementPrice => upgradeIncrementPrice;

        [HorizontalGroup("Data")]
        [BoxGroup("Data/Player")]
        [SerializeField] int powerIncrememt_Player = 5;
        public int PowerIncrememt_Player => powerIncrememt_Player;

        [BoxGroup("Data/Player")]
        [SerializeField] int healthIncrement_Player = 50;
        public int HealthIncrement_Player => healthIncrement_Player;

        [BoxGroup("Data/Agent")]
        [SerializeField] int powerIncrememt_Agent = 7;
        public int PowerIncrememt_Agent => powerIncrememt_Agent;

        [BoxGroup("Data/Agent")]
        [SerializeField] int healthIncrement_Agent = 55;
        public int HealthIncrement_Agent => healthIncrement_Agent;
    }

    [System.Serializable]
    public class SettingsData
    {
        [SerializeField] private bool viberateOn;
        public bool ViberateOn 
        { 
            get
            {
                //string k = 
                if (ES3.KeyExists(StaticStrings.vibrateOn_Key))
                {
                    viberateOn = ES3.Load<bool>(StaticStrings.vibrateOn_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.vibrateOn_Key, viberateOn);
                }
                return viberateOn;
            }
            set => ES3.Save(StaticStrings.vibrateOn_Key, viberateOn = value); 
        }

        [SerializeField] private bool sfxOn;
        public bool SfxOn 
        {
            get
            {
                //string k = 
                if (ES3.KeyExists(StaticStrings.SfxOn_Key))
                {
                    sfxOn = ES3.Load<bool>(StaticStrings.SfxOn_Key);
                }
                else
                {
                    ES3.Save(StaticStrings.SfxOn_Key, sfxOn);
                }
                return sfxOn;
            }
            set => ES3.Save(StaticStrings.SfxOn_Key, sfxOn = value); 
        }

        [SerializeField] private bool hasViewedTutorial;
        public bool HasViewedTutorial 
        { 
            get
            {
                if (ES3.KeyExists(StaticStrings.HasViewTutorial_key))
                {
                    hasViewedTutorial = ES3.Load<bool>(StaticStrings.HasViewTutorial_key);
                }
                else
                {
                    ES3.Save(StaticStrings.HasViewTutorial_key, hasViewedTutorial);
                }

                return hasViewedTutorial;
            }
            set => ES3.Save(StaticStrings.HasViewTutorial_key, hasViewedTutorial = value);
        }
    }
}
