using System;
using System.Collections.Generic;
using System.Text;

namespace Channel1 {

    public class EuclidPredictor : BasePredictor {

        private Dictionary<int, float[]> vectorCache;

        public EuclidPredictor(int K, int L) : base(K, L) {

        }

        protected override void ComputeSimilarity() {
            vectorCache = new Dictionary<int, float[]>();
            var allItems = GetItemsAsList();
            foreach (var user in GetAllUsers()) {
                var items = GetUserItems(user);
                var vector = new float[allItems.Count];
                for (int i = 0; i < allItems.Count; i++) {
                    vector[i] = items.Contains(allItems[i]) ? 1 : 0;
                }
                vectorCache.Add(user, vector);
            }
        }

        protected override float GetSimilarity(int user1, int user2) {
            return GetEuclidSimilarity(vectorCache[user1], vectorCache[user2]);
        }

        public static float GetEuclidSimilarity(float[] V1, float[] V2) {
            int N = ((V2.Length < V1.Length) ? V2.Length : V1.Length);
            float sum = 0.0f;
            for (int n = 0; n < N; n++) {
                sum += (V1[n] - V2[n]) * (V1[n] - V2[n]);
            }
            return (float)Math.Sqrt(sum);
        }
    }
}
