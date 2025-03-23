using UnityEngine;
using UnityEngine.XR.ARFoundation;



public class InfoTextScript : MonoBehaviour {

    [Header("References")]
    [Tooltip("Plane controller to listen for plane selection.")]
    [SerializeField] private PlaneController _planeController;



    void Awake() {
        _planeController.OnPlaneSelected += OnPlaneSelected;
    }



    private void OnPlaneSelected(ARPlane plane) {
        gameObject.SetActive(false);        
    }



    void OnDestroy() {
        _planeController.OnPlaneSelected -= OnPlaneSelected;
    }



}
