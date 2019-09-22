using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class UserGUI : MonoBehaviour {
	private UserAction action;
	private GUIStyle MyStyle;
	private GUIStyle MyButtonStyle;
	public int if_win_or_not;
	public int show;

	void Start(){
		action = Director.get_Instance ().curren as UserAction;

		MyStyle = new GUIStyle ();
		MyStyle.fontSize = 40;
		MyStyle.normal.textColor = new Color (255f, 0, 0);
		MyStyle.alignment = TextAnchor.MiddleCenter;
	}
	void reStart(){
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 50, 150, 50), "Restart")) {
			if_win_or_not = 0;
			action.restart ();
		}
	}
	void OnGUI(){
		reStart ();
		if (if_win_or_not == -1) {
			GUI.Label (new Rect (Screen.width/2 - 80, 100, 100, 50), "Game Over!!!", MyStyle);
			reStart ();
			action.pause();
			Director.cn_move = 1;
		} else if (if_win_or_not == 1) {
			GUI.Label (new Rect (Screen.width/2 - 80, 100, 100, 50), "You Win!!!", MyStyle);
			reStart ();
			action.pause();
			Director.cn_move = 1;
		}
	}
}
