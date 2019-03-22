using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using Galaga_Exercise_3._1.GalagaStates;

namespace Galaga_Exercise_3._1 {
    public class Game : IGameEventProcessor<object> {
        private GameTimer gameTimer;
        private StateMachine stateMachine;
        private Window win;

        public Game() {
            win = new Window("Galaga", 500, 500);
            
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.GameStateEvent
            });
            stateMachine = new StateMachine();
            win.RegisterEventBus(GalagaBus.GetBus());
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent,this);

            gameTimer = new GameTimer(60, 60);
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    // Update game logic here
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }
                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                GalagaBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                        "", ""));
                break;
            }
        }

        public void KeyRelease(string key) { }
    }
}