using UnityEngine;



public class ScreenController : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("If true, the screen will never turn off.")]
    [SerializeField] private bool _alwaysOnScreen = true;
    


    void Start() {
        if (_alwaysOnScreen)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }



}
