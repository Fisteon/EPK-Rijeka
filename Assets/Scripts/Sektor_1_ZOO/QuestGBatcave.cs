using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGBatcave : Scene
{
    public BoxCollider spawnZone;
    public GameObject starPrefab;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Interact", "Good thing I collected those stars!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        SpawnStars();
        PushMessageToMaster(texts["Interact"]);
        RemoveFromInteractables();
        this.GetComponent<BoxCollider>().enabled = false;
        finished = true;
    }

    void SpawnStars()
    {
        for (int i = 0; i < 10 /* stars collected */; i++)
        {
            Vector3 spawnPoint = GetRandomPoint();
            Instantiate(starPrefab, spawnPoint, Quaternion.identity, this.transform);
        }
    }

    Vector3 GetRandomPoint()
    {
        return new Vector3(
            Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x),
            Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y),
            Random.Range(spawnZone.bounds.min.z, spawnZone.bounds.max.z));
    }
}
