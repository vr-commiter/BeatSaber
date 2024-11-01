using System.Threading;
using TrueGearSDK;
using TrueGear;
using System.IO;

namespace MyTrueGear
{
    public class TrueGearMod
    {
        private static TrueGearPlayer _player = null;

        private static ManualResetEvent headInObstacleMRE = new ManualResetEvent(false);

        public TrueGearMod() 
        {
            _player = new TrueGearPlayer();
            RegisterFilesFromDisk();
            _player.Start();
            new Thread(new ThreadStart(this.HeadInObstacle)).Start();
        }
        public void HeadInObstacle()
        {
            while (true)
            {
                headInObstacleMRE.WaitOne();
                _player.SendPlay("HeadInObstacle");
                Thread.Sleep(200);
            }            
        }
        private void RegisterFilesFromDisk()
        {
            FileInfo[] files = new DirectoryInfo(".//Plugins//TrueGear")
                    .GetFiles("*.asset_json", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i].Name;
                string fullName = files[i].FullName;
                if (name == "." || name == "..")
                {
                    continue;
                }
                string jsonStr = File.ReadAllText(fullName);
                JSONNode jSONNode = JSON.Parse(jsonStr);
                EffectObject _curAssetObj = EffectObject.ToObject(jSONNode.AsObject);
                string _effectUUID = _curAssetObj.uuid;
                _player.SetupRegister(_effectUUID, jsonStr);
            }
        }

        public void Play(string Event)
        { 
            _player.SendPlay(Event);
        }

        public void StartHeadInObstacle()
        {
            headInObstacleMRE.Set();
        }

        public void StopHeadInObstacle()
        {
            headInObstacleMRE.Reset();
        }



    }
}
