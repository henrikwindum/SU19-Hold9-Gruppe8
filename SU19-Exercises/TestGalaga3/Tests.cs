using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace TestGalaga3 {
    [TestFixture]
    public class Tests {
        [Test]
        public void GamePauseTest1() {
            Assert.AreEqual("GAME_PAUSED",
                StateTransformer.TransformStateToString(GameStateType.GamePaused));
        }

        [Test]
        public void GamePauseTest2() {
            Assert.AreEqual(GameStateType.GamePaused,
                StateTransformer.TransformStringToState("GAME_PAUSED"));
        }

        [Test]
        public void GameRunningTest1() {
            Assert.AreEqual("GAME_RUNNING",
                StateTransformer.TransformStateToString(GameStateType.GameRunning));
        }

        [Test]
        public void GameRunningTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }

        [Test]
        public void MainMenuTest1() {
            Assert.AreEqual("MAIN_MENU",
                StateTransformer.TransformStateToString(GameStateType.MainMenu));
        }

        [Test]
        public void MainMenuTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }
    }
}