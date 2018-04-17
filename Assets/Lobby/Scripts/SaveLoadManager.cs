using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    private static string m_FolderPath = Path.Combine(Application.persistentDataPath, "Characters");//

    public static void SaveCharacter(CharacterStats _stats)
    {
        string filePath = Path.Combine(m_FolderPath, _stats.m_Name + ".sav");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream;


        try
        {
            stream = new FileStream(filePath, FileMode.Create);
        }
        catch (Exception)
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Characters"));//
            Debug.Log("Created Directory: " + Application.persistentDataPath + "/Characters/");//

            stream = new FileStream(filePath, FileMode.Create);
        }

        CharacterStats data = _stats;

        bf.Serialize(stream, data);

        stream.Flush();
        stream.Close();
        stream.Dispose();
    }

    public static CharacterStats LoadCharacter(string _characterName)
    {
        string filePath = Path.Combine(m_FolderPath, _characterName + ".sav");

        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            CharacterStats data = (CharacterStats)bf.Deserialize(stream);

            stream.Flush();
            stream.Close();
            stream.Dispose();

            CharacterStats cs = new CharacterStats(data.m_Name, data.m_Model);
            return cs;
        }
        else
        {
            Debug.LogError("Character could not be loaded.");

            CharacterStats cs = new CharacterStats("ERROR", new byte[7]);
            return cs;
        }
    }
}