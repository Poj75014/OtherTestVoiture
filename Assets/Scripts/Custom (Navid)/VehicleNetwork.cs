using UnityEngine;
using UnityEngine.Networking;

namespace RVP
{
    [NetworkSettings(sendInterval = 1/29)]
    public class VehicleNetwork : NetworkBehaviour
    {
    
        VehicleParent m_vp;
    
        [SyncVar(hook = "OnSyncSteerChange") ]
        float m_syncSteer;
        void OnSyncSteerChange(float steer)
        {
            m_syncSteer = steer;
            m_vp.steerInput = steer;
        }

        [SyncVar(hook = "OnSyncAccelChange")]
        float m_syncAccel;
        void OnSyncAccelChange(float accel)
        {
            m_syncAccel = accel;
            m_vp.accelInput = accel;
        }

        [SyncVar(hook = "OnSyncBrakeChange")]
        float m_syncBrake;
        void OnSyncBrakeChange(float brake)
        {
            m_syncBrake = brake;
            m_vp.SetBrake(brake);
        }

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
                
            //m_syncSteer = m_vp.steerInput;
            CmdSendInput(m_vp.steerInput, m_vp.accelInput, m_vp.brakeInput);

            //m_syncAccel = m_vp.accelInput;

            //m_syncBrake = m_vp.brakeInput;
        }

        [Command]
        void CmdSendInput(float steer, float accel, float brake)
        {
            m_syncSteer = steer;
            m_syncAccel = accel;
            m_syncBrake = brake;
        }
        
    }
}
