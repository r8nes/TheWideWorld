namespace TheWideWorld.Adventures.Models
{
    public class Objective
    {
        public ObjectType ObjectType;
    }

    public enum ObjectType { 
        MonsterInRoom,
        AllMonsters,
        ItemObtained
    }
}
