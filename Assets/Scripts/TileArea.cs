using UnityEngine;
using System.Collections;

struct Combo{
	public Tile.TileType type;
	public Vector2[] tileList;
	public Combo(Tile.TileType type,Vector2[] tileList){
		this.type = type;
		this.tileList = tileList;
	}
}

public class TileArea : MonoBehaviour {
	int animationLock = 0;
	public Texture2D white;
	//float animationPause = 1f;
	//float animationTimer = 0f;
	ArrayList comboList = new ArrayList();
	public Tile tileType;
	public static int horizontalCount = 8; 
	public static int verticalCount = 10;
	Tile[,] tiles = new Tile[horizontalCount,verticalCount];
	float delayAdd = .02f;

	public float tilePadding = .5f;
	public TouchArea touchArea;
	public TouchArea[,] touchAreas= new TouchArea[7,9];
	int vertTouch = 9;
	int horTouch = 7;
	
	Vector2 lastTouchPos = Vector2.zero;
	bool touchedLastFrame = false;
	
	private int comboScore = 0;
	private bool scoringMove = false;

	public AudioClip rotateSFX, destroySFX;
	
	// Use this for initialization
	void Start () {
		
		if(!GameGlobals.tilesLoaded)
			GameGlobals.LoadTileImages();
		
		GameGlobals.currentScore = 0;
		for(int i =0; i < horizontalCount; i++){
			for (int j = 0; j < verticalCount; j++) {
				//tiles[i,j] = 
				tiles[i,j] = (Tile)Instantiate(tileType, 
					new Vector3((i-horizontalCount/2)*(1f+tilePadding),(j-verticalCount/2)*(1f+tilePadding) , 5f),
					Quaternion.identity);
				
				tiles[i,j].transform.parent = this.transform;
				tiles[i,j].point= new Vector2(i,j);
				tiles[i,j].SetSquare(Random.Range(0,4),Random.Range(0,4));
			}
		}
		for(int i =0; i < horTouch; i++){
			for (int j = 0; j < vertTouch; j++) {
				float x = 
					tiles[i,j].transform.position.x
					+tiles[i,(j+1)].transform.position.x
					+tiles[i+1,j].transform.position.x
					+tiles[i+1,(j+1)].transform.position.x;
				x=x/4f;
				float y = 
					tiles[i,j].transform.position.y
					+tiles[i,(j+1)].transform.position.y
					+tiles[i+1,j].transform.position.y+
						tiles[i+1,(j+1)].transform.position.y;
				y=y/4f;
				Vector3 position = new Vector3(x,y,4.5f);
				touchAreas[i,j] = (TouchArea)Instantiate(touchArea, position, Quaternion.identity);
				touchAreas[i,j].transform.parent = this.transform;
				touchAreas[i,j].x = i;
				touchAreas[i,j].y = j;
			}
		}
	}
	void TouchUp ()
	{
		if(GameGlobals.timeRemaining > 0f)
		{
			Ray ray=Camera.main.ScreenPointToRay(lastTouchPos);
			RaycastHit hit= new RaycastHit();
			
			if(Physics.Raycast(ray,out hit))
			{
				
				for(int i = 0; i < horTouch; i++)
				{
					for(int j = 0; j < vertTouch; j++)
					{
						if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID())
						{
							( (TouchArea) touchAreas[i,j].GetComponent(typeof(TouchArea)) ).Touched();
							//if(Input.GetMouseButtonUp(0))
								RotateRoundPoint(i, j);
							
							//Hover(touchAreas[i].x, touchAreas[i].y);
						}
					}
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if(!GameGlobals.gamePaused)
		{
			if(animationLock==0 && animationLock==0)
			{
	#if UNITY_EDITOR || UNITY_WEB_PLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
			if(Input.GetMouseButtonUp(0))//add touch code here later :P
			{
				lastTouchPos = Input.mousePosition;
				TouchUp();
			}
	#elif UNITY_IOS || UNITY_ANDROID
			if(Input.touchCount>0)//add touch code here later :P
			{
					lastTouchPos = Input.mousePosition;
					if(!touchedLastFrame)
					{
						TouchUp();
					}
					touchedLastFrame = true;
			}
			else
			{
					touchedLastFrame= false;
					//TouchUp();
			}
			
	#endif
						
			//TouchUp ();
				//}
	#if UNITY_EDITOR
			/*	if(Input.GetMouseButtonUp(1))//add touch code here later :P
				{
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									
									Vector2[] tileList1 = {new Vector2(i, j), new Vector2(i, j+1)};
									TestCombo(tiles[i, j].tileType, tileList1);
									AnimateChain();
								}
							}
						}
					}
				}*/
				if(Input.GetKeyDown(KeyCode.Alpha1)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									for(int line = 0; line <2; line++){
										Destroy(tiles[i, j+line].gameObject);
										//tiles[i, j+line]= null;
										SpawnBlock(i, j+line, Tile.TileType.Green);
									}
											Scan();
									AnimateChain();
									
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.Alpha2)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									for(int line = 0; line <2; line++){
										Destroy(tiles[i, j+line].gameObject);
										//tiles[i, j+line]= null;
										SpawnBlock(i, j+line, Tile.TileType.Purple);
									}
											Scan();
									AnimateChain();
									
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.Alpha3)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									for(int line = 0; line <2; line++){
										Destroy(tiles[i, j+line].gameObject);
										//tiles[i, j+line]= null;
										SpawnBlock(i, j+line, Tile.TileType.Red);
									}
											Scan();
									AnimateChain();
									
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.Alpha4)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									for(int line = 0; line <2; line++){
										Destroy(tiles[i, j+line].gameObject);
										//tiles[i, j+line]= null;
										SpawnBlock(i, j+line, Tile.TileType.Blue);
									}
											Scan();
									AnimateChain();
									
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.Q)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									tiles[i, j+1].special = Tile.Special.none;
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.W)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									tiles[i, j+1].special = Tile.Special.longSection;
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.E)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									tiles[i, j+1].special = Tile.Special.crossSection;
								}
							}
						}
					}
				}
				if(Input.GetKeyDown(KeyCode.R)){
					Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
					//Ray ray = new Ray(, Vector3.back);
					RaycastHit hit= new RaycastHit();
					
					if(Physics.Raycast(ray,out hit))
					{
						
						for(int i = 0; i < horTouch; i++){
							for(int j = 0; j < vertTouch; j++){
								if(touchAreas[i,j].collider.GetInstanceID() == hit.collider.GetInstanceID()){
									tiles[i, j+1].special = Tile.Special.squareSection;
								}
							}
						}
					}
				}
	#endif
			}
			
			/*if(animationLock <= 0 && comboList.Count == 0){
				bool changes = false;
				foreach(Tile t in tiles){
					if(t!=null){
						if(t.gameObject.renderer.material.mainTexture == white){
							//SpawnBlock(i, j+line,(verticalCount-1-verticalCount/2)*(1f+tilePadding)+ (tilePadding+1)*(i-fillFrom+1));
							Destroy(t.gameObject);
							changes = true;
						}
					}
				}
				if(changes){
					Scan();
					AnimateChain();
				}
			}*/
			/*if(animationLock <= 0 && comboList.Count == 0){
				bool changes = false;
				for(int y = 0; y < verticalCount; y++){
					for(int x = 0; x < horizontalCount; x++){
						if(tiles[x,y] !=null){
							if(tiles[x,y].gameObject.renderer.material.mainTexture == white){
								Destroy(tiles[x,y].gameObject);
								//AddBlocks(x,y);
								changes = true;
							}
						}
					}
				}
				if(changes){
					animationLock++;
					Scan();
					AnimateChain();
					animationLock--;
				}
			}*/
			/*for(int y = 0; y < verticalCount; y++){
				for(int x = 0; x < horizontalCount; x++){
					if(tiles[x,y] ==null){
						AddBlocks(x,y);
					}
				}
			}*/
			if(animationLock <= 0 && comboList.Count == 0){
				bool changes = false;
				for(int y = 0; y < verticalCount; y++){
					for(int x = 0; x < horizontalCount; x++){
						if(tiles[x,y] !=null){
							if(tiles[x,y].gameObject.renderer.material.mainTexture == white){
								Vector2[] tileList = {new Vector2(x,y)};
								comboList.Add(new Combo(Tile.TileType.Blue, tileList));
							}
						}
					}
				}
				if(changes){
					animationLock++;
					Scan();
					AnimateChain();
					animationLock--;
				}
			}
			
			if(GameGlobals.MultiplierDuration > 0f)
				GameGlobals.MultiplierDuration-=Time.deltaTime;
			
		}//gamePaused
	}
	
	void TestCombo(Tile.TileType type,Vector2[] tileList){
		scoringMove = true;
		foreach(Vector2 v in tileList){
			foreach(Combo c in comboList){
				foreach(Vector2 v2 in c.tileList){
					if(v == v2)
						tiles[(int)v.x, (int)v.y].special = Tile.Special.newCrossSection;
				}
			}
		}
		comboList.Add(new Combo(type, tileList));
	}
	
	void RotateRoundPoint(int x, int y){
		StartCoroutine(Rotate(x, y));
	}
	
	IEnumerator Rotate(int x, int y){
		if(GameGlobals.sound)
			GameObject.FindWithTag("MainCamera").audio.PlayOneShot(rotateSFX);
		
		animationLock++;
		float timeframe = .1f;
		float angle = -90f;
		float angleOverTime = angle/timeframe;
		float timePassed = 0f;
		
		float totalRotation = 0;
		
		Vector3 center = touchAreas[x,y].transform.position;
		center = new Vector3(center.x, center.y, tiles[x, y].transform.position.z);
		
		Vector3 posA = tiles[x,y].transform.position;
		Vector3 posB = tiles[x+1,y].transform.position;
		Vector3 posC = tiles[x+1,y+1].transform.position;
		Vector3 posD = tiles[x,y+1].transform.position;
		
		while(timePassed < timeframe){
			tiles[x,y].transform.RotateAround(center, Vector3.forward, angleOverTime*Time.deltaTime);
			tiles[x+1,y].transform.RotateAround(center, Vector3.forward, angleOverTime*Time.deltaTime);
			tiles[x,y+1].transform.RotateAround(center, Vector3.forward, angleOverTime*Time.deltaTime);
			tiles[x+1,y+1].transform.RotateAround(center, Vector3.forward, angleOverTime*Time.deltaTime);
			
			
			tiles[x,y].transform.rotation = Quaternion.identity;
			tiles[x+1,y].transform.rotation = Quaternion.identity;
			tiles[x,y+1].transform.rotation = Quaternion.identity;
			tiles[x+1,y+1].transform.rotation = Quaternion.identity;
					
			/*tiles[x,y].transform.RotateAround(Vector3.up,Mathf.Deg2Rad*180f);
			tiles[x+1,y].transform.RotateAround(Vector3.up,Mathf.Deg2Rad*180f);
			tiles[x,y+1].transform.RotateAround(Vector3.up,Mathf.Deg2Rad*180f);
			tiles[x+1,y+1].transform.RotateAround(Vector3.up,Mathf.Deg2Rad*180f);*/
					
			tiles[x,y].transform.Rotate(Vector3.up,180f);
			tiles[x+1,y].transform.Rotate(Vector3.up,180f);
			tiles[x,y+1].transform.Rotate(Vector3.up,180f);
			tiles[x+1,y+1].transform.Rotate(Vector3.up,180f);
					
					
					
			timePassed+=Time.deltaTime;
			yield return null;
			
		}
		tiles[x,y].transform.position =posD;
		tiles[x+1,y].transform.position = posA;
		tiles[x,y+1].transform.position = posC; 
		tiles[x+1,y+1].transform.position = posB;
		
		Tile a = tiles[x,y];
		tiles[x,y] = tiles[x+1,y];//a _- d
		tiles[x+1,y] = tiles[x+1,y+1];// d - c
		tiles[x+1,y+1]= tiles[x,y+1];//c - b
		tiles[x,y+1] = a;
		
		animationLock--;
		
		
		Scan();
		
		
		if(comboList.Count == 0)
		{
			GameGlobals.currentScore-=25;
			if(GameGlobals.currentScore < 0)
				GameGlobals.currentScore = 0;
			comboScore = 0;
		}
		else{
			comboScore++;
		}
		
		AnimateChain();
	}
	public void PulseTile(int x, int y, float intensity){
		tiles[x, y].Pulse(1f);
	}
	
	void Scan(){
		for(int x = 0; x <horizontalCount;x++){
			int chainCount = 0;
			int startIndex = -1;
			
			for(int y = 0; y < verticalCount-1; y ++){
				if(tiles[x,y]!=null && tiles[x, y+1] !=null && tiles[x,y].tileType == tiles[x, y+1].tileType){
					if(startIndex <0)
						startIndex =y;
					chainCount++;
				}
				else{
					if(chainCount == 6){
						
						Vector2[] tileList1 = {new Vector2(x,y-6),new Vector2(x,y)};
						TestCombo(tiles[x,y-6].tileType, tileList1);
						Vector2[] tileList2 = {new Vector2(x,y-5),new Vector2(x,y-1)};
						TestCombo(tiles[x,y-5].tileType, tileList2);
						Vector2[] tileList3 = {new Vector2(x,y-4),new Vector2(x,y-2)};
						TestCombo(tiles[x,y-4].tileType, tileList3);
						
						
						Destroy(tiles[x,y-3].gameObject);
						SpawnSpecialBlocks(x,y-3,tiles[x,y].tileType, Tile.Special.longSection);
						
						if(GameGlobals.sound){
							if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
								GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
								GameObject.FindWithTag("MainCamera").audio.Play();
							}
						}
					}
					else if(chainCount >=3){
						
						for(int i=chainCount;i >= 0; i--){//here
							Vector2[] tileList = {new Vector2(x,y-i)};
							TestCombo(tiles[x,y-i].tileType, tileList);
						}
						
						int score = (chainCount-3)*50;
						if(GameGlobals.MultiplierDuration > 0)
							score*=2;
						GameGlobals.currentScore += score;
						
						if(GameGlobals.sound){
							if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
								GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
								GameObject.FindWithTag("MainCamera").audio.Play();
							}
						}
						
					}
					startIndex = -1;
					chainCount = 0;
				}
			}
			if(chainCount ==6){
						int y = verticalCount-1;
				Vector2[] tileList1 = {new Vector2(x,y-6),new Vector2(x,y)};
				TestCombo(tiles[x,y-6].tileType, tileList1);
				Vector2[] tileList2 = {new Vector2(x,y-5),new Vector2(x,y-1)};
				TestCombo(tiles[x,y-5].tileType, tileList2);
				Vector2[] tileList3 = {new Vector2(x,y-4),new Vector2(x,y-2)};
				TestCombo(tiles[x,y-4].tileType, tileList3);
				
				
				Destroy(tiles[x,y-3].gameObject);
				SpawnSpecialBlocks(x,y-3,tiles[x,y].tileType, Tile.Special.longSection);
				
				if(GameGlobals.sound){
					if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
						GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
						GameObject.FindWithTag("MainCamera").audio.Play();
					}
				}
				
			}
			else if(chainCount>=3){
				
				int y = verticalCount - chainCount - 1;
				for(int i = 0;i <= chainCount; i++){
							
					Vector2[] tileList = {new Vector2(x,y+i)};
					TestCombo(tiles[x,y+i].tileType, tileList);
				}
				
				if(GameGlobals.sound){
					if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
						GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
						GameObject.FindWithTag("MainCamera").audio.Play();
					}
				}
			}
		}
		
		for(int y = 0; y < verticalCount; y++){
			int chainCount = 0;
			int startIndex = -1;
			for(int x = 0; x < horizontalCount-1; x ++){
				if(tiles[x,y]!= null &&  tiles[x+1, y] !=null && tiles[x,y].tileType == tiles[x+1, y].tileType){
					if(startIndex <0)
						startIndex =x;
					chainCount++;
				}
				else{
					if(chainCount ==6){
						
						Vector2[] tileList1 = {new Vector2(x-6,y),new Vector2(x,y)};
						TestCombo(tiles[x-6,y].tileType, tileList1);
						Vector2[] tileList2 = {new Vector2(x-5,y),new Vector2(x-1,y)};
						TestCombo(tiles[x-5,y].tileType, tileList2);
						Vector2[] tileList3 = {new Vector2(x-4,y),new Vector2(x-2,y)};
						TestCombo(tiles[x-4,y].tileType, tileList3);
						
						
						Destroy(tiles[x-3,y].gameObject);
						SpawnSpecialBlocks(x-3,y,tiles[x,y].tileType, Tile.Special.longSection);
						
						if(GameGlobals.sound){
							if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
								GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
								GameObject.FindWithTag("MainCamera").audio.Play();
							}
						}
						
					}
					else if(chainCount >=3){
                		//(old) here would be a good place to check if chaincount is greater than 5
                		
						//comboList.Add(new Combo(tiles[x,y].tileType, new Vector2(startIndex,y), new Vector2(x, y), chainCount+1));
						
						for(int i=chainCount;i >= 0; i--){
							Vector2[] tileList = {new Vector2(x-i,y)};
							TestCombo(tiles[x-i,y].tileType, tileList);
						}
						
						int score = (chainCount-3)*50;
						if(GameGlobals.MultiplierDuration > 0)
							score*=2;
						GameGlobals.currentScore += score;
						
						if(GameGlobals.sound){
							if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
								GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
								GameObject.FindWithTag("MainCamera").audio.Play();
							}
						}
					}
					startIndex = -1;
					chainCount = 0;
				}
			}
			if(chainCount ==6){
						int x = horizontalCount-1;
				Vector2[] tileList1 = {new Vector2(x-6,y),new Vector2(x,y)};
				TestCombo(tiles[x-6,y].tileType, tileList1);
				Vector2[] tileList2 = {new Vector2(x-5,y),new Vector2(x-1,y)};
				TestCombo(tiles[x-5,y].tileType, tileList2);
				Vector2[] tileList3 = {new Vector2(x-4,y),new Vector2(x-2,y)};
				TestCombo(tiles[x-4,y].tileType, tileList3);
				
				
				Destroy(tiles[x-3,y].gameObject);
				SpawnSpecialBlocks(x-3,y,tiles[x,y].tileType, Tile.Special.longSection);
				
		
				if(GameGlobals.sound){
					if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
						GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
						GameObject.FindWithTag("MainCamera").audio.Play();
					}
				}
				
			}
			else if(chainCount>=3){
				
				int x = horizontalCount - chainCount - 1;
				for(int i = 0;i <= chainCount; i++){
							
					Vector2[] tileList = {new Vector2(x+i,y)};
					TestCombo(tiles[x+i,y].tileType, tileList);
				}
				
				if(GameGlobals.sound){
					if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
						GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
						GameObject.FindWithTag("MainCamera").audio.Play();
					}
				}
			}
		}
		
        //here is where to check all inside tiles for being sorounded by the same block
        for(int y = 1; y < verticalCount-1; y++){
            for(int x = 1; x < horizontalCount-1; x++){
                if(
                    (tiles[x-1,y-1] != null)&&
                    (tiles[x,y-1] != null)&&
                    (tiles[x+1,y-1] != null)&&
                    (tiles[x-1,y] != null)&&
                    (tiles[x,y+1] != null)&&
                    (tiles[x-1,y+1] != null)&&
                    (tiles[x+1,y+1] != null)&&
                    (tiles[x+1,y] != null)
                )
                {
					if(
	                    (tiles[x-1,y-1].tileType == tiles[x,y-1].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x+1,y-1].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x-1,y].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x,y+1].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x-1,y+1].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x+1,y+1].tileType)&&
	                    (tiles[x-1,y-1].tileType == tiles[x+1,y].tileType)
	                )
	                {
	                    //Make the center block special
						Vector2[] tileList1 = {new Vector2(x-1,y-1),new Vector2(x+1,y+1)};
						comboList.Add(new Combo(tiles[x-1,y-1].tileType, tileList1));
						Vector2[] tileList2 = {new Vector2(x,y-1),new Vector2(x,y+1)};
						comboList.Add(new Combo(tiles[x-1,y-1].tileType, tileList2));
						Vector2[] tileList3 = {new Vector2(x+1,y-1),new Vector2(x-1,y+1)};
						comboList.Add(new Combo(tiles[x-1,y-1].tileType, tileList3));
						Vector2[] tileList4 = {new Vector2(x+1,y),new Vector2(x-1,y)};
						comboList.Add(new Combo(tiles[x-1,y-1].tileType, tileList4));
						
						
						//Destroy(tiles[x,y].gameObject);
						//SpawnSpecialBlocks(x,y,tiles[x-1,y-1].tileType, Tile.Special.squareSection);
						tiles[x,y].special = Tile.Special.squareSection;
						
		
						if(GameGlobals.sound){
							if(!GameObject.FindWithTag("MainCamera").audio.isPlaying){
								GameObject.FindWithTag("MainCamera").audio.clip = destroySFX;
								GameObject.FindWithTag("MainCamera").audio.Play();
							}
						}
	                }
                }
            }
        }
		
	}
	
	
	void SpawnBlock(int x,int y, float height){
		tiles[x,y] = (Tile)Instantiate(tileType, 
		new Vector3((x-horizontalCount/2)*(1f+tilePadding),
			height ,
			5f),
		Quaternion.identity);
		tiles[x,y].transform.parent = this.transform;
		tiles[x,y].point= new Vector2(x,y);
		int tileNum = Random.Range(0,4);
		tiles[x,y].SetSquare(tileNum,Random.Range(0,4));
		Scan();
		if(comboList.Count > 0){
			tiles[x,y].SetSquare((tileNum+ Random.Range(1,4))%4,Random.Range(0,4));
			comboList.Clear();
		}
			
	}
			
	void SpawnBlock(int x,int y, Tile.TileType tileType2){
		tiles[x,y] = (Tile)Instantiate(tileType, 
		new Vector3((x-horizontalCount/2)*(1f+tilePadding),(y-verticalCount/2)*(1f+tilePadding) , 5f),
		Quaternion.identity);
		
		tiles[x,y].transform.parent = this.transform;
		tiles[x,y].point= new Vector2(x,y);
		tiles[x,y].SetSquare((int)tileType2,Random.Range(0,4));
			
	}
	
	void SpawnSpecialBlocks(int x,int y, Tile.TileType tileType2, Tile.Special special){
		tiles[x,y] = (Tile)Instantiate(tileType, 
		new Vector3((x-horizontalCount/2)*(1f+tilePadding),(y-verticalCount/2)*(1f+tilePadding) , 5f),
		Quaternion.identity);
		
		tiles[x,y].transform.parent = this.transform;
		tiles[x,y].point= new Vector2(x,y);
		tiles[x,y].SetSquare((int)tileType2,Random.Range(0,4));
		tiles[x,y].special = special;
	}
	
	void SettleGaps(){
		for(int x = 0; x <8; x++){
			float delay = 0f;
			for (int y = 0; y < 10; y++)
			{
				
				if(tiles[x,y] == null){
					if(y !=9){
						int i = y;
						for(; i < 10 && tiles[x,i] == null; i++){}
						if(i!= 10){
							tiles[x,y] = tiles[x,i];
							tiles[x,i] = null;
							StartCoroutine(FallingBlock(tiles[x,y],i, y, delay));
							delay+=delayAdd;
						}
					}
				}
			}
			for(int i = 9; i >=0; i--){
				if(
					tiles[x,0] == null &&
					tiles[x,1] == null &&
					tiles[x,2] == null &&
					tiles[x,3] == null &&
					tiles[x,4] == null &&
					tiles[x,5] == null &&
					tiles[x,6] == null &&
					tiles[x,7] == null &&
					tiles[x,8] == null &&
					tiles[x,9] == null
				){
					AddBlocks(x, 0);
					break;
				}
						
				else if(tiles[x,i] != null)
				{
					AddBlocks(x, i+1);
					break;
				}
			}
		}
		foreach(Tile t in tiles)
		{
			if(t.special == Tile.Special.newCrossSection)
				t.special = Tile.Special.crossSection;
		}
	}
	void AddBlocks(int x, int fillFrom){
		
		for(int i = fillFrom; i < 10; i++){
			SpawnBlock(x, i, (verticalCount-1-verticalCount/2)*(1f+tilePadding)+ (tilePadding+1)*(i-fillFrom+1));
			
				StartCoroutine(FallingBlock(tiles[x,i], 9+(i-fillFrom),i-1, delayAdd*(i)));
		}
	}
	IEnumerator FallingBlock(Tile tile,int oldPos, int newPos, float delay =0){
		animationLock++;
		yield return new WaitForSeconds(delay);
		if(tile == null){
			animationLock--;
			yield break;
		}
		float destination = tile.transform.position.y +(-tilePadding-1)*(oldPos-newPos);
		float fallingSpeed = 16f;
		
		tile.point = new Vector2(tile.point.x, newPos);
		while(tile.transform.position.y > destination){
			if(tile == null){
				animationLock--;
				yield break;
			}
			tile.transform.Translate(0, -fallingSpeed*Time.deltaTime, 0);
			yield return null;
		}
		if(tile == null){
			animationLock--;
			yield break;
		}
		tile.transform.position = new Vector3(tile.transform.position.x, destination, tile.transform.position.z);
		animationLock--;
	}
	
	
	void DestroySpecial(GameObject tileObject){
		animationLock++;
		Destroy(tileObject);
		animationLock--;
	}
	
	void AnimateChain(){
		StartCoroutine(ChainCoroutine());
	}

	IEnumerator ChainCoroutine(){
		animationLock++;
		float blockDelay = .05f;
		float chainDelay = .2f;
		int chains=0;
		while(comboList.Count >0){
			foreach(Combo c in comboList){
				
				foreach(Vector2 t in c.tileList){
					if(tiles[(int)t.x, (int)t.y] != null)
						if(tiles[(int)t.x, (int)t.y].special != Tile.Special.newCrossSection)
							//tiles[(int)t.x, (int)t.y].gameObject.renderer.material.mainTexture = white;
							CheckSpecialGlow((int)t.x, (int)t.y);
				}
			}
			foreach(Combo c in comboList){	
				Tile.TileType tileColour = c.type;
						
				chains++;
				
				foreach(Vector2 t in c.tileList){
					if(tiles[(int)t.x, (int)t.y] != null){
						CheckSpecial((int)t.x, (int)t.y);
					}
				}
				yield return new WaitForSeconds(blockDelay);
				//yield return new WaitForSeconds(chainDelay);
			}
			yield return new WaitForSeconds(blockDelay);
			comboList.Clear();
			int startingAnimLock = animationLock;
			SettleGaps();
			while(startingAnimLock != animationLock){
				yield return null;	
			}
			Scan();
		}
		animationLock--;
	}
			
	void CheckSpecial(int x, int y)
	{
		if(tiles[x,y] != null){
			if(tiles[x,y].special == Tile.Special.none){
				DestroySpecial(tiles[x, y].gameObject);
				
				int score = 25 + (25 * (int)((float)comboScore * 1.25f));
				if(GameGlobals.MultiplierDuration > 0)
					score*=2;
				GameGlobals.currentScore += score;
			}
			if(tiles[x,y].special == Tile.Special.crossSection){
					
				for(int j = 0; j < verticalCount; j++){
					if(tiles[x,j] != null && j != y)
						DestroySpecial(tiles[x, j].gameObject);
				}		
				for(int i = 0; i < horizontalCount; i++){
					if(tiles[i,y] != null)
						DestroySpecial(tiles[i,y].gameObject);
				}
				
				int score = 1000;
				if(GameGlobals.MultiplierDuration > 0)
					score*=2;
				GameGlobals.currentScore += score;
			}
			else if(tiles[x,y].special == Tile.Special.longSection){
				
				if(x>0 && y>0)
					if(tiles[x-1, y-1] != null)
						DestroySpecial(tiles[x-1, y-1].gameObject);
				if(y>0)
					if(tiles[x, y-1] != null)
						DestroySpecial(tiles[x, y-1].gameObject);
				if(x < horizontalCount-1 && y > 0)
					if(tiles[x+1, y-1] != null)
						DestroySpecial(tiles[x+1, y-1].gameObject);
				if(x > 0)
					if(tiles[x-1, y] != null)
						DestroySpecial(tiles[x-1, y].gameObject);
				
					if(tiles[x, y] != null)
						DestroySpecial(tiles[x, y].gameObject);
				if(x < horizontalCount-1)
					if(tiles[x+1, y] != null)
						DestroySpecial(tiles[x+1, y].gameObject);
				if(x > 0 && y < verticalCount-1)
					if(tiles[x-1, y+1] != null)
						DestroySpecial(tiles[x-1, y+1].gameObject);
				if(y < verticalCount-1)
					if(tiles[x, y+1] != null)
						DestroySpecial(tiles[x, y+1].gameObject);
				if(x < horizontalCount-1 && y < verticalCount-1)
					if(tiles[x+1, y+1] != null)
						DestroySpecial(tiles[x+1, y+1].gameObject);
				
				int score = 1000;
				if(GameGlobals.MultiplierDuration > 0)
					score*=2;
				GameGlobals.currentScore += score;
			}
			else if(tiles[x,y].special == Tile.Special.squareSection){
				
				foreach(Tile t in tiles){
					if(t != null){
						if(t.tileType == tiles[x,y].tileType){
							DestroySpecial(t.gameObject);
						}
					}
				}
				
				int score = 1000;
				if(GameGlobals.MultiplierDuration > 0)
					score*=2;
				GameGlobals.currentScore += score;
			}
		}
		//else if(tiles[x,y].special == Tile.Special.newCrossSection){
		//	tiles[x,y].special = Tile.Special.crossSection;
		//}
	}
	
	void CheckSpecialGlow(int x, int y)
	{
		if(tiles[x,y]!=null){
			if(tiles[x,y].special == Tile.Special.none){
				tiles[x,y].gameObject.renderer.material.mainTexture = white;
			}
			if(tiles[x,y].special == Tile.Special.crossSection){
					
				for(int j = 0; j < verticalCount; j++){
					if(tiles[x,j] != null && j != y)
						tiles[x, j].gameObject.renderer.material.mainTexture = white;
				}		
				for(int i = 0; i < horizontalCount; i++){
					if(tiles[i,y] != null)
						tiles[i,y].gameObject.renderer.material.mainTexture = white;
				}
			}
			else if(tiles[x,y].special == Tile.Special.longSection){
				
				if(x>0 && y>0)
					if(tiles[x-1, y-1] != null)
						tiles[x-1, y-1].gameObject.renderer.material.mainTexture = white;
				if(y>0)
					if(tiles[x, y-1] != null)
						tiles[x, y-1].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1 && y > 0)
					if(tiles[x+1, y-1] != null)
						tiles[x+1, y-1].gameObject.renderer.material.mainTexture = white;
				if(x > 0)
					if(tiles[x-1, y] != null)
						tiles[x-1, y].gameObject.renderer.material.mainTexture = white;
				
					if(tiles[x, y] != null)
						tiles[x, y].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1)
					if(tiles[x+1, y] != null)
						tiles[x+1, y].gameObject.renderer.material.mainTexture = white;
				if(x > 0 && y < verticalCount-1)
					if(tiles[x-1, y+1] != null)
						tiles[x-1, y+1].gameObject.renderer.material.mainTexture = white;
				if(y < verticalCount-1)
					if(tiles[x, y+1] != null)
						tiles[x, y+1].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1 && y < verticalCount-1)
					if(tiles[x+1, y+1] != null)
						tiles[x+1, y+1].gameObject.renderer.material.mainTexture = white;
			}
			else if(tiles[x,y].special == Tile.Special.squareSection){
				foreach(Tile t in tiles){
					if(t != null){
						if(t.tileType == tiles[x,y].tileType){
							t.gameObject.renderer.material.mainTexture = white;
						}
					}
				}
				/*
				if(x>0 && y>0)
					if(tiles[x-1, y-1] != null)
						tiles[x-1, y-1].gameObject.renderer.material.mainTexture = white;
				if(y>0)
					if(tiles[x, y-1] != null)
						tiles[x, y-1].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1 && y > 0)
					if(tiles[x+1, y-1] != null)
						tiles[x+1, y-1].gameObject.renderer.material.mainTexture = white;
				if(x > 0)
					if(tiles[x-1, y] != null)
						tiles[x-1, y].gameObject.renderer.material.mainTexture = white;
				
					if(tiles[x, y] != null)
						tiles[x, y].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1)
					if(tiles[x+1, y] != null)
						tiles[x+1, y].gameObject.renderer.material.mainTexture = white;
				if(x > 0 && y < verticalCount-1)
					if(tiles[x-1, y+1] != null)
						tiles[x-1, y+1].gameObject.renderer.material.mainTexture = white;
				if(y < verticalCount-1)
					if(tiles[x, y+1] != null)
						tiles[x, y+1].gameObject.renderer.material.mainTexture = white;
				if(x < horizontalCount-1 && y < verticalCount-1)
					if(tiles[x+1, y+1] != null)
						tiles[x+1, y+1].gameObject.renderer.material.mainTexture = white;
				*/
			}
			//else if(tiles[x,y].special == Tile.Special.newCrossSection){
			//	tiles[x,y].special = Tile.Special.crossSection;
			//}
		}
	}
	
}
