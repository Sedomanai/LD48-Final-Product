#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Animations;
using UnityEditor;

namespace Ilang
{
    // Integrated with Ilang Atlas (.atls) files exported from Ilang Atlas Editor
    // 수제 에디터로 추출한 아틀라스 (.atls) 파일 연동
    public class AtlasWindow : EditorWindow
    {
        string[] lines;
        string path, dir, animDir;
        Texture2D texture;
        TextAsset atlas;
        bool makeAnimationClips;
        bool makeController;

        [UnityEditor.AssetImporters.ScriptedImporter(1, "atls")]
        public class SrtImporter : UnityEditor.AssetImporters.ScriptedImporter
        {
            public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext c) {
                TextAsset subAsset = new TextAsset(File.ReadAllText(c.assetPath));
                c.AddObjectToAsset("text", subAsset);
                c.SetMainObject(subAsset);
            }

        }
        [MenuItem("Window/Ilang/Atlas Window")]
        static void OpenWindow() {
            AtlasWindow window = (AtlasWindow)GetWindow(typeof(AtlasWindow), false, "Ilang Atlas Window");
            window.maxSize = new Vector2(300, 400);
            window.minSize = new Vector2(300, 100);
            window.Show();
        }

        void OnGUI() {
            atlas = (TextAsset)EditorGUILayout.ObjectField("Ilang Atlas File", atlas, typeof(TextAsset), false);
            texture = (Texture2D)EditorGUILayout.ObjectField("Image", texture, typeof(Texture2D), false);
            makeAnimationClips = EditorGUILayout.Toggle("Make Animation Clips", makeAnimationClips);
            if (!makeAnimationClips)
                GUI.enabled = false;
            makeController = EditorGUILayout.Toggle("Make Controller", makeController);
            if (!makeAnimationClips)
                makeController = false;
            GUI.enabled = true;

            if (GUILayout.Button("GENERATE")) {
                path = AssetDatabase.GetAssetPath(texture);
                dir = path.Substring(0, path.LastIndexOf('/'));
                string folderName = texture.name + "Anim";
                animDir = dir + "/" + folderName;
                if (makeAnimationClips && !AssetDatabase.IsValidFolder(animDir)) {
                    AssetDatabase.CreateFolder(dir, folderName);
                }

                TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
                if (ti.spriteImportMode == SpriteImportMode.Multiple) {
                    ti.spriteImportMode = SpriteImportMode.Single;
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }

                ti.isReadable = true;
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Multiple;
                ti.mipmapEnabled = false;
                ti.filterMode = FilterMode.Point;
                
                FillAtlas(ti);
            }
        }
        void FillAtlas(TextureImporter ti) {
            bool init = false;
            List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();

            Dictionary<string, Sprite> sprites = null;
            List<ObjectReferenceKeyframe> frames = null;
            AnimatorController controller = null;

            EditorCurveBinding binding = new EditorCurveBinding();
            binding.propertyName = "m_Sprite";
            binding.type = typeof(SpriteRenderer);

// TODO: PENDING (Load using .NET 5.0 span), not important right now
#if NET5_0
            string strwidth = "width";
            string strheight = "height";
            string struser = "\t@u";
            string strcell = "\t@c";
            string strclip = "\t@a";

            int width = 0, height = 0;

            List<string> users = new();


            string text = File.ReadAllText("alex.atls");

            // 0 width
            // 1 height
            // 2 \t@u user
            // 3 \t@c cell
            // 4 \t@a clip
            int act = 0;
            text.AsMemory().Iterate('\n', (ReadOnlyMemory<char> line) => {
                int i = 0;
                line.Iterate(' ', (ReadOnlyMemory<char> token) => {
                    switch (i) {
                        case 0:
                            if (token.Compare(ref strcell)) {
                                act = 3;
                            } else if (token.Compare(ref strclip)) {
                                act = 4;
                            } else if (token.Compare(ref struser)) {
                                act = 2;
                            } else if (token.Compare(ref strwidth)) {
                                act = 0;
                            } else if (token.Compare(ref strheight)) {
                                act = 1;
                            }
                            break;
                        case 1:
                            switch (act) {
                                case 0: width = int.Parse(token.Span); break;
                                case 1: height = int.Parse(token.Span); break;
                                case 2: users.Add(token.ToString()); break;
                            }
                            break;
                    }

                    if (act == 3) {
                        switch (i) {
                            case 2: Console.Write(token + " "); break;
                            case 3: Console.Write(token + " "); break;
                            case 4: Console.Write(token + " "); break;
                            case 5: Console.Write(token + " "); break;
                            case 6: Console.Write(token + " "); break;
                            case 7: Console.Write(token + " "); break;
                            case 8: Console.Write(token + " "); break;
                            case 9: Console.WriteLine(token); break;
                        }
                    }

                    i++;
                });
            });
#else
            lines = atlas.text.Split('\n');
            for (uint i = 0; i < lines.Length; i++) {
                string[] tokens = lines[i].Split(' ');
                switch (tokens[0]) {
                    case "\t@c":
                        SpriteMetaData data = new SpriteMetaData();
                        var w = int.Parse(tokens[4]);
                        var h = int.Parse(tokens[5]);
                        data.name = tokens[9];
                        data.rect = new Rect(int.Parse(tokens[2]), texture.height - h - int.Parse(tokens[3]), int.Parse(tokens[4]), h);
                        data.pivot = new Vector2(
                            (float)((w / 2 - int.Parse(tokens[6])) / (double)w),
                            (float)((h / 2 + int.Parse(tokens[7])) / (double)h)
                        );
                        data.alignment = 9;
                        spritesheet.Add(data);
                        break;
                    case "clips":
                        if (!init) {
                            ti.spritesheet = spritesheet.ToArray();
                            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                            init = true;
                            frames = new List<ObjectReferenceKeyframe>();
                            sprites = new Dictionary<string, Sprite>();

                            var contFilename = animDir + "/" + texture.name + ".controller";
                            if (makeController && !File.Exists(contFilename)) {
                                controller = AnimatorController.CreateAnimatorControllerAtPath(contFilename);
                            } else {
                                controller = AssetDatabase.LoadAssetAtPath(contFilename, typeof(AnimatorController)) as AnimatorController;
                            }

                            Sprite[] buffer = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();

                            for (uint j = 0; j < buffer.Length; j++) {
                                sprites.Add(buffer[j].name, buffer[j]);
                            }
                        }
                        break;

                    case "\t@a":
                        if (makeAnimationClips) {
                            AnimationClip clip;
                            string clipFilename = animDir + "/" + tokens[1] + ".anim";

                            if (!File.Exists(clipFilename)) {
                                clip = new AnimationClip();
                                AssetDatabase.CreateAsset(clip, clipFilename);
                            } else {
                                clip = AssetDatabase.LoadAssetAtPath(clipFilename, typeof(AnimationClip)) as AnimationClip;
                            }

                            if (clip) {
                                frames.Clear();
                                float time = 0.0f;
                                ObjectReferenceKeyframe frame;

                                float speed = 4.0f;
                                float mult = 1.0f / 60.0f * speed;

                                for (uint j = 2; j < tokens.Length; j += 2) {
                                    frame = new ObjectReferenceKeyframe();
                                    frame.time = time;
                                    var index = int.Parse(tokens[j]);
                                    frame.value = sprites[spritesheet[index].name];
                                    frames.Add(frame);
                                    time += float.Parse(tokens[j + 1]) * mult;
                                }

                                var last = frames.Last();
                                frame = new ObjectReferenceKeyframe();
                                frame.time = time - 1 / 60.0f;
                                frame.value = last.value;
                                frames.Add(frame);

                                if (makeController && controller) {
                                    bool exists = false;
                                    for (int j = 0; j < controller.animationClips.Length; j++) {
                                        if (clip == controller.animationClips[j]) {
                                            exists = true;
                                            break;
                                        }
                                    }
                                   
                                    if (!exists)
                                        controller.AddMotion(clip);
                                }

                                AnimationUtility.SetObjectReferenceCurve(clip, binding, frames.ToArray());
                            }
                        }
                        break;
                }
            }
#endif
            if (!init) {
                ti.spritesheet = spritesheet.ToArray();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
    }
    //TODO : Revisit when NET 5.0 is available on Unity
#if NET5_0
    public static class StringExtension
    {
        public static void Iterate(this ReadOnlyMemory<char> mem, char delim, Action<ReadOnlyMemory<char>> func) {
            if (mem.Length > 0) {
                var span = mem.Span;
                int start = 0;
                for (int i = 0; i < span.Length; i++) {
                    if (span[i] == delim) {
                        var slice = mem.Slice(start, i - start);
                        if (slice.Length > 0)
                            func(slice);
                        start = i + 1;
                    }
                }
                if (span[span.Length - 1] != delim) {
                    func(mem.Slice(start, span.Length - start));
                }
            }
        }

        public static void Tokenize(this ReadOnlyMemory<char> mem, int index, Action<ReadOnlyMemory<char>, ReadOnlyMemory<char>> func) {
            if (index != -1) {
                int last = mem.Length - index - 1;
                func(mem.Slice(0, index), mem.Slice(index + 1, last));
            }
        }

        public static bool Compare(this ReadOnlyMemory<char> left, ref string right) {
            var lspan = left.Span;
            if (left.Length == right.Length) {
                for (int i = 0; i < left.Length; i++) {
                    if (lspan[i] != right[i]) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
#endif

}
#endif