using System.Threading;
using TrueGearSDK;
using System.IO;
using System.Xml.Serialization;

namespace MyTrueGear
{
    public class TrueGearMod
    {
        private static TrueGearPlayer _player = null;

        private static ManualResetEvent headInObstacleMRE = new ManualResetEvent(false);
        private static ManualResetEvent pauseMRE = new ManualResetEvent(true);

        public TrueGearMod() 
        {
            //_player = new TrueGearPlayer();
            //RegisterFilesFromDisk();
            _player = new TrueGearPlayer("620980","Beat Saber");
            _player.Start();
            new Thread(new ThreadStart(this.HeadInObstacle)).Start();
        }

        //private void RegisterFilesFromDisk()
        //{
        //    FileInfo[] files = new DirectoryInfo(".//Plugins//TrueGear")   //  (".//BepInEx//plugins//TrueGear")
        //            .GetFiles("*.asset_json", SearchOption.AllDirectories);

        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        string name = files[i].Name;
        //        string fullName = files[i].FullName;
        //        if (name == "." || name == "..")
        //        {
        //            continue;
        //        }
        //        string jsonStr = File.ReadAllText(fullName);
        //        JSONNode jSONNode = JSON.Parse(jsonStr);
        //        EffectObject _curAssetObj = EffectObject.ToObject(jSONNode.AsObject);
        //        string uuidName = Path.GetFileNameWithoutExtension(fullName);
        //        _curAssetObj.uuid = uuidName;
        //        _curAssetObj.name = uuidName;
        //        _player.SetupRegister(uuidName, jsonStr);
        //    }
        //}
        public void HeadInObstacle()
        {
            while (true)
            {
                pauseMRE.WaitOne();
                headInObstacleMRE.WaitOne();
                _player.SendPlay("HeadInObstacle");
                Thread.Sleep(200);
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

        public void IsPause()
        { 
            pauseMRE.Reset();
        }

        public void NotPause()
        {
            pauseMRE.Set();
        }


    }
}
