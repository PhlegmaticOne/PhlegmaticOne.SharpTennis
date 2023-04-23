using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Table
{
    public class TennisTable : MeshableObject
    {
        public TableTopPart TableTopPart { get; }
        public TableNet TableNet { get; }

        public TennisTable(TableTopPart tableTopPart, TableNet tableNet)
        {
            TableTopPart = tableTopPart;
            TableNet = tableNet;
        }
    }
}