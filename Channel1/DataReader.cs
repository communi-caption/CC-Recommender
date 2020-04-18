using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Channel1 {

    public static class DataReader {

        public static List<Rating> ReadData(string filePath) {
            var dataset = File.ReadAllLines(filePath).Select(x => {
                var ss = x.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (int.Parse(ss[2]) < 2) {
                    return (Rating?)null;
                }

                var entry = new Rating();
                entry.UserId = int.Parse(ss[0]);
                entry.ItemId = int.Parse(ss[1]);
                return entry;
            }).Where(x => x != null).Select(x => x.Value).ToList();

            return dataset;
        }
    }
}