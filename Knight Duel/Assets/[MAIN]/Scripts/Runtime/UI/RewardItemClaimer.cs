using System.Collections;
using UnityEngine;



namespace KnightDuel
{
    public class RewardItemClaimer : MonoBehaviour
    {
        [SerializeField] int itemIndex;

        public int ItemIndex { get => itemIndex; set => itemIndex = value; }

        public ItemType Type { get; set; }

        public ShopItem[] shopItems { get; set; }

        [SerializeField] GameObject button;
        public GameObject Button => button;

        public void Claim()
        {
            Debug.Log("PLAY AD TO CLAIM REWARD");
            AdsManager.Instance.ShowAd(AdType.RewardedVideo, OnClaimed);

        }

        public void OnClaimed()
        {
            button.SetActive(false);
            switch (Type)
            {
                case ItemType.Skin:
                    GameManager.instance.ResourcesManager.Skins[itemIndex].IsUnlocked = true;
                    //shopItems[itemIndex].Select();
                    break;
                case ItemType.Weapon:
                    GameManager.instance.ResourcesManager.Weapons[itemIndex].IsUnlocked = true;
                    //shopItems[itemIndex].Select();
                    break;
                default:
                    break;
            }
        }
    }
}