/*
 * MeshMerge.cs
 * 
 * Merges the meshes of Child GameObjects of the GameObject this script is attached to
 * 
 * @Author: Chris Thodesen
 * @Contact: chris@yetikatt.com
 * @Version: 1.2
 */
// IMPORTS //
using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace com.yetikatt.Utils
{
    [RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
    public class MeshMerge : MonoBehaviour
    {
		// uses EditorUtility so cannot be used at runtime.
        #if UNITY_EDITOR

		// CONSTANTS //
		/// <summary>
		/// integer value returned if a material wasn't found
		/// </summary>
        private const int MATERIAL_NOT_FOUND = -1;

		/// <summary>
		/// The max number of vertices that can be merged together by CombineInstance
		/// </summary>
        private const int MAX_VERT_COUNT = 65535;

		// VARIABLES //
		/// <summary>
		/// If the Merged Mesh should be created and stored in a new GameObject as a Child 
		/// of this GameObject
		/// </summary>
        public bool SaveAsNew = false;

		/// <summary>
		/// Array of centering option strings for the dropdown menu
		/// </summary>
        public string[] centeringOptions = new string[] { "Don't Center", "Center", "Grid Align Center" };

		/// <summary>
		/// The centering parent mode to use.
		/// </summary>
        public int CenterParentMode = 2;

		/// <summary>
		/// The increment on the x axis to move the parent transform.position to that 
		/// is closest to the center
		/// </summary>
        public float GridCenteringX = 1f;

		/// <summary>
		/// The increment on the y axis to move the parents transform.position to that 
		/// is closest to the center
		/// </summary>
        public float GridCenteringY = 0.5f;

		/// <summary>
		/// The increment on the z axis to move the parents transform.position to that 
		/// is closest to the center
		/// </summary>
        public float GridCenteringZ = 1f;

        /// <summary>
        /// Boolean value for if a custom path should be used to Save Prefabs to, Seperate to the selected 
		/// save location of the Mesh
		/// </summary>
        public bool UseCustomPath = false;

		/// <summary>
		/// The string value of the custom prefab save path, if it is being used.
		/// </summary>
        public string CustomPrefabPath = "Assets/";

		/// <summary>
		/// Boolean value for if this GameObjects mesh has been merged.
		/// </summary>
        [SerializeField]
        private bool _merged = false;
        public bool Merged
        {
            get { return _merged; }
        }

		/// <summary>
		/// Boolean value for if this GameObject has had a MeshCollider of the merged mesh added.
		/// </summary>
		[SerializeField]
		private bool _colliderAdded = false;
		public bool ColliderAdded
		{
			get { return _colliderAdded; }
		}

		/// <summary>
		/// Combines each of the meshes of the children of this GameObject and then disables them. 
		/// Stores the combined mesh in the MeshFilter of this GameObject, or in a new child GameObject
		/// if save as new was selected.
		/// </summary>
		/// <param name="saveAsNewObject">If set to <c>true</c> saves a new child object with the combined mesh.</param>
		/// <param name="disableGameObjects">If set to <c>true</c> disables the GameObjects of each child.</param>
		/// <param name="centerParentMode">the parent centering mode to use when combining the meshes.</param>
        public void CombineMeshes(bool saveAsNewObject = false, bool disableGameObjects = false, int centerParentMode = 0)
        {
            // ArrayList of each material used
            List<Material> materialsList = new List<Material>();

            // ArrayList of each set of meshes to combine (per material)
            List<List<CombineInstance>> combineInstancesList = new List<List<CombineInstance>>();

            Vector3 originalTransformPosition = transform.position;
            Vector3 originalEulerAngles = transform.eulerAngles;
			Vector3 originalScale = transform.localScale;
            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
			transform.localScale = Vector3.one;

            Vector3 minimumChildPosition = new Vector3();
            Vector3 maximumChildPosition = new Vector3();
            Vector3 middleChildPosition = new Vector3();
            Vector3 displacementVector = new Vector3();
            Vector3 gridAlignVector = new Vector3();

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

            if (centerParentMode > 0)
            {
                bool first = true;
                foreach (MeshFilter mF in meshFilters)
                {
                    Vector3 pos = mF.transform.localPosition;

                    if (mF.gameObject == transform.gameObject)
                    {
                        continue;
                    }
                    else if(mF.transform.parent != transform)
                    {
                        continue;
                    }

                    if (first)
                    {
                        minimumChildPosition.x = maximumChildPosition.x = pos.x;
                        minimumChildPosition.y = maximumChildPosition.y = pos.y;
                        minimumChildPosition.z = maximumChildPosition.z = pos.z;
                        first = false;
                    }
                    else
                    {
                        if (pos.x < minimumChildPosition.x)
                            minimumChildPosition.x = pos.x;
                        if (pos.y < minimumChildPosition.y)
                            minimumChildPosition.y = pos.y;
                        if (pos.z < minimumChildPosition.z)
                            minimumChildPosition.z = pos.z;

                        if (pos.x > maximumChildPosition.x)
                            maximumChildPosition.x = pos.x;
                        if (pos.y > maximumChildPosition.y)
                            maximumChildPosition.y = pos.y;
                        if (pos.z > maximumChildPosition.z)
                            maximumChildPosition.z = pos.z;
                    }
                }

                middleChildPosition.x = minimumChildPosition.x + ((maximumChildPosition.x - minimumChildPosition.x) / 2f);
                middleChildPosition.y = minimumChildPosition.y + ((maximumChildPosition.y - minimumChildPosition.y) / 2f);
                middleChildPosition.z = minimumChildPosition.z + ((maximumChildPosition.z - minimumChildPosition.z) / 2f);

                displacementVector = -middleChildPosition;

                if (centerParentMode == 2)
                {
                    gridAlignVector.x = originalTransformPosition.x - Round(originalTransformPosition.x, GridCenteringX);
                    gridAlignVector.y = originalTransformPosition.y - Round(originalTransformPosition.y, GridCenteringY);
                    gridAlignVector.z = originalTransformPosition.z - Round(originalTransformPosition.z, GridCenteringZ);
                    displacementVector.x = Round(displacementVector.x, GridCenteringX);
                    displacementVector.y = Round(displacementVector.y, GridCenteringY);
                    displacementVector.z = Round(displacementVector.z, GridCenteringZ);
                }

            }

            //Keep a running total of the vertices to be combined, CombineInstance cannot combine meshes with over 65535 verts total  
            int totalVertexCount = 0;

            foreach( MeshFilter mF in meshFilters )
            {
                MeshRenderer mR = mF.GetComponent<MeshRenderer>();

                /* This GameObject is also included in the GetComponentsInChildren call.
                * We don't want to include it though, so skip it. */
                if (mF.gameObject == transform.gameObject)
                {
                    continue;
                }

                // Handle errors
                if ( mR == null )
                {
                    Debug.LogError("[MeshMerge.cs] ERROR: MeshFilter "+mF.name+" does not have corresponding MeshRenderer component.");
                    continue;
                }

                if( mF.sharedMesh == null )
                {
                    Debug.LogWarning("[MeshMerge.cs] ERROR: MeshFilter "+mF.name+" has no mesh assigned, skipping.");
                    continue;
                }

                if (centerParentMode > 0)
                {
                    if (mF.transform.parent == transform)
                    {
                        mF.transform.localPosition += displacementVector;
                        if (centerParentMode == 2)
                        {
                            mF.transform.localPosition += gridAlignVector;
                        }
                    }
                }

                totalVertexCount += mF.sharedMesh.vertexCount;
                if(totalVertexCount > MAX_VERT_COUNT)
                {
                    Debug.LogWarning("[MeshMerge] Cannot Combine Meshes with a total vert count of over 65536 vertices, please split into multiple meshes");

					MeshFilter[] children = GetComponentsInChildren<MeshFilter>(true);
					foreach(MeshFilter m in children)
					{
						m.gameObject.SetActive(true);
					}
					
					_merged = false;

                    return;
                }

                int materialsArrayIndex = ContainsMaterial(materialsList, mR.sharedMaterial.name);

                if (materialsArrayIndex == MATERIAL_NOT_FOUND)
                {
                    materialsList.Add(mR.sharedMaterial);
                    materialsArrayIndex = materialsList.Count - 1;
                }

                combineInstancesList.Add(new List<CombineInstance>());

                CombineInstance combineInstance = new CombineInstance();
                combineInstance.transform = mF.transform.localToWorldMatrix;
                combineInstance.subMeshIndex = 0;
                combineInstance.mesh = mF.sharedMesh;

                combineInstancesList[materialsArrayIndex].Add(combineInstance);

                if (!saveAsNewObject)
                {
                    mF.gameObject.SetActive(false);
                }
            }

            MeshFilter meshFilter;
            MeshRenderer meshRenderer;
            if(saveAsNewObject)
            {
                GameObject newGameObject = new GameObject();
                meshFilter = newGameObject.AddComponent<MeshFilter>();
                meshRenderer = newGameObject.AddComponent<MeshRenderer>();
                newGameObject.name = transform.name;
                newGameObject.transform.parent = transform;
                newGameObject.transform.SetAsFirstSibling();
            }
            else
            {
                meshFilter = GetComponent<MeshFilter>();
                if (meshFilter == null)
                {
                    meshFilter = gameObject.AddComponent<MeshFilter>();
                }

                meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
            }

            Mesh[] meshArray = new Mesh[materialsList.Count];
            CombineInstance[] combineInstancesArray = new CombineInstance[materialsList.Count];

            for (int materialsArrayIndex = 0; materialsArrayIndex < materialsList.Count; materialsArrayIndex++)
            {
                CombineInstance[] combineInstanceArray = combineInstancesList[materialsArrayIndex].ToArray();
                meshArray[materialsArrayIndex] = new Mesh();
                meshArray[materialsArrayIndex].CombineMeshes(combineInstanceArray, true, true);

                combineInstancesArray[materialsArrayIndex] = new CombineInstance();
                combineInstancesArray[materialsArrayIndex].mesh = meshArray[materialsArrayIndex];
                combineInstancesArray[materialsArrayIndex].subMeshIndex = 0;
            }

            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh.name = transform.gameObject.name;
			meshFilter.sharedMesh.CombineMeshes(combineInstancesArray, false, false);

			

            foreach (Mesh mesh in meshArray)
            {
                mesh.Clear();
                DestroyImmediate(mesh);
            }

            Material[] materialsArray = materialsList.ToArray();
            meshRenderer.materials = materialsArray;

			// reapply the gameobjects original transform settings.
            transform.position = originalTransformPosition;
            transform.eulerAngles = originalEulerAngles;
			transform.localScale = originalScale;
            
			// if we're moving the origin of the parent move it back to its original position
            if(centerParentMode > 0)
            {
                transform.position -= displacementVector;
                if (centerParentMode == 2)
                {
                    transform.position -= gridAlignVector;
                }
            }

            if (!saveAsNewObject)
            {
                _merged = true;
            }
        }

		/// <summary>
		/// Saves the existing Mesh
		/// </summary>
        public void SaveMesh()
        {
            MeshFilter saveFilter = transform.GetComponent<MeshFilter>();
            
            if( saveFilter == null )
            {
                Debug.LogWarning("[MeshMerge.cs] Error: MeshFilter is null, has it been removed?");
                return;
            }

            Mesh saveMesh = saveFilter.sharedMesh;
            SaveMeshAsAsset(saveMesh, saveMesh.name, false, true);
        }

		/// <summary>
		/// Saves a new copy of the Mesh
		/// </summary>
        public void SaveMeshAsNew()
        {
            MeshFilter saveFilter = transform.GetComponent<MeshFilter>();

            if (saveFilter == null)
            {
                Debug.LogWarning("[MeshMerge.cs] Error: MeshFilter is null, has it been removed?");
                return;
            }

            Mesh saveMesh = saveFilter.sharedMesh;
            SaveMeshAsAsset(saveMesh, saveMesh.name, true, true);
        }

		/// <summary>
		/// Saves the Mesh to the Project Folder.	
		/// </summary>
		/// <param name="mesh">the Mesh to save</param>
		/// <param name="name">the Name of the mesh that is being saved</param>
		/// <param name="saveAsNewObject">If set to <c>true</c> save as new instance of this mesh.</param>
		/// <param name="doOptimize">If set to <c>true</c> the mesh should be optimised before saving.</param>
        private void SaveMeshAsAsset(Mesh mesh, string name, bool saveAsNewObject, bool doOptimize)
        {
            string savePath = EditorUtility.SaveFilePanel("Save Mesh", "Assets/", name, "asset");

            if (string.IsNullOrEmpty(savePath))
            {
                Debug.LogError("[Error] Save path is empty, cannot save mesh");
                return;
            }

            savePath = FileUtil.GetProjectRelativePath(savePath);

            Mesh saveMesh = saveAsNewObject ? Instantiate(mesh) as Mesh : mesh;

            if(doOptimize)
            {
                saveMesh.Optimize();
            }

            AssetDatabase.CreateAsset(saveMesh, savePath);
            AssetDatabase.SaveAssets();
        }

		/// <summary>
		/// Saves the Mesh as an asset, with a MeshCollider if one is present on the GameObject
		/// </summary>
        public void SaveAsset()
        {
            MeshFilter saveFilter = transform.GetComponent<MeshFilter>();

            if (saveFilter == null)
            {
                Debug.LogWarning("[MeshMerge.cs] Error: MeshFilter is null, has it been removed?");
                return;
            }

            Mesh mesh = saveFilter.sharedMesh;

            if( mesh == null)
            {
                Debug.LogWarning("[MeshMerge.cs] Error: Mesh is null, this can happen if you use 'Save Mesh' and then delete the mesh from the Project Folder.");
                SeperateMesh();
                return;
            }

            GameObject saveObject = new GameObject();
            saveObject.AddComponent<MeshRenderer>();
            saveObject.AddComponent<MeshFilter>();

            MeshRenderer renderer = saveFilter.gameObject.GetComponent<MeshRenderer>();

            string savePath = EditorUtility.SaveFilePanel("Save Mesh", "Assets/", saveObject.name, "asset");

            if (string.IsNullOrEmpty(savePath))
            {
                Debug.LogError("[MeshMerge.cs] Error: Save path is empty, cannot save mesh");
                return;
            }

            savePath = FileUtil.GetProjectRelativePath(savePath);

            Mesh saveMesh = Instantiate(mesh) as Mesh;
            saveMesh.Optimize();
            
            AssetDatabase.CreateAsset(saveMesh, savePath);
            AssetDatabase.SaveAssets();

            saveObject.GetComponent<MeshFilter>().mesh = saveMesh;
            saveObject.GetComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;

            int saveFileNameIndex = savePath.LastIndexOf("/") + 1;

            string saveDirectory = savePath.Substring(0, saveFileNameIndex);
            string saveObjectChosenName = savePath.Substring(saveFileNameIndex, ((savePath.Length - 6) - saveFileNameIndex));

            saveObject.name = saveObjectChosenName;
            string selectedPath = UseCustomPath ? CustomPrefabPath : saveDirectory;
                
            GameObject prefab = PrefabUtility.CreatePrefab(selectedPath + saveObject.name+".prefab", saveObject);

            prefab.GetComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;

			if( _colliderAdded )
			{
				prefab.AddComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			}

            Debug.Log("[MeshMerger.cs] " + prefab.name + " was saved at: '" + selectedPath + saveObject.name + ".prefab'");

            DestroyImmediate(saveObject);
        }

		/// <summary>
		/// Sets up the MeshCollider on the GameObject using the GameObjects sharedMesh.
		/// </summary>
		public void SetupCollider()
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			_colliderAdded = true;
		}
	
		/// <summary>
		/// Removes the MeshCollder from the GameObject
		/// </summary>
		public void ClearCollider()
		{
			MeshCollider mC = gameObject.GetComponent<MeshCollider>();
			DestroyImmediate(mC);
			_colliderAdded = false;
		}

		/// <summary>
		/// Clears the CombinedMesh from the GameObject and reactivates any children GameObjects with MeshFilters
		/// </summary>
        public void SeperateMesh()
        {
            MeshFilter meshFilter = transform.GetComponent<MeshFilter>();

            if (meshFilter.sharedMesh != null)
            {
                meshFilter.sharedMesh.Clear();
                DestroyImmediate(meshFilter.sharedMesh,true);
            }
            Resources.UnloadUnusedAssets();

            MeshFilter[] children = GetComponentsInChildren<MeshFilter>(true);
            foreach(MeshFilter mF in children)
            {
                mF.gameObject.SetActive(true);
            }

            _merged = false;
        }
      
		/// <summary>
		/// Searches the List of Materials for the specified material type and returns its index
		/// or a special MATERIAL_NOT_FOUND integer if it wasn't present in the List.
		/// </summary>
		/// <returns>The index of the material in the List, or MATERIAL_NOT_FOUND</returns>
		/// <param name="searchList">The List that should be searched</param>
		/// <param name="searchName">The name of the material to find</param>
        private int ContainsMaterial(List<Material> searchList, string searchName)
        {
            for (int i = 0; i < searchList.Count; i++)
            {
                if (searchList[i].name == searchName)
                {
                    return i;
                }
            }
            return MATERIAL_NOT_FOUND;
        }

		/// <summary>
		/// Round the input value to the nearest roundValue
		/// </summary>
		/// <param name="input">The value to round</param>
		/// <param name="roundValue">The increment to round to.</param>
        private float Round(float input, float roundValue)
        {
            if (roundValue == 0)
                return input;

            float d = input / roundValue;
            int r = Mathf.RoundToInt(d);
            float m = r * roundValue;
            return m;
        }

		/// <summary>
		/// Round the input value to the nearest roundValue minus the initial input
		/// </summary>
		/// <param name="input">The value to round</param>
		/// <param name="roundValue">The increment to round to.</param>
        private float RoundAmount(float input, float roundValue)
        {
            if (roundValue == 0)
                return input;

            float d = input / roundValue;
            int r = Mathf.RoundToInt(d);
            float m = r * roundValue;
            return m - input;
        }
#endif
	}
}
