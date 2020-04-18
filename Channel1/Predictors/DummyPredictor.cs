using System;
using System.Collections.Generic;
using System.Text;

namespace Channel1 {
    public class DummyPredictor : BasePredictor {

        public DummyPredictor(int K, int L) : base(K, L) {

        }

        protected override void ComputeSimilarity() {

        }

        protected override float GetSimilarity(int user1, int user2) {
            return 0;
        }
    }
}