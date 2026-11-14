using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;

public class LevelOneValues: MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    static public Character[] characters;
    static public bool isPlayerAlive;
    static public int globalLight;

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            await RemoteConfigService.Instance.FetchConfigsAsync(
                new userAttributes(),
                new appAttributes());
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Remote Config could not be initialized: {exception.Message}");
        }
    }
}
