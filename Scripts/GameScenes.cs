using System;
using Godot;

public enum GAME_SCENES
{
    LOADING_SCREEN,
    SANTA_HOUSE,
    OUTSIDE_FOREST,
    FACTORY,
    BARN
}

public class GameScenes
{
    public static PackedScene GetPackedScene(GAME_SCENES scene)
    {
        return (PackedScene)ResourceLoader.Load("res://Maps/" + scene.ToString().ToLower() + ".tscn");
    }
}

