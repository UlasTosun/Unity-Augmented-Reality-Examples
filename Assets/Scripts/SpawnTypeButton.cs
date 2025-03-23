using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;



public class SpawnTypeButton : MonoBehaviour {

    [Header("References")]
    [Tooltip("Plane controller to listen for plane selection.")]
    [SerializeField] private PlaneController _planeController;
    [Tooltip("Spawner to set the spawn type.")]
    [SerializeField] private Spawner _spawner;
    [Tooltip("Text to display the current spawn type.")]
    [SerializeField] private TextMeshProUGUI _text;

    private Button _button;



    void Awake() {
        _planeController.OnPlaneSelected += OnPlaneSelected;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        gameObject.SetActive(false);
    }



    void Start() {
        SetText();
    }



    private void OnPlaneSelected(ARPlane plane) {
        gameObject.SetActive(true);        
    }



    private void OnClick() {
        if (_spawner.SpawnType == SpawnType.Obstacle) {
            _spawner.SpawnType = SpawnType.Agent;
        } else if (_spawner.SpawnType == SpawnType.Agent) {
            _spawner.SpawnType = SpawnType.None;
        } else {
            _spawner.SpawnType = SpawnType.Obstacle;
        }

        SetText();
    }



    private void SetText() {
        if (_spawner.SpawnType == SpawnType.Obstacle) {
            _text.text = "Spawn Obstacle";
        } else if (_spawner.SpawnType == SpawnType.Agent) {
            _text.text = "Spawn Agent";
        } else {
            _text.text = "Control Agent";
        }
    }



    void OnDestroy() {
        _planeController.OnPlaneSelected -= OnPlaneSelected;
    }


}
