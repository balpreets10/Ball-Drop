using BallDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    
    public void GrantIGC(Product product)
    {
        //int coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0);
        //coins += (int)product.definition.payout.quantity;
        //PreferenceManager.Instance.UpdateIntPref(PrefKey.Coins, coins);
        Debug.Log("coins - "+ product.definition.payout.quantity);
    }

    public void OnPurchaseFailed(Product product , PurchaseFailureReason failureReason)
    {
        MyEventManager.Instance.ShowMessage.Dispatch(GameStrings.PurchaseFailureMsg + failureReason);
    }
}
