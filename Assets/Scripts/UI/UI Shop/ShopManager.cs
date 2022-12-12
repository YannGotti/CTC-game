using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int countCoins;

    public TMP_Text coinsUI;
    public ShopItemSO[] shopItemsSO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns; 


    void Start()
    {
        LoadPanels();
        CheckPurchaseable(); 

        coinsUI.text = $"Монеты: {countCoins}";
    }

    public void AddCoins()
    {
        countCoins += 10;
        coinsUI.text = $"Монеты: {countCoins}";
        CheckPurchaseable(); 
    }

    public void PurchaseableItem(int btnNum)
    {
        if(countCoins >= shopItemsSO[btnNum].baseCost)
        {
            countCoins -= shopItemsSO[btnNum].baseCost;
            coinsUI.text = $"Монеты: {countCoins}";
            CheckPurchaseable(); 
            // ActivateBuyPanel(); 
        }
    }

    private void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (countCoins >= shopItemsSO[i].baseCost)
                myPurchaseBtns[i].interactable = true;
            else
                myPurchaseBtns[i].interactable = false; 
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsSO[i].title; 
            shopPanels[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanels[i].iconItem.sprite = shopItemsSO[i].iconItem; 
            shopPanels[i].costTxt.text = $"Цена: {shopItemsSO[i].baseCost} монет"; 
        }
    }
}
