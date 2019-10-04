using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
namespace Com.Engine
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        void Hit(Vector3 pos);
        void Restart();
        int GetScore();
        bool RoundStop();
        int GetRound();
    }
    public class SSActionManager : MonoBehaviour //used as the former work devil & priest
    {
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>(); // get action id
        private List<SSAction> waitingToAdd = new List<SSAction>();
        private List<int> watingToDelete = new List<int>();

        protected void Update()
        {
            foreach (SSAction ac in waitingToAdd)
            {
                actions[ac.GetInstanceID()] = ac;
            }
            waitingToAdd.Clear();

            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destroy)
                {
                    watingToDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enable)
                {
                    ac.Update();
                }
            }

            foreach (int key in watingToDelete)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                DestroyObject(ac);
            }
            watingToDelete.Clear();
        }

        public void addAction(GameObject gameObject, SSAction action, SSActionCallback ICallBack)
        {
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.CallBack = ICallBack;
            waitingToAdd.Add(action);
            action.Start();
        }
    }
    public enum SSActionEventType : int { Started, Completed }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
    public class SSDirector : System.Object
    {   
        private static SSDirector _instance;

        public ISceneController currentScenceController { get; set; }
        public bool running { get; set; }

        public static SSDirector getInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }

        public int getFPS()
        {
            return Application.targetFrameRate;
        }

        public void setFPS(int fps)
        {
            Application.targetFrameRate = fps;
        }
    }
    public class SSAction : ScriptableObject
    {

        public bool enable = true;
        public bool destroy = false;

        public GameObject gameObject;
        public Transform transform;
        public SSActionCallback CallBack;

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    public class CCMoveToAction : SSAction
    {
        public float speedx;
        public float speedy = 0;

        private CCMoveToAction() { }
        public static CCMoveToAction getAction(float speedx)
        {
            CCMoveToAction action = CreateInstance<CCMoveToAction>();
            action.speedx = speedx;
            return action;
        }

        public override void Update()
        {
            this.transform.position += new Vector3(speedx*Time.deltaTime, -speedy*Time.deltaTime+(float)-0.5*10*Time.deltaTime*Time.deltaTime,0);
            speedy += 10*Time.deltaTime;
            if (transform.position.y <= 1)
            {
                destroy = true;
                CallBack.SSActionCallback(this);
            }
        }

        public override void Start()
        {

        }
    }
    public class Disk : MonoBehaviour {
        public Vector3 StartPoint { get { return gameObject.transform.position; } set { gameObject.transform.position = value; } }
        public Color color { get { return gameObject.GetComponent<Renderer>().material.color; } set { gameObject.GetComponent<Renderer>().material.color = value; } }
        public float speed { get;set; }
        public Vector3 Direction { get { return Direction; } set { gameObject.transform.Rotate(value); } }
    }
}