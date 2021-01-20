using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapPlayerArrow : MonoBehaviour
{
    public Transform north;
    public Image mapPlayerArrow;
    public RectTransform minimapArrow;

    private Vector3 playerLastPosition;
    private float northOffset;

    private void Start()
    {
        this.playerLastPosition = PlayerController._PlayerController.transform.position;
        northOffset = north.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        Vector3 playerPositionDelta = this.playerLastPosition - PlayerController._PlayerController.transform.position;
        this.playerLastPosition = PlayerController._PlayerController.transform.position;
        Vector3 playerMoveDelta = Quaternion.AngleAxis(-northOffset, Vector3.up) * playerPositionDelta;

        this.mapPlayerArrow.GetComponent<RectTransform>().rotation = minimapArrow.rotation;
        playerMoveDelta.y = playerMoveDelta.z;

        RectTransform rectTransform = this.mapPlayerArrow.rectTransform;
        if (SceneManager.GetActiveScene().name == "Sector_3")
        {
            rectTransform.position = rectTransform.position - playerMoveDelta * 2.21f;
        }
        else
        {
            rectTransform.position = rectTransform.position - playerMoveDelta * 2.21f;
        }
    }
}