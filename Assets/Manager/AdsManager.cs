using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public bool adsIsShowing
    {
        get { return Advertisement.isShowing; }
    }
    void Start()
    {
        string gameId = "4197957";
        Advertisement.Initialize(gameId, true);
    }

    public void ShowAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
