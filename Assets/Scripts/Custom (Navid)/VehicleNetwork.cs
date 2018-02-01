using UnityEngine;
using UnityEngine.Networking;

namespace RVP
{
    public class VehicleNetwork : NetworkBehaviour
    {
    
    VehicleParent m_vp;
    
    [SyncVar(hook = "OnSyncSteerChange") ]
    float m_syncSteer;

        // Use this for initialization
        void Start()
        {
        m_vp = GetComponent<VehicleParent>();
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
        
        void FixedUpdate(){
            if(!isLocalPlayer)
                return;
                
            m_syncSteer = m_vp.steerInput;
        }
        
        void OnSyncSteerChange(float steer){
            m_syncSteer = steer;
            m_vp.steerInput = steer;
        }
        
    }
}
