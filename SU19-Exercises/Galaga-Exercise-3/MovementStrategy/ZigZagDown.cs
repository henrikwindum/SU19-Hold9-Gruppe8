using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;

namespace Galaga_Exercise_3.MovementStrategy {
    public class ZigZagDown : IMovementStrategy{
        public void MoveEnemy(Enemy enemy) {
            var yi = enemy.Shape.Position.Y - 0.00039f;
            var xi = enemy.StartPosition.X + 0.05f *
                     (float) Math.Sin(2 * Math.PI * (enemy.StartPosition.Y - yi) / 0.045f);

            enemy.Shape.SetPosition(new Vec2F(xi, yi));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}