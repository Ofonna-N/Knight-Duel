using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using com.adjust.sdk;
using Sirenix.OdinInspector;

namespace KnightDuel
{
    public class SDK_Implementation : MonoBehaviour
    {
        public static SDK_Implementation instance;


        [SerializeField, TitleGroup("FACEBOOK SDK", Alignment = TitleAlignments.Centered)]
        FacebookSDK_Integration facebookSDK;


        [SerializeField, TitleGroup("ADJUST SDK", Alignment = TitleAlignments.Centered)]
        AdjustSDK_Integration adjustSDK;



        private void Awake()
        {
            AwakeInit();
            adjustSDK.IAwake();
            facebookSDK.IAwake();
            Debug.Log("Awake");
        }

        private void Start()
        {
            adjustSDK.IStart();
            facebookSDK.IStart();
            Debug.Log("Start");
        }

        private void OnApplicationPause(bool pause)
        {
            facebookSDK.OnPause(pause);
        }


        private void AwakeInit()
        {
            if (SDK_Implementation.instance == null)
            {
                SDK_Implementation.instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }



        //===========public functions

        public void LevelStarted(int level, int round)
        {
            facebookSDK.LogLevelStartedEvent(level, round);
        }

        /*public void LevelRestart(int level, int round)
        {
            facebookSDK.LogLevelRestartEvent(level, round);
        }*/

        /*public void LevelSkipped()
        {

        }*/

        public void LevelFailed(int level, int round)
        {
            facebookSDK.LogLevelFailedEvent(level, round);
        }

        public void LevelComplete(int level)
        {
            facebookSDK.LogLevelCompleteEvent(level);
        }

        public void UpgradePurchased(string upgrade)
        {
            facebookSDK.LogUpgradePurchasedEvent(upgrade);
        }

        public void ContentUnlocked(string content)
        {
            facebookSDK.LogContentUnlockedEvent(content);
        }

        public void TutorialComplete()
        {
            facebookSDK.LogTutorialCompleteEvent();
        }

        public void RoundStarted(int round)
        {
            facebookSDK.LogRoundStartedEvent(round);
        }

        public void RoundComplete(int round)
        {
            facebookSDK.LogRoundCompleteEvent(round);
        }

        public void ACTIONPerformed(string action)
        {
            facebookSDK.LogACTIONPerformedEvent(action);
        }


    }



    [System.Serializable]
    public class AdjustSDK_Integration : ISDK_Integration
    {
        [ShowInInspector, ReadOnly]
        private string token = "e8fwyq5mz400";

        public void IAwake()
        {
            //throw new System.NotImplementedException();
        }

        public void IStart()
        {
#if UNITY_IOS
        /* Mandatory - set your iOS app token here */
        InitAdjust("e8fwyq5mz400");
#elif UNITY_ANDROID
            /* Mandatory - set your Android app token here */
            InitAdjust(token);
#endif
        }


        private void InitAdjust(string adjustAppToken)
        {
            var adjustConfig = new AdjustConfig(
                adjustAppToken,
                AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
                true
            );
            adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
            adjustConfig.setSendInBackground(true);
            new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename

            // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

            //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
            //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
            //});

            Adjust.start(adjustConfig);

        }

    }



    [System.Serializable]
    public class FacebookSDK_Integration : ISDK_Integration
    {
        public void IAwake()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }

        public void IStart()
        {
            
            //throw new System.NotImplementedException();
        }

        public void LogLevelStartedEvent(int level, int round)
        {
            var parameters = new Dictionary<string, object>();
            parameters["level"] = level;
            parameters["Round"] = round;
            FB.LogAppEvent(
                "Level Started",
                parameters:parameters
            );
        }

        public void LogLevelRestartEvent(int level, int round)
        {
            var parameters = new Dictionary<string, object>();
            parameters["level"] = level;
            parameters["Round"] = round;
            FB.LogAppEvent(
                "Level Restart",
                parameters:parameters
            );
        }

        public void LogLevelFailedEvent(int level, int round)
        {
            var parameters = new Dictionary<string, object>();
            parameters["level"] = level;
            parameters["Round"] = round;
            FB.LogAppEvent(
                "Level Failed",
                parameters:parameters
            );
        }

        public void LogLevelCompleteEvent(int level)
        {
            var parameters = new Dictionary<string, object>();
            parameters["level"] = level;
            FB.LogAppEvent(
                "Level Complete",
                parameters:parameters
            );
        }

        public void LogUpgradePurchasedEvent(string upgrade)
        {
            var parameters = new Dictionary<string, object>();
            parameters["upgrade"] = upgrade;
            FB.LogAppEvent(
                "Upgrade Purchased",
                parameters:parameters
            );
        }

        public void LogContentUnlockedEvent(string content)
        {
            var parameters = new Dictionary<string, object>();
            parameters["content"] = content;
            FB.LogAppEvent(
                "Content Unlocked",
                parameters:parameters
            );
        }

        public void LogTutorialCompleteEvent()
        {
            FB.LogAppEvent(
                "Tutorial Complete"
            );
        }

        public void LogRoundStartedEvent(int round)
        {
            var parameters = new Dictionary<string, object>();
            parameters["round"] = round;
            FB.LogAppEvent(
                "Round Started",
                parameters:parameters
            );
        }

        public void LogRoundCompleteEvent(int round)
        {
            var parameters = new Dictionary<string, object>();
            parameters["round"] = round;
            FB.LogAppEvent(
                "Round Complete",
                parameters:parameters
            );
        }

        public void LogACTIONPerformedEvent(string action)
        {
            var parameters = new Dictionary<string, object>();
            parameters["action"] = action;
            FB.LogAppEvent(
                "ACTION Performed",
                parameters:parameters
            );
        }

        public void OnPause(bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground
            // or background
            if (!pauseStatus)
            {
                //app resume
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    //Handle FB.Init
                    FB.Init(() =>
                    {
                        FB.ActivateApp();
                    });
                }
            }
        }
    }
}
