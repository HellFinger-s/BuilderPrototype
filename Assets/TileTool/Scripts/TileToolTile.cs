//
//TileToolTile - Handles the removal of sides from meshes
//

using UnityEngine;
using System;
using System.Collections.Generic;

[AddComponentMenu("TileTool/TileTool Tile")]

public class TileToolTile:MonoBehaviour{
    
    public bool liquid;
    public LayerMask TileLayer = (LayerMask)(-1);
    [HideInInspector]
    public TileToolTile rTile;
    [HideInInspector]
    public TileToolTile lTile;
    [HideInInspector]
    public TileToolTile fTile;
    [HideInInspector]
    public TileToolTile bTile;
    [HideInInspector]
    public TileToolTile uTile;
    [HideInInspector]
    public TileToolTile dTile;
    public int rSide;
    public int lSide;
    public int fSide;
    public int bSide;
    public int uSide;
    public int dSide;
    [HideInInspector]
    public int rSideWorld;
    [HideInInspector]
    public int lSideWorld;
    [HideInInspector]
    public int fSideWorld;
    [HideInInspector]
    public int bSideWorld;
    [HideInInspector]
    public int uSideWorld;
    [HideInInspector]
    public int dSideWorld;
    public int sideRemovedCounter;
    
    public void Init() {
    	WorldRotationSides();
    }
    
    public void ResetTilePositionBuffers(){
    	rTile = null;
    	lTile = null;
    	fTile = null;
    	bTile = null;
    	uTile = null;
    	dTile = null;
    }
    
    public string WorldRotationSides(){
    	uSideWorld = uSide;
    	dSideWorld = dSide;
    	if(Mathf.Approximately(transform.eulerAngles.y, 0.0f)){
    		lSideWorld = lSide;
    		rSideWorld = rSide;
    		bSideWorld = bSide;
    		fSideWorld = fSide;
    		return null;
    	}
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 90){
    		lSideWorld = fSide;
    		rSideWorld = bSide;
    		bSideWorld = lSide;
    		fSideWorld = rSide;
    		return null;
    	}
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 180){
    		lSideWorld = rSide;
    		rSideWorld = lSide;
    		bSideWorld = fSide;
    		fSideWorld = bSide;
    		return null;
    	}
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 270){
    		lSideWorld = bSide;
    		rSideWorld = fSide;
    		bSideWorld = rSide;
    		fSideWorld = lSide;
    		return null;
    	}
    	return "";
    }
    
    public string Magic(string side){
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 90){
    		if(side == "left") return "front";
    		if(side == "right") return "back";
    		if(side == "back") return "left";
    		if(side == "front") return "right";
    	}
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 180){
    		if(side == "left") return "right";
    		if(side == "right") return "left";
    		if(side == "back") return "front";
    		if(side == "front") return "back";
    	}
    	if(Mathf.FloorToInt(transform.eulerAngles.y) == 270){
    		if(side == "left") return "back";
    		if(side == "right") return "front";
    		if(side == "back") return "right";
    		if(side == "front") return "left";
    	}
    	return "";
    }
    
    public bool CompareSides(int s1,int s2,bool sameY){
    	if(s1 == s2 && sameY|| s2 == 1 && sameY){
    		return true;
    	}
    	if(s1 == 2 && s2 == 1){
    		return true;
    	}
    	if(s1 == 2 && s2 == 4){
    		return true;
    	}
    	return false;
    }
    
    public void DetectTiles() {
    	ResetTilePositionBuffers();
    	WorldRotationSides();
    	Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f, TileLayer);	
    	for(int i = 0; i< hitColliders.Length; i++){
    		TileToolTile currentTR = hitColliders[i].transform.parent.GetComponent<TileToolTile>();
    		if(currentTR != null){
    			if((rTile == null) && currentTR.transform.position.x < transform.position.x && currentTR.transform.position.z == transform.position.z &&(currentTR.transform.position.y == transform.position.y || currentTR.transform.position.y == transform.position.y -.5f && !liquid ||currentTR.transform.position.y == transform.position.y +.5f && !liquid)){			
    				rTile = currentTR;
    				currentTR.lTile = this;
    				currentTR.WorldRotationSides();
    				if(CompareSides(rSideWorld, rTile.lSideWorld, currentTR.transform.position.y == transform.position.y)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "right" , gameObject);
    				}		
    			}else if((lTile == null) && currentTR.transform.position.x > transform.position.x && currentTR.transform.position.z == transform.position.z &&(currentTR.transform.position.y == transform.position.y || currentTR.transform.position.y == transform.position.y -.5f && !liquid ||currentTR.transform.position.y == transform.position.y +.5f && !liquid)){		
    				lTile = currentTR;
    				currentTR.WorldRotationSides();
    				if(CompareSides(lSideWorld, lTile.rSideWorld, currentTR.transform.position.y == transform.position.y)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "left" , gameObject);
    				}
    			}else if((uTile == null) && currentTR.transform.position.y > transform.position.y && currentTR.transform.position.x == transform.position.x && currentTR.transform.position.z == transform.position.z){			
    				uTile = currentTR;
    				currentTR.WorldRotationSides();
    				if(CompareSides(uSideWorld, uTile.dSideWorld, true)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "up" , gameObject);
    				}
    			}else if((dTile == null) && currentTR.transform.position.y < transform.position.y && currentTR.transform.position.x == transform.position.x && currentTR.transform.position.z == transform.position.z){			
    				dTile = currentTR;
    				currentTR.WorldRotationSides();
    				if(CompareSides(dSideWorld, dTile.uSideWorld, true)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "down" , gameObject);
    				}	
    			}else if((fTile == null) && currentTR.transform.position.z > transform.position.z && currentTR.transform.position.x == transform.position.x &&(currentTR.transform.position.y == transform.position.y || currentTR.transform.position.y == transform.position.y -.5f && !liquid||currentTR.transform.position.y == transform.position.y +.5f && !liquid)){					
    				fTile = currentTR;
    				currentTR.WorldRotationSides();
    				if(CompareSides(fSideWorld, fTile.bSideWorld, currentTR.transform.position.y == transform.position.y)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "front" , gameObject);
    				}	
    			}else if((bTile == null) && currentTR.transform.position.z < transform.position.z && currentTR.transform.position.x == transform.position.x &&(currentTR.transform.position.y == transform.position.y || currentTR.transform.position.y == transform.position.y -.5f  && !liquid||currentTR.transform.position.y == transform.position.y +.5f && !liquid)){		
    				bTile = currentTR;
    				currentTR.WorldRotationSides();
    				if(CompareSides(bSideWorld, bTile.fSideWorld, currentTR.transform.position.y == transform.position.y)){
    					RemoveSide(transform.Find("Model").GetComponent<MeshFilter>(), "back" , gameObject);
    				}	
    			}		
    		}
    	}
    }
    
    public void FindAndSetTileValues(){
    	// Fills in values that are missing, only happens in editor
    	if(rSide == 0){
    		if(gameObject.name.Contains("_SQ_")){
    			rSide = 1;
    			lSide = 1;
    			fSide = 1;
    			bSide = 1;
    			uSide = 1;
    			dSide = 1;	
    		}
    		if(gameObject.name.Contains("_PL_")){
    			rSide = 2;
    			lSide = 2;
    			fSide = 2;
    			bSide = 2;
    			uSide = 2;
    			dSide = 2;	
    		}	
    		if(gameObject.name.Contains("_SQ_Bend")){
    			rSide = 2;
    			lSide = 1;
    			fSide = 4;
    			bSide = 4;
    			uSide = 1;
    			dSide = -1;	
    		}else if(gameObject.name.Contains("_PL_Triangle")){
    			rSide = -1;
    			lSide = 2;
    			fSide = -1;
    			bSide = 2;
    			uSide = 7;
    			dSide = 7;	
    		}else if(gameObject.name.Contains("_SQ_Slope2PL")){
    			rSide = 2;
    			lSide = 1;
    			fSide = 5;
    			bSide = 5;
    			uSide = -1;
    			dSide = 1;	
    		}else if(gameObject.name.Contains("_SQ_Slope")){
    			rSide = -1;
    			lSide = 1;
    			fSide = 3;
    			bSide = 3;
    			uSide = -1;
    			dSide = 1;	
    		}else if(gameObject.name.Contains("_PL_SlopeEndSlope")){
    			rSide = -1;
    			lSide = 2;
    			fSide = 8;
    			bSide = 8;
    			uSide = 1;
    			dSide = 2;	
    		}else if(gameObject.name.Contains("_SQ_DirtCorner")||gameObject.name.Contains("_SQ_Corner")){
    			rSide = -1;
    			lSide = 1;
    			fSide = 9;
    			bSide = 9;
    			uSide = 1;
    			dSide = -1;	
    		}else if(gameObject.name.Contains("_SQ_Arch")){
    			rSide = 2;
    			lSide = 1;
    			fSide = 10;
    			bSide = 10;
    			uSide = 1;
    			dSide = -1;	
    		}else if(gameObject.name.Contains("_SQ_Liquid")){
    			rSide = 101;
    			lSide = 101;
    			fSide = 101;
    			bSide = 101;
    			uSide = 101;
    			dSide = 101;	
    		}else if(gameObject.name.Contains("_PL_Liquid")){
    			rSide = 102;
    			lSide = 102;
    			fSide = 102;
    			bSide = 102;
    			uSide = -1;
    			dSide = 101;	
    		}
    
    	}
    }
    
    public void RemoveSide(MeshFilter mf,string sidex,GameObject gameObj) {
    	string side = sidex;
    	if(side != "up" && side != "down" && transform.eulerAngles.y != 0){
    		side = Magic(side);
    	}
    
    	Mesh mesh = mf.sharedMesh;
    	if(mesh != null){
    		Mesh meshCopy = (Mesh)Mesh.Instantiate(mesh);
    		mesh = mf.mesh = meshCopy;
    		Vector3[] vertices = mesh.vertices;
			List<int> indices = new List<int>(mesh.triangles);
			int count  = indices.Count / 3;
    		for (int i = count - 1; i >= 0; i--) {
				Vector3 V1  = vertices[indices[i * 3 + 0]];
				Vector3 V2 = vertices[indices[i * 3 + 1]];
				Vector3 V3 = vertices[indices[i * 3 + 2]];
    		if (side == "up") {
    			if (V1.y > 0.49 && V2.y > 0.49 && V3.y > 0.49) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    		if (side == "down") {
    			if (V1.y < -0.49 && V2.y < -0.49 && V3.y < -0.49){ //||gameObj.name.Contains("_PL_") && V1.y == 0 && V2.y == 0 && V3.y == 0) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    		if (side == "back") {
    			if (V1.z < -0.49 && V2.z < -0.49 && V3.z < -0.49) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    		if (side == "front") {
    			if (V1.z > 0.49 && V2.z > 0.49 && V3.z > 0.49) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    		if (side == "left") {
    			if (V1.x > 0.49 && V2.x > 0.49 && V3.x > 0.49) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    		if (side == "right") {
    			if (V1.x < -0.49 && V2.x < -0.49 && V3.x < -0.49) {
    			    indices.RemoveRange(i * 3, 3);
    			}
    		}
    	}
    	mesh.triangles = indices.ToArray();
    	}
    }
}