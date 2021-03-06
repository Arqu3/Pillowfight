﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHomingMissiles : Modifier
{
    //Public vars
    public string m_ModName = "";
    public GameObject m_MissilePrefab;
    public GameObject m_SpawnpointPrefab;
    public bool m_SpawnForEachPlayer = true;
    [Range(0, 100)]
    public int m_SpawnAmount = 1;
    public bool m_SpawnOnlyOnStart = false;
    public List<GameObject> m_FilteredMods = new List<GameObject>();

    //Round start vars
    private List<GameObject> m_Missiles = new List<GameObject>();
    private List<Transform> m_Spawnpoints = new List<Transform>();
    private GameObject m_Spawns;

    //Component vars
    private ControllerScene m_Scene;

    //ID vars
    public int m_ID = 0;

    protected override void Start()
    {
        m_Scene = FindObjectOfType<ControllerScene>();

        if (!m_MissilePrefab)
        {
            Debug.Log("Homing missiles is missing its prefab!");
            enabled = false;
            return;
        }

        m_Spawns = (GameObject)Instantiate(m_SpawnpointPrefab, Vector3.zero, Quaternion.identity);
        var points = m_Spawns.GetComponentsInChildren<Transform>();
        for (int i = 1; i < points.Length; i++)
        {
            m_Spawnpoints.Add(points[i]);
        }

        if (m_SpawnOnlyOnStart)
            SpawnMissiles();
    }

    void DestroyMissiles()
    {
        //Destroy all leftover missiles
        for (int i = 0; i < m_Missiles.Count; i++)
        {
            Destroy(m_Missiles[i]);
        }
        m_Missiles.Clear();
    }

    void SpawnMissiles()
    {
        //Spawn new missiles
        List<Transform> tempSpawn = new List<Transform>();

        for (int i = 0; i < m_Spawnpoints.Count; i++)
        {
            tempSpawn.Add(m_Spawnpoints[i]);
        }

        List<Transform> tempPlayers = new List<Transform>();
        int length = m_Scene.GetPlayers().Count;
        for (int i = 0; i < length; i++)
        {
            tempPlayers.Add(m_Scene.GetPlayers()[i].transform);
        }

        int amount = 0;
        if (m_SpawnForEachPlayer)
            amount = ControllerScene.GetPlayerCount();
        else
            amount = m_SpawnAmount;

        for (int i = 0; i < amount; i++)
        {
            int random = Random.Range(0, tempSpawn.Count);
            int randomPlayer = Random.Range(0, tempPlayers.Count);

            GameObject clone = (GameObject)Instantiate(m_MissilePrefab, tempSpawn[random].position, tempSpawn[random].rotation);
            if (clone.GetComponent<HomingMissile>())
                clone.GetComponent<HomingMissile>().SetTarget(tempPlayers[randomPlayer]);

            m_Missiles.Add(clone);

            tempSpawn.RemoveAt(random);
            if (tempPlayers.Count > randomPlayer)
                tempPlayers.RemoveAt(randomPlayer);
        }
    }

    public override void OnRoundStart()
    {
        if (!m_SpawnOnlyOnStart)
            SpawnMissiles();
    }

    public override void OnRoundEnd()
    {
        if (!m_SpawnOnlyOnStart)
            DestroyMissiles();
    }

    protected override void OnDestroy()
    {
        DestroyMissiles();

        //Remove spawnpoints
        if (m_Spawns)
        {
            Destroy(m_Spawns);
            m_Spawnpoints.Clear();
        }
    }

    public override string GetName()
    {
        return m_ModName;
    }

    public override int GetID()
    {
        return m_ID;
    }

    public override List<int> GetFilteredMods()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < m_FilteredMods.Count; i++)
        {
            Modifier mod = m_FilteredMods[i].GetComponent<Modifier>();
            if (mod)
                list.Add(mod.GetID());
        }
        return list;
    }

    public override void SetID(int id)
    {
        m_ID = id;
    }
}
