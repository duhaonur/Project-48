using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { set; get; }

    private string appID = "";

    private BannerView bannerView;
    private string bannerID = "";

    private InterstitialAd interstitialAd;
    private string interstitialID = "";

    private void Start()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        MobileAds.Initialize(appID);

        RequestVideoAd();
    }

    public void RequestBanner()
    {
        bannerView = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);

        ShowBanner();
    }

    private IEnumerator ShowBanner()
    {
        yield return new WaitForSeconds(2);

        bannerView.Show();
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }

    public void RequestVideoAd()
    {
        interstitialAd = new InterstitialAd(interstitialID);

        AdRequest request = new AdRequest.Builder().Build();

        interstitialAd.LoadAd(request);
    }

    public void ShowVideoAd()
    {
        if (interstitialAd.IsLoaded())
            interstitialAd.Show();
    }
}
