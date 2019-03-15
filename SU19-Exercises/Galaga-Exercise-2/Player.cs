using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Player : IGameEventProcessor<object> {
        private Game game;
        public Entity player;

        public Player(Game game) {
            player = new Entity(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            this.game = game;
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Parameter1) {
                case "Moving_Left":
                    var vec = new Vec2F(-0.005f, 0.0f);
                    Direction(vec);
                    break;
                case "Moving_Right":
                    var vec2 = new Vec2F(0.005f, 0.0f);
                    Direction(vec2);
                    break;
                case "No_Movement":
                    var vec3 = new Vec2F(0.0f, 0.0f);
                    Direction(vec3);
                    break;
                }
            }
        }

        private void Direction(Vec2F vec) {
            player.Shape.AsDynamicShape().Direction = vec;
        }

        public void Move() {
            if (player.Shape.AsDynamicShape().Position.X +
                player.Shape.AsDynamicShape().Direction.X <= 0.9f &&
                player.Shape.AsDynamicShape().Position.X +
                player.Shape.AsDynamicShape().Direction.X >= 0.0f) {
                player.Shape.Move();
            }
        }

        public void AddShots() {
            var shot = new PlayerShot(game,
                new DynamicShape(
                    new Vec2F(player.Shape.Position.X + 0.046f, player.Shape.Position.Y),
                    new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(shot);
        }
    }
}