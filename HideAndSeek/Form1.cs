using System.Threading;
using System.Windows.Forms;

namespace HideAndSeek
{
    public partial class Form1 : Form
    {
        private OutsideWithDoor _frontYard;
        private OutsideWithDoor _backYard;
        private OutsideWithHidingPlace _garden;
        private RoomWithDoor _livingRoom;
        private RoomWithDoor _kitchen;
        private Room _diningRoom;
        private Room _stairs;
        private RoomWithHidingPlace _upstairsHallway;
        private RoomWithHidingPlace _masterBedroom;
        private RoomWithHidingPlace _secondBedroom;
        private RoomWithHidingPlace _bathroom;
        private OutsideWithHidingPlace _driveway;

        private Location _currentLocation;
        private Opponent _opponent;

        private int _moves;

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
            _garden = new OutsideWithHidingPlace("Garden", false, "Shed");
            _livingRoom = new RoomWithDoor("Living Room", "Antique Carpet", "Behind the TV", "An Oak Door With A Brass Knob");
            _kitchen = new RoomWithDoor("Kitchen", "Stainless Steel appliances", "Refrigerator", "A Screen Door");
            _diningRoom = new Room("Dining Room", "Crystal Chandelier");
            _stairs = new Room("Stairs", "Wooden banister");
            _upstairsHallway = new RoomWithHidingPlace("Upstairs Hallway", "Picture of a Dog", "Closet");
            _masterBedroom = new RoomWithHidingPlace("Master Bedroom", "Large Bed", "Under the Bed");
            _secondBedroom = new RoomWithHidingPlace("Second Bedroom", "Small Bed", "Under the Bed");
            _bathroom = new RoomWithHidingPlace("Bathroom", "Sink and Toilet", "Shower");
            _driveway = new OutsideWithHidingPlace("Driveway", true, "Garage");

            _frontYard.Exits = new Location[] {_backYard, _garden, _driveway};
            _backYard.Exits = new Location[] {_frontYard, _garden, _driveway};
            _garden.Exits = new Location[] {_frontYard, _backYard};
            _livingRoom.Exits = new Location[] {_diningRoom, _stairs};
            _kitchen.Exits = new Location[] {_diningRoom};
            _diningRoom.Exits = new Location[] {_livingRoom, _kitchen};
            _stairs.Exits = new Location[] {_livingRoom, _upstairsHallway};
            _upstairsHallway.Exits = new Location[] {_stairs, _masterBedroom, _secondBedroom, _bathroom};
            _masterBedroom.Exits = new Location[] {_upstairsHallway};
            _secondBedroom.Exits = new Location[] {_upstairsHallway};
            _bathroom.Exits = new Location[] {_upstairsHallway};
            _driveway.Exits = new Location[] {_frontYard, _backYard};

            _frontYard.DoorLocation = _livingRoom;
            _livingRoom.DoorLocation = _frontYard;
            _backYard.DoorLocation = _kitchen;
            _kitchen.DoorLocation = _backYard;

            _opponent = new Opponent(_frontYard);
        }

        public void MoveToANewLocation(Location newLocation)
        {
            _currentLocation = newLocation;
            RedrawForm();
        }

        private void goHere_Click(object sender, System.EventArgs e)
        {
            _moves++;
            MoveToANewLocation(_currentLocation.Exits[exits.SelectedIndex]);
        }

        private void goThroughTheDoor_Click(object sender, System.EventArgs e)
        {
            _moves++;
            if (_currentLocation is IHasExteriorDoor currentDoorLocation)
                MoveToANewLocation(currentDoorLocation.DoorLocation);
        }

        private void check_Click(object sender, System.EventArgs e)
        {
            _moves ++;
            if (_opponent.Check(_currentLocation) && (_currentLocation is IHidingPlace))
            {
                var hidingPlace = (IHidingPlace)_currentLocation;
                MessageBox.Show($@"You found me in {_moves} moves!");
                ResetGame();
            }
            else
            {
                description.Text = @"Not here!";
                Application.DoEvents();
                Thread.Sleep(2000);
                description.Text = _currentLocation.Description;
            }
            
        }

        public void ResetGame()
        {
            _moves = 0;
            _opponent.Reset();
            MoveToANewLocation(_frontYard);
        }

        public void RedrawForm()
        {
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

            if (_currentLocation is IHidingPlace)
            {
                var currentLocationsHidingPlace = (IHidingPlace)_currentLocation;
                check.Enabled = true;
                check.Text = $@"Check {currentLocationsHidingPlace.HidingPlace}";
            }
            else
            {
                check.Enabled = false;
            }

            if (!_opponent.Hidden)
            {
                goHere.Enabled = false;
                exits.Enabled = false;
                goThroughTheDoor.Enabled = false;
                check.Enabled = false;
                description.Text = "";
                hide.Enabled = true;
            }
            else
            {
                hide.Enabled = false;
                goHere.Enabled = true;
                exits.Enabled = true;
            }

        }

        private void hide_Click(object sender, System.EventArgs e)
        {
            hide.Enabled = false;

            for (int i = 1; i < 11; i++)
            {
                _opponent.Move();
                description.Text = $@"{i}...";
                Application.DoEvents();
                Thread.Sleep(1000);
            }

            description.Text = @"Ready or not here I come!";
            Application.DoEvents();
            Thread.Sleep(2000);
            RedrawForm();
        }
    }
}
