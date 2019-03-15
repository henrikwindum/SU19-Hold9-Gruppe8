using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;

        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
        }

        public void Direction(Vec2F vec) {
            Shape.AsDynamicShape().Direction = vec;
        }

        public void Move() {
            if (Shape.AsDynamicShape().Position.X + Shape.AsDynamicShape().Direction.X <= 0.9f &&
                Shape.AsDynamicShape().Position.X + Shape.AsDynamicShape().Direction.X >= 0.0f) {
                Shape.Move();
            }
        }

        public void AddShots() {
            var shot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + 0.046f, Shape.Position.Y),
                    new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(shot);
        }
    }
}