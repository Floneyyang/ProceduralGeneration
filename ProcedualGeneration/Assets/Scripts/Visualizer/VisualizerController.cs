using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerController : MonoBehaviour
{
    public GameObject visualizerObjectPrefab;
    public Material materialReference;
    public float distanceBetweenObjects;
    public float startScale = 1f;
    public float maxScale = 10f;

    struct VisualizerObject{
        public int band;
        public GameObject visualizerOb;
    }


    private VisualizerObject[] visualizerObjects = new VisualizerObject[8];

    private void Start()
    {
        for(int i = 0; i < visualizerObjects.Length; i++)
        {
            visualizerObjects[i].band = i;
            visualizerObjects[i].visualizerOb = Instantiate(visualizerObjectPrefab,
                new Vector3(transform.position.x + i * distanceBetweenObjects, transform.position.y, transform.position.z), transform.rotation);
        }
    }


    private void Update()
    {
        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            visualizerObjects[i].visualizerOb.transform.localScale =
                new Vector3(visualizerObjects[i].visualizerOb.transform.localScale.x, (Audio.bandBuffer[visualizerObjects[i].band] * maxScale)
                + startScale, visualizerObjects[i].visualizerOb.transform.localScale.z);
            Color _color = new Color(Audio.audioBandBuffer[visualizerObjects[i].band]/2, Audio.audioBandBuffer[visualizerObjects[i].band]/2, Audio.audioBandBuffer[visualizerObjects[i].band]);
            materialReference.SetColor("_EmissionColor", _color);
        }
            
    }

}
