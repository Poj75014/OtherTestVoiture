using UnityEngine;
using UnityEngine.UI;

public class CarElectricity : MonoBehaviour
{
    [SerializeField]
    private Slider electricityBarSlider;

    [SerializeField]
    private float electricityReductionPerSecond;

    [SerializeField]
    private float electricityReloadPerSecond;

    public void ElectricityReduction()
    {
        electricityBarSlider.value -= Time.deltaTime * electricityReductionPerSecond;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "ElectricityReloader")
        {
            electricityBarSlider.value += Time.deltaTime * electricityReloadPerSecond;
        }
    }
}
