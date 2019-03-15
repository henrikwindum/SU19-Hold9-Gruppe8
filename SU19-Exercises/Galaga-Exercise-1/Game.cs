using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace Galaga_Exercise_1 {
    public class Game : IGameEventProcessor<object> {
        private List<Enemy> enemies;
        private Enemy enemy;
        private List<Image> enemyStrides;
        private GameEventBus<object> eventBus;
        private int explosionLength = 500;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private GameTimer gameTimer;
        private ImageStride imageStride;
        private Player player;
        private Score score;
        private Window win;

        public Game() {
            win = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(60, 60);
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent,
                GameEventType.WindowEvent
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);

            score = new Score(new Vec2F(0.8f, 0.5f),
                new Vec2F(0.2f, 0.2f));

            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(2);

            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();

            playerShots = new List<PlayerShot>();
        }

        public List<PlayerShot> playerShots { get; }

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

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        public void AddEnemies(float x, float y) {
            imageStride = new ImageStride(80, enemyStrides);
            enemy = new Enemy(this,
                new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.1f)),
                imageStride);
            enemies.Add(enemy);
        }

        // 
        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var enemy in enemies) {
                    switch (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape)
                        .Collision) {
                    case true:
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                            0.1f, 0.1f);
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        break;
                    }
                }

                var newEnemies = new List<Enemy>();
                foreach (var enemy in enemies) {
                    if (!enemy.IsDeleted()) {
                        newEnemies.Add(enemy);
                    }

                    if (enemy.IsDeleted()) {
                        score.AddPoints();
                    }
                }

                enemies = newEnemies;
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    // Update game logic here
                    player.Move();
                    eventBus.ProcessEvents();
                    IterateShots();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    // Render gameplay entities here
                    score.RenderScore();
                    explosions.RenderAnimations();
                    player.RenderEntity();

                    foreach (var enemy in enemies) {
                        enemy.RenderEntity();
                    }

                    foreach (var shot in playerShots) {
                        shot.RenderEntity();
                    }

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
            var lvec = new Vec2F(-0.005f, 0f);
            var rvec = new Vec2F(0.005f, 0f);
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                        "", ""));
                break;
            case "KEY_A":
                player.Direction(lvec);
                break;
            case "KEY_D":
                player.Direction(rvec);
                break;
            case "KEY_LEFT":
                player.Direction(lvec);
                break;
            case "KEY_RIGHT":
                player.Direction(rvec);
                break;
            case "KEY_SPACE":
                player.AddShots();
                break;
            }
        }

        public void KeyRelease(string key) {
            var nVec = new Vec2F(0.0f, 0.0f);
            switch (key) {
            case "KEY_A":
                player.Direction(nVec);
                break;
            case "KEY_D":
                player.Direction(nVec);
                break;
            case "KEY_LEFT":
                player.Direction(nVec);
                break;
            case "KEY_RIGHT":
                player.Direction(nVec);
                break;
            }
        }
    }
}