using System;

namespace Channel1 {
    public class RandomPredictor : BasePredictor {
        private readonly Random random;

        public RandomPredictor(int K, int L) : base(K, L) {
            this.random = new Random();
        }

        protected override void ComputeSimilarity() {

        }
        protected override float GetSimilarity(int user1, int user2) {
            return (float)random.NextDouble();
        }
    }
}