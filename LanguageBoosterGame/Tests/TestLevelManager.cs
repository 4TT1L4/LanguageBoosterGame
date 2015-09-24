using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HelixToolkit.Wpf;

namespace LanguageBoosterGame.Tests
{   
    [TestFixture]
    class TestLevelManager
    {
        [STAThread]
        private bool LevelAddsEnemies(Action level, HelixViewport3D view)
        {
            level();

            return view.Children.Count > 0;
        }

        [STAThread]
        [Test]
        public void Test_Initialization()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            var generators = manager.LevelGenertorsReadOnly;

            Assert.Greater(generators.Count, 0);
        }

        [STAThread]
        [Test]
        public void Test_Level1_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel1, view);
            Assert.IsTrue(AddsEnemies);
        }

        [STAThread]
        [Test]
        public void Test_Level2_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel2, view);
            Assert.IsTrue(AddsEnemies);
        }

        [STAThread]
        [Test]
        public void Test_Level3_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel3, view);
            Assert.IsTrue(AddsEnemies);
        }
        [STAThread]
        [Test]
        public void Test_Level4_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel4, view);
            Assert.IsTrue(AddsEnemies);
        }
        [STAThread]
        [Test]
        public void Test_Level5_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel5, view);
            Assert.IsTrue(AddsEnemies);
        }
        [STAThread]
        [Test]
        public void Test_Level6_AddsEnemies()
        {
            var view = new HelixToolkit.Wpf.HelixViewport3D();
            var manager = new LanguageBoosterGame.LevelManager(view);

            bool AddsEnemies = LevelAddsEnemies(manager.LoadLevel6, view);
            Assert.IsTrue(AddsEnemies);
        }
        [STAThread]
        [Test]
        public void Test_Next_Level_WorksFor15Levels()
        {
            for(int i = 1; i <= 15; i ++)
            {
                var view = new HelixToolkit.Wpf.HelixViewport3D();
                var manager = new LanguageBoosterGame.LevelManager(view);

                manager.LoadLevel(i);

                Assert.IsTrue(view.Children.Count > 0);
            }
        }


    }
}
