using UnityEngine;

public class SphereZone : MonoBehaviour {
    
	void Start () {
        Renderer r = gameObject.GetComponent<Renderer>();
        Color materialColor = r.material.color;
        r.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0.2f);
    }

    private void OnTriggerExit(Collider collider)
    {
        collider.transform.root.gameObject.SetActive(false);
    }

}
