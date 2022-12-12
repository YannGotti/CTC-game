using UnityEngine;

[CreateAssetMenu(fileName ="shopMenu", menuName ="Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemSO : ScriptableObject
{
    [SerializeField] public string title; 
    [SerializeField] public string description; 
    [SerializeField] public int baseCost;
    [SerializeField] public Sprite iconItem; 
}
