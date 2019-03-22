using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3._1.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance;
        private int activeMenuButton;
        private int inactiveMenuButton;

        private Entity backGroundImage;
        private Text[] menuButtons;

        public MainMenu() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
            menuButtons = new[] {
                new Text("New Game", new Vec2F(0.400f, 0.250f), new Vec2F(0.3f, 0.3f)),
                new Text("Exit", new Vec2F(0.400f, 0.150f), new Vec2F(0.3f, 0.3f))
            };

            activeMenuButton = 0;
            inactiveMenuButton = 1;
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            menuButtons[activeMenuButton].SetColor(Color.Red);
            menuButtons[inactiveMenuButton].SetColor(Color.White);
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (var button in menuButtons) {
                button.RenderText();
            }
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void HandleKeyEvent(string keyValue, string keyAction){  // Message, Parameter1 
            switch (keyValue) {
            case "KEY_UP":
                if (keyAction == "KEY_PRESS") {
                    activeMenuButton = 0;
                    inactiveMenuButton = 1;
                }
                break;
            case "KEY_DOWN":
                if (keyAction == "KEY_PRESS") {
                    activeMenuButton = 1;
                    inactiveMenuButton = 0;
                }
                break;
            case "KEY_ENTER":
                switch (activeMenuButton) {
                    case 0:
                        if (keyAction == "KEY_PRESS") {
                            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_RUNNING", ""));    
                        }
                        break;
                    case 1:
                        GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                        break;
                }
                break;
            }
        }
    }
}