using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public static class SaveLoadManager
{
    // leads to C:\Users\user\AppData\LocalLow\companyName\projectTitle\Characters
    private static string m_FolderPath = Path.Combine(Application.persistentDataPath, "Characters");

    /// <summary>
    /// takes in relevant information and stores them in a binary file to prevent modifaction/cheating
    /// </summary>
    /// <param relevant character information="_stats"></param>
    public static void SaveCharacter(CharacterStats _stats)
    {
        string filePath = Path.Combine(m_FolderPath, _stats.m_StatsName + ".sav");
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

    /// <summary>
    /// takes in a character name and loads a file with a matching name
    /// returns the loaded information in form of a character stats class
    /// </summary>
    /// <param character name="_characterName"></param>
    /// <returns></returns>
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

            CharacterStats cs = new CharacterStats(data.m_StatsName, data.m_Model);
            return cs;
        }
        else
        {
            Debug.LogError("Character could not be loaded.");

            CharacterStats cs = new CharacterStats("ERROR", new int[9]);
            return cs;
        }
    }
}