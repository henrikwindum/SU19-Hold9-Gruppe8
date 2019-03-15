using DIKUArcade.Entities;

namespace Galaga_Exercise_2.MovementStrategy {
    public class NoMove : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Move(0.0f, 0.0f);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}