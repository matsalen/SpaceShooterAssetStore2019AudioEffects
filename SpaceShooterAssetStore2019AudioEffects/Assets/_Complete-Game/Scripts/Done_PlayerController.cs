using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
    [SerializeField] float volumeInicial;
    [SerializeField] float menorVolume;
    [SerializeField] float maiorVolume;
    [SerializeField] float menorPan;
    [SerializeField] float maiorPan;
    float incrementoAudio;

    Rigidbody rb;
    AudioSource audioS;
	 
	private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
        incrementoAudio = maiorVolume - volumeInicial;
    }

    void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play ();

            maiorVolume = 1.0f - volumeInicial;
            audioS.volume = volumeInicial + ConversorEscala(boundary.xMin, boundary.xMax, Mathf.Abs(transform.position.x), menorVolume, maiorVolume);
            audioS.panStereo = ConversorEscala(boundary.xMin, boundary.xMax, transform.position.x, menorPan, maiorPan);
            audioS.Play();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;
		
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

    private float ConversorEscala(float min, float max, float valor, float minFinal, float maxFinal)
    {
        return ((valor - min) / (max - min)) * (maxFinal - minFinal) + minFinal;
    }
}
