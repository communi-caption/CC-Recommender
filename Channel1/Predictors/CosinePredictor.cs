using System;
using System.Collections.Generic;
using System.Text;

namespace Channel1 {

    public class CosinePredictor : BasePredictor {

        private Dictionary<int, float[]> vectorCache;

        public CosinePredictor(int K, int L) : base(K, L) {

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
            return GetCosineSimilarity(vectorCache[user1], vectorCache[user2]);
        }

        public static float GetCosineSimilarity(float[] V1, float[] V2) {
            int N = ((V2.Length < V1.Length) ? V2.Length : V1.Length);
            double dot = 0.0d;
            double mag1 = 0.0d;
            double mag2 = 0.0d;
            for (int n = 0; n < N; n++) {
                dot += V1[n] * V2[n];
                mag1 += Math.Pow(V1[n], 2);
                mag2 += Math.Pow(V2[n], 2);
            }
            return (float)(dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));
        }
    }
}