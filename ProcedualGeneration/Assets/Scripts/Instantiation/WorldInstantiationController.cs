using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInstantiationController : MonoBehaviour
{
    public enum Theme
    {
        Forest,
        Space,
        Desert,
        Lava
    };
    [Header("Theme")]
    public Theme theme;
    [Header("Player Settings")]
    public float playerRange = 30f;
    [Header("Terrain Settings")]
    [Range(1, 10000)]
    public float terrainSize = 1000f;

    [Header("Foreground Settings")]
    [Range(0, 1000)]
    public int foregroundObjectsNumber = 100;

    [Header("Background Settings")]
    [Range(0, 3)]
    public float backgroundObjectsInitialHeight = 0.2384666f;
    public float backgroundObjectsRange = 10f;
    [Range(0, 10)]
    public float backgroundScaleOverDist = 1f;
    [Range(0, 1000)]
    public int backgroundObjectsNumber = 100;

    [Header("Upground Settings")]
    public float skyObjectsMinHeight = 30f;
    public float skyObjectsMaxHeight = 60f;
    [Range(0, 1000)]
    public int upgroundObjectsNumber = 100;


    enum ObjectType
    {
        Backgrounds,
        Foregrounds,
        Upgrounds
    };

    private void Start()
    {
        TerrainInstantiation();

        //Instantiate foreground objects
        ObjectsInstantiation(playerRange, terrainSize/2, 0f, true, foregroundObjectsNumber, ObjectType.Foregrounds);

        //Instantiate background objects
        ObjectsInstantiation(terrainSize/2/Mathf.Sqrt(2), terrainSize/2+backgroundObjectsRange, -3f, false, backgroundObjectsNumber, ObjectType.Backgrounds);

        //Instantiate upground objects
        ObjectsInstantiation(0, terrainSize / 2, 0f, true, upgroundObjectsNumber, ObjectType.Upgrounds);

    }

    //Load tip: all prefabs need to put in the Resources folder to ensure Resources.Load works
    Material LoadMaterial(string dir)
    {
        return Resources.Load<Material>(dir);
    }

   GameObject[] LoadGameobjects(string dir)
    {
        return Resources.LoadAll<GameObject>(dir);
    }

    void TerrainInstantiation()
    {
        GameObject terrainParent = new GameObject();
        terrainParent.name = "Terrain";
        terrainParent.transform.parent = transform;
        GameObject terrainInst = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(terrainInst.GetComponent<CapsuleCollider>());
        terrainInst.AddComponent<BoxCollider>();
        terrainInst.transform.localScale = new Vector3(terrainSize,1f,terrainSize);
        terrainInst.transform.parent = terrainParent.transform;

        string dir = "Terrains/" + theme.ToString();
        Material terrainMat = LoadMaterial(dir);
        terrainInst.GetComponent<MeshRenderer>().material = terrainMat;

        dir = "Skyboxes/" + theme.ToString();
        Material skyBox = LoadMaterial(dir);
        RenderSettings.skybox = skyBox;
    }

    void ObjectsInstantiation(float minRange, float maxRange, float height, bool withinRadius, int objectsNumber, ObjectType type)
    {
        //Load Objects
        string dir = type.ToString() + "/" + theme.ToString();
        GameObject[] objects = LoadGameobjects(dir);

        if(type == ObjectType.Backgrounds)
        {
            for(int o = 0; o < objects.Length; o++)
            {
                objects[o].transform.localScale = new Vector3(objects[o].transform.localScale.x, backgroundObjectsInitialHeight,objects[o].transform.localScale.z);
            }
        }

        //Set up Parent Object
        GameObject parentGameObject = new GameObject();
        parentGameObject.transform.parent = transform;
        parentGameObject.name = type.ToString();
        Transform parent = parentGameObject.transform;

        float area = (minRange + maxRange) * 4 * (maxRange - minRange);
        int left_num = (int)(2 * (maxRange - minRange) * maxRange / area * objectsNumber);
        int up_num = (int)(2 * (maxRange - minRange) * minRange / area * objectsNumber);

        for (int i = 0; i < left_num; i++)
        {
            Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);

            //Right
            if (type == ObjectType.Upgrounds) height = Random.Range(skyObjectsMinHeight, skyObjectsMaxHeight);
            Vector3 randomSpawnPosition1 = InstantiateBasedOnRangeRight(minRange, maxRange, height, terrainSize / 2, withinRadius);
            GameObject newObject1;
            if (type == ObjectType.Foregrounds)
            {
                int a = Random.Range(0, objects.Length - 1);
                newObject1 = (GameObject)Instantiate(objects[a], randomSpawnPosition1, Quaternion.Euler(randomSpawnRotation));
            }
            else if (type == ObjectType.Backgrounds)
            {
                int a = Random.Range(0, objects.Length - 1);
                newObject1 = (GameObject)Instantiate(objects[a], randomSpawnPosition1, Quaternion.Euler(randomSpawnRotation));
                float scale = CalculateScale(randomSpawnPosition1);
                newObject1.transform.localScale = new Vector3(newObject1.transform.localScale.x * scale,
                newObject1.transform.localScale.y * scale * backgroundScaleOverDist, newObject1.transform.localScale.z * scale);
            }
            else
            {
                int a = Random.Range(0, objects.Length - 1);
                newObject1 = (GameObject)Instantiate(objects[a], randomSpawnPosition1, Quaternion.Euler(randomSpawnRotation));
            }
            newObject1.transform.parent = parent;

            //Left
            if (type == ObjectType.Upgrounds) height = Random.Range(skyObjectsMinHeight, skyObjectsMaxHeight);
            Vector3 randomSpawnPosition4 = InstantiateBasedOnRangeLeft(minRange, maxRange, height, terrainSize / 2, withinRadius);
            GameObject newObject4;
            if (type == ObjectType.Foregrounds)
            {
                int d = Random.Range(0, objects.Length - 1);
                newObject4 = (GameObject)Instantiate(objects[d], randomSpawnPosition4, Quaternion.Euler(randomSpawnRotation));
            }
            else if (type == ObjectType.Backgrounds)
            {
                int d = Random.Range(0, objects.Length - 1);
                newObject4 = (GameObject)Instantiate(objects[d], randomSpawnPosition4, Quaternion.Euler(randomSpawnRotation));
                float scale = CalculateScale(randomSpawnPosition4);
                newObject4.transform.localScale = new Vector3(newObject4.transform.localScale.x * scale,
                newObject4.transform.localScale.y * scale * backgroundScaleOverDist, newObject4.transform.localScale.z * scale);
            }
            else
            {
                int d = Random.Range(0, objects.Length - 1);
                newObject4 = (GameObject)Instantiate(objects[d], randomSpawnPosition4, Quaternion.Euler(randomSpawnRotation));
            }
            newObject4.transform.parent = parent;
        }

        for (int i = 0; i < up_num; i++)
        {
            Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);

            //Up
            if (type == ObjectType.Upgrounds) height = Random.Range(skyObjectsMinHeight, skyObjectsMaxHeight);
            Vector3 randomSpawnPosition2 = InstantiateBasedOnRangeUp(minRange, maxRange, height, terrainSize / 2, withinRadius);
            GameObject newObject2;
            if (type == ObjectType.Foregrounds)
            {
                int b = Random.Range(0, objects.Length - 1);
                newObject2 = (GameObject)Instantiate(objects[b], randomSpawnPosition2, Quaternion.Euler(randomSpawnRotation));
            }
            else if (type == ObjectType.Backgrounds)
            {
                int b = Random.Range(0, objects.Length - 1);
                newObject2 = (GameObject)Instantiate(objects[b], randomSpawnPosition2, Quaternion.Euler(randomSpawnRotation));
                float scale = CalculateScale(randomSpawnPosition2);
                newObject2.transform.localScale = new Vector3(newObject2.transform.localScale.x * scale,
                    newObject2.transform.localScale.y * scale * backgroundScaleOverDist, newObject2.transform.localScale.z * scale);
            }
            else
            {
                int d = Random.Range(0, objects.Length - 1);
                newObject2 = (GameObject)Instantiate(objects[d], randomSpawnPosition2, Quaternion.Euler(randomSpawnRotation));
            }
            newObject2.transform.parent = parent;

            //Down
            if (type == ObjectType.Upgrounds) height = Random.Range(skyObjectsMinHeight, skyObjectsMaxHeight);
            Vector3 randomSpawnPosition3 = InstantiateBasedOnRangeDown(minRange, maxRange, height, terrainSize / 2, withinRadius);
            GameObject newObject3;
            if (type == ObjectType.Foregrounds)
            {
                int c = Random.Range(0, objects.Length - 1);
                newObject3 = (GameObject)Instantiate(objects[c], randomSpawnPosition3, Quaternion.Euler(randomSpawnRotation));
            }
            else if (type == ObjectType.Backgrounds)
            {
                int c = Random.Range(0, objects.Length - 1);
                newObject3 = (GameObject)Instantiate(objects[c], randomSpawnPosition3, Quaternion.Euler(randomSpawnRotation));
                float scale = CalculateScale(randomSpawnPosition3);
                newObject3.transform.localScale = new Vector3(newObject3.transform.localScale.x * scale,
                    newObject3.transform.localScale.y * scale * backgroundScaleOverDist, newObject3.transform.localScale.z * scale);
            }
            else
            {
                int c = Random.Range(0, objects.Length - 1);
                newObject3 = (GameObject)Instantiate(objects[c], randomSpawnPosition3, Quaternion.Euler(randomSpawnRotation));
            }
            newObject3.transform.parent = parent;
        }
    }


    //1)InstantiateBasedOnRangeRight: find a random location at the RIGHT range under range restrictions
    Vector3 InstantiateBasedOnRangeRight(float minRange, float maxRange, float height, float radius, bool withinRadius)
    {
        Vector3 randomSpawnPosition;
        do
        {
            randomSpawnPosition = new Vector3(Random.Range(minRange, maxRange), transform.position.y + height, Random.Range(-maxRange, maxRange));
        } while (!Comparator(randomSpawnPosition,radius,withinRadius));
        return randomSpawnPosition;
    }

    //2)InstantiateBasedOnRangeUp: find a random location at the UP range under range restrictions
    Vector3 InstantiateBasedOnRangeUp(float minRange, float maxRange, float height, float radius, bool withinRadius)
    {
        Vector3 randomSpawnPosition;
        do
        {
            randomSpawnPosition = new Vector3(Random.Range(-minRange, minRange), transform.position.y + height, Random.Range(minRange, maxRange));
        } while (!Comparator(randomSpawnPosition, radius, withinRadius));
        return randomSpawnPosition;
    }

    //3)InstantiateBasedOnRangeDown: find a random location at the DOWN range under range restrictions
    Vector3 InstantiateBasedOnRangeDown(float minRange, float maxRange, float height, float radius, bool withinRadius)
    {
        Vector3 randomSpawnPosition;
        do
        {
            randomSpawnPosition = new Vector3(Random.Range(-minRange, minRange), transform.position.y + height, Random.Range(-maxRange, -minRange));
        } while (!Comparator(randomSpawnPosition, radius, withinRadius));
        return randomSpawnPosition;
    }

    //4)InstantiateBasedOnRangeLeft: find a random location at the LEFT range under range restrictions
    Vector3 InstantiateBasedOnRangeLeft(float minRange, float maxRange, float height, float radius, bool withinRadius)
    {
        Vector3 randomSpawnPosition;
        do
        {
            randomSpawnPosition = new Vector3(Random.Range(-maxRange, -minRange), transform.position.y + height, Random.Range(-maxRange, maxRange));
        } while (!Comparator(randomSpawnPosition, radius, withinRadius));
        return randomSpawnPosition;
    }

    bool Comparator(Vector3 randomSpawnPosition, float radius, bool withinRadius)
    {
        if (withinRadius)
        {
            if (Vector3.Distance(randomSpawnPosition, transform.position) < radius) return true;
            else return false;
        }
        else
        {
            if (Vector3.Distance(randomSpawnPosition, transform.position) > radius) return true;
            else return false;
        }
    }

    float CalculateScale(Vector3 randomSpawnPosition)
    {
        float dist = Vector3.Distance(randomSpawnPosition, transform.position) - terrainSize / 2;
        float maxDist = (terrainSize / 2 + backgroundObjectsRange)*Mathf.Sqrt(2) - terrainSize / 2;
        return 1 + dist / maxDist;
    }
}
