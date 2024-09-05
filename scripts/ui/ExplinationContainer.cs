using Godot;
using System;
using System.Collections.Generic;
using heigpdg2024.scripts.resources;

public partial class ExplinationContainer : VBoxContainer
{
    private static Dictionary<BlockType, string> _explinations = new Dictionary<BlockType, string> {
        {BlockType.Source, "Source des notes musique, relier des tronçons de portée pour commencer la production de vos sons."},
        {BlockType.Belt, "Portée, elle sert à transporter vos notes de musiques."},
        {BlockType.Instrument1, "Instuments, modifie les son que produirat votre note/accord."},
        {BlockType.ShiftUp, "Bas/Haut , permet de modifier la note en passant à la note supérieur/inférieur. Exemple : un “la” qui passe dans le modificateur haut deviendra un “si”."},
        {BlockType.Merger, "Fusion, fusionne vos notes pour créer un accord."},
        {BlockType.SpeedUp, "Rallonge/Racourcis la durée de la note/accord."},
        {BlockType.Speaker, "Enceinte, valide les notes/accords reçues et les joue."},
    };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        foreach (KeyValuePair<BlockType, string> block in _explinations) {
            HBoxContainer child = new HBoxContainer();
            child.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            
            MarginContainer margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left", 64);
            child.AddChild(margin);
            
            TextureRect textureRect = new TextureRect();
            textureRect.Texture = GD.Load<Block>("res://resources/block/" + block.Key + ".tres").Sprite;
            textureRect.SetCustomMinimumSize(new Vector2(32, 32));
            textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            textureRect.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
            child.AddChild(textureRect);
            
            Label label = new Label();
            label.Text = block.Value;
            label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            label.AutowrapMode = TextServer.AutowrapMode.Word;
            child.AddChild(label);
            
            AddChild(child);
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
