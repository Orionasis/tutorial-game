using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkItem : MonoBehaviour
{
    private int itemID;
    public GameObject bodyPart;
   
    public List<GameObject> itemList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            itemID = gameObject.GetComponentInChildren<ItemOnObject>().item.itemID;
        }
        else
        {
            itemID = 0;
            for(int i = 0; i < itemList.Count; i++)
            {
                itemList[i].SetActive(false);
            }
        }
        if (bodyPart.transform.childCount > 1)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                itemList[i].SetActive(false);
            }
        }
        if(itemID==1 && transform.childCount > 0)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                if (i == 0)
                {
                    itemList[i].SetActive(true);
                }
            }
        }
        
        if (itemID == 3 && transform.childCount > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (i == 0  || i==1)
                {
                    itemList[i].SetActive(true);
                }
            }
        }
        if (itemID == 2 && transform.childCount > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (i == 0)
                {
                    itemList[i].SetActive(true);
                }
            }
        }
        if(itemID == 4 && transform.childCount > 0)
        {
            for(int i = 1; i < itemList.Count; i++)
            {
                if (i == 1)
                {
                    itemList[i].SetActive(true);
                }
            }
        }
    }
}
