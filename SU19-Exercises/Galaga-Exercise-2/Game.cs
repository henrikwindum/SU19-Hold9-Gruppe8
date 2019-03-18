using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_2.MovementStrategy;
using Galaga_Exercise_2.Squadrons;

namespace Galaga_Exercise_2 {
    public class Game : IGameEventProcessor<object> {
        private GameEventBus<object> eventBus;
        private int explosionLength = 500;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private GameTimer gameTimer;
        private ISquadron monsters;
        private ISquadron monsters2;
        private IMovementStrategy zigzagdown;
        private IMovementStrategy movedown;
        private Player player;
        private Score score;
        private Window win;

        public Game() {
            win = new Window("Galaga", 500, 500);
            player = new Player(this);
            List<Image> enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));

            List<Image> enemyStrides2 = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "GreenMonster.png"));
            
            // Wave 1
            monsters = new Parallel(4);
            monsters.CreateEnemies(enemyStrides);

            zigzagdown = new ZigZagDown();

            // Wave 2
            monsters2 = new Triangle(4);
            monsters2.CreateEnemies(enemyStrides2);

            movedown = new MoveDown();
                
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            gameTimer = new GameTimer(60, 60);

            playerShots = new List<PlayerShot>();
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(10);

            score = new Score(new Vec2F(0.8f, 0.5f),
                new Vec2F(0.2f, 0.2f));
        }

        public List<PlayerShot> playerShots { get; set; }

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

        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                // Collision detection for 1. wave                
                if (monsters.Enemies.CountEntities() != 0) {
                    foreach (Entity enemy in monsters.Enemies) {
                        switch (CollisionDetection.Aabb((DynamicShape) shot.Shape, enemy.Shape)
                            .Collision) {
                        case true:
                            AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                                0.1f, 0.1f);
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            break;
                        }
                    }

                    // Makes sure that PlayerShot gets deleted when colliding
                    var newShots = new List<PlayerShot>();
                    foreach (var deleteshot in playerShots) {
                        if (!deleteshot.IsDeleted()) {
                            newShots.Add(deleteshot);
                        }
                    }
                    playerShots = newShots;

                    // Makes sure that Enemy gets deleted when colliding
                    var newEnemies = new EntityContainer<Enemy>();
                    foreach (Enemy enemy in monsters.Enemies) {
                        if (!enemy.IsDeleted()) {
                            newEnemies.AddDynamicEntity(enemy);
                        }

                        if (enemy.IsDeleted()) {
                            score.AddPoints();
                        }
                    }
                    monsters.Enemies = newEnemies;    
                }
                
                // Collision detection for 2. wave 
                if (monsters.Enemies.CountEntities() == 0) {
                    foreach (Entity enemy in monsters2.Enemies) {
                        switch (CollisionDetection.Aabb((DynamicShape) shot.Shape, enemy.Shape)
                            .Collision) {
                        case true:
                            AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                                0.1f, 0.1f);
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            break;
                        }
                    }

                    var newShots = new List<PlayerShot>();
                    foreach (var deleteshot in playerShots) {
                        if (!deleteshot.IsDeleted()) {
                            newShots.Add(deleteshot);
                        }
                    }
                    playerShots = newShots;

                    var newEnemies = new EntityContainer<Enemy>();
                    foreach (Enemy enemy in monsters2.Enemies) {
                        if (!enemy.IsDeleted()) {
                            newEnemies.AddDynamicEntity(enemy);
                        }

                        if (enemy.IsDeleted()) {
                            score.AddPoints();
                        }
                    }
                    monsters2.Enemies = newEnemies;    
                }
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    // Update game logic here
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    
                    player.Move();
                    
                    // Adding movement to player entities
                    zigzagdown.MoveEnemies(monsters.Enemies);
                    if (monsters.Enemies.CountEntities() == 0) {
                        movedown.MoveEnemies(monsters2.Enemies);    
                    }
                    IterateShots();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();

                    // Render gameplay entities here
                    player.player.RenderEntity();

                    foreach (Entity entity in monsters.Enemies) {
                        entity.RenderEntity();
                    }

                    if (monsters.Enemies.CountEntities() == 0) {
                        foreach (Entity wave2 in monsters2.Enemies) {
                            wave2.RenderEntity();
                        }
                    }

                    foreach (var shot in playerShots) {
                        shot.RenderEntity();
                    }

                    explosions.RenderAnimations();
                    score.RenderScore();
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
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                        "", ""));
                break;
            case "KEY_A":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "",
                        "Moving_Left", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "",
                        "Moving_Left", ""));
                break;
            case "KEY_D":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "",
                        "Moving_Right", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "",
                        "Moving_Right", ""));
                break;
            case "KEY_SPACE":
                player.AddShots();
                break;
            }
        }

        public void KeyRelease(string key) {
            switch (key) {
            case "KEY_D":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "", "No_Movement", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "", "No_Movement", ""));
                break;
            case "KEY_A":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "", "No_Movement", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, player, "", "No_Movement", ""));
                break;
            }
        }
    }
}