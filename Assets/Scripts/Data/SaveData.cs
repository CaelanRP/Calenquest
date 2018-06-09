using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using System.Runtime.Serialization;

[System.Serializable]
public class SaveData {
	public static SaveData current;
	public static int currentVersion = 5; //UPDATE THIS WHENEVER NON-PRIMITIVE DATA IS ADDED;
    protected Dictionary<string, System.Object> savedVars;

    public const string savedPosition = "savedPosition";

    public SaveData(){

        savedVars = new Dictionary<string, System.Object>()
        {
            {"savedVersion", currentVersion},
        };
	}

    /* This method takes some existing data and confirms it can be used with the
    current format by adding any missing vars. It is very important that this method be
    kept up-to-date with all existing saveable vars. */
    public static SaveData LoadOldData(SaveData old){
        SaveData template = new SaveData();
        foreach (string s in template.savedVars.Keys){
            old.Add(s, template.savedVars[s]);
        }
        return old;
    }

    public static bool GetBool(string index){
        try{
            return (bool)(current.savedVars[index]);
        } catch {
            Debug.LogError("Attempted to access saved int called \"" + index + "\", but no such saved var exists. Returning false.");
            return false;
        }
    }

    public static int GetInt(string index){
        try{
            return (int)(current.savedVars[index]);
        } catch {
            Debug.LogError("Attempted to access saved int called \"" + index + "\", but no such saved var exists. Returning 0.");
            return 0;
        }
    }

    public static bool HasInt(string index){
        return current.savedVars[index] != null;
    }

    public static float GetFloat(string index){
        try{
            return (float)(current.savedVars[index]);
        } catch {
            Debug.LogError("Attempted to access saved float called \"" + index + "\", but no such saved var exists. Returning 0.");
            return 0;
        }
    }

    public static string GetString(string index){
        try{
            return (string)(current.savedVars[index]);
        } catch {
            Debug.LogError("Attempted to access saved string called \"" + index + "\", but no such saved var exists. Returning empty string.");
            return "";
        }
    }

    public static void ChangeValue(string index, int change){
        try{
            int val = (int)(current.savedVars[index]);
            val += change;
            current.savedVars[index] = val;
        } catch {
            Set(index, change);
            //Debug.LogError("Failed to modify saved var called \"" + index + "\". Either mismatched type, missing variable, or save data has not been loaded.");
        }
    }

    public static void ChangeValue(string index, float change){
        try{
            float val = (float)(current.savedVars[index]);
            val += change;
            current.savedVars[index] = val;
        } catch {

            Set(index, change);
            //Debug.LogError("Failed to modify saved var called \"" + index + "\". Either mismatched type, missing variable, or save data has not been loaded.");
        }
    }

    // Attempts to add a new key, but will not replace an old one.
    public void Add(string key, System.Object obj){
        if (!savedVars.ContainsKey(key)){
            savedVars.Add(key, obj);
        }
    }

    public static void Set(string key, System.Object obj){
        try{
            if (current.savedVars.ContainsKey(key)){
                current.savedVars[key] = obj;
            }
            else{
                current.savedVars.Add(key, obj);
            }
        } catch {
            Debug.LogError("Failed to set saved var \"" + key + "\".");
        }
    }

    


}