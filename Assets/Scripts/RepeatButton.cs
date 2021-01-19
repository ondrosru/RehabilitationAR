using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RepeatButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("ReabilitationAR");
    }
}
