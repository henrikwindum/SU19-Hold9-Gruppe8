using DIKUArcade.Entities;

namespace Galaga_Exercise_2.MovementStrategy {
    public class MoveDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Move(0.0f, -0.0003f);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}