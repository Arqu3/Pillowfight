﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStats : Modifier
{
    //Public vars
    public string m_ModName = "";
    public float m_MoveSpeedMod = 1.0f;
    public List<GameObject> m_FilteredMods = new List<GameObject>();

    //ID vars
    public int m_ID = 0;

    protected override void Start()
    {
        if (Toolbox.Instance)
            Toolbox.Instance.m_MovementSpeed += m_MoveSpeedMod;
    }

    public override void OnRoundEnd()
    {
    }

    public override void OnRoundStart()
    {   
    }

    protected override void OnDestroy()
    {
        if (Toolbox.Instance)
            Toolbox.Instance.m_MovementSpeed -= m_MoveSpeedMod;
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
