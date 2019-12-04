using UnityEngine;


public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region Fields

	[SerializeField] Component[] preloadedComponents;
	[SerializeField] Component[] postloadedComponents;

	#endregion



	#region Unity lifecycle

	protected override void Awake()
	{
		base.Awake();
		CreateComponents(preloadedComponents);

		Application.targetFrameRate = 60;
	}


	void Start()
	{
		CreateComponents(postloadedComponents);
		//LevelsManager.Instance.CreateLevel();
	}

	#endregion



	#region Private methods

	void CreateComponents(Component[] prefabs)
	{
		for (int i = 0; i < prefabs.Length; i++)
		{
			CreateManager(prefabs[i]);
		}
	}


	void CreateManager(Component prefab)
	{
		var obj = Instantiate(prefab, transform);
		//obj.ResetLocal();
		obj.name = prefab.name;
	}

	#endregion
}
