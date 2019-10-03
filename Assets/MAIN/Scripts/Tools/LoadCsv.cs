using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCsv : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset CsvData = Resources.Load<TextAsset>("ObjVoler");
        string[] data = CsvData.text.Split(new char[] { '\n' });
        //Debug.Log(data.Length);
        for(int i =1; i<data.Length -1; i++)
        {
            string[] row = data[i].Split(new char[] { ';' });
            DataObj d = new DataObj();
            d.name = row[0];
            int.TryParse(row[1], out d.Player);
            int.TryParse(row[2], out d.get);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
