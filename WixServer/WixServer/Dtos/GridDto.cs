using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WixServer.Dtos
{
    public class GridDto
    {
        public int Id { get; set; }
        public int XLen { get; set; }
        public int YLen { get; set; }
        public List<ItemDto> Items { get; set; }
    }

    public class ItemDto
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int XLen { get; set; }
        public int YLen { get; set; }
    }

    public class GridItemDto : ItemDto
    {
        public string Name { get; set; }
    }

    public class GridTableDto : ItemDto
    {
        public int TableNumber { get; set; }
        public bool SmokingAllowed { get; set; }
        public int MaxCapacity { get; set; }
    }


}