using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Channel1;

namespace Recommender.Api {

    public class Channel1Service {
        private CosinePredictor predictor;

        public Channel1Service() {
            this.predictor = new CosinePredictor(5, 3);
        }

        public void Train(IEnumerable<Rating> ratings) {
            var newPredictor = new CosinePredictor(5, 3);
            newPredictor.Train(ratings);
            predictor = newPredictor;
        }

        public float Predict(int userId, int itemId) {
            return predictor.Predict(userId, itemId);
        }

        public int[] Recommend(int userId, int count) {
            var list = new List<ValueTuple<int, float>>();
            var all = predictor.GetItemsAsList();

            for (int i = 0; i < all.Count; i++) {
                list.Add(new ValueTuple<int, float>(all[i], Predict(userId, all[i])));
            }

            list.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            return list.Take(count).Select(x => x.Item1).ToArray();
        }

        public string Info() {
            return predictor.DebugInfo;
        }
    }
}
