using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HalfEdgeModel{
	public class HalfEdge {	
		public HalfVert srcVert{get;set;}
		public HalfVert dstVert{get;set;}
		public HalfEdge pair{get;set;}
		public HalfEdge next{get;set;}

		public HalfEdge(){
			this.pair=null;
		}
		public bool isEqual(HalfEdge other){
			if(this.srcVert.vertexID==other.srcVert.vertexID&&this.dstVert.vertexID==other.dstVert.vertexID)
				return true;
			return false;
		}
	}

	public class HalfVert{
		public int vertexID{get;set;}
		public HalfEdge edge{get;set;}
	}

	public class HalfMesh{
		public List<HalfEdge> edges=new List<HalfEdge>();
		public List<HalfVert>  verts=new List<HalfVert>();

		HalfVert[] halVerts;
		public HalfMesh(Mesh mesh){
			Initial(mesh);
		}
		public void Initial(Mesh mesh){
			int lengthOfVerts=mesh.vertexCount;
			int[] mark=new int[lengthOfVerts];
			halVerts=new HalfVert[lengthOfVerts];
			for(int i=0;i<lengthOfVerts;i++){ 
				mark[i]=0;
				halVerts[i]=new HalfVert();
			}
			int[] triangles=mesh.triangles;
			int lengthOfTris=triangles.Length;
			for(int i=0;i<lengthOfTris;i=i+3){
				int temp=0;
				int triIndex;
				int halVertsIndex;
				//三角面片的三个边初始化为半边结构 todo 重构
				HalfEdge[]  HalfEdges=new HalfEdge[3];
				HalfEdges[0]=new HalfEdge();
				HalfEdges[1]=new HalfEdge();
				HalfEdges[2]=new HalfEdge();
				makeHalfEdge(ref HalfEdges,i,0,ref triangles);
				makeHalfEdge(ref HalfEdges,i,1,ref triangles);	
				makeHalfEdge(ref HalfEdges,i,2,ref triangles);
				while(temp<3){
					triIndex=temp+i;
					halVertsIndex=triangles[triIndex];
					if(mark[halVertsIndex]==0){
						halVerts[halVertsIndex].vertexID=halVertsIndex;
						halVerts[halVertsIndex].edge=HalfEdges[temp];
						mark[halVertsIndex]=1;
						verts.Add(halVerts[halVertsIndex]);
					}
					temp++;
				}
			}
			assignPair();
		}

		private void makeHalfEdge(ref HalfEdge[] haledges,int i,int offset,ref int[] triangles){
			haledges[offset].srcVert=halVerts[triangles[i+offset]];
			haledges[offset].dstVert=halVerts[triangles[i+(offset+1)%3]];
			haledges[offset].next=edges[(offset+1)%3];
			edges.Add(haledges[offset]);
		}
		//找到每条半边相反的边
		private void assignPair(){
			int count=edges.Count;
			for(int i=0;i<count;i++){
				if(edges[i].pair==null){
					for(int j=i+1;j<count;j++){
						if(edges[i].isEqual(edges[j])){
							edges[i].pair=edges[j];
							edges[j].pair=edges[i];
							break;
						}
					}
				}
			}
		}
	}
}