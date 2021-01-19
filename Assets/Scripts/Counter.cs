using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    private int count;
    public void SetCount(int count)
    {
        this.count = count;
        this.GetComponent<Text>().text = "Count: " + count.ToString();
    }
    public int GetCount()
    {
        return count;
    }
}
