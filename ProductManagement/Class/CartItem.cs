using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Class
{
    public class CartItem
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string DVT { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string Hinh { get; set; }
    }
    public static class CartService
    {

        public static readonly List<CartItem> _items = new List<CartItem>();
        public static IReadOnlyList<CartItem> Items => _items;
        public static void AddItem(CartItem item)
        {
            var hientai = Items.FirstOrDefault(x => x.MaSP == item.MaSP);
            if(hientai != null)
            {
                hientai.SoLuong += item.SoLuong;
            }
            else
            {
                _items.Add(item);
            }
        }
        public static void RemoveItem(string MaSP)
        {
            var muctieu = _items.FirstOrDefault(x => x.MaSP == MaSP);
            if(muctieu!=null)
            {
                _items.Remove(muctieu);
            }
        }
        public static void Clear()
        {
            _items.Clear();
        }
        public static decimal GetTotal()
        {
            return _items.Sum(x => x.Gia * x.SoLuong);
        }
    }
}
