using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public enum TileType{Purple=0, Blue, Green, Red};
	public enum TileFace{a=0,b,c,d};
	
	public TileType tileType = TileType.Purple;
	TileFace tileFace = TileFace.a;
	
	public Vector2 point;
	float pulseValue = 0f;
	public float pulseDecay = 1f; 
	
	public enum Special {none = 0, newCrossSection, crossSection, longSection, squareSection};
	public Special special = Special.none;
	
	private float animationRate = 0f;
	public float animationSpeed = 4f;
	
	// Use this for initialization
	void Start () 
	{
		
		//ToDo:tile type will set index offset on texture
		//renderer.material.mainTexture = textures[(int)tileType];
		//transform.RotateAround(Vector3.up,Mathf.Deg2Rad*180f);
		transform.Rotate(Vector3.up,180f);
		
	}
	public void SetSquare(int i, int j)
	{
		StartCoroutine(Thing(i,j));
	}
	// Update is called once per frame
	void Update () {
		if(pulseValue > 0)
		{
			pulseValue -=Time.deltaTime* pulseDecay;
		}
		else
		{
			pulseValue = 0;
		}
		
		this.renderer.material.SetFloat("_Blend", pulseValue);
		
		if(special != Special.none){
			animationRate+=Time.deltaTime*animationSpeed;
			animationRate%=4f;
			
			renderer.material.mainTexture = GameGlobals.TileImages[(int)tileType + (int)animationRate * 4];
		}
	}
	public void AnimateDestruction()
	{
		GameObject.Destroy(this);
	}
	public void Pulse(float pulse)
	{
		 Debug.Log(pulse);
		pulseValue = pulse;
	}
	
	IEnumerator Thing(int i, int j){
		while(!GameGlobals.tilesLoaded)
			yield return new WaitForSeconds(.1f);
		
		this.renderer.material.mainTexture = GameGlobals.TileImages[i];
		tileType = (TileType)i;
		tileFace = (TileFace)j;
	}
}