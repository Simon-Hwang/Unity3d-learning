using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

namespace Com.Engine{
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
    public class SSActionManager : MonoBehaviour
    {
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
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
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        int GetScore();
        void Restart();
        bool GetGameState();
        void MovePlayer(float translationX, float translationZ);
    }

    public enum SSActionEventType : int { Started, Completed }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}
