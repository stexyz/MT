using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MT_REST_API.Models;

namespace MT_REST_API.HttpStack {
    public static class CsvParser {
        public static IEnumerable<DetailedOrderRow> ParseCsv(string csv) {
            List<DetailedOrderRow> result = new List<DetailedOrderRow>();
            foreach (string s in csv.Split('\n').Skip(1)) {
                string[] cols = s.Split(';');
                string orderCode = cols[0];
                string amount = cols[1];
                string productCode = cols[2];
                string productName = cols[3];
                string prize = cols[6];
                DetailedOrderRow row = new DetailedOrderRow(orderCode, amount, productCode, productName, prize);
                result.Add(row);
            }
            return result;
        }

        public static Dictionary<string, decimal?> ParsePurchasePrices(string csv) {
            Dictionary<string, decimal?> result = new Dictionary<string, decimal?>();
            foreach (string s in csv.Split('\n').Skip(1)) {
                string[] cols = s.Split(';');
                string productCode = cols[0];
                decimal prize;
                if (cols.Count() == 2 && decimal.TryParse(cols[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out prize)) {
                    result[productCode] = prize;
                } else {
                    result[productCode] = null;
                }
            }
            return result;
        }

        // TODO: check the col nums!!
        private static int StockCsvProductCodeIndex = 0;
        private static int StockCsvStockCountIndex = 0;
        public static Dictionary<string, decimal?> ParseStockCsv(string csv) {
            Dictionary<string, decimal?> result = new Dictionary<string, decimal?>();
            foreach (string s in csv.Split('\n').Skip(1)) {
                string[] cols = s.Split(';');
                string productCode = cols[StockCsvProductCodeIndex];
                decimal stockCount;
                if (cols.Count() == 2 && decimal.TryParse(cols[StockCsvStockCountIndex].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out stockCount)) {
                    if (!result.ContainsKey(productCode)) {
                        result[productCode] = 0;
                    }
                    result[productCode] += stockCount;
                } else {
                    result[productCode] = null;
                }
            }
            throw new NotImplementedException("TODO");
            //            return result;
        }

        public static Dictionary<string, List<string>> ParseStockCategoriesCsv(string csv) {
            throw new NotImplementedException("TODO");
        }

    }
}