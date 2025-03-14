using System.Collections;
using System.Collections.Generic;
/*using CAS;*/
using UnityEngine;

public class AdsManager : MonoBehaviour
{

    /*private bool isAppReturnEnable = false;
    private bool userRewardEarned = false;

    private IMediationManager manager;
    private IAdView bannerView;
    public bool menuAds = false;

    public static AdsManager instance;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        instance = this;

        manager = GetAdManager();
        CreateBanner(AdSize.Banner);

        // -- Subscribe to CAS events:
        manager.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
        manager.OnInterstitialAdFailedToLoad += OnInterstitialAdFailedToLoad;

        manager.OnInterstitialAdClosed += OnInstAdCompleted;
        manager.OnRewardedAdLoaded += OnRewardedAdLoaded;
        manager.OnRewardedAdFailedToLoad += OnRewardedAdFailedToLoad;
        manager.OnRewardedAdCompleted += OnRewardedAdCompleted;
        manager.OnRewardedAdClosed += OnRewardedAdClosed;

        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(1);
    }
    public static IMediationManager GetAdManager()
    {
        // Configure MobileAds.settings before initialize
        return MobileAds.BuildManager()
            // Optional initialize listener
            .WithCompletionListener((config) => {
                string initErrorOrNull = config.error;
                string userCountryISO2OrNull = config.countryCode;
                bool protectionApplied = config.isConsentRequired;
                IMediationManager manager = config.manager;
            })
            .Build();
    }

    private void OnDestroy()
    {
        // -- Unsubscribe from CAS events:
        manager.OnInterstitialAdLoaded -= OnInterstitialAdLoaded;
        manager.OnInterstitialAdFailedToLoad -= OnInterstitialAdFailedToLoad;

        manager.OnInterstitialAdClosed -= OnInstAdCompleted;

        manager.OnRewardedAdLoaded -= OnRewardedAdLoaded;
        manager.OnRewardedAdFailedToLoad -= OnRewardedAdFailedToLoad;
        manager.OnRewardedAdCompleted -= OnRewardedAdCompleted;
        manager.OnRewardedAdClosed -= OnRewardedAdClosed;

    }


    public bool CanShowInterstitional()
    {
        return manager.IsReadyAd(AdType.Interstitial);
    }

    public bool CanShowRewarded()
    {
        //return Appodeal.CanShow(AppodealAdType.RewardedVideo);
        return manager.IsReadyAd(AdType.Rewarded);
    }

    private void CreateBanner(AdSize size)
    {
        StopBanner();
        bannerView = manager.GetAdView(size);
        SetBannerPosition(AdPosition.TopCenter);
    }

    public void ShowBanner()
    {
            bannerView.SetActive(true);
            Debug.Log("show banner");
    }

    public void StopBanner()
    {
        if (bannerView != null)
            bannerView.SetActive(false);
    }

    public void SetBannerSize(int sizeID)
    {
        CreateBanner((AdSize)sizeID);
    }

    public void SetBannerPosition(int positionEnum)
    {
        bannerView.position = (AdPosition)positionEnum;
    }

    public void SetBannerPosition(AdPosition position)
    {
        bannerView.position = position;
    }

    IEnumerator ShowInst()
    {
        int i = 0;
        while (!CanShowInterstitional())
        {
            yield return new WaitForEndOfFrame();
            i++;
            if (i >= 300)
                yield break;
        }
        ShowInter();
    }

    public void ShowInterstitional()
    {
            Debug.Log("Reklama inst");
            ShowInter();
    }

    private void ShowInter()
    {
        manager.ShowAd(AdType.Interstitial);
    }

    private int rew = 0;
    public void ShowReward(RewardedPrizeType rew_type)
    {
        userRewardEarned = false;
        rew = (int)rew_type;
        manager.ShowAd(AdType.Rewarded);
    }

    public void ChangeAppReturnState()
    {
        isAppReturnEnable = !isAppReturnEnable;
        manager.SetAppReturnAdsEnabled(isAppReturnEnable);
    }

    private void Update()
    {
        if (userRewardEarned)
        {
            userRewardEarned = false;

            if (rew == 1)
            {

            }

            rew = 0;
        }
    }

    #region Events

    private void OnInstAdCompleted()
    {
        //AnalyticsManager.instance.ReportEvent("Show inst");
    }
    private void OnRewardedAdCompleted()
    {
        userRewardEarned = true;
        Debug.Log("User reward earned");
    }

    private void OnRewardedAdClosed()
    {
        if (!userRewardEarned)
            Debug.Log("User are not rewarded");
    }

    private void OnRewardedAdLoaded()
    {
        Debug.Log("Ready");
    }

    private void OnRewardedAdFailedToLoad(AdError error)
    {
        Debug.Log(error.GetMessage());
    }

    private void OnInterstitialAdFailedToLoad(AdError error)
    {
        Debug.Log(error.GetMessage());
    }

    private void OnInterstitialAdLoaded()
    {
        Debug.Log("Ready");
    }
    #endregion

    public void SetDouble(int money)
    {
        //readyDoubleMoney = money;
    }*/
}

public enum RewardedPrizeType
{
    none,
    money,
    car,
    other1
}

