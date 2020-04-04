using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class master : MonoBehaviour
{
    public float health;
    public TextMeshProUGUI health_text;
    public string gameover_scene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        health_text_update();
    }
    void health_text_update()
    {
        health_text.text = health.ToString("N0");
    }
    void gameover()
    {
        SceneManager.LoadScene(sceneName: gameover_scene);
    }
}
