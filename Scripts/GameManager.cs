using System;
using Godot;

public class GameManager : Node
{
	public static GameManager Instance;
	public Player oPlayer;
	public bool sceneInitialized = false;
	public int GameDeltaTime;
	private QuestManager qm;
	public Dialog Dialog;

	private static readonly string pPlayer = "res://Prefabs/Player.tscn";

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
		Tick();
		GameDeltaTime = 0;
		Dialog = new Dialog();

#if DEBUG
		qm = new QuestManager(1);
#endif
	}

	public void NewGame()
    {
		qm = new QuestManager(1);
		LoadGameScene(GAME_SCENES.SANTA_HOUSE);
	}

	private async void Tick()
    {
		while(true)
        {
			GameDeltaTime++;
			QuestManager.HighlightCurrentQuestNpc();
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

			GD.Print(QuestManager.Instance.CurrentQuestId);
        }
    }

    public void LoadGameScene(GAME_SCENES scene)
	{
		GD.Print("Loading scene " + scene.ToString());
		QuestManager.Instance.ResetNpcs();
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
