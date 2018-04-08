using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//TEST
using System.Linq;

public static class SaveLoadManager
{
    private static string m_FolderPath = Path.Combine(Application.dataPath, "Characters");

    public static void SaveCharacter(Character _character)
    {
        string filePath = Path.Combine(m_FolderPath, _character.m_CName + ".sav");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream;

        try
        {
            stream = new FileStream(filePath, FileMode.Create);
        }
        catch (Exception)
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Characters"));
            Debug.Log("Created Directory: " + Application.dataPath + "/Characters/");

            stream = new FileStream(filePath, FileMode.Create);
        }

        CharacterData data = new CharacterData(_character);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadCharacter(string _characterName, out string _name, out int[] _model)
    {
        string filePath = Path.Combine(m_FolderPath, _characterName + ".sav");

        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            CharacterData data = (CharacterData)bf.Deserialize(stream);

            stream.Close();

            _name = data.m_CharacterName;
            _model = data.m_CharacterModel;
        }
        else
        {
            Debug.LogError("Character could not be loaded.");
            _name = "ERROR";
            _model = new int[4];
        }
    }
}

[Serializable]
public class CharacterData
{
    public string m_CharacterName;
    public int[] m_CharacterModel = new int[4];

    public CharacterData(Character _character)
    {
        m_CharacterName = _character.m_CName;// Name
        m_CharacterModel[0] = _character.m_CModel[0];// Color
        m_CharacterModel[1] = _character.m_CModel[1];// Body
        m_CharacterModel[2] = _character.m_CModel[2];// Face
        m_CharacterModel[3] = _character.m_CModel[3];// Hair
    }
}