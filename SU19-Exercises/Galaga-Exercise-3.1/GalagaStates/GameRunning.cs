using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using Galaga_Exercise_3._1.GalagaEntities;
using Galaga_Exercise_3._1.MovementStrategy;
using Galaga_Exercise_3._1.Squadrons;

namespace Galaga_Exercise_3._1.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private ISquadron monsters;
        private ISquadron monsters2;
        private IMovementStrategy zigzagdown;
        private Player player;
        private IMovementStrategy movedown;

        private int explosionLength = 500;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;

        private Score score;
        
        public GameRunning(){
            player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f),
                        new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            var enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            
            var enemyStrides2 = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "GreenMonster.png"));
            
            monsters = new Parallel(4);
            monsters.CreateEnemies(enemyStrides);
            
            monsters2 = new Triangle(4);
            monsters2.CreateEnemies(enemyStrides2);
            
            zigzagdown = new ZigZagDown();
            movedown = new MoveDown();
            
            playerShots = new List<PlayerShot>();
            
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(10);
            
            score = new Score(new Vec2F(0.8f, 0.5f),
                new Vec2F(0.2f, 0.2f));
        }
        
        public List<PlayerShot> playerShots { get; set; }
        
        public void AddShots() {
            var shot = new PlayerShot(new DynamicShape(
                    new Vec2F(player.Shape.Position.X + 0.046f, player.Shape.Position.Y + 0.08f),
                    new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            playerShots.Add(shot);
        }
        
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
        }

        public void UpdateGameLogic() {
            zigzagdown.MoveEnemies(monsters.Enemies);
            player.Move();
            IterateShots();
            if (monsters.Enemies.CountEntities() == 0) {
                 movedown.MoveEnemies(monsters2.Enemies);
            }
        }

        public void RenderState() {
            foreach (Entity entity in monsters.Enemies) {
                entity.RenderEntity();
            }
            player.RenderEntity();
            
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
        
        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
        
        private void KeyPress(string key) {}

        private void Direction(Vec2F vec) {
            player.Shape.AsDynamicShape().Direction = vec;
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            var vec3 = new Vec2F(0.0f,0.0f);
            switch (keyValue) {
            case "KEY_P":
                GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors
                (GameEventType.GameStateEvent, this,
                    "CHANGE_STATE", "GAME_PAUSED", ""));
                break;
            case "KEY_A":
            case "KEY_LEFT":
                if (keyAction == "KEY_PRESS") {
                    var vec = new Vec2F(-0.005f,0.0f);
                    Direction(vec);    
                } else if (keyAction == "KEY_RELEASE") {
                    Direction(vec3);
                }
            break;
            case "KEY_D":
            case "KEY_RIGHT":
                if (keyAction == "KEY_PRESS") {
                    var vec2 = new Vec2F(0.005f,0.0f);
                    Direction(vec2);    
                } else if (keyAction == "KEY_RELEASE") {
                    Direction(vec3);
                }
                break;
            case "KEY_SPACE":
                if (keyAction == "KEY_PRESS") {
                    AddShots();    
                }
                break;
            }
        }
    }
}