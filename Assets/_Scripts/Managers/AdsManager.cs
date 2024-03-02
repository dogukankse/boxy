// using System.Collections;
// using UnityEngine;
// using UnityEngine.Advertisements;
// using UnityEngine.Events;
//
// namespace Managers
// {
//     public class AdsManager : IUnityAdsListener
//     {
// #if PLATFORM_ANDROID
//         private string gameID = "3392127";
// #elif Unity_IOS
//         private string gameID = "3392126"
// #endif
//
// #if UNITY_EDITOR
//         private bool testMode = true;
// #else
// private bool testMode = false;
// #endif
//         private string bannerID = "banner";
//         private string fullscreenID = "video";
//         private string rewardedID = "rewardedVideo";
//
//         private static AdsManager instance;
//
//         public UnityAction OnRewardedVideoFinished;
//         private int buttonID;
//
//         private AdsManager()
//         {
//             Advertisement.AddListener(this);
//             Advertisement.Initialize(gameID, testMode);
//         }
//
//         public static AdsManager Instance()
//         {
//             return instance ?? (instance = new AdsManager());
//         }
//
//         public IEnumerator ShowBannerWhenReady()
//         {
//             while (!Advertisement.IsReady(bannerID))
//             {
//                 yield return new WaitForSeconds(.1f);
//             }
//
//             Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
//             Advertisement.Banner.Show(bannerID);
//         }
//
//         public void HideBanner()
//         {
//             Advertisement.Banner.Hide();
//         }
//
//         public void ShowRewardedVideo(int buttonID)
//         {
//             this.buttonID = buttonID;
//             Advertisement.Show(rewardedID);
//         }
//
//         public void ShowVideo()
//         {
//             Advertisement.Show();
//         }
//
//         public void OnUnityAdsReady(string placementId)
//         {
//         }
//
//         public void OnUnityAdsDidError(string message)
//         {
//             Debug.Log("Rewarded Ads error: " + message);
//         }
//
//         public void OnUnityAdsDidStart(string placementId)
//         {
//         }
//
//         public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
//         {
//             Debug.Log($"burada {placementId} {showResult}");
//             switch (showResult)
//             {
//                 case ShowResult.Finished:
//                     if (buttonID == 1)
//                     {
//                         GameData.Instance().bombBoosterCount += 1;
//                         Debug.Log("burada1");
//                     }
//                     else if (buttonID == 2)
//                     {
//                         GameData.Instance().slowBoosterCount += 1;
//                         Debug.Log("burada2");
//                     }
//                     else if (buttonID == 3)
//                     {
//                         Debug.Log("burada3");
//
//                         GameData.Instance().magnetBoosterCount += 1;
//                     }
//
//                     buttonID = 0;
//                     OnRewardedVideoFinished();
//                     Debug.Log("placement id: " + placementId);
//                     break;
//                 case ShowResult.Failed:
//                     Debug.LogWarning("The ad did not finish due to an error.");
//                     break;
//                 case ShowResult.Skipped:
//                     break;
//             }
//         }
//     }
// }