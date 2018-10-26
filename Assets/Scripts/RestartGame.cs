using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void OnClick(string TestingV2)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(TestingV2);
    }
}
