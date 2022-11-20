using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancement : MonoBehaviour, IDataPersistence
{
    public int currentLevel;
    public List<Upgrade> enhancementList = new();

    public void LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
