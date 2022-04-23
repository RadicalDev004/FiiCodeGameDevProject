using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using RadicalKit;
public class Ads : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4709662";
#elif UNITY_ANDROID
    private string gameId = "4709663";
    #endif
    private bool testMode = true;
    string mySurfacingId = "Rewarded_Android";
    string mySurfacingId2 = "Interstitial_Android";

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId2))
        {

            int a = Random.Range(1, 4);
            if (a == 1)
                return;
            Advertisement.Show(mySurfacingId2);

            Advertisement.RemoveListener(this);



        }
        else
        {
            
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
    public void Update()
    {

    }
    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId))
        {

            Advertisement.AddListener(this);

            Advertisement.Show(mySurfacingId);

        
            
            
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("currentEnergy", PlayerPrefs.GetInt("currentEnergy") + 1);

            FindObjectOfType<Energy>().GiveEnergy();

            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
    
    
}
