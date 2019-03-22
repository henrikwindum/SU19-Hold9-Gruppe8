using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_3._1 {
    public class Player : Entity {

        public Player(Shape shape, IBaseImage image) : base(shape, image) {}
        
        public void Move() {
            if (Shape.AsDynamicShape().Position.X +
                Shape.AsDynamicShape().Direction.X <= 0.9f &&
                Shape.AsDynamicShape().Position.X +
                Shape.AsDynamicShape().Direction.X >= 0.0f) {
                Shape.Move();
            }
        }
    }
}