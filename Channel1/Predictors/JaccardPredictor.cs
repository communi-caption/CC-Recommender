using System;
using System.Collections.Generic;
using System.Text;

namespace Channel1 {


    public class JaccardPredictor : BasePredictor {

        public JaccardPredictor(int K, int L) : base(K, L) {

        }

        protected override void ComputeSimilarity() {

        }

        protected override float GetSimilarity(int user1, int user2) {
            var items1 = GetUserItems(user1);
            var items2 = GetUserItems(user2);

            var allItems = new HashSet<int>();
            foreach (var item in items1) allItems.Add(item);
            foreach (var item in items2) allItems.Add(item);

            var sameItems = new HashSet<int>();
            foreach (var item in items1) {
                if (items2.Contains(item)) {
                    sameItems.Add(item);
                }
            }

            return ((float)sameItems.Count) / allItems.Count;
        }
    }
}