using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class MySceneActionManager:SSActionManager  
{                  
	private SSMoveToAction move_boat;     
	private SequenceAction action_seq;     

	public MySceneController sceneController;

	protected new void Start()
	{
		sceneController = (MySceneController)Director.get_Instance().curren;
		sceneController.actionManager = this;
	}
	public void moveBoat(GameObject boat, Vector3 target, float speed)
	{
		move_boat = SSMoveToAction.GetSSAction(target, speed);  //新建一个动作
		this.RunAction(boat, move_boat, this); //RunAction添加动作对象和执行传入的动作
	}

	public void moveRole(GameObject role, Vector3 middle_pos, Vector3 end_pos,float speed)
	{
		SSAction action1 = SSMoveToAction.GetSSAction(middle_pos, speed); // GetSSAction添加动作到动作列表
		SSAction action2 = SSMoveToAction.GetSSAction(end_pos, speed);
		action_seq = SequenceAction.GetSSAcition(1, 0, new List<SSAction>{action1, action2}); // 1, 0 对应动作是否重复等
		this.RunAction(role, action_seq, this);//逐条执行动作
	}
}
