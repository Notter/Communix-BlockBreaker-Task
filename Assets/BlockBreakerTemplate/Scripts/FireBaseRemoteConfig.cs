using Firebase.RemoteConfig;
using Firebase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;
using System.Collections.Specialized;

public class FireBaseRemoteConfig
{
    [Serializable]
    public class KeyValue
    {
        public string key;
        public object value;
    }

    Dictionary<string, object> defaults = new();

    // Load Default values
    public FireBaseRemoteConfig()
    {
        string jsonFilePath = Path.Combine(Application.dataPath, "Firebase/remote_config_defaults.json");
        if (jsonFilePath == null) return;

        string jsonContents = File.ReadAllText(jsonFilePath);

        // These are the values that are used if we haven't fetched data from the server
        // yet, or if we ask for values that the server doesn't have
        defaults = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContents);
    }

    // Establish connection to Firebase remote config
    private async Task<FirebaseRemoteConfig> ConnectToFireBaseConfig()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();

        FirebaseRemoteConfig mFirebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;

        // Set default values from local stored defaults
        await mFirebaseRemoteConfig.SetDefaultsAsync(defaults);

        // Fetch Remote Config settings from the server
        await mFirebaseRemoteConfig.FetchAsync();

        // Activate the fetched settings
        await mFirebaseRemoteConfig.ActivateAsync();

        return mFirebaseRemoteConfig;
    }

    public async Task<ConfigValue> GetParameter(string configParamater)
    {
        FirebaseRemoteConfig mFirebaseRemoteConfig = await ConnectToFireBaseConfig();

        // Access Remote Config values
        return mFirebaseRemoteConfig.GetValue(configParamater);
    }

    public async Task<Dictionary<string, ConfigValue>> GetParameters(params string[] configParamters)
    {
        FirebaseRemoteConfig mFirebaseRemoteConfig = await ConnectToFireBaseConfig();

        var fetchedValues = new Dictionary<string, ConfigValue>();

        // Access Remote Config values and add to a dictionary by paramter name
        for (int i = 0; i < configParamters.Length; i++)
            fetchedValues.Add(configParamters[i], mFirebaseRemoteConfig.GetValue(configParamters[i]));

        return fetchedValues;
    }
}
