using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



public class DeleteButton : MonoBehaviour {

    [Header("References")]
    [Tooltip("The AR ray interactor to get the raycast hit from.")]
    [SerializeField] private XRRayInteractor _rayInteractor;
    [Tooltip("Plane controller to listen for plane selection.")]
    [SerializeField] private PlaneController _planeController;

    private Button _button;
    private GameObject _selectedObject;



    void Awake() {
        _rayInteractor.selectEntered.AddListener(OnSelectEntered);
        _planeController.OnPlaneSelected += OnPlaneSelected;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        gameObject.SetActive(false);
    }



    private void OnPlaneSelected(ARPlane plane) {
        gameObject.SetActive(true);        
    }



    private void OnSelectEntered(SelectEnterEventArgs args) {
        _selectedObject = args.interactableObject.transform.gameObject;
    }



    private void OnClick() {
        if (_selectedObject != null) {
            Destroy(_selectedObject);
            _selectedObject = null;
        }
    }



    void OnDestroy() {
        _rayInteractor.selectEntered.RemoveListener(OnSelectEntered);
        _planeController.OnPlaneSelected -= OnPlaneSelected;
    }


}
