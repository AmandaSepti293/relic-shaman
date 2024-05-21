using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject menupanel;
    public GameObject optionspanel;
    // Start is called before the first frame update
    void Start()
    {
        menupanel.SetActive(true);
        optionspanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void OptionsButton()
    {
        menupanel.SetActive(false);
        optionspanel.SetActive(true);
    }

    public void BackButton()
    {
        menupanel.SetActive(true);
        optionspanel.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Tombol Keluar Telah Ditekan");
    }
}
