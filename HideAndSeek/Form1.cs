using System.Windows.Forms;

namespace HideAndSeek
{
    public partial class Form1 : Form
    {
        private OutsideWithDoor _frontYard;
        private OutsideWithDoor _backYard;
        private Outside _garden;
        private RoomWithDoor _livingRoom;
        private RoomWithDoor _kitchen;
        private Room _diningRoom;

        private Location _currentLocation;

        public Form1()
        {
            InitializeComponent();
            CreateObjects();
            MoveToANewLocation(_frontYard);
        }

        public void CreateObjects()
        {
            _frontYard = new OutsideWithDoor("Front Yard", false, "An Oak Door With A Brass Knob");
            _backYard = new OutsideWithDoor("Back Yard", true, "A Screen Door");
            _garden = new Outside("Garden", false);
            _livingRoom = new RoomWithDoor("Living Room", "Antique Carpet", "Hiding Placeholder", "An Oak Door With A Brass Knob");
            _kitchen = new RoomWithDoor("Kitchen", "Stainless Steel appliances", "Hiding Placeholder", "A Screen Door");
            _diningRoom = new Room("Dining Room", "Crystal Chandelier");

            _frontYard.Exits = new Location[] {_backYard, _garden};
            _backYard.Exits = new Location[] {_frontYard, _garden};
            _garden.Exits = new Location[] {_frontYard, _backYard};
            _livingRoom.Exits = new Location[] {_diningRoom};
            _kitchen.Exits = new Location[] {_diningRoom};
            _diningRoom.Exits = new Location[] {_livingRoom, _kitchen};

            _frontYard.DoorLocation = _livingRoom;
            _livingRoom.DoorLocation = _frontYard;
            _backYard.DoorLocation = _kitchen;
            _kitchen.DoorLocation = _backYard;
        }

        public void MoveToANewLocation(Location newLocation)
        {
            _currentLocation = newLocation;
            exits.Items.Clear();
            foreach (var exit in _currentLocation.Exits)
            {
                exits.Items.Add(exit.Name);
            }

            exits.SelectedIndex = 0;

            description.Text = _currentLocation.Description;

            if (_currentLocation is IHasExteriorDoor)
            {
                goThroughTheDoor.Enabled = true;
            }
            else
            {
                goThroughTheDoor.Enabled = false;
            }
        }

        private void goHere_Click(object sender, System.EventArgs e)
        {
            MoveToANewLocation(_currentLocation.Exits[exits.SelectedIndex]);
        }

        private void goThroughTheDoor_Click(object sender, System.EventArgs e)
        {
            if (_currentLocation is IHasExteriorDoor currentDoorLocation)
                MoveToANewLocation(currentDoorLocation.DoorLocation);
        }

        private void check_Click(object sender, System.EventArgs e)
        {

        }

        private void hide_Click(object sender, System.EventArgs e)
        {

        }
    }
}
