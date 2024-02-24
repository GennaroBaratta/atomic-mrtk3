using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
	public class ReferenceTable : MonoBehaviour, IExposedPropertyTable
	{
		//singleton
		private static ReferenceTable _instance;
		public static ReferenceTable Instance
		{
			get
			{
				if (_instance == null)
					_instance = FindObjectOfType<ReferenceTable>();
				if (_instance == null)
				{
					var go = new GameObject("ReferenceTable");
					_instance = go.AddComponent<ReferenceTable>();
				}
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				// Distruggi l'istanza corrente se un'altra istanza è già presente
				Destroy(gameObject);
				return;
			}

			_instance = this;
			// Opzionale: rendi l'istanza persistente tra le scene
			DontDestroyOnLoad(gameObject);
		}
		[SerializeField]
		TaskNode taskSO;

		public List<PropertyName> properties = new List<PropertyName>();

		public List<Object> references = new List<Object>();

		public Object GetReferenceValue(PropertyName id, out bool idValid)
		{
			var index = properties.IndexOf(id);

			if (index == -1)
			{
				idValid = false;
				return null;
			}

			idValid = true;
			return references[index];
		}

		public void SetReferenceValue(PropertyName id, Object value)
		{
			var index = properties.IndexOf(id);

			if (index == -1)
			{
				properties.Add(id);
				references.Add(value);
			}
			else
			{
				references[index] = value;
			}

		}

		public void ClearReferenceValue(PropertyName id)
		{
			var index = properties.IndexOf(id);

			if (index == -1)
				return;

			properties.RemoveAt(index);
			references.RemoveAt(index);
		}

		public ExposedReference<T>? SetReferenceValue<T>(T newValue) where T : Object
		{
			if (newValue == null)
				return null;
			GameObject go = newValue as GameObject ?? (newValue as Component)?.gameObject;
			if (go == null)
				return null;
			var id = new PropertyName(CreatePrefab(go));

			SetReferenceValue(id, newValue);

			ExposedReference<T> exposedReference = new() { exposedName = id, defaultValue = newValue };
			return exposedReference;
		}

		string CreatePrefab(GameObject gameObject)
		{
			// Create folder Prefabs and set the path as within the Prefabs folder,
			// and name it as the GameObject's name with the .Prefab format
			if (!Directory.Exists("Assets/Prefabs/ObjectForTasks"))
				AssetDatabase.CreateFolder("Assets/Prefabs", "ObjectForTasks");
			string localPath = "Assets/Prefabs/ObjectForTasks/" + gameObject.name + ".prefab";

			//gameObject is already a prefab?
			bool isPrefab = PrefabUtility.IsPartOfAnyPrefab(gameObject) || PrefabUtility.IsPartOfPrefabInstance(gameObject) || PrefabUtility.IsPartOfRegularPrefab(gameObject) || PrefabUtility.IsPartOfVariantPrefab(gameObject);
			if (isPrefab)
				return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);

			// Make sure the file name is unique, in case an existing Prefab has the same name.
			localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

			return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction));
		}

		public void PopulateReferenceTable()
		{
			var propertiesFromSO = taskSO.Data.instructions.Select(instruction => new PropertyName(instruction.Data.Interaction.data.interactableRef.exposedName)).ToList();

			// Iterate through all GameObjects in the scene
			foreach (GameObject obj in FindObjectsOfType<GameObject>())
			{
				// Check if the GameObject is part of a prefab
				if (PrefabUtility.IsPartOfAnyPrefab(obj) || PrefabUtility.IsPartOfPrefabInstance(obj) || PrefabUtility.IsPartOfRegularPrefab(obj) || PrefabUtility.IsPartOfVariantPrefab(obj))
				{
					// Get the asset path of the prefab this GameObject is an instance of
					string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj);

					// If the prefab path is in our list, it's one we're interested in
					int index = propertiesFromSO.IndexOf(prefabPath);
					if (index != -1)
					{
						// Generate ID for this instance
						PropertyName id = new PropertyName(propertiesFromSO[index]);

						// Add to reference table if not already present
						if (!properties.Contains(id))
						{
							properties.Add(id);
							references.Add(obj);
						}
					}
				}
			}
		}
	}
}
