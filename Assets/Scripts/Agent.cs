using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



public class Agent : MonoBehaviour {

    private XRRayInteractor _rayInteractor;
    private PlaneController _planeController;
    private Spawner _spawner;
    private NavMeshAgent _navMeshAgent;
    private Transform _selectedObject;



    void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }



    public void Initialize(XRRayInteractor rayInteractor, PlaneController planeController, Spawner spawner) {
        _rayInteractor = rayInteractor;
        _planeController = planeController;
        _spawner = spawner;

        _rayInteractor.selectEntered.AddListener(OnSelectEntered);
    }



    void Update() {
        if (_spawner.SpawnType != SpawnType.None)
            return;

        CheckForPlanes();
    }



    private void CheckForPlanes() {
        var isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);

        if (!isPointerOverUI && _rayInteractor.TryGetCurrentARRaycastHit(out var arRaycastHit)) {
            if (arRaycastHit.trackable is ARPlane plane)
                if (_planeController.SelectedPlane == plane && _selectedObject != null && _selectedObject == transform)
                    _navMeshAgent.SetDestination(arRaycastHit.pose.position);
        }
    }



    private void OnSelectEntered(SelectEnterEventArgs args) {
        _selectedObject = args.interactableObject.transform;
    }



    void OnDestroy() {
        _rayInteractor.selectEntered.RemoveListener(OnSelectEntered);
    }



}
