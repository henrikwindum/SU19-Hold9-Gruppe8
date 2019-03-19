using DIKUArcade.Entities;
using Galaga_Exercise_3.GalagaEntities;

namespace Galaga_Exercise_3.MovementStrategy {
    public class MoveDown : IMovementStrategy{
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Move(0.0f, -0.005f);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}