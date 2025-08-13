using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour , IUnityAdsInitializationListener , IUnityAdsLoadListener , IUnityAdsShowListener
{

    public bool IsLoaded = false;
    
    string gameId;
    string unitID;
    Button adButton;


    [SerializeField] private string androidID;
    [SerializeField] private string iosID;
    [SerializeField] private bool testMode;
    [SerializeField] private string iosUnitID;
    [SerializeField] private string androidUnitID;


    Canvas adCanvas;
    
    private void Awake()
    {
        initializeAds();
        adButton = GameObject.Find("AdButton").GetComponent<Button>();
        adButton.interactable = false;
        IsLoaded = false ;

        adCanvas = GameObject.Find("WatchAdCanvas").GetComponent<Canvas>();
    }


    void initializeAds()
    {
        #if UNITY_ANDROID
            gameId = androidID;
            unitID = androidUnitID;
        #elif UNITY_IOS
            gameID = iosID;
            unitID = iosUnitID;
        #elif UNITY_EDITOR
            gameID = androidID;
            unitID = androidUnitID;
        #endif

        Debug.Log($"Game ID is : {gameId}");

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            IUnityAdsInitializationListener l = this as IUnityAdsInitializationListener;
            Advertisement.Initialize(gameId , testMode , l );
        }



    }


    public void LoadAd()
    {
        if (Advertisement.isInitialized && !IsLoaded)
        {
            IUnityAdsLoadListener l = this as IUnityAdsLoadListener;
            Advertisement.Load(unitID, l);

        }

    }

    public void ShowAd()
    {
       
        adButton.interactable = false;
        IUnityAdsShowListener l = this as IUnityAdsShowListener;
        Advertisement.Show(unitID ,  l );
    }




    public void OnInitializationComplete()
    {
        Debug.Log("Initialization of UnityAds is successfull!!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Initialization failed : {error.ToString()}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(unitID) && !IsLoaded)
        {
            adButton.onClick.AddListener(ShowAd);
            adButton.interactable = true;
            IsLoaded = true;

        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Failed to load ad. error message=> {error.ToString()} {message}");
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId.Equals(unitID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            adButton.interactable = true;
            GameManager.Instance.OnRewardedAdWatched();
            Debug.Log("Reward has been received");
            IsLoaded = false;
            adCanvas.enabled = false;
        }
        Debug.Log("What The Error is Going on ?");

    }

    public void OnUnityAdsShowClick(string placementId){}
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message){}

    public void OnUnityAdsShowStart(string placementId){}


    private void OnDestroy()
    {
        adButton.onClick.RemoveAllListeners(); 
    }

    private void Update()
    {
        LoadAd();
    }


}
