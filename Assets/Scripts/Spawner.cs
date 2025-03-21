using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



public class Spawner : MonoBehaviour {

    [Header("References")]
    [Tooltip("Prefab to spawn.")]
    [SerializeField] private GameObject _prefabToSpawn;
    [Tooltip("Plane controller to get the selected plane from.")]
    [SerializeField] private PlaneController _planeController;
    [Tooltip("The AR ray interactor to get the raycast hit from.")]
    [SerializeField] private XRRayInteractor _rayInteractor;

    private bool _isPlaneSelected;
    private bool _readyToSpawn;
    bool _everHadSelection;

    public event UnityAction<GameObject> OnObjectSpawned;



    void Awake() {
        _planeController.OnPlaneSelected += OnPlaneSelected;
    }



    void Update() {
        if (!_isPlaneSelected)
            return;

        CheckForSpawn();
    }



    private void CheckForSpawn() {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
            return;

        if (_readyToSpawn && _rayInteractor.TryGetCurrentARRaycastHit(out var arRaycastHit)) {
            if ((ARPlane)arRaycastHit.trackable == _planeController.SelectedPlane) {
                Spawn(arRaycastHit.pose.position);
                _readyToSpawn = false;
                return;
            }
        }

        if (_rayInteractor.logicalSelectState.wasPerformedThisFrame)
            _everHadSelection = _rayInteractor.hasSelection;
        else if (_rayInteractor.logicalSelectState.active)
            _everHadSelection |= _rayInteractor.hasSelection;

        _readyToSpawn = false;
        
        if (_rayInteractor.logicalSelectState.wasCompletedThisFrame)
            _readyToSpawn = !_rayInteractor.hasSelection && !_everHadSelection;
    }



    private void Spawn(Vector3 position) {
        var spawnedObject = Instantiate(_prefabToSpawn, position, Quaternion.identity, transform);
        OnObjectSpawned?.Invoke(spawnedObject);
    }



    private void OnPlaneSelected(ARPlane plane) {
        _isPlaneSelected = true;        
    }



    void OnDestroy() {
        _planeController.OnPlaneSelected -= OnPlaneSelected;
    }



}
