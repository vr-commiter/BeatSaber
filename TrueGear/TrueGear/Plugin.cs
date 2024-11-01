using IPA;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using System.Reflection.Emit;
using System.Threading;
using MyTrueGear;
using UnityEngine.XR;

namespace BeatSaber_TrueGear
{
    [Plugin(RuntimeOptions.SingleStartInit)]


    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private static TrueGearMod _TrueGear = null;

        private static bool isSlider = false;

        private static bool isDeath = false;


        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("TrueGear initialized.");

        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            new GameObject("TrueGearController").AddComponent<TrueGearController>();
            Harmony.CreateAndPatchAll(typeof(Plugin));
            _TrueGear = new TrueGearMod();
            Debug.Log("--------------------------------------------------");
            Debug.Log("LeftHandMeleeHit");
            _TrueGear.Play("LeftHandMeleeHit");
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }


        [HarmonyPostfix, HarmonyPatch(typeof(NoteCutHapticEffect), "HitNote")]
        public static void NoteCutHapticEffect_HitNote_PostPatch(SaberType saberType, NoteCutHapticEffect.Type type)
        {
            if (isDeath)
            {
                return;
            }
            if (saberType.Node().ToString() == "LeftHand")
            {
                switch (type)
                {
                    case NoteCutHapticEffect.Type.Normal:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeHit");
                        _TrueGear.Play("LeftHandMeleeHit");
                        break;
                    case NoteCutHapticEffect.Type.ShortNormal:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeShortHit");
                        _TrueGear.Play("LeftHandMeleeShortHit");
                        break;
                    case NoteCutHapticEffect.Type.ShortWeak:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeShortHit");
                        _TrueGear.Play("LeftHandMeleeShortHit");
                        break;
                    case NoteCutHapticEffect.Type.Bomb:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeBombHit");
                        _TrueGear.Play("LeftHandMeleeBombHit");
                        break;
                    case NoteCutHapticEffect.Type.BadCut:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeBadHit");
                        _TrueGear.Play("LeftHandMeleeBadHit");
                        break;
                    default:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("LeftHandMeleeHit");
                        _TrueGear.Play("LeftHandMeleeHit");
                        break;
                }
            }
            else if (saberType.Node().ToString() == "RightHand")
            {
                switch (type)
                {
                    case NoteCutHapticEffect.Type.Normal:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeHit");
                        _TrueGear.Play("RightHandMeleeHit");
                        break;
                    case NoteCutHapticEffect.Type.ShortNormal:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeShortHit");
                        _TrueGear.Play("RightHandMeleeShortHit");
                        break;
                    case NoteCutHapticEffect.Type.ShortWeak:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeShortHit");
                        _TrueGear.Play("RightHandMeleeShortHit");
                        break;
                    case NoteCutHapticEffect.Type.Bomb:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeBombHit");
                        _TrueGear.Play("RightHandMeleeBombHit");
                        break;
                    case NoteCutHapticEffect.Type.BadCut:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeBadHit");
                        _TrueGear.Play("RightHandMeleeBadHit");
                        break;
                    default:
                        Debug.Log("--------------------------------------------------");
                        Debug.Log("RightHandMeleeHit");
                        _TrueGear.Play("RightHandMeleeHit");
                        break;
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(SliderHapticFeedbackInteractionEffect), "Vibrate")]
        public static void SliderHapticFeedbackInteractionEffect_Vibrate_PostPatch(SliderHapticFeedbackInteractionEffect __instance)
        {
            if (isDeath)
            {
                return;
            }
            if (!isSlider)
            {
                isSlider = true;



                //var sliderInteractionEffect = GameObject.FindObjectsOfType<SliderInteractionEffect>();
                //var colorType = Traverse.Create(sliderInteractionEffect).Field("colorType").GetValue<ColorType>();
                //var _saberType = colorType.ToSaberType();
                if (__instance._saberType == SaberType.SaberA)
                {
                    Debug.Log("--------------------------------------------------");
                    Debug.Log("LeftHandMeleeSliderHit");
                    _TrueGear.Play("LeftHandMeleeSliderHit");
                }
                else if (__instance._saberType == SaberType.SaberB)
                {
                    Debug.Log("--------------------------------------------------");
                    Debug.Log("RightHandMeleeSliderHit");
                    _TrueGear.Play("RightHandMeleeSliderHit");
                }
                Timer sliderTimer = new Timer(SliderTimerCallBack, null, 40, Timeout.Infinite);
            }

            
        }
        private static void SliderTimerCallBack(System.Object o)
        {
            isSlider = false;
        }



        [HarmonyPostfix, HarmonyPatch(typeof(ScoreController), "HandleNoteWasMissed")]
        public static void ScoreController_HandleNoteWasMissed_PostPatch(NoteController noteController)
        {
            if (isDeath)
            {
                return;
            }
            if (noteController.noteData.colorType == ColorType.None)
            {
                return;
            }
            Debug.Log("--------------------------------------------------");
            Debug.Log("HandleNoteWasMissed");
            _TrueGear.Play("HandleNoteWasMissed");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LevelFailedTextEffect), "ShowEffect")]
        public static void LevelFailedText_ShowEffect_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("LevelFailed");
            _TrueGear.Play("LevelFailed");
            _TrueGear.StopHeadInObstacle();
            isDeath = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LevelScenesTransitionSetupDataSO), "BeforeScenesWillBeActivatedAsync")]
        public static void LevelScenesTransitionSetupDataSO_BeforeScenesWillBeActivatedAsync_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("LevelStarted");
            _TrueGear.Play("LevelStarted");
            isDeath = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StandardLevelGameplayManager), "HandleSongDidFinish")]
        public static void StandardLevelGameplayManager_HandleSongDidFinish_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("LevelFinished");
            _TrueGear.Play("LevelFinished");
            isDeath = false;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(HeadObstacleLowPassAudioEffect), "Update")]
        public static IEnumerable<CodeInstruction> HeadInObstacleHapticTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var triggerLowPassMethod = typeof(MainAudioEffects).GetMethod("TriggerLowPass");
            var resumeNormalSoundMethod = typeof(MainAudioEffects).GetMethod("ResumeNormalSound");

            var headInObstacleHapticMethod = typeof(Plugin).GetMethod("HeadInObstacleHapticMethod");
            var notheadInObstacleHapticMethod = typeof(Plugin).GetMethod("NotHeadInObstacleHapticMethod");

            for (int i = 0; i < codes.Count; i++)
            {
                // 检查当前指令是否是调用PlayHapticFeedback的方法
                if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == triggerLowPassMethod)
                {
                    // 插入你的方法调用
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, headInObstacleHapticMethod));
                    i += 1; // 跳过我们刚刚插入的指令
                }
                else if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == resumeNormalSoundMethod)
                {
                    // 插入你的方法调用
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, notheadInObstacleHapticMethod));
                    i += 1; // 跳过我们刚刚插入的指令
                }
            }
            return codes.AsEnumerable();
        }
        public static void HeadInObstacleHapticMethod()
        {
            if (isDeath)
            {
                return;
            }
            Debug.Log("--------------------------------------------------");
            Debug.Log("StartHeadInObstacle");
            _TrueGear.StartHeadInObstacle();
        }
        public static void NotHeadInObstacleHapticMethod()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("StopHeadInObstacle");
            _TrueGear.StopHeadInObstacle();
        }


        [HarmonyTranspiler, HarmonyPatch(typeof(ObstacleSaberSparkleEffectManager), "Update")]
        public static IEnumerable<CodeInstruction> ObstacleSaberSparkleTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var playHapticFeedbackMethod = typeof(HapticFeedbackManager).GetMethod("PlayHapticFeedback");
            var obstacleSaberSparkleMethod = typeof(Plugin).GetMethod("ObstacleSaberSparkleMethod");

            for (int i = 0; i < codes.Count; i++)
            {
                // 检查当前指令是否是调用PlayHapticFeedback的方法
                if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == playHapticFeedbackMethod)
                {
                    // 插入你的方法调用
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, obstacleSaberSparkleMethod));
                    i += 1; // 跳过我们刚刚插入的指令
                }
            }
            return codes.AsEnumerable();
        }
        public static void ObstacleSaberSparkleMethod()
        {
            if (isDeath)
            {
                return;
            }
            Debug.Log("--------------------------------------------------");
            Debug.Log("ObstacleSaberSparkle");
            _TrueGear.Play("ObstacleSaberSparkle");
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(SaberClashEffect), "LateUpdate")]
        public static IEnumerable<CodeInstruction> SaberClashTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var playHapticFeedbackMethod = typeof(HapticFeedbackManager).GetMethod("PlayHapticFeedback");
            var saberClashMethod = typeof(Plugin).GetMethod("SaberClashMethod");

            int playHapticFeedbackCount = 0;

            for (int i = 0; i < codes.Count; i++)
            {
                // 检查当前指令是否是调用PlayHapticFeedback的方法
                if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == playHapticFeedbackMethod)
                {
                    playHapticFeedbackCount++;
                    if (playHapticFeedbackCount == 2)
                    {
                        codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, saberClashMethod));
                        i += 1; // 跳过我们刚刚插入的指令
                    }                    
                }
            }
            return codes.AsEnumerable();
        }
        public static void SaberClashMethod()
        {
            if (isDeath)
            {
                return;
            }
            Debug.Log("--------------------------------------------------");
            Debug.Log("SaberClash");
            _TrueGear.Play("SaberClash");
        }




        [HarmonyPostfix, HarmonyPatch(typeof(PauseMenuManager), "ShowMenu")]
        public static void PauseMenuManager_ShowMenu_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("ShowMenu");
            _TrueGear.IsPause();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PauseMenuManager), "StartResumeAnimation")]
        public static void PauseMenuManager_StartResumeAnimation_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("StartResumeAnimation");
            isDeath = false;
            _TrueGear.NotPause();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PauseMenuManager), "MenuButtonPressed")]
        public static void PauseMenuManager_MenuButtonPressed_PostPatch()
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("MenuButtonPressed");
            _TrueGear.StopHeadInObstacle();
            isDeath = true;
            _TrueGear.NotPause();
        }


        /*[HarmonyPostfix, HarmonyPatch(typeof(ObstacleSaberSparkleEffectManager), "Update")]
        public static void ObstacleSaberSparkleEffectManager_Update_PostPatch()
        {
            var obstacleSaberSparkleEffectManager = GameObject.FindObjectsOfType<ObstacleSaberSparkleEffectManager>();
            var _beatmapObjectManager = Traverse.Create(obstacleSaberSparkleEffectManager).Property("_beatmapObjectManager").GetValue<BeatmapObjectManager>();
            var _sabers = Traverse.Create(obstacleSaberSparkleEffectManager).Property("_sabers").GetValue<Saber[]>();
            var _saberManager = Traverse.Create(obstacleSaberSparkleEffectManager).Property("_saberManager").GetValue<SaberManager>();
            var _minimumTimeUntilHapticEnd = Traverse.Create(obstacleSaberSparkleEffectManager).Property("_minimumTimeUntilHapticEnd").GetValue<float>();

            //_sabers = new Saber[2];
           // _sabers[0] = _saberManager.leftSaber;
           // _sabers[1] = _saberManager.rightSaber;

            Debug.Log(_beatmapObjectManager);

            foreach (ObstacleController obstacleController in _beatmapObjectManager.activeObstacleControllers)
            {
                Bounds bounds = obstacleController.bounds;
                for (int i = 0; i < 2; i++)
                {
                    Vector3 vector;
                    if (_sabers[i].isActiveAndEnabled && GetBurnMarkPos(bounds, obstacleController.transform, _sabers[i].saberBladeBottomPos, _sabers[i].saberBladeTopPos, out vector))
                    {
                        Debug.Log("------------------------");
                        Debug.Log("ObstacleSaberSparkle");
                    }
                }
            }
         }

        private static bool GetBurnMarkPos(Bounds bounds, Transform transform, Vector3 bladeBottomPos, Vector3 bladeTopPos, out Vector3 burnMarkPos)
        {
            bladeBottomPos = transform.InverseTransformPoint(bladeBottomPos);
            bladeTopPos = transform.InverseTransformPoint(bladeTopPos);
            float num = Vector3.Distance(bladeBottomPos, bladeTopPos);
            Vector3 vector = bladeTopPos - bladeBottomPos;
            vector.Normalize();
            float num2;
            if (bounds.IntersectRay(new Ray(bladeBottomPos, vector), out num2) && num2 <= num)
            {
                burnMarkPos = transform.TransformPoint(bladeBottomPos + vector * num2);
                return true;
            }
            burnMarkPos = Vector3.zero;
            return false;
        }*/

    }
}