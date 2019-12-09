using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Managers
{
    public class AdsManager
    {
#if PLATFORM_ANDROID
        private string gameID = "3392127";
#elif Unity_IOS
        private string gameID = "3392126"
#endif

        private bool testMode = true;
        private string bannerID = "banner";

        private static AdsManager instance;


        private AdsManager()
        {
            Advertisement.Initialize(gameID, testMode);
        }

        public static AdsManager Instance()
        {
            if (instance == null)
            {
                instance = new AdsManager();
            }

            return instance;
        }

        public IEnumerator ShowBannerWhenReady()
        {
            while (!Advertisement.IsReady(bannerID))
            {
                yield return new WaitForSeconds(.5f);
            }

            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(bannerID);
        }

        public void HideBanner()
        {
            Advertisement.Banner.Hide();
        }
    }
}