using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel1 {

    public abstract class BasePredictor {
        private Dictionary<int, HashSet<int>> userItems;
        private HashSet<int> userIds;
        private HashSet<int> itemIds;
        private Dictionary<int, int[]> userNeighbors;
        private Dictionary<UserPair, float> userSimilarities;
        private int[] userIdsArray;
        private int K, L;

        public BasePredictor(int K, int L) {
            this.K = K;
            this.L = L;
        }

        public void Train(IEnumerable<Rating> ratings) {
            Console.WriteLine("Training...");

            userItems = new Dictionary<int, HashSet<int>>();
            foreach (var rating in ratings) {
                int user = rating.UserId;
                int item = rating.ItemId;

                if (!userItems.ContainsKey(user))
                    userItems[user] = new HashSet<int>();
                userItems[user].Add(item);
            }

            userIds = new HashSet<int>();
            foreach (var userId in userItems.Keys) {
                userIds.Add(userId);
            }

            itemIds = new HashSet<int>();
            foreach (var items in userItems.Values) {
                foreach (var itemId in items) {
                    itemIds.Add(itemId);
                }
            }

            userIdsArray = new int[userIds.Count];
            int i = 0;
            foreach (var userId in userIds) {
                userIdsArray[i] = userId;
                i++;
            }

            ComputeSimilarity();
            userSimilarities = new Dictionary<UserPair, float>();
            foreach (var user1 in userIds) {
                foreach (var user2 in userIds) {
                    if (user1 == user2) continue;
                    var pairId = PairId(user1, user2);
                    if (userSimilarities.ContainsKey(pairId))
                        continue;
                    userSimilarities.Add(pairId, GetSimilarity(user1, user2));
                }
                Console.Write((int)((float)userSimilarities.Count / (userIds.Count * userIds.Count - userIds.Count) * 100) + "%");
            }

            userNeighbors = new Dictionary<int, int[]>();
            foreach (var userId in userIds) {
                userNeighbors[userId] = MostSimilarUsers(userId);
                Console.Write((int)((float)userNeighbors.Count / userIds.Count * 50 + 50) + "%");
            }

            Console.WriteLine();
        }

        protected abstract void ComputeSimilarity();

        private int[] MostSimilarUsers(int userId) {
            var allUsers = userIdsArray.ToArray();
            Array.Sort(allUsers, (int user1, int user2) => {
                float sim1 = userSimilarities.GetValueOrDefault(PairId(userId, user1), 0);
                float sim2 = userSimilarities.GetValueOrDefault(PairId(userId, user2), 0);
                return sim1.CompareTo(sim2);
            });
            Array.Reverse(allUsers);
            return allUsers.Take(Math.Min(K, allUsers.Length)).ToArray();
        }

        protected UserPair PairId(int id1, int id2) {
            return new UserPair(id1, id2);
        }

        protected HashSet<int> GetUserItems(int userId) {
            return userItems[userId];
        }

        protected abstract float GetSimilarity(int user1, int user2);

        public float Predict(int userId, int itemId) {
            if (!userIds.Contains(userId)) {
                throw new PredictionException("user not found");
            }
            //if (userItems[userId].Contains(itemId)) {
            //    throw new PredictionException("already rated");
            //}

            int[] neighbors = userNeighbors.GetValueOrDefault(userId, null);
            if (neighbors.Length < L) {
                throw new PredictionException("user neighbors is not sufficient: " + neighbors.Length + "<" + L);
            }

            float count = 0;
            float all = 0;

            for (int i = 0; i < neighbors.Length; i++) {
                int user = neighbors[i];

                float factor = (neighbors.Length - i) * (neighbors.Length - i);

                if (userItems[user].Contains(itemId))
                    count += factor;
                all += factor;
            }

            return count / all;
        }

        public List<int> GetItemsAsList() {
            var list = new List<int>();
            foreach (var item in itemIds) {
                list.Add(item);
            }
            return list;
        }

        protected int[] GetAllUsers() {
            return userIdsArray;
        }
    }
}
