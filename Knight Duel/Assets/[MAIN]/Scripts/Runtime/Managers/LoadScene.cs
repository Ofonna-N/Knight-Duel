using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnightDuel
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private int scene;

        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene(scene);
        }
    }
}
