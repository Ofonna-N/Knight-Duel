using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    public class AdsManager : MonoBehaviour, IUnityAdsListener
    {
        private static AdsManager instance;
        public static AdsManager Instance => instance;

        [BoxGroup("Platform IDs"), SerializeField]
        private string android_ID;

        [BoxGroup("Platform IDs"), SerializeField]
        private string IOS_ID;

        [BoxGroup("Placement IDs")]
        [BoxGroup("Placement IDs/Banner"), ReadOnly, ShowInInspector]
        private string Banner_Android = "Banner_Android";

        [BoxGroup("Placement IDs/Banner"), ReadOnly, ShowInInspector]
        private string Banner_ios = "Banner_iOS";

        [BoxGroup("Placement IDs/Interstitial"), ReadOnly, ShowInInspector]
        private string Interstitial_Android = "Interstitial_Android";

        [BoxGroup("Placement IDs/Interstitial"), ReadOnly, ShowInInspector]
        private string Interstitial_ios = "Interstitial_iOS";

        [BoxGroup("Placement IDs/RewardedVideo"), ReadOnly, ShowInInspector]
        private string Rewarded_Android = "Rewarded_Android";

        [BoxGroup("Placement IDs/RewardedVideo"), ReadOnly, ShowInInspector]
        private string Rewarded_ios = "Rewarded_iOS";

        private Platform platform;

        private System.Action SenderCallback;

        private IEnumerator Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            Advertisement.AddListener(this);
            if (Application.platform == RuntimePlatform.Android)
            {
                platform = Platform.Android;
                Advertisement.Initialize(android_ID, true);
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = Platform.IOS;
                Advertisement.Initialize(IOS_ID, true);
            }
            else
            {
                Debug.Log("Debuggging...");
                platform = Platform.Android;
                Advertisement.Initialize(android_ID, true, false);
            }

            yield return new WaitUntil(() => Advertisement.isInitialized);
            Debug.Log("Ad initialized");
            ShowAd(AdType.Banner, ()=> Debug.Log("Bannder showed"));
            
        }

        public void ShowAd(AdType adType, System.Action callback)
        {
            /*if(Advertisement.isInitialized)
            {
                Debug.Log("Ad not initialized");
                return;
            }*/
            SenderCallback = callback;
            switch (adType)
            {
                case AdType.Banner:
                    ShowBanner();
                    break;
                case AdType.Interstitial:
                    ShowInterstitial();
                    break;
                case AdType.RewardedVideo:
                    ShowRewardedVideo();
                    break;
                default:
                    break;
            }


        }

        private void ShowBanner()
        {
            switch (platform)
            {
                case Platform.Android:
                    Advertisement.Show(Banner_Android);
                    break;
                case Platform.IOS:
                    Advertisement.Show(Banner_ios);
                    break;
                default:
                    break;
            }
        }

        private void ShowInterstitial()
        {
            switch (platform)
            {
                case Platform.Android:
                    Advertisement.Show(Interstitial_Android);
                    break;
                case Platform.IOS:
                    Advertisement.Show(Interstitial_ios);
                    break;
                default:
                    break;
            }
        }

        private void ShowRewardedVideo()
        {
            switch (platform)
            {
                case Platform.Android:
                    Advertisement.Show(Rewarded_Android);
                    break;
                case Platform.IOS:
                    Advertisement.Show(Rewarded_ios);
                    break;
                default:
                    break;
            }
        }


        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log("Ads Ready");
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log("Ads Error");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log("Ads Started");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Debug.Log("Ads Finished");
            SenderCallback();
        }
    }
}
