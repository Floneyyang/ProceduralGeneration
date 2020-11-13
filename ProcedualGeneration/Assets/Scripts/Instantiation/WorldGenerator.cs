using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject player;
    [Header("Terrain Settings")]

    [Header("Terrain-Organizer")]
    public Transform terrain_parent;
    [Header("Terrain-Instantiation Object Reference")]
    public GameObject[] terrains_grass;
    public GameObject[] terrains_sand;
    [Header("Terrain-Instantiation Settings")]
    public float terrain_x;
    public float terrain_z;
    public float terrainDistance;
    [Header("Terrain-Variations Settings")]
    public bool sand;

    [Header("Ground Object Settings")]

    [Header("Ground Object-Organizer")]
    public Transform groundObject_parent;
    [Header("Ground Object-Instantiation Object Reference")]
    public GameObject[] ground_objects;
    public GameObject[] ground_mountian;
    [Header("Ground Object-Instantiation Settings")]
    public int mountain_number;
    public int grass_number;
    [Header("Ground Object-Variations Settings")]
    public bool mountain;

    [Header("Sky Settings")]

    [Header("Sky-Organizer")]
    public Transform sky_parent;
    [Header("Sky-Instantiation Object Reference")]
    public GameObject[] sky_cloud;
    [Header("Sky Object-Instantiation Settings")]
    public int cloud_number;
    public float cloudHeight;
    [Range(1, 10)]
    public float cloudSize;


    enum ObjectType
    {
        others,
        mountain,
        cloud
    }

    private void Start()
    {
        InitializeTerrainGeneration();
        WorldObjectGeneration(grass_number, 0f, terrain_x * terrainDistance, ObjectType.others);
        WorldObjectGeneration(cloud_number, 0f, terrain_x * terrainDistance, ObjectType.cloud);


    }

    void WorldObjectGeneration(int number, float minRange, float instantiationRange, ObjectType type)
    {

        for (int i = 0; i < number; i++)
        {
            Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);

            int r;
            GameObject newObject;
            Vector3 randomSpawnPosition;
            if (type == ObjectType.others)
            {
                randomSpawnPosition = new Vector3(Random.Range(minRange, instantiationRange), transform.position.y + 8f, Random.Range(minRange, instantiationRange));
                r = Random.Range(0, ground_objects.Length);
                newObject = (GameObject)Instantiate(ground_objects[r], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));

                newObject.transform.localScale = new Vector3(10f, 10f, 10f);
                newObject.transform.parent = groundObject_parent;
            }
            else if (type == ObjectType.mountain)
            {
                randomSpawnPosition = new Vector3(Random.Range(minRange-20f, minRange+20f), transform.position.y + 8f, Random.Range(instantiationRange-20f, instantiationRange+20f));
                r = Random.Range(0, ground_mountian.Length);
                newObject = (GameObject)Instantiate(ground_mountian[r], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));

                //newObject.transform.localScale = new Vector3(10f, 10f, 10f);
                newObject.transform.parent = groundObject_parent;
            }
            else if(type == ObjectType.cloud)
            {
                randomSpawnPosition = new Vector3(Random.Range(minRange, instantiationRange), Random.Range(transform.position.y + cloudHeight, transform.position.y + cloudHeight+terrainDistance), Random.Range(minRange, instantiationRange));
                r = Random.Range(0, sky_cloud.Length);
                newObject = (GameObject)Instantiate(sky_cloud[r], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));

                newObject.transform.localScale = new Vector3(cloudSize, cloudSize, cloudSize);
                newObject.transform.parent = sky_parent;
            }
        }
    }



    void InitializeTerrainGeneration()
    {
        player.transform.position = new Vector3(terrain_x / 2 * terrainDistance, player.transform.position.y, terrain_z / 2 * terrainDistance);
        Vector3 randomSpawnPosition = transform.position;
        for (int x = 0; x < terrain_x; x++)
        {
            float first_x_pos = randomSpawnPosition.x;
            for (int z = 0; z < terrain_z; z++)
            {
                GenerateTerrain(randomSpawnPosition);
                if (z != terrain_z - 1)
                {
                    randomSpawnPosition = new Vector3(randomSpawnPosition.x + terrainDistance, randomSpawnPosition.y, randomSpawnPosition.z);
                }
                if ((x==0 && z == 0) || (x == 0 && z == terrain_z - 1) || (x == terrain_x - 1 && z == 0) || (x == terrain_x - 1 && z == terrain_z - 1))
                {
                    float dist = terrainDistance;
                    float x_pos = randomSpawnPosition.x;
                    float z_pos = randomSpawnPosition.z;
                    if(x == 0) x_pos -= dist;
                    else x_pos += dist;

                    if (z == 0) z_pos += dist;
                    else z_pos -= dist;
                 
                    if (mountain)
                    {
                        WorldObjectGeneration((int)mountain_number/4, x_pos, z_pos, ObjectType.mountain);
                    }
                }
            }
            randomSpawnPosition = new Vector3(first_x_pos, randomSpawnPosition.y, randomSpawnPosition.z + terrainDistance);
        }
    }

    void GenerateTerrain(Vector3 randomSpawnPosition)
    {
        int i;
        GameObject newObject;
        Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);
        if (sand)
        {
            int probability = Random.Range(0, 10);
            if(probability > 2)
            {
                i = Random.Range(0, terrains_grass.Length);
                newObject = (GameObject)Instantiate(terrains_grass[i], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));
            }
            else
            {
                i = Random.Range(0, terrains_sand.Length);
                newObject = (GameObject)Instantiate(terrains_sand[i], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));
            }
        }
        else
        {
            i = Random.Range(0, terrains_grass.Length);
            newObject = (GameObject)Instantiate(terrains_grass[i], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));
        }

        newObject.transform.parent = terrain_parent;
    }
}
