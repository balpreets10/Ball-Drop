using BallDrop.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BallDrop
{
    public enum ItemType
    {
        Trail,
        Splatter
    }

    public class ShopScrollItem : ScrollItem
    {
        //public ItemType CurrentItemType = ItemType.Splatter;
        //public int CurrentTrailIndex;
        //public int CurrentSplatterIndex;
        //public int TrailCount;
        //public int SplatterCount;

        //private float Trailfactor;
        //private float Splatterfactor;
        //public ScrollRect TrailScrollRect, SplatterScrollRect;

        ////public Scrollbar TrailScrollBar;
        ////public Scrollbar SplatterScrollBar;

        //public GameObject SplatterPanel, TrailPanel;

        //public GameObject BtnSplatters, BtnTrails, Background;

        //public Button SelectTrailBtn, SelectSplatterBtn, UnlockTrailBtn, UnlockSplatterBtn;

        //public NoInternetConnection NoInternetConnection;


        //private void OnEnable()
        //{
        //    MyEventManager.Instance.UpdateUI.AddListener(UpdateItems);
        //    ShowSplatterPanel();
        //}

        //private void OnDisable()
        //{
        //    if (MyEventManager.Instance != null)
        //    {
        //        MyEventManager.Instance.UpdateUI.RemoveListener(UpdateItems);
        //    }
        //}

        //private void Start()
        //{
        //    TrailCount = TrailScrollRect.content.childCount;
        //    SplatterCount = SplatterScrollRect.content.childCount;
        //    SetupSplatters();
        //    SetupTrails();
        //    Trailfactor = 1.00f / (TrailCount - 1);
        //    Splatterfactor = 1.00f / (SplatterCount - 1);
        //    SelectTrailBtn.SetActive(true);
        //    SelectSplatterBtn.SetActive(true);
        //    SelectTrailBtn.interactable = SelectSplatterBtn.interactable = false;
        //    UnlockTrailBtn.SetActive(false);
        //    UnlockSplatterBtn.SetActive(false);
        //    BtnSplatters.GetComponent<Button>().onClick.Invoke();
        //}

        //private void SetupSplatters()
        //{
        //    for (int i = 0; i < SplatterCount; i++)
        //    {
        //        SplatterScrollRect.content.GetChild(i).GetComponent<UnlockableItems>().index = i;
        //    }
        //}

        //private void SetupTrails()
        //{
        //    for (int i = 0; i < TrailCount; i++)
        //    {
        //        TrailScrollRect.content.GetChild(i).GetComponent<UnlockableItems>().index = i;
        //    }
        //}

        //public void OnTrailDragged(BaseEventData eventData)
        //{
        //    ScrollSnap(TrailScrollRect.horizontalScrollbar, Trailfactor);
        //}

        //public void OnSplatterDragged(BaseEventData eventData)
        //{
        //    ScrollSnap(SplatterScrollRect.horizontalScrollbar, Splatterfactor);
        //}

        //private void ScrollSnap(Scrollbar scrollbar, float factor)
        //{
        //    int index = Mathf.RoundToInt(scrollbar.value / factor);
        //    ScrollSnap(scrollbar, factor, index);
        //}

        //private void ScrollSnap(Scrollbar scrollbar, float factor, int index)
        //{
        //    LeanTween.value(scrollbar.value, factor * index, .2f).setOnUpdate(UpdateScrollBar, scrollbar).setOnComplete(OnItemSelected, index);
        //    if (CurrentItemType == ItemType.Trail)
        //        CurrentTrailIndex = index;
        //    if (CurrentItemType == ItemType.Splatter)
        //        CurrentSplatterIndex = index;
        //}

        //private void OnItemSelected(object index)
        //{
        //    MyEventManager.Instance.UpdateScrollItems.Dispatch((int)index, CurrentItemType);
        //    UnlockOrSelect((int)index);
        //}

        //private void UnlockOrSelect(int index)
        //{

        //    switch (CurrentItemType)
        //    {
        //        case ItemType.Trail:
        //            ManageButtons(UnlockTrailBtn, SelectTrailBtn, DataManager.Instance.playerdata.trailData.trailStatus[index]);
        //            break;
        //        case ItemType.Splatter:
        //            ManageButtons(UnlockSplatterBtn, SelectSplatterBtn, DataManager.Instance.playerdata.splatterData.SplatterStatus[index]);
        //            break;
        //    }
        //}

        //private void ManageButtons(Button Unlock, Button Select, LockStatus status)
        //{
        //    switch (status)
        //    {
        //        case LockStatus.Locked:
        //            Unlock.SetActive(true);
        //            Select.SetActive(false);
        //            Select.interactable = true;
        //            break;
        //        case LockStatus.Selected:
        //            Unlock.SetActive(false);
        //            Select.SetActive(true);
        //            Select.interactable = false;
        //            break;
        //        case LockStatus.Unlocked:
        //            Unlock.SetActive(false);
        //            Select.SetActive(true);
        //            Select.interactable = true;
        //            break;
        //    }
        //}

        //private void UpdateItems()
        //{
        //    NoInternetConnection.Deactivate();
        //    for (int i = 0; i < DataManager.Instance.playerdata.trailData.trailStatus.Count; i++)
        //    {
        //        if (DataManager.Instance.playerdata.trailData.trailStatus[i] == LockStatus.Selected)
        //        {
        //            CurrentTrailIndex = i;
        //            TrailScrollRect.horizontalScrollbar.value = Trailfactor * i;
        //            MyEventManager.Instance.UpdateScrollItems.Dispatch(i, ItemType.Trail);
        //        }
        //        MyEventManager.Instance.UpdateLockStatus.Dispatch(i, ItemType.Trail, DataManager.Instance.playerdata.trailData.trailStatus[i]);
        //    }
        //    for (int i = 0; i < DataManager.Instance.playerdata.splatterData.SplatterStatus.Count; i++)
        //    {

        //        if (DataManager.Instance.playerdata.splatterData.SplatterStatus[i] == LockStatus.Selected)
        //        {
        //            CurrentSplatterIndex = i;
        //            SplatterScrollRect.horizontalScrollbar.value = Splatterfactor * i;
        //            MyEventManager.Instance.UpdateScrollItems.Dispatch(i, ItemType.Splatter);
        //        }
        //        MyEventManager.Instance.UpdateLockStatus.Dispatch(i, ItemType.Splatter, DataManager.Instance.playerdata.splatterData.SplatterStatus[i]);
        //    }
        //}

        //private void UpdateScrollBar(float value, object scrollbar)
        //{
        //    ((Scrollbar)scrollbar).value = value;
        //}

        //public void SelectTrail()
        //{
        //    SelectItem(CurrentTrailIndex, ItemType.Trail, DataManager.Instance.playerdata.trailData.trailStatus,
        //        PrefKey.TrailData, PlayfabKeys.TrailData, DataManager.Instance.playerdata.trailData);
        //}

        //public void SelectSpaltter()
        //{
        //    SelectItem(CurrentSplatterIndex, ItemType.Splatter, DataManager.Instance.playerdata.splatterData.SplatterStatus,
        //       PrefKey.SplatterData, PlayfabKeys.SplatterData, DataManager.Instance.playerdata.splatterData);
        //}

        //public void SelectItem(int currentIndex, ItemType itemType, List<LockStatus> lockStatuses, PrefKey prefKey, PlayfabKeys key, object data)
        //{
        //    Debug.Log("Selected " + itemType);
        //    UpdateUnlockables(lockStatuses, itemType, currentIndex);
        //    Dictionary<PlayfabKeys, string> keyValues = new Dictionary<PlayfabKeys, string>
        //        {
        //            { key, JsonUtility.ToJson(data) }
        //        };
        //    UpdateDataOnPlayfab(keyValues);
        //    PreferenceManager.Instance.UpdateCustomPref(prefKey, lockStatuses);
        //}

        //public void UnlockTrail()
        //{
        //    int cost = DataManager.Instance.TrailCostList.trailCost[CurrentTrailIndex];

        //    UnlockItem(CurrentTrailIndex, cost, ItemType.Trail, DataManager.Instance.playerdata.trailData.trailStatus,
        //                PrefKey.TrailData, PlayfabKeys.TrailData, DataManager.Instance.playerdata.trailData);
        //}

        //public void UnlockSplatter()
        //{
        //    int cost = DataManager.Instance.SplatterCostList.splatterCost[CurrentSplatterIndex];

        //    UnlockItem(CurrentSplatterIndex, cost, ItemType.Splatter, DataManager.Instance.playerdata.splatterData.SplatterStatus,
        //                PrefKey.SplatterData, PlayfabKeys.SplatterData, DataManager.Instance.playerdata.splatterData);
        //}

        //public void UnlockItem(int currentIndex, int cost, ItemType itemType, List<LockStatus> lockStatuses, PrefKey prefKey, PlayfabKeys key, object data)
        //{
        //    Debug.Log("Unlocking " + itemType);
        //    if (cost <= DataManager.Instance.playerdata.Coins)
        //    {
        //        DataManager.Instance.playerdata.Coins -= cost;
        //        UpdateUnlockables(lockStatuses, itemType, currentIndex);

        //        Dictionary<PlayfabKeys, string> keyValues = new Dictionary<PlayfabKeys, string>
        //        {
        //            { key, JsonUtility.ToJson(data) },
        //            { PlayfabKeys.Coins, DataManager.Instance.playerdata.Coins.ToString() }
        //        };
        //        UpdateDataOnPlayfab(keyValues);

        //        PreferenceManager.Instance.UpdateIntPref(PrefKey.Coins, DataManager.Instance.playerdata.Coins);
        //        PreferenceManager.Instance.UpdateCustomPref(prefKey, lockStatuses);
        //    }
        //    else
        //    {
        //        MyEventManager.Instance.ShowMessage.Dispatch("You do not have enough coins");
        //    }
        //}

        //public void UpdateUnlockables(List<LockStatus> lockStatuses, ItemType itemType, int currentIndex)
        //{
        //    for (int i = 0; i < lockStatuses.Count; i++)
        //    {
        //        if (lockStatuses[i] == LockStatus.Selected)
        //        {
        //            lockStatuses[i] = LockStatus.Unlocked;
        //            MyEventManager.Instance.UpdateLockStatus.Dispatch(i, itemType, LockStatus.Unlocked);
        //        }
        //        if (i == currentIndex)
        //        {
        //            lockStatuses[i] = LockStatus.Selected;
        //            MyEventManager.Instance.UpdateLockStatus.Dispatch(currentIndex, itemType, LockStatus.Selected);
        //        }

        //    }
        //    UnlockOrSelect(currentIndex);
        //}

        //private void UpdateDataOnPlayfab(Dictionary<PlayfabKeys, string> keyValues)
        //{

        //    PlayfabManager.Instance.UpdatePlayfabUserData(keyValues,
        //        result =>
        //        {
        //            Debug.Log("Data updated on playfab");
        //            PreferenceManager.Instance.UpdateBoolpref(PrefKey.UpdateDataOnPlayfab, false);
        //        },
        //        error =>
        //        {
        //            Debug.Log("Data could not be updated on playfab");
        //            PreferenceManager.Instance.UpdateBoolpref(PrefKey.UpdateDataOnPlayfab, true);
        //        });

        //}

        //public void ShowSplatterPanel()
        //{
        //    CurrentItemType = ItemType.Splatter;
        //    ActivatePanels();
        //    LeanTween.moveLocalX(SplatterPanel, 0, .3f).setEase(LeanTweenType.easeOutBack);
        //    LeanTween.moveLocalX(TrailPanel, 1536, .2f).setOnComplete(DeactivatePanel).setOnCompleteParam(TrailPanel);
        //    ManageTabs(BtnSplatters);
        //}

        //public void ShowTrailPanel()
        //{
        //    CurrentItemType = ItemType.Trail;
        //    ActivatePanels();
        //    LeanTween.moveLocalX(TrailPanel, 0, .3f).setEase(LeanTweenType.easeOutBack);
        //    LeanTween.moveLocalX(SplatterPanel, -1536, .2f).setOnComplete(DeactivatePanel).setOnCompleteParam(SplatterPanel);
        //    ManageTabs(BtnTrails);

        //}

        //private void DeactivatePanel(object panel)
        //{
        //    //((GameObject)panel).SetActive(false);
        //}

        //private void ActivatePanels()
        //{
        //    LeanTween.cancel(TrailPanel);
        //    LeanTween.cancel(SplatterPanel);
        //    TrailPanel.SetActive(true);
        //    SplatterPanel.SetActive(true);
        //}

        //public void MoveLeft()
        //{
        //    if (CurrentItemType == ItemType.Trail)
        //    {
        //        if (CurrentTrailIndex > 0)
        //            ScrollSnap(TrailScrollRect.horizontalScrollbar, Trailfactor, --CurrentTrailIndex);
        //    }
        //    else
        //    {
        //        if (CurrentSplatterIndex > 0)
        //        {
        //            ScrollSnap(SplatterScrollRect.horizontalScrollbar, Splatterfactor, --CurrentSplatterIndex);
        //        }
        //    }
        //}

        //public void MoveRight()
        //{
        //    if (CurrentItemType == ItemType.Trail)
        //    {
        //        if (CurrentTrailIndex < TrailCount - 1)
        //            ScrollSnap(TrailScrollRect.horizontalScrollbar, Trailfactor, ++CurrentTrailIndex);
        //    }
        //    else
        //    {
        //        if (CurrentSplatterIndex < SplatterCount - 1)
        //        {
        //            ScrollSnap(SplatterScrollRect.horizontalScrollbar, Splatterfactor, ++CurrentSplatterIndex);
        //        }
        //    }

        //}

        //private void ManageTabs(GameObject button)
        //{
        //    Background.transform.SetAsLastSibling();
        //    button.transform.SetAsLastSibling();
        //}
    }

}