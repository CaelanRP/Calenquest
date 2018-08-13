using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenuAttribute]
public class CalenVisualStats : ScriptableObject{
	public AudioClip shotgunShot;
    public AudioClip shotgunReload;

    public GameObject damageNumPrefab;
    public Vector2 damageNumOffset;
    public float damageNumTime, damageNumDistance;
}
