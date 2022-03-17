using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{

    public Inventory inventoryPlayer;
    public GameObject shopPanel;
    public ItemDataBaseList itemDb;
    public PlayerInventory playerInv;

    [Header("ID des items du shop")]
    public int item1Id;
    public int item2Id;
    public int item3Id;
    public int item4Id;

    public Text textItem1;
    public Text textItem2;
    public Text textItem3;
    public Text textItem4;

    public Image iconItem1;
    public Image iconItem2;
    public Image iconItem3;
    public Image iconItem4;

    private int amountSlots;
    private int slotsChecked;
    private bool transactionDone;

    Item theItem1;
    Item theItem2;
    Item theItem3;
    Item theItem4;
    // Tous les paramètres des objets du shop

    // Use this for initialization
    void Start()
    {
        shopPanel.SetActive(false);
    }

    void PrepareShop()
    {
        theItem1 = itemDb.getItemByID(item1Id);
        theItem2 = itemDb.getItemByID(item2Id);
        theItem3 = itemDb.getItemByID(item3Id);
        theItem4 = itemDb.getItemByID(item4Id);

        textItem1.text = theItem1.itemName + " (Prix : " + theItem1.itemValue + ") ";
        textItem2.text = theItem2.itemName + " (Prix : " + theItem2.itemValue + ") ";
        textItem3.text = theItem3.itemName + " (Prix : " + theItem3.itemValue + ") ";
        textItem4.text = theItem4.itemName + " (Prix : " + theItem4.itemValue + ") ";

        iconItem1.sprite = theItem1.itemIcon;
        iconItem2.sprite = theItem2.itemIcon;
        iconItem3.sprite = theItem3.itemIcon;
        iconItem4.sprite = theItem4.itemIcon;
        
        iconItem1.transform.GetComponent<Button>().onClick.AddListener(delegate { buyItem(theItem1); });
        iconItem2.transform.GetComponent<Button>().onClick.AddListener(delegate { buyItem(theItem2); });
        iconItem3.transform.GetComponent<Button>().onClick.AddListener(delegate { buyItem(theItem3); });
        iconItem4.transform.GetComponent<Button>().onClick.AddListener(delegate { buyItem(theItem4); });

        shopPanel.SetActive(true);
    }

    void buyItem(Item finalItem)
    {
        amountSlots = inventoryPlayer.transform.GetChild(1).childCount;
        transactionDone = false;
        slotsChecked = 0;
        foreach(Transform child in inventoryPlayer.transform.GetChild(1))
        {
            if (child.childCount == 0)
            {
                if (playerInv.goldCoins >= finalItem.itemValue)
                {
                    playerInv.goldCoins -= finalItem.itemValue;
                    inventoryPlayer.addItemToInventory(finalItem.itemID);
                    transactionDone = true;
                    print("Le joueur a acheté l'objet : " + finalItem.itemName);
                    break;
                }
                else
                {
                    print("Transaction refusée, le joueur n'a pas assez d'argent");
                }
            }
            slotsChecked++;
        }
        if (slotsChecked == amountSlots && transactionDone == false)
        {
            print("Transaction annulée, pas de place dans l'inventaire");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PrepareShop();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            iconItem1.GetComponent<Button>().onClick.RemoveAllListeners();
            iconItem2.GetComponent<Button>().onClick.RemoveAllListeners();
            iconItem3.GetComponent<Button>().onClick.RemoveAllListeners();
            iconItem4.GetComponent<Button>().onClick.RemoveAllListeners();
            shopPanel.SetActive(false);
        }
    }

}