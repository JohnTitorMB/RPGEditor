using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGComponent : MonoBehaviour
{
    private List<GameObject> onTriggerEnterList = new List<GameObject>();
    private List<GameObject> onTriggerStayList = new List<GameObject>();
    private List<GameObject> onTriggerExitList = new List<GameObject>();

    public List<GameObject> OnTriggerExitList { get => onTriggerExitList; set => onTriggerExitList = value; }
    public List<GameObject> OnTriggerStayList { get => onTriggerStayList; set => onTriggerStayList = value; }
    public List<GameObject> OnTriggerEnterList { get => onTriggerEnterList; set => onTriggerEnterList = value; }

    private void LateUpdate()
    {
        onTriggerEnterList.Clear();
        onTriggerExitList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RPGComponent>())
            onTriggerEnterList.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<RPGComponent>())
        {
            if(onTriggerStayList.Contains(other.gameObject))
                OnTriggerStayList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<RPGComponent>())
            onTriggerEnterList.Remove(other.gameObject);

        if (other.gameObject.GetComponent<RPGComponent>())
            onTriggerStayList.Remove(other.gameObject);

        if (other.gameObject.GetComponent<RPGComponent>())
            onTriggerExitList.Add(other.gameObject);
    }


    public void Test(int i, string s, int ii, string ss, int iii, string sss, int iiii, string ssss)
    {
        Debug.Log("oui");
    }

    public void Bonnour(int i, string s, string l)
    {
        Debug.Log("oui");
    }
}
