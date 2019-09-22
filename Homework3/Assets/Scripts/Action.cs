using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.Engine
{
	
	public class SSAction : ScriptableObject       //SSAction为管理器     
	{

		public bool enable = true;                      
		public bool destroy = false;                    

		public GameObject gameobject;                   
		public Transform transform;                     
		public ISSActionCallback callback;              

		protected SSAction() {}                        

		public virtual void Start()                    
		{
			throw new System.NotImplementedException();
		}

		public virtual void Update()
		{
			throw new System.NotImplementedException();
		}
	}

	public class SSMoveToAction : SSAction                        //创建船移动的动作
	{
		public Vector3 target;        
		public float speed;           

		private SSMoveToAction(){}
		public static SSMoveToAction GetSSAction(Vector3 target, float speed) 
		{
			SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
			action.target = target;
			action.speed = speed;
			return action;
		}

		public override void Update() 
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed*Time.deltaTime);
			if (this.transform.position == target) 
			{
				this.destroy = true;
				this.callback.SSActionEvent(this);      
			}
		}

		public override void Start() 
		{
			
		}
	}

 	public class SequenceAction: SSAction, ISSActionCallback  //创建上下船的动作
	{           
		public List<SSAction> sequence;    
		public int repeat = -1;           
		public int start = 0;              

		public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence) 
		{
			SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
			action.repeat = repeat;
			action.sequence = sequence;
			action.start = start;
			return action;
		}

		public override void Update() 
		{
			if (sequence.Count == 0) return;
			if (start < sequence.Count) 
			{
				sequence[start].Update();     
			}
		}

		public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
			int intParam = 0, string strParam = null, Object objectParam = null)
		{
			source.destroy = false;          //先保留这个动作，如果是无限循环动作组合之后还需要使用
			this.start++;
			if (this.start >= sequence.Count) 
			{
				this.start = 0;
				if (repeat > 0) repeat--;
				if (repeat == 0) 
				{
					this.destroy = true;              
					this.callback.SSActionEvent(this);  //重复执行
				}
			}
		}

		public override void Start() 
		{
			foreach(SSAction action in sequence) 
			{
				action.gameobject = this.gameobject;
				action.transform = this.transform;
				action.callback = this;                
				action.Start();
			}
		}

		void OnDestroy() 
		{
			
		}
	}

	public enum SSActionEventType : int { Started, Competeted }  

	public interface ISSActionCallback  
	{  
		void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
			int intParam = 0, string strParam = null, Object objectParam = null);  
	} 

	public class SSActionManager: MonoBehaviour, ISSActionCallback                      //执行动作的类
	{   

		private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //将执行的动作的字典集合,int为key，SSAction为value
		private List<SSAction> waitingAdd = new List<SSAction>();                       //等待去执行的动作列表
		private List<int> waitingDelete = new List<int>();                              //等待删除的动作的key                

		protected void Update() 
		{
			foreach(SSAction ac in waitingAdd)                                         
			{
				actions[ac.GetInstanceID()] = ac;                                      //获取动作实例的ID作为key
			}
			waitingAdd.Clear();

			foreach(KeyValuePair<int, SSAction> kv in actions) 
			{
				SSAction ac = kv.Value;
				if (ac.destroy) 
				{
					waitingDelete.Add(ac.GetInstanceID()); //清空动作
				} 
				else if (ac.enable) 
				{
					ac.Update();
				}
			}

			foreach(int key in waitingDelete) 
			{
				SSAction ac = actions[key];
				actions.Remove(key);
				DestroyObject(ac);
			}
			waitingDelete.Clear();
		}
		
		public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) //添加到动作到动作管理器的成员中 后由update自动执行列表中的动作并清空
		{
			action.gameobject = gameobject;
			action.transform = gameobject.transform;
			action.callback = manager;                                               
			waitingAdd.Add(action);                                                    
			action.Start();
		}

		public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
			int intParam = 0, string strParam = null, Object objectParam = null)
		{
			
		}
	}
	public class Director: System.Object
	{
		public static int cn_move = 0;//0->move, 1->pause
		private static Director _instance;
		public SceneController curren{ get; set;}
		public static Director get_Instance(){
			if (_instance == null)
			{
				_instance = new Director();
			}
			return _instance;
		}
	}
	public interface SceneController // 创建控制器
	{
		void loadResources();
	}
	//-----------CharacterController-----------------
	public class MyCharacterController{
		private GameObject character;
		//readonly moveable Cmove;
		readonly ClickGUI clickgui;
		readonly int Ctype;//0->priset, 1->devil
		private bool isOnboat;
		private CoastController coastcontroller;

		public MyCharacterController(string Myname){ //控制器专门创建对象 管理对象属性
			if(Myname == "priest"){
				character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity,null) as GameObject;
				Ctype = 0;
			}
			else{
				character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity,null) as GameObject;
				Ctype = 1;
			}
			//Cmove = character.AddComponent(typeof(moveable)) as moveable;
			clickgui = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
			clickgui.setController(this);
		}
		public GameObject getGameObject(){
			return character;
		}
		public int getType(){
			return Ctype;
		}
		public void setName(string name){
			character.name = name;
		}
		public string getName(){
			return character.name;
		}
		public void setPosition(Vector3 postion){
			character.transform.position = postion;
		}
		public void getOnBoat(BoatController tem_boat){
			coastcontroller = null;
			character.transform.parent = tem_boat.getGameObject ().transform;
			isOnboat = true;
		}
		public void getOnCoast(CoastController coastCon){
			coastcontroller = coastCon;
			character.transform.parent = null;
			isOnboat = false;
		}
		public bool _isOnBoat(){
			return isOnboat;
		}
		public CoastController getCoastController(){
			return coastcontroller;
		}
		public void reset(){
			//Cmove.reset ();
			coastcontroller = (Director.get_Instance ().curren as MySceneController).fromCoast;
			getOnCoast(coastcontroller);
			setPosition (coastcontroller.getEmptyPosition ());
			coastcontroller.getOnCoast (this);
		}
	}
	public interface UserAction{
		void moveboat();
		void isClickChar (MyCharacterController tem_char);
		void restart();
	}
	//-----------CoastController---------------------
	public class CoastController{
		readonly GameObject coast;
		readonly Vector3 from_pos = new Vector3(9,1,0);
		readonly Vector3 to_pos = new Vector3(-9,1,0);
		readonly Vector3[] postion;
		readonly int TFflag;//-1->to, 1->from

		private MyCharacterController[] passengerPlaner;

		public CoastController(string to_or_from){
			postion = new Vector3[] {
				new Vector3 (6.5f, 2.25f, 0),
				new Vector3 (7.5f, 2.25f, 0),
				new Vector3 (8.5f, 2.25f, 0),
				new Vector3 (9.5f, 2.25f, 0),
				new Vector3 (10.5f, 2.25f, 0),
				new Vector3 (11.5f, 2.25f, 0)
			};
			passengerPlaner = new MyCharacterController[6];
			if(to_or_from == "from"){
				coast = Object.Instantiate(Resources.Load("Prefabs/Mycoast", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
				coast.name = "from";
				TFflag = 1;
			}
			else{
				coast = Object.Instantiate(Resources.Load("Prefabs/Mycoast", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
				coast.name = "to";
				TFflag = -1;
			}
		}
		public int getTFflag(){
			return TFflag;
		}
		public MyCharacterController getOffCoast(string object_name){
			for(int i=0; i<passengerPlaner.Length; i++){
				if(passengerPlaner[i] != null && passengerPlaner[i].getName() == object_name){
					MyCharacterController myCharacter = passengerPlaner[i];
					passengerPlaner[i] = null;
					return myCharacter;
				}
			}
			return null;
		}
		public int getEmptyIndex(){
			for(int i=0; i<passengerPlaner.Length; i++){
				if(passengerPlaner[i] == null){
					return i;
				}
			}
			return -1;
		}
		public Vector3 getEmptyPosition(){
			int index = getEmptyIndex();
			Vector3 pos = postion[index];
			pos.x *= TFflag;
			return pos;
		}
		public void getOnCoast(MyCharacterController myCharacter){
			int index = getEmptyIndex();
			passengerPlaner[index] = myCharacter;
		}
		public int[] getCharacterNum(){
			int[] count = {0,0};
			for(int i=0; i<passengerPlaner.Length; i++){
				if(passengerPlaner[i] == null) continue;
				if(passengerPlaner[i].getType() == 0) count[0]++;
				else count[1]++;
			}
			return count;
		}
		public void reset(){
			passengerPlaner = new MyCharacterController[6];
		}
	}
	
	//------------------------------BoatController--------------------------------------
	public class BoatController{
		readonly GameObject boat;
		//readonly moveable Cmove;
		readonly Vector3 fromPos = new Vector3 (5, 1, 0);
		readonly Vector3 toPos = new Vector3 (-5, 1, 0);
		readonly Vector3[] from_pos;
		readonly Vector3[] to_pos;
		private int TFflag;//-1->to, 1->from
		private MyCharacterController[] passenger = new MyCharacterController[2];
		public float move_speed = 20;                                         //动作分离版本新增

		public BoatController(){
			TFflag = 1;
			from_pos = new Vector3[]{ new Vector3 (4.5f, 1.5f, 0), new Vector3 (5.5f, 1.5f, 0) };
			to_pos = new Vector3[]{ new Vector3 (-5.5f, 1.5f, 0), new Vector3 (-4.5f, 1.5f, 0) };
			boat = Object.Instantiate (Resources.Load ("Prefabs/Boat", typeof(GameObject)), fromPos, Quaternion.identity, null) as GameObject;
			boat.name = "boat";
			//Cmove = boat.AddComponent (typeof(moveable)) as moveable;
			boat.AddComponent (typeof(ClickGUI));

		}
		public Vector3 boatMove(){
			if (TFflag == 1) {
				TFflag = -1;
				return new Vector3 (-5, 1, 0);
			} else {
				TFflag = 1;
				return new Vector3 (5, 1, 0);
			}
		}
		public void getOnBoat(MyCharacterController tem_cha){
			int index = getEmptyIndex ();
			passenger [index] = tem_cha;
		}
		public MyCharacterController getOffBoat(string object_name){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null && passenger [i].getName () == object_name) {
					MyCharacterController temp_character = passenger [i];
					passenger [i] = null;
					return temp_character;
				}
			}
			return null;
		}
		public int getEmptyIndex(){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					return i;
			}
			return -1;
		}
		public bool IfEmpty(){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null)
					return false;
			}
			return true;
		}
		public Vector3 getEmptyPosition(){
			Vector3 pos;
			int index = getEmptyIndex ();
			if (TFflag == 1) {
				pos = from_pos [index];
			} else {
				pos = to_pos [index];
			}
			return pos;
		}
		public GameObject getGameObject(){
			return boat;
		}
		public int getTFflag(){
			return TFflag;
		}
		public int[] getCharacterNum(){
			int[] count = { 0, 0 };
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					continue;
				if (passenger [i].getType () == 0) {
					count [0]++;
				} else {
					count [1]++;
				}
			}
			return count;
		}
		public void reset(){
			//Cmove.reset ();
			if (TFflag == -1) {
				boatMove ();
			}
			boat.transform.position = new Vector3 (5, 1, 0);
			passenger = new MyCharacterController[2];
		}
	}		
}
