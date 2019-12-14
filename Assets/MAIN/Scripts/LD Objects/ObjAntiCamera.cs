using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjAntiCamera : Obj
{
    public List<CameraRotate> came=  new List<CameraRotate>();
    protected override void Start()
    {
        base.Start();

        StartCoroutine(ActiveOrinactiveCamera());
    }
    // Start is called before the first frame update
    public override void ActiveEvent()
    {
        base.ActiveEvent();
        foreach (CameraRotate cam in came)
        {
            cam.gameObject.GetComponent<Collider2D>().enabled = true;
            if(cam.transform.childCount>0)
                cam.transform.GetChild(0).gameObject.SetActive(true);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

            if (collision.gameObject.GetComponent<CameraRotate>())
            {
                came.Add(collision.gameObject.GetComponent<CameraRotate>());
            }
     
    }
    IEnumerator ActiveOrinactiveCamera()
    {
        yield return new WaitForSeconds(1);
        foreach(CameraRotate cam in came)
        {
            cam.gameObject.GetComponent<Collider2D>().enabled = false;
            if (cam.transform.childCount > 0)
                cam.transform.GetChild(0).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2);
        foreach (CameraRotate cam in came)
        {
            cam.gameObject.GetComponent<Collider2D>().enabled = true;
            if (cam.transform.childCount > 0)
                cam.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (this != null) StartCoroutine(ActiveOrinactiveCamera());

    }
}
