using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public SkinnedMeshRenderer shirt;
    public SkinnedMeshRenderer pants;

    public SkinnedMeshRenderer rootPlayerObject;
    public List<Material> PJ_Materials;
    [SerializeField]
    private Canvas GUI_canvas;

    public GameObject interactionKeyInstruction;

    private TextMeshProUGUI GUI_interact_text;
    private RectTransform GUI_canvas_transform;

    public Camera camera;
    public GameObject minimapCamera;
    public GameObject minimapArrow;
    private float startingRotation;

    #region Singleton
    private static PlayerController _PController = null;
    public static PlayerController _PlayerController
    {
        get
        {
            if (_PController == null)
            {
                _PController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
            }
            if (_PController == null)
            {
                GameObject obj = new GameObject("GameMaster");
                _PController = obj.AddComponent(typeof(PlayerController)) as PlayerController;
            }
            return _PController;
        }
    }
    #endregion

    public List<GameObject> interactables;

    private void Awake()
    {
        Material[] mats = rootPlayerObject.GetComponent<Renderer>().materials;
        Debug.Log(DataTransfer.material);


        if (DataTransfer.material > 0 && DataTransfer.material <= 4)
        {
            mats[1] = PJ_Materials[DataTransfer.material - 1];
            Destroy(DataTransfer.data);
        }
        else
        {
            mats[1] = PJ_Materials[1];
        }
        rootPlayerObject.GetComponent<Renderer>().materials = mats;
    }

    void Start()
    {
        interactables = new List<GameObject>();
        GUI_canvas_transform = GUI_canvas.GetComponent<RectTransform>();
        startingRotation = transform.eulerAngles.y + minimapArrow.GetComponent<RectTransform>().rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactables.Count != 0)
        {
            RepositionInteractionText(interactables[0]);
            if (Input.GetButtonDown("Interact"))//GetKeyDown(KeyCode.E))
            {
                //GameController.Master.GetInputFromPlayer(interactables[0]);
                interactables[0].GetComponent<Scene>().OnPlayerInteract();
            }
        }
        else
        {
            interactionKeyInstruction.SetActive(false);
        }
        if (minimapCamera != null) RepositionMinimapCamera();
        RotateMinimapArrow();
    }

    private void RepositionMinimapCamera()
    {
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.y = minimapCamera.transform.position.y;
        minimapCamera.transform.position = newCameraPosition;
    }

    private void RotateMinimapArrow()
    {
        minimapArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, startingRotation - transform.eulerAngles.y);
    }

    private void RepositionInteractionText(GameObject questItem)
    {
        Vector3 position = questItem.transform.position;
        Vector2 worldPosition = camera.WorldToViewportPoint(position);
        Vector2 screenPosition = new Vector2(
            (worldPosition.x * GUI_canvas_transform.sizeDelta.x - GUI_canvas_transform.sizeDelta.x * 0.5f),
            (worldPosition.y * GUI_canvas_transform.sizeDelta.y - GUI_canvas_transform.sizeDelta.y * 0.5f));

        interactionKeyInstruction.SetActive(true);
        interactionKeyInstruction.GetComponent<RectTransform>().anchoredPosition = screenPosition;
    }

    public void TogglePlayerOnOff(bool state)
    {
        camera.gameObject.SetActive(state);
        GameController.Master._GUI_interaction_text.SetActive(state);
        GameController.Master.questSolving = !state;
    }
}