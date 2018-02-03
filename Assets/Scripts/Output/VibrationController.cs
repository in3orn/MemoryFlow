using System;
using Dev.Krk.MemoryFlow.Game;
using UnityEngine;

public class VibrationController : MonoBehaviour
{
    [SerializeField]
    private LevelController levelController;

    void Start()
    {

    }

    void OnEnable()
    {
        levelController.OnPlayerFailed += ProcessPlayerFailed;
    }

    void OnDisable()
    {
        if(levelController != null)
        {
            levelController.OnPlayerFailed -= ProcessPlayerFailed;
        }
    }

    private void ProcessPlayerFailed()
    {
        Handheld.Vibrate();
    }
}
