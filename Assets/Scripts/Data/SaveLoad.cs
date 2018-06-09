using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using System;
using Rewired;

public static class SaveLoad {
	public const string PATH = "/save.tank";
    public static bool newSave = false;
    public const string SETTINGSPATH = "/settings.mork";
	public static void Save(){
        SaveGameData();
        SaveSettingsData();
	}

    public static void SaveGameData(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + PATH);
        bf.Serialize(file, SaveData.current);
        file.Close();
    }
    public static void SaveSettingsData(){
        // Save rewired controls
        if (ReInput.userDataStore != null){
            ReInput.userDataStore.Save();
        }

        BinaryFormatter settingsbf = new BinaryFormatter();
        FileStream settingsfile = File.Create (Application.persistentDataPath + SETTINGSPATH);
        settingsbf.Serialize(settingsfile, SettingsData.current);
        settingsfile.Close();
    }

    public static bool LoadSave(){
        if(File.Exists(Application.persistentDataPath + PATH)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try{
                file = File.Open(Application.persistentDataPath + PATH, FileMode.Open);
                SaveData d = (SaveData)bf.Deserialize(file);
                file.Close();
                SaveData.current = SaveData.LoadOldData(d);
            } catch (Exception e){
                if (file != null)
                {
                    file.Close();
                }
                Debug.LogError(e);
                Debug.Log("Data mismatch. Creating new save...");
                SaveData.current = new SaveData();
                SaveGameData();
                return false;
            }
            Debug.Log("Old file loaded.");
            Debug.Log("Games Played: " + SaveData.GetInt("gamesPlayed"));
            Debug.Log("Total frags: " + SaveData.GetInt("kills"));
            file.Close();
        }
        else{

            newSave = true;
            Debug.Log("Old save does not exist. Creating new save...");
            SaveData.current = new SaveData();
            SaveGameData();
        }
        return true;
    }

    public static bool LoadSettings(){
        if(File.Exists(Application.persistentDataPath + SETTINGSPATH)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try{
                file = File.Open(Application.persistentDataPath + SETTINGSPATH, FileMode.Open);
                SettingsData d = (SettingsData)bf.Deserialize(file);
                file.Close();
                SettingsData.current = SettingsData.LoadOldData(d);
                // Save rewired controls
                if (ReInput.userDataStore != null){
                    ReInput.userDataStore.Load();
                }
            } catch (Exception e){
                if (file != null)
                {
                    file.Close();
                }
                Debug.LogError(e);
                Debug.Log("Data mismatch. Creating new settings save...");
                SettingsData.current = new SettingsData();
                SaveSettingsData();
                return false;
            }
            Debug.Log("Old settings loaded.");
            file.Close();
        }
        else{
            Debug.Log("Old settings do not exist. Creating new settings...");
            SettingsData.current = new SettingsData();
            SaveSettingsData();
        }
        return true;
    }

	public static bool Load(){
        bool saveWorked = LoadSave();
        if (!saveWorked)
        {
            return false;
        }

        bool settingsWorked = LoadSettings();
        if (!saveWorked)
        {
            return false;
        }
        return true;
	}

	public static void NewFile(){
		SaveData.current = new SaveData();
        SettingsData.current = new SettingsData();
		Save();
	}



}
