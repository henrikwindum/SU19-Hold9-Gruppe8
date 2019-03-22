using System.Collections.Generic;
using DIKUArcade.EventBus;
using Galaga_Exercise_3._1;
using Galaga_Exercise_3._1.GalagaStates;
using NUnit.Framework;


namespace StateMachineTesting {
    [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();
            
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent
            });
            
            stateMachine = new StateMachine();
            
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        }
        
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventPaused() {
            GalagaBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE", 
                    "GAME_PAUSED", ""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
        
        [Test]
        public void TestEventRunning() {
            GalagaBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE", 
                    "GAME_RUNNING", ""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
        
        [Test]
        public void TestEventMainMenu() {
            GalagaBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE", 
                    "MAIN_MENU", ""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
    }
}