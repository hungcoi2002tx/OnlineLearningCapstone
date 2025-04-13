using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.ResponseModel
{
    public class PagedResult<T>
    {
        public int Total { get; set; }              // Tổng số bản ghi
        public int Index { get; set; }               // Trang hiện tại
        public int Size { get; set; }           // Số lượng / trang
        public IEnumerable<T> Data { get; set; } = new List<T>();  // Dữ liệu
    }
}
