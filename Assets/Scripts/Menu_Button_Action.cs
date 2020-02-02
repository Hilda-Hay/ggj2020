using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu_Button_Action : MonoBehaviour
{
    public void changeScene()
    {
         SceneManager.LoadScene("Scene");
    }
}
