using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBanner : MonoBehaviour
{
    private BannerView bannerView;
    void Start()
    {
        MobileAds.Initialize(initStatus => {});

        this.RequestBanner();

        // AdRequest request = new AdRequest.Builder().Build();

        // this.bannerView.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2035314174521668~5021375044";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-2035314174521668~5021375044";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);


    }
}
