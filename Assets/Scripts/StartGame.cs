using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void OnClick(string TestingV2)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(TestingV2);
    }
}
