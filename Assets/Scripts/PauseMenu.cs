using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject s;
    private bool k = false;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (k == false)
            {
                k = true;
                s.SetActive(true);
                Time.timeScale = 0f;
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                
                Collider2D collider = obj.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
            }
            else{
                Time.timeScale = 1f;
                k = false;
                s.SetActive(false);
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                
                Collider2D collider = obj.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
            }
        }
    }
}
