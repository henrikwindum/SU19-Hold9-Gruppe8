using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_3._1 {
    public class Player : Entity {

        public Player(Shape shape, IBaseImage image) : base(shape, image) {}
        
        public void Move() {
            if (this.Shape.AsDynamicShape().Position.X +
                this.Shape.AsDynamicShape().Direction.X <= 0.9f &&
                this.Shape.AsDynamicShape().Position.X +
                this.Shape.AsDynamicShape().Direction.X >= 0.0f) {
                this.Shape.Move();
            }
        }
    }
}