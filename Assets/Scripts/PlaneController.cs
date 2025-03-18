using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



public class PlaneController : MonoBehaviour {

    [Header("References")]
    [Tooltip("The AR plane manager to get the planes from.")]
    [SerializeField] private ARPlaneManager _planeManager;
    [Tooltip("The AR ray interactor to get the raycast hit from.")]
    [SerializeField] private XRRayInteractor _rayInteractor;



    private ARPlane _selectedPlane;

    public event UnityAction<ARPlane> OnPlaneSelected;
    public ARPlane SelectedPlane {
        get {
            return _selectedPlane;
        }

        set {
            if (_selectedPlane != value) {
                _selectedPlane = value;
                MarkPlane(_selectedPlane);
                StopPlaneDetection();
                OnPlaneSelected?.Invoke(_selectedPlane);
            }
        }
    }



    void Update() {
        if (SelectedPlane != null)
            return;

        CheckForPlanes();
    }



    private void CheckForPlanes() {
        var isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);

        if (!isPointerOverUI && _rayInteractor.TryGetCurrentARRaycastHit(out var arRaycastHit)) {
            if (arRaycastHit.trackable is ARPlane plane)
                SelectedPlane = plane;
        }
    }



    private void MarkPlane(ARPlane plane) {
        plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
        
        plane.GetComponent<LineRenderer>().enabled = true;
        plane.GetComponent<LineRenderer>().startColor = Color.green;
        plane.GetComponent<LineRenderer>().endColor = Color.green;        
    }



    private void StopPlaneDetection() {
        foreach (var plane in _planeManager.trackables) {
            if (plane != SelectedPlane)
                plane.gameObject.SetActive(false);
        }

        _planeManager.enabled = false;
    }



}
