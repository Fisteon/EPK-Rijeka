using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleProjectile : MonoBehaviour
{
    public Vector3 targetLocation;
    public bool isItPrecise;
    public int position;
    System.Random rand;
    // Start is called before the first frame update
    void Start()
    {
        targetLocation = new Vector3(targetLocation.x, targetLocation.y - 1.5f, targetLocation.z);
        StartCoroutine(Fly());
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fly()
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 positionStart = this.transform.position;
        Quaternion rotationStart = this.transform.rotation;
        Quaternion rotationTarget = Random.rotation;
        
        while (elapsed < duration)
        {
            float x = (elapsed / duration) * 3;
            float y = 1 - Mathf.Exp(-x);

            this.transform.position = new Vector3(Mathf.Lerp(positionStart.x, targetLocation.x, elapsed / duration), 
                Mathf.Lerp(positionStart.y, targetLocation.y, y), Mathf.Lerp(positionStart.z, targetLocation.z, elapsed / duration));

            this.transform.rotation = Quaternion.Slerp(rotationStart, rotationTarget, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.parent.GetComponent<QuestCMonkey>().ThrowFinished(position, isItPrecise);
        Destroy(this.gameObject);
    }
}
