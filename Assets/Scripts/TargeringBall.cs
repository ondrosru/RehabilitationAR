using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TargeringBall : MonoBehaviour
{
    private float angleBetween = 0.0f;
    public List<GameObject> targets;
    private const float angleEps = 0.5f;
    private const float sizeEps = 0.01f;
    private Sprite hoverImg;
    private Sprite aimImg;
    private Camera mainCamera;
    private bool gameWon = false;
    public Counter counter;
    public AudioSource burstSound;
    private float waitingTime = 0.5f;

    void Start()
    {
        mainCamera = Camera.main;
        aimImg = Resources.Load<Sprite>("Sprites/Aim");
        hoverImg = Resources.Load<Sprite>("Sprites/HoverAim");
    }
    void Update()
    {
        if (gameWon)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime < 0)
            {
                SceneManager.LoadScene("WinMenu");
            }
        }
        else
        {
            bool aimed = false;
            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 targetDir = targets[i].transform.position - mainCamera.transform.position;
                angleBetween = Vector3.Angle(mainCamera.transform.forward, targetDir);
                if (angleBetween <= angleEps)
                {
                    aimed = true;
                    var heading = targets[i].transform.position - mainCamera.transform.position;
                    var distance = heading.magnitude;
                    var scale = targets[i].transform.lossyScale;
                    float sizeAim = GameObject.Find("Canvas/Image").transform.lossyScale.x;
                    float apparentSize = scale.x * 10 / distance;
                    if (Mathf.Abs(sizeAim - apparentSize) <= sizeEps)
                    {
                        burstSound.Play();
                        var target = targets[i];
                        targets.Remove(target);
                        GameObject.Find("ImageTarget").GetComponent<TrajectoryMesh>().RemoveBall(target);
                        Destroy(target);
                        counter.SetCount(targets.Count);
                        if (targets.Count == 0)
                        {
                            gameWon = true;
                        }
                        i--;
                    }
                }
            }
            Image aimSprite = GameObject.Find("Canvas/Image").GetComponent<Image>();
            aimSprite.sprite = aimed ? hoverImg : aimImg;
        }
    }
}
