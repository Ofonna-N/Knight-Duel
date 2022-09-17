using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace KnightDuel
{
    public class ShopItem : MonoBehaviour
    {

        [SerializeField] Image iconImg;
        public Sprite Icon => iconImg.sprite;

        [ShowInInspector, ReadOnly] ItemType type;

        [ShowInInspector, ReadOnly] UnlockType unlockType;

        [SerializeField] GameObject[] itemLockGraphics;

        [SerializeField] TextMeshProUGUI textState;

        [SerializeField] Color selectedColor, deselectedColor;

        [SerializeField] Image[] selectImgIndicators;

        //[Title("RUNTIME DATA", TitleAlignment = TitleAlignments.Centered)]
        [ShowIf("@unlockType == level"), ReadOnly]
        private int unlockLevel;
        public int UnlockLevel => unlockLevel;

        [ShowInInspector, ReadOnly]
        private int index;
        //public int Index => index;

        [ShowInInspector, ReadOnly] bool isUnlocked;
        public bool IsUnlocked => isUnlocked;

        [ShowInInspector, ReadOnly] bool isClaimed;
        public bool IsClaimed => isClaimed;

        [ShowInInspector, ReadOnly]
        private bool isSelected;
        public bool IsSelected => isSelected;

        //for attribute purposes
        UnlockType level = UnlockType.Level;

        public void Init(Sprite icon, UnlockType u_type, ItemType t, bool unlocked, bool claimed, int unlockL, bool isSel, int inde_x)
        {
            type = t;

            iconImg.sprite = icon;

            unlockType = u_type;

            unlockLevel = unlockL;

            isUnlocked = unlocked;

            isClaimed = claimed;

            index = inde_x;

            UpdateLockedGraphic();

            itemLockGraphics[0].SetActive(!isUnlocked);

            isSelected = isSel;

            if (isSelected) Select();

            textState.text = unlockType switch
            {
                UnlockType.Level => $"LVL {unlockLevel}",
                UnlockType.Ad => $"{StaticStrings.TMP_ADIconFormat} CLAIM",
                _ => $"ERROR!",
            };


        }


        public void Select()
        {
            if (isClaimed)
            {
                isSelected = true;

                GameManager.instance.DeselectOtherShopItems(type, index);

                GameManager.instance.SelectShopItem(type, index);

                for (int i = 0; i < selectImgIndicators.Length; i++)
                {
                    selectImgIndicators[i].color = selectedColor;
                }
            }
            else if (isUnlocked && !IsClaimed)
            {
                Debug.Log("SHOW REWARDED AD TO CLAIM ITEM");
                AdsManager.Instance.ShowAd(AdType.RewardedVideo, OnAdWatched);
            }
            else
            {
                Debug.Log("YOU BEED TO UNLOCK ITEM");
            }
        }

        private void OnAdWatched()
        {
            switch (type)
            {
                case ItemType.Skin:
                    GameManager.instance.ResourcesManager.Skins[index].IsClaimed = true;
                    GameManager.instance.ResourcesManager.SelectedSkin = index;
                    isClaimed = true;
                    UpdateLockedGraphic();
                    break;
                case ItemType.Weapon:
                    GameManager.instance.ResourcesManager.Weapons[index].IsClaimed = true;
                    GameManager.instance.ResourcesManager.SelectedWeapon = index;
                    isClaimed = true;
                    UpdateLockedGraphic();
                    break;
                default:
                    break;
            }
        }

        private void UpdateLockedGraphic()
        {
            for (int i = 0; i < itemLockGraphics.Length; i++)
            {
                itemLockGraphics[i].SetActive(!isClaimed);
            }
        }

        public void Deselect()
        {
            isSelected = false;
            for (int i = 0; i < selectImgIndicators.Length; i++)
            {
                selectImgIndicators[i].color = deselectedColor;
            }
        }
    }
}
