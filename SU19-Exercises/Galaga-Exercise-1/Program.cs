namespace Galaga_Exercise_1 {
    internal class Program {
        public static void Main(string[] args) {
            var game = new Game();

            game.AddEnemies(0.05f, 0.9f);
            game.AddEnemies(0.15f, 0.9f);
            game.AddEnemies(0.25f, 0.9f);
            game.AddEnemies(0.35f, 0.9f);
            game.AddEnemies(0.45f, 0.9f);
            game.AddEnemies(0.55f, 0.9f);
            game.AddEnemies(0.65f, 0.9f);
            game.AddEnemies(0.75f, 0.9f);
            game.AddEnemies(0.85f, 0.9f);

            game.GameLoop();
        }
    }
}