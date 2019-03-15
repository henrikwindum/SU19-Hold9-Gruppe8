using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_1 {
    public class Enemy : Entity {
        private Game game;

        public Enemy(Game game, DynamicShape shape, ImageStride image)
            : base(shape, image) {
            this.game = game;
        }
    }
}