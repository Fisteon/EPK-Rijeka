using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectiblesGeneration : MonoBehaviour
{
    [Header("Sound")]
    public GameObject endMusic;
    public GameObject turnOffMusic;

    [Header("Running Path")]
    [Range(10, 70)]
    public int spawnablesCount = 20;
    public PathCreator runningPath;

    public List<float> pairs;

    public List<GameObject> spawnables;
    public List<int> poolSpawnables;

    [Header("Scene")]
    public ToiletPaperActivation TP;
    public EducationalObjects slowmotionTrigger;


    RaycastHit rayInfo;

    int extraSeconds;
    float distanceTravelled;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = this.GetComponent<PathFollower>().speed;
        pairs = new List<float>();
        VertexPath path = runningPath.path;
        GameController.Master.StartTheClock();

        int pairsAmount = spawnablesCount / 2;
        float range = 90f / pairsAmount;

        for (int i = 0; i < pairsAmount; i++)
        {
            float A = Random.Range(0f, range);
            float B = range - A;
            pairs.Add(A);
            pairs.Add(B);
        }
        ShuffleListOrder(pairs);
        CreatePoolSpawnableItems(pairs.Count);

        float lastDistance = 3f;
        for (int i = 0; i < pairs.Count; i++)
        {
            Vector3 pointOnPath = path.GetPointAtDistance(((lastDistance + pairs[i]) / 100) * path.length);
            if (Physics.Raycast(pointOnPath, Vector3.down, out rayInfo, 1 << 9)){
                pointOnPath = new Vector3(pointOnPath.x, rayInfo.point.y + 1.5f, pointOnPath.z);
            }
            Vector3 rotation = path.GetRotationAtDistance((lastDistance + pairs[i]) / 100 * path.length).eulerAngles;
            rotation.x = 0;
            rotation.y += 180f;
            rotation.z = 0;
            Instantiate(spawnables[poolSpawnables[i]], pointOnPath, Quaternion.Euler(rotation));
            lastDistance += pairs[i] + 7f / spawnablesCount;
        }
        StartCoroutine(WaitForSlowmotion());
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.U))
        {
            TP.finished = true;
        }
    }

    void ShuffleListOrder<T>(List<T> pairs)
    {
        int count = pairs.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int r = UnityEngine.Random.Range(i, count);

            T temp = pairs[i];
            pairs[i] = pairs[r];
            pairs[r] = temp;
        }
    }

    void CreatePoolSpawnableItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i < (count * 0.05f))
            {
                extraSeconds += 10;
                poolSpawnables.Add((int)Spawnables.GasMask);
            }
            else if (i < (count * 0.2f))
            {
                extraSeconds += 5;
                poolSpawnables.Add((int)Spawnables.FirstAid);
            }
            else if (i < (count * 0.5f))
            {
                extraSeconds += 3;
                poolSpawnables.Add((int)Spawnables.Pills);
            }
            else
            {
                extraSeconds += 1;
                poolSpawnables.Add((int)Spawnables.SprayBottle);
            }
        }
        ShuffleListOrder(poolSpawnables);
    }

    IEnumerator WaitForSlowmotion()
    {
        yield return new WaitUntil(() => slowmotionTrigger.visited == true);
        endMusic.SetActive(true);

        while(Time.timeScale > 0.3f)
        {
            Time.timeScale -= 0.05f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitUntil(() => distanceTravelled >= runningPath.path.length);
        yield return new WaitForSeconds(2.5f);
        TP.finished = true;
        Time.timeScale = 1f;
    }

    enum Spawnables
    {
        SprayBottle,
        Pills,
        FirstAid,
        GasMask
    }
}
