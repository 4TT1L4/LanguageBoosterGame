using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LanguageBoosterGame
{
    partial class LevelManager
    {
        private Color RandomColor()
        {
            return Color.FromArgb((byte)255, (byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
        }

        Random r = new Random();
        private HelixViewport3D view = null;

        public LevelManager(HelixViewport3D view)
        {
            this.view = view;

            LevelGenerators.Add(LoadLevel3);
            LevelGenerators.Add(LoadLevel5);
            LevelGenerators.Add(LoadLevel1);
            LevelGenerators.Add(LoadLevel6);
            LevelGenerators.Add(LoadLevel2);
            LevelGenerators.Add(LoadLevel4);
            LevelGenerators.Add(LoadLevel5);
            LevelGenerators.Add(LoadLevel5);
            LevelGenerators.Add(LoadLevel4);
            LevelGenerators.Add(LoadLevel1);
            LevelGenerators.Add(LoadLevel2);
            LevelGenerators.Add(LoadLevel5);
            LevelGenerators.Add(LoadLevel4);
            LevelGenerators.Add(LoadLevel6);
            LevelGenerators.Add(LoadLevel5);
        }

        List<Action> LevelGenerators = new List<Action>();

       public ReadOnlyCollection<Action> LevelGenertorsReadOnly
        {
            get
            {
                return LevelGenerators.AsReadOnly();
            }
        }

        internal void LoadLevel(int Level)
        {
            int LevelToLoad = (Level - 1) % LevelGenerators.Count;
            LevelGenerators[LevelToLoad]();
            
        }

        private class EnemyType
        {
            public const string Enemy1 = "Models/SI1.obj";
            public const string Enemy2 = "Models/SI2.obj";
            public const string Enemy3 = "Models/SI3.obj";
        }

        private HelixToolkit.Wpf.ModelImporter importer = new HelixToolkit.Wpf.ModelImporter();
        private Model3DGroup CreateEnemyModel(string EnemyType, Color color)
        {
            Model3DGroup EnemyModel = null;
            if (EnemyType.Contains("#"))
            {
                MeshBuilder b = new MeshBuilder();
                if (EnemyType.Contains("box"))
                {
                    b.AddBox(new Point3D(1, 1, 1), 40, 40, 40);
                }
                else
                if (EnemyType.Contains("sphere"))
                {
                    b.AddSphere(new Point3D(1, 1, 1), 40);
                }

                var Mesh = b.ToMesh();
                var GeometryModel = new GeometryModel3D { Geometry = Mesh };

                ModelVisual3D ModelVisual = new ModelVisual3D();
                ModelVisual.Content = GeometryModel;
                EnemyModel = new Model3DGroup();
                EnemyModel.Children.Add(ModelVisual.Content);
            }
            else
            {
                EnemyModel = importer.Load(EnemyType.ToString(), null, false);
            }


            foreach (var Child in EnemyModel.Children)
            {
                var Model = ((GeometryModel3D)Child);
                Model.Material = MaterialHelper.CreateMaterial(color);
            }

            return EnemyModel;
        }
        private ModelVisual3D CreateEnemy(String EnemyType, Color EnemyColor, double x, double y, double z, double size = 1)
        {
            ModelVisual3D Enemy = new ModelVisual3D();

            Enemy.SetName("Enemy");

            Enemy.Content = CreateEnemyModel(EnemyType, EnemyColor);

            Transform3DGroup Transforms = new Transform3DGroup();
            Transforms.Children.Add(new ScaleTransform3D(0.01 * size, 0.01 * size, 0.01 * size));
            Transforms.Children.Add(new TranslateTransform3D(x, y, z));
            Enemy.Transform = Transforms;

            return Enemy;
        }


       
    }
}
