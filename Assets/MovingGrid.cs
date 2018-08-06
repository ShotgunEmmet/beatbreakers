using UnityEngine;
using System.Collections;

public class MovingGrid : MonoBehaviour {
	
	public Renderer background;
	private Color bgColour;
	
	private float offsetX = 0;
	private float offsetY = 0;
	
	private Vector2 direction;
	private float rotation = 0;
	
	public float rotationSpeed = 0.1f;
	public float movementSpeed = 0.1f;
	public float colourChangeSpeed = 0.1f;
	public float colourIntensity = 1f;
	
	private float colourCounter = 0;
	private int currentColour = 0;
	private Color[] colours = {Color.red,Color.yellow,Color.green,Color.blue};
	
	void Start(){
		renderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
		
		bgColour = Color.black;
		
		rotation = Random.Range(0f,360f);
		direction = new Vector2(1,0);
		Update ();
	}
	
	void Update(){
		//background
		rotation+=Time.deltaTime*rotationSpeed;
		rotation%=360;
		direction = new Vector2((float)Mathf.Cos(rotation), (float)Mathf.Sin(rotation));
		direction.Normalize();
		
		offsetX += Time.deltaTime*(float)Mathf.Cos(rotation)*movementSpeed;
		offsetX %= 1f;
		offsetY += Time.deltaTime*(float)Mathf.Sin(rotation)*movementSpeed;
		offsetY %= 1f;
		renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
		
		
		colourCounter+=Time.deltaTime*colourChangeSpeed;
		
		if(colourCounter > 1){
			colourCounter %= 1;
			currentColour++;
			currentColour %= colours.Length;
		}
		colourIntensity = Mathf.Clamp(colourIntensity,0f,1f);
		background.material.color = Color.Lerp(colours[currentColour]*colourIntensity,colours[(currentColour+1)%colours.Length]*colourIntensity,colourCounter);
	}
}
