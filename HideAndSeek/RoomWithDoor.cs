namespace HideAndSeek
{
    public class RoomWithDoor : RoomWithHidingPlace, IHasExteriorDoor
    {
        public RoomWithDoor(string name, string decoration, string hidingPlace, string doorDescription) 
            : base(name, decoration, hidingPlace)
        {
            DoorDescription = doorDescription;
        }

        public string DoorDescription { get; }
        public Location DoorLocation { get; set; }

        public override string Description
        {
            get { return base.Description + $"There is {DoorDescription}."; }
        }
    }
}