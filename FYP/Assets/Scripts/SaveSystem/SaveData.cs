using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public PlayerData currentPlayerData { get; set; }

    public List<EnemyData> enemyData { get; set; }

    public List<itemData> itemData { get; set; }

    public BossData bossData { get; set; }

    public List<potionData> potionDataL { get; set; }

    public DateTime MyDateTime { get; set; }

    public String myScene { get; set; }
    public SaveData()
    {
        enemyData = new List<EnemyData>();
        itemData = new List<itemData>();
        potionDataL = new List<potionData>();
        MyDateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public float currentHP { get; set; }
    public float maxHP { get; set; }

    public float positionx { get; set; }
    public float positiony { get; set; }

    public PlayerData(float currHP, float maxHP, float posX, float posY)
    {
        this.currentHP = currHP;
        this.maxHP = maxHP;
        this.positionx = posX;
        this.positiony = posY;
    }
}
[Serializable]
public class EnemyData
{
    public String type { get; set; }
    public float posX { get; set; }
    public float posY { get; set; }

    public String waypoints { get; set; }
    public String name { get; set; }


    public List<EnemyAI> enemies { get; set; }

    public EnemyData(String type, float posX, float posY, string waypoints, string name)
    {
        this.type = type;
        this.posX = posX;
        this.posY = posY;
        this.waypoints = waypoints;
        this.name = name;

    }
}

[Serializable]
public class BossData
{
    public String name { get; set; }
    public float posX { get; set; }
    public float posY { get; set; }

    public BossData(String name, float posX, float posY)
    {
        this.name = name;
        this.posX = posX;
        this.posY = posY;
    }
}


[Serializable]
public class itemData
{
    public String name { get; set; }
    public float posX { get; set; }
    public float posY { get; set; }

    public itemData(String name, float posX, float posY)
    {
        this.name = name;
        this.posX = posX;
        this.posY = posY;
    }

}



[Serializable]
public class potionData
{
    public String name { get; set; }
    public float posX { get; set; }
    public float posY { get; set; }

    public potionData(String name, float posX, float posY)
    {
        this.name = name;
        this.posX = posX;
        this.posY = posY;
    }

}