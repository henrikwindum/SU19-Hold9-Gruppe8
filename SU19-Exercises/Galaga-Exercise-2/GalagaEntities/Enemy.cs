using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Enemy : Entity {
        public Enemy(DynamicShape shape, ImageStride image)
            : base(shape, image) {
            StartPosition = shape.Position.Copy();
        }

        public Vec2F StartPosition { get; }
    }
}