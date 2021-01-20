using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject sceneTrigger;
    public List<GameObject> trees;

    public Camera cylinderCamera;

    GameController.GMScene scene;
    bool cylinderEnabled;
    bool cleanup;

    // Start is called before the first frame update
    void Start()
    {
        cylinderEnabled = false;
        cleanup = false;
        scene = GameController.Master.scenes["Statue"];
        StartCoroutine(WaitForSceneStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (scene.started)
        {
            this.GetComponent<CapsuleCollider>().enabled = true;
            Debug.Log("starting statue scene");
        }
        else
        {
            return;
        }
        if (scene.interacted && !cylinderEnabled && !cylinder.GetComponent<StatueCylinder>().solved)
        {
            // disable player
            // swap cameras
            // enable cylinder

            ToggleCylinder(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && cylinderEnabled)
        {
            // enable player
            // swap cameras
            // disable cylinder
            scene.interacted = false;
            ToggleCylinder(false);
        }

        if (cylinder.GetComponent<StatueCylinder>().solved && !cleanup)
        {
            cleanup = true;
            ToggleCylinder(false);
            PlayerController._PlayerController.interactables.Remove(this.gameObject);
            Destroy(this.gameObject);
            foreach(GameObject tree in trees)
            {
                Destroy(tree);
            }
        }
    }

    IEnumerator WaitForSceneStart()
    {
        yield return new WaitUntil(() => sceneTrigger.GetComponent<SceneStarter>().playerEntered);
        scene.startingCondition = true;
    }

    private void ToggleCylinder(bool state)
    {
        GameController.Master.questSolving = state;
        PlayerController._PlayerController.camera.enabled = !state;
        this.GetComponent<Behaviour>().enabled = !state;
        cylinderCamera.enabled = state;
        cylinder.SetActive(state);
        cylinderEnabled = state;
    }
}
