using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    public class Player : Entity {
        private Game game;
        public Entity player;

        public Player(Shape shape, IBaseImage image) : base(shape, image) {}
/*
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.InputEvent) {
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
*/
        public void Move() {
            if (this.Shape.AsDynamicShape().Position.X +
                this.Shape.AsDynamicShape().Direction.X <= 0.9f &&
                this.Shape.AsDynamicShape().Position.X +
                this.Shape.AsDynamicShape().Direction.X >= 0.0f) {
                this.Shape.Move();
            }
        }
/*
        public void AddShots() {
            var shot = new PlayerShot(game,
                new DynamicShape(
                    new Vec2F(player.Shape.Position.X + 0.046f, player.Shape.Position.Y),
                    new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(shot);
        }*/
    }
}