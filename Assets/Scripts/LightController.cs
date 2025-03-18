using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



public class LightController : MonoBehaviour {

    [Header("References")]
    [Tooltip("The light whose direction will be controlled.")]
    [SerializeField] private Light _light;
    [Tooltip("The ARCameraManager whose light estimation will be used.")]
    [SerializeField] private ARCameraManager _ARCameraManager;

    [Header("Settings")]
    [Tooltip("If true, the light direction will be updated every frame.")]
    [SerializeField] private bool _runEveryFrame = false;

    private bool _isFirstFrame = true;



    void Awake() {
        _ARCameraManager.frameReceived += OnFrameReceived;
    }



    private void OnFrameReceived(ARCameraFrameEventArgs args) {
        if (_isFirstFrame || _runEveryFrame) {
            SetLightDirection(args.lightEstimation.mainLightDirection);
            _isFirstFrame = false;
        }
    }



    private void SetLightDirection(Vector3? direction) {
        if (direction == null)
            return;
        
        _light.transform.rotation = Quaternion.LookRotation((Vector3)direction);
    }



    void OnDestroy() {
        _ARCameraManager.frameReceived -= OnFrameReceived;
    }

    

}
