using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int countCoins;

    [SerializeField] private TMP_Text coinsUI;
    [SerializeField] private ShopItemSO[] shopItemsSO;
    [SerializeField] private ShopTemplate[] shopPanels;
    [SerializeField] private Button[] myPurchaseBtns;

    [SerializeField] private MySqlConnector _connector;


    void Start()
    {
        countCoins = _connector.SelectMoney();

        LoadPanels();
        CheckPurchaseable();

        coinsUI.text = $"{countCoins}";
    }

    public void PurchaseableItem(int btnNum)
    {
        if(countCoins >= shopItemsSO[btnNum].baseCost)
        {
            _connector.UpdateMoneyUser(-shopItemsSO[btnNum].baseCost);
            countCoins -= shopItemsSO[btnNum].baseCost;
            coinsUI.text = $"{countCoins}";
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
