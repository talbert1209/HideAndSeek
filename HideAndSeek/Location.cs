namespace HideAndSeek
{
    public abstract class Location
    {
        //Fields & Properties
        public string Name { get; private set; }
        public Location[] Exits;
        public virtual string Description
        {
            get
            {
                string description = $"You're standing in the {Name}. You see exits to the following places:";
                for (int i = 0; i < Exits.Length; i++)
                {
                    description += $" {Exits[i].Name}";
                    if (i != Exits.Length - 1)
                    {
                        description += ",";
                    }
                }

                description += ".";
                return description;
            }
        }

        //Constructor
        public Location(string name)
        {
            Name = name;
        }

        //Methods
        
    }
}