using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class ClickGUI : MonoBehaviour {

	// Use this for initialization
	UserAction action;
	MyCharacterController character;

	public void setController(MyCharacterController charac){
		character = charac;
	}
	void Start(){
		action = Director.get_Instance ().curren as UserAction;
	}
	void OnMouseDown(){
		if (gameObject.name == "boat") {
			action.moveboat (); //实际上只是创建动作 还是由管理器去实现
		} else {
			action.isClickChar (character);
		}
	}
}
