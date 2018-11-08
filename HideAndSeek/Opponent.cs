using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace HideAndSeek
{
    public class Opponent
    {
        private Location myLocation;
        private Random random;
        public bool Hidden { get; private set; }

        public Opponent(Location startingLocation)
        {
            myLocation = startingLocation;
            random = new Random();
            Hidden = false;
        }

        public void Reset()
        {
            Hidden = false;
        }

        public void Move()
        {

            var coinFlip = random.Next(2);

            if (myLocation is IHasExteriorDoor)
            {
                if (coinFlip == 1)
                {
                    var myLocationWithDoor = (IHasExteriorDoor)myLocation;
                    myLocation = myLocationWithDoor.DoorLocation;
                }
            }

            myLocation = myLocation.Exits[random.Next(myLocation.Exits.Length)];
            while (!(myLocation is IHidingPlace))
            {
                myLocation = myLocation.Exits[random.Next(myLocation.Exits.Length)];
            }

            Hidden = true;

        }

        public bool Check(Location location)
        {
            return location == myLocation;
        }

    }
}