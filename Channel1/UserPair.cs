using System;

namespace Channel1 {

    public struct UserPair {
        public int User1 { get; private set; }
        public int User2 { get; private set; }

        public UserPair(int user1, int user2) {
            this.User1 = user1;
            this.User2 = user2;
        }

        private static ValueTuple<int, int> T(UserPair userPair) {
            int user1 = userPair.User1;
            int user2 = userPair.User2;
            return user1 < user2 ? new ValueTuple<int, int>(user1, user2) : new ValueTuple<int, int>(user2, user1);
        }

        public override bool Equals(object obj) {
            if (obj is UserPair == false)
                return false;
            return T(this).Equals(T((UserPair)obj));
        }

        public override int GetHashCode() {
            return T(this).GetHashCode();
        }

        public override string ToString() {
            return T(this).ToString();
        }
    }
}