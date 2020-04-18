using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel1 {

    public class Evaluator {
        private readonly int kFold;
        private readonly List<Rating> dataset;
        private readonly BasePredictor predictor;
        private readonly float epsilon;

        public Evaluator(int kFold, List<Rating> dataset, BasePredictor predictor, float epsilon) {
            this.kFold = kFold;
            this.dataset = dataset;
            this.predictor = predictor;
            this.epsilon = epsilon;
        }

        public void Perform() {
            float overall = 0;

            int foldSize = (int)Math.Ceiling((float)dataset.Count / kFold);
            for (int k = 0; k < kFold; k++) {
                Console.WriteLine("Fold" + (k + 1));
                var parts = SplitList(dataset, foldSize);
                var trainData = new List<Rating>();
                var testData = parts[k];

                for (int i = 0; i < kFold; i++) {
                    if (i != k) {
                        trainData.AddRange(parts[i]);
                    }
                }

                predictor.Train(trainData);

                int successCount = 0;

                foreach (var rating in testData) {
                    int userId = rating.UserId;
                    int itemId = rating.ItemId;

                    float prediction;

                    try {
                        prediction = predictor.Predict(userId, itemId);
                    }
                    catch (PredictionException) {
                        prediction = 0;
                    }

                    if (Math.Abs(1 - prediction) < epsilon) {
                        successCount++;
                    }
                }

                float current = (100.0f * successCount / testData.Count);
                overall += current;
                Console.WriteLine("R: %" + current);
                Console.WriteLine();
            }

            overall /= kFold;

            Console.WriteLine("Overall");
            Console.WriteLine("R: %" + (int)overall);
        }

        private static List<List<T>> SplitList<T>(List<T> list, int nSize) {
            var res = new List<List<T>>();
            for (int i = 0; i < list.Count; i += nSize) {
                res.Add(list.GetRange(i, Math.Min(nSize, list.Count - i)));
            }
            return res;
        }
    }
}