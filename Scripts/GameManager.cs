using System;
using Godot;

public class GameManager : Node
{
	public static GameManager Instance;
	public Player oPlayer;
	public bool sceneInitialized = false;

	private static readonly string pPlayer = "res://Prefabs/Player.tscn";

	public override void _Ready()
	{
		base._Ready();
		Instance = this;

		// For debug
		//LoadGameScene(GAME_SCENES.SANTA_HOUSE);
	}

	public void LoadGameScene(GAME_SCENES scene)
	{
		GD.Print("Loading scene " + scene.ToString());
		PackedScene pscn = GameScenes.GetPackedScene(scene);
		GetTree().ChangeSceneTo(pscn);
		sceneInitialized = false;
	}

	public static void InitScene(Node owner)
	{
		GD.Print("Initializing current scene...");

		PackedScene playerRes = (PackedScene)ResourceLoader.Load(pPlayer);
		if(playerRes == null)
		{
			GD.PrintErr("Failed to load player prefab from " + pPlayer);
			return;
		}

		var player = playerRes.Instance();
		owner.AddChild(player);
		GameManager.Instance.oPlayer = player.GetNode<Player>("oPlayer");
		GameManager.Instance.sceneInitialized = true;

		GD.Print("This scene has been initialized successfully.");
	}
}
