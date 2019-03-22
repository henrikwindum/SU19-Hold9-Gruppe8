using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3._1 {
    public class Score {
        private Text display;
        private int score;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        public void AddPoints() {
            score += 100;
        }

        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }
    }
}