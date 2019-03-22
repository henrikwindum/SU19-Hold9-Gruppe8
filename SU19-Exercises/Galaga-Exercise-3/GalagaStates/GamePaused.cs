using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private int activePauseButton;
        private int inactivePauseButton;

        private Entity pauseGroundImage;
        private Text[] pauseButtons;

        public GamePaused() {
            pauseGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f,0.0f),new Vec2F(1.0f,1.0f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
            pauseButtons = new[] {
                new Text("Resume", new Vec2F(0.4f, 0.25f), new Vec2F(0.3f, 0.3f)),
                new Text("Quit to menu", new Vec2F(0.4f, 0.15f), new Vec2F(0.3f, 0.3f))
            };

            activePauseButton = 0;
            inactivePauseButton = 1;
        }

        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            throw new System.NotImplementedException();
        }

        public void UpdateGameLogic() {
            pauseButtons[activePauseButton].SetColor(Color.Red);
            pauseButtons[inactivePauseButton].SetColor(Color.White);
        }

        public void RenderState() {
            pauseGroundImage.RenderEntity();
            foreach (var button in pauseButtons) {
                button.RenderText();
            }
        }
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_UP":
                if (keyAction == "KEY_PRESS") {
                    activePauseButton = 0;
                    inactivePauseButton = 1;
                }

                break;
            case "KEY_DOWN":
                if (keyAction == "KEY_PRESS") {
                    activePauseButton = 1;
                    inactivePauseButton = 0;
                }
                break;
            case "KEY_ENTER":
                switch (activePauseButton) {
                case 0:
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors
                        (GameEventType.GameStateEvent, this,
                            "CHANGE_STATE", "GAME_RUNNING", ""));
                    break;
                case 1:
                    if (keyAction == "KEY_PRESS") {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, 
                                "CHANGE_STATE", "MAIN_MENU", ""));    
                    }
                    break;
                }
            break;
            }
        }
    }
}