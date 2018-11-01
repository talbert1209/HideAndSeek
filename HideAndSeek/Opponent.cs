using System;
using System.Runtime.CompilerServices;

namespace HideAndSeek
{
    public class Opponent
    {
        private Location myLocation;
        private readonly Random random;

        public Opponent(Location startingLocation)
        {
            myLocation = startingLocation;
            random = new Random();
        }

        public void Move()
        {
            if (myLocation is IHasExteriorDoor)
            {
                var coinFlip = random.Next(2);
                if (coinFlip == 1)
                {
                    var myLocationWithDoor = (IHasExteriorDoor) myLocation;
                    myLocation = myLocationWithDoor.DoorLocation;
                }
            }

            myLocation = myLocation.Exits[random.Next(myLocation.Exits.Length)];
            while (!(myLocation is IHidingPlace))
            {
                myLocation = myLocation.Exits[random.Next(myLocation.Exits.Length)];
            }
        }

        public bool Check(Location location)
        {
            return location == myLocation;
        }

    }
}