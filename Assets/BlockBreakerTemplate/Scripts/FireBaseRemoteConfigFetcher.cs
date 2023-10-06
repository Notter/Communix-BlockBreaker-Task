using Firebase.RemoteConfig;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;

public class FireBaseRemoteConfigFetcher
{
    [Serializable]
    public class KeyValue
    {
        public string key;
        public object value;
    }

    Dictionary<string, object> defaults = new();

    public FireBaseRemoteConfigFetcher()
    {
        string jsonFilePath = Path.Combine(Application.dataPath, "Firebase/remote_config_defaults.json");
        if (jsonFilePath == null) return;

        string jsonContents = File.ReadAllText(jsonFilePath);

        // These are the values that are used if we haven't fetched data from the server
        // yet, or if we ask for values that the server doesn't have:
        defaults = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContents);
    }

    public async Task<ConfigValue> GetParameter(string configParamater)
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();

        FirebaseRemoteConfig mFirebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;

        // Set default values from local stored defaults
        await mFirebaseRemoteConfig.SetDefaultsAsync(defaults);

        // Fetch Remote Config settings from the server
        await mFirebaseRemoteConfig.FetchAsync();

        // Activate the fetched settings
        await mFirebaseRemoteConfig.ActivateAsync();

        // Access Remote Config values
        return mFirebaseRemoteConfig.GetValue(configParamater);
    }
}
