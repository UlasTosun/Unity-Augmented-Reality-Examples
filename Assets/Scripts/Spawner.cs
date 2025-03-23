using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



public class Spawner : MonoBehaviour {

    [Header("References")]
    [Tooltip("Obstacle to spawn.")]
    [SerializeField] private GameObject _obstacleToSpawn;
    [Tooltip("Agent to spawn.")]
    [SerializeField] private GameObject _agentToSpawn;
    [Tooltip("Plane controller to get the selected plane from.")]
    [SerializeField] private PlaneController _planeController;
    [Tooltip("The AR ray interactor to get the raycast hit from.")]
    [SerializeField] private XRRayInteractor _rayInteractor;

    private bool _isPlaneSelected;
    private bool _readyToSpawn;
    bool _everHadSelection;
    private GameObject _prefabToSpawn;
    private SpawnType _spawnType;

    public event UnityAction<GameObject> OnObjectSpawned;

    public SpawnType SpawnType { 
        get => _spawnType;
        set {
            _spawnType = value;
            _prefabToSpawn = _spawnType == SpawnType.Obstacle ? _obstacleToSpawn : _spawnType == SpawnType.Agent ? _agentToSpawn : null;
        }
    }



    void Awake() {
        SpawnType = SpawnType.Obstacle;
        _planeController.OnPlaneSelected += OnPlaneSelected;
    }



    void Update() {
        if (!_isPlaneSelected || _prefabToSpawn == null)
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

        if (SpawnType == SpawnType.Agent) {
            Agent agent = spawnedObject.GetComponent<Agent>();
            agent.Initialize(_rayInteractor, _planeController, this);
        }

        OnObjectSpawned?.Invoke(spawnedObject);
    }



    private void OnPlaneSelected(ARPlane plane) {
        _isPlaneSelected = true;        
    }



    void OnDestroy() {
        _planeController.OnPlaneSelected -= OnPlaneSelected;
    }



}



public enum SpawnType {
    Obstacle,
    Agent,
    None
}
