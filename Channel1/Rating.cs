using System;
using System.Collections.Generic;
using System.Text;

namespace Channel1 {

    public struct Rating {
        public int UserId { get; set; }
        public int ItemId { get; set; }

        public Rating(int userId, int itemId) {
            UserId = userId;
            ItemId = itemId;
        }
    }

}