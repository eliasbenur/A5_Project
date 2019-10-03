using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance;
    public GameObject panelInventory;
    public GameObject PrefabInvetoryUI;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

   public GameObject AddObjInInvetory(Sprite sp)
    {
        GameObject go = Instantiate(PrefabInvetoryUI, panelInventory.transform);
        if (go.GetComponent<Image>() != null) go.GetComponent<Image>().sprite = sp;
        return go;
    }
}
