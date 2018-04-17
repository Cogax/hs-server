using System;
namespace ShoppingListApi.Models
{
    public class ShoppingListItem
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public bool IsComplete { get; set; }
    }
}
