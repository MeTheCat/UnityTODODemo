using GameServices;
using UnityEngine;

public class ServiceLoader : MonoBehaviour
{
    public void Init()
    {
        Services.Init();
        CreateServices();
    }

    public void CreateServices()
    {
        Services.Instance.Register(new JsonConverterService());
        Services.Instance.Register(new HttpService());
        Services.Instance.Register(new UserDataService());
    }
}
