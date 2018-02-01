using UnityEngine;
using UnityEngine.Networking;

namespace RVP
{
    public class VehicleNetwork : NetworkBehaviour
    {

        // Use this for initialization
        void Start()
        {
            if (!isLocalPlayer)
            {
                this.GetComponent<BasicInput>().enabled = false;
            }
            else
            {
                Camera.main.GetComponent<CameraControl>().target = this.transform;
                Camera.main.GetComponent<CameraControl>().Initialize();
            }
        }
    }
}