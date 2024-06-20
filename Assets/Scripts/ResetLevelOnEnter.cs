using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevelOnEnter : MonoBehaviour
{
    void OnTriggerEnter2D(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
