using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace ConsoleApp5 {
    class PicRating {

        [LoadColumn(0)]
        public float userID2 { get; set; }
        [LoadColumn(1)]
        public float venID { get; set; }
        [LoadColumn(2)]
        public float Label { get; set; }
    }
}
