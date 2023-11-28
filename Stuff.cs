using System.Diagnostics;
using System.Text.RegularExpressions;

namespace YARPG
{
    [Flags]
    public enum SpotType
    {
        None = 0,
        Solid = 1,
        Room = 2,
        Wall = 4,
        Door = 8,
        Path = 16,
        StairsUp = 32,
        StairsDown = 64,
        Takable = 128,
        Gold = 256,
        Potion = 512,
        Scroll = 1024,
        Weapon = 2048,
        Armor = 4096,
        LightSource = 8192,
        Creature = 16384,
        Animal = 32768,
        Monster = 65536,
        NPC = 131072,
        Player = 262144,
        //we can add more flags here
    }
    public enum SeenType
    {
        Hidden,
        NotSeen,
        Remembered,
        Seen,
    }
    public class Spot
    {
        public SpotType Type { get; set; }
        public SeenType Seen { get; set; }
        public Spot() { } //default constructor
        public Spot(SpotType type, SeenType seen) => (Type, Seen) = (type, seen);
        public void DontSee()
        {
            if (Seen == SeenType.Hidden) //if it is hidden
            {
                return; //leave it alone (exit)
            }
            if (Type.HasFlag(SpotType.Room) || Type.HasFlag(SpotType.Door) || Type.HasFlag(SpotType.Path) ||
                Type.HasFlag(SpotType.StairsDown) || Type.HasFlag(SpotType.StairsUp) || 
                Type.HasFlag(SpotType.Solid)) //if type is room, door, path, or stairs
            {
                if (Seen == SeenType.Seen) //and it is seen
                {
                    Seen = SeenType.Remembered; //make it remembered
                    return; //and we are done
                }
                return; //we don't want to make remembered items hidden, so exit here
            }
            Seen = SeenType.NotSeen; //otherwise make it hidden
        }
        public void See()
        {
            if (Seen != SeenType.Hidden) //if it isn't hidden
            {
                Seen = SeenType.Seen; //make it seen
            }
        }
        public void UnHide()
        {
            if (Seen == SeenType.Hidden) //if it was hidden
            {
                Seen = SeenType.Seen; //make it seen
            }
        }
    }
    public enum RoomType
    {
        Normal,
        Safe,
        Lit,
        Shop,
        //we can add more if needed
    }
    public class Room
    {
        public int X, Y, Wide, High;
        public RoomType Type { get; set; } = RoomType.Normal;
        public List<Spot> Spots { get; set; } = new();
        public List<Point> SpotPoints { get; set; } = new();
        public Room() { } //default constructor
        public Room(int x, int y, int wide, int high)
        {
            (X, Y, Wide, High) = (x, y, wide, high);
            for (int i = L; i <= R; i++) //for each spot in room
            {
                for (int j = T; j <= B; j++)
                {
                    SpotType st = SpotType.Room; //default to room
                    if (i == L || i == R || j == T || j == B) //if we are on the edge
                    {
                        st = SpotType.Wall; //make it a wall
                    }
                    Spots.Add(new(st, SeenType.NotSeen)); //add spot to list
                    SpotPoints.Add(new(i, j)); //add spot point to list
                }
            }
        }
        public Room(StreamReader sr) => Load(sr);
        private int GetSpotIndex(int x, int y) => SpotPoints.FindIndex(p => p.X == x && p.Y == y);
        public Spot GetSpot(int x, int y) => Spots[GetSpotIndex(x, y)];
        public void SetSpot(int x, int y, Spot spot) => Spots[GetSpotIndex(x, y)] = spot;
        public int L => X;
        public int R => X + Wide - 1;
        public int T => Y;
        public int B => Y + High - 1;
        public int MidX => X + Wide / 2;
        public int MidY => Y + High / 2;
        public Point SideL => new(L, MidY);
        public Point SideR => new(R, MidY);
        public Point SideT => new(MidX, T);
        public Point SideB => new(MidX, B);
        public Point PlacePoint() => new(Rand.FromTo(L + 1, R - 1), Rand.FromTo(T + 1, B - 1));
        public (int, int) Place() => (Rand.FromTo(L + 1, R - 1), Rand.FromTo(T + 1, B - 1));
        public bool Touches(Point p)
        {
            if (L <= p.X && p.X <= R && T <= p.Y && p.Y <= B) //if point is in room
                return true;
            return false;
        }
        public bool Inside(Point p) => Inside(p.X, p.Y);
        public bool Inside(int x, int y)
        {
            if (L < x && x < R && T < y && y < B) //if point is in room
                return true;
            return false;
        }
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
    }

    public class Door
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Room Room { get; set; } = new();
        public Spot Spot { get; set; } = new();
        private void InitDoor(int x, int y, Room room)
        {
            (X, Y, Room) = (x, y, room); //copy position and room
            Spot = new(SpotType.Room | SpotType.Door | SpotType.Path, SeenType.NotSeen); //set spot
            Room.SetSpot(x, y, Spot); //set spot within room
        }
        public Door(int x, int y, Room room)
        {
            InitDoor(x, y, room); //handle initialization
            Room = room; //keep compiler from complaining
        }
        public Door(Point p, Room room)
        {
            InitDoor(p.X, p.Y, room); //handle initialization
            Room = room; //keep compiler from complaining
        }
        public Door(StreamReader sr) => Load(sr);
        public int GetSide() //returns an integer corresponding to which side of the room the door is on
        {
            if (X == Room.L)
            {
                return 1;
            }
            if (X == Room.R)
            {
                return 2;
            }
            if (Y == Room.T)
            {
                return 3;
            }
            if (Y == Room.B)
            {
                return 4;
            }
            return -1;
        }
        public bool Inside(Point p) => Inside(p.X, p.Y);
        public bool Inside(int x, int y)
        {
            if (X == x && Y == y) //if point is in door
                return true;
            return false;
        }
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
    }

    public class Path
    {
        public int X, Y;
        public Spot Spot { get; set; } = new();
        public Path(int x, int y) => (X, Y, Spot) = (x, y, new(SpotType.Path, SeenType.NotSeen));
        public Path(Point p) => (X, Y, Spot) = (p.X, p.Y, new(SpotType.Path, SeenType.NotSeen));
        public Path(StreamReader sr) => Load(sr);
        public bool Inside(Point p) => Inside(p.X, p.Y);
        public bool Inside(int x, int y)
        {
            if (X == x && Y == y) //if point is in path
                return true;
            return false;
        }
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
    }

    public class LevGen
    {
        public int Wide { get; set; }
        public int High { get; set; }
        public List<Room> Rooms { get; set; } = new();
        public List<Path> Paths { get; set; } = new();
        public List<Door> Doors { get; set; } = new();
        private readonly int MinRoom = 10, MaxRoom = 15;

        public LevGen(int wide, int high)
        {
            Wide = wide;
            High = high;
            Gen(10);
        }

        public LevGen(int wide, int high, int numrooms)
        {
            Wide = wide;
            High = high;
            Gen(numrooms);
        }

        public LevGen(int wide, int high, int numrooms, int minroom, int maxroom)
        {
            Wide = wide;
            High = high;
            MinRoom = minroom; //this seems weird since MinRoom and MaxRoom are readonly but constructors can override that
            MaxRoom = maxroom;
            Gen(numrooms);
        }

        public void Gen(int numrooms)
        {
            Room croom;
            int w = Rand.FromTo(MinRoom, MaxRoom); //rand.Next(minroom, maxroom + 1);
            int h = Rand.FromTo(MinRoom, MaxRoom); //rand.Next(minroom, maxroom + 1);
            int x = Rand.FromTo(Wide / 2 - MaxRoom, Wide / 2 + MaxRoom) - w / 2; //rand.Next(15, Wide - w - 15);
            int y = Rand.FromTo(High / 2 - MaxRoom, High / 2 + MaxRoom) - h / 2; //rand.Next(15, High - h - 15);
            Rooms.Add(new(x, y, w, h));
            while (Rooms.Count < numrooms)
            {
                int ri = Rand.UpTo(Rooms.Count); //get an index of a room that exists
                croom = Rooms[ri]; //set the current room to that index
                Sprout(croom); //sprout from current room
            }
            CleanUpPaths();
        }

        private void Sprout(Room croom)
        {
            if (DidThat(croom, Direction.Left) && DidThat(croom, Direction.Right) && DidThat(croom, Direction.Up) && DidThat(croom, Direction.Down))
                return; //if we did all the directions off this room, we can't sprout, so return
            var dir = (Direction)Rand.UpTo(4); // 0: Left, 1: Right, 2: Up, 3: Down
            while (DidThat(croom, dir)) //while we already did a direction, try another
            {
                dir = (Direction)Rand.UpTo(4);//rand.Next(4);
            }
            int dx = 0, dy = 0;
            if (dir == Direction.Left) //if we are going left
            {
                Doors.Add(new(croom.SideL, croom)); //add a door and path on the left side of the room
                Paths.Add(new(croom.SideL));
                dx = -1; //move left
            }
            if (dir == Direction.Right) //if we are going right
            {
                Doors.Add(new(croom.SideR, croom)); //add a door and path on the right side of the room
                Paths.Add(new(croom.SideR));
                dx = 1; //move right
            }
            if (dir == Direction.Up) //if we are going up
            {
                Doors.Add(new(croom.SideT, croom)); //add a door and path on the top side of the room
                Paths.Add(new(croom.SideT));
                dy = -1; //move up
            }
            if (dir == Direction.Down) //if we are going down
            {
                Doors.Add(new(croom.SideB, croom)); //add a door and path on the bottom side of the room
                Paths.Add(new(croom.SideB));
                dy = 1; //move down
            }
            int x = Paths[^1].X, y = Paths[^1].Y; //get the last path
            int l = 0; //length of path is 0
            while (true) //keep generating paths
            {
                x += dx; //move along the path
                y += dy;
                if (Rooms.Any(r => r.L <= x && r.R >= x && r.T <= y && r.B >= y)) //if we hit a room
                {
                    return; //exit
                }
                Paths.Add(new(x, y)); //add to paths
                l++; //increment length
                if (x < 5 || x >= Wide - 5 || y < 5 || y >= High + 5) //if we hit a wall, we're done
                    break;
                if (Rand.UpTo(100) < 35) //35% chance of a sidemove
                {
                    if (dir == Direction.Left || dir == Direction.Right) //if we are going left or right
                    {
                        if (Rand.UpTo(100) < 50) //50% chance of going up or down
                        {
                            y--; //move up
                        }
                        else
                        {
                            y++; //move down
                        }
                    }
                    else //if we are going up or down
                    {
                        if (Rand.UpTo(100) < 50) //50% chance of going left or right
                        {
                            x--; //move left
                        }
                        else
                        {
                            x++; //move right
                        }
                    }
                    Paths.Add(new(x, y)); //add to paths
                    x += dx; //move along the path
                    y += dy;
                    Paths.Add(new(x, y)); //add to paths
                }
                int ch = (l - 5) * 5; //chance of making a room (5% for each path beyond first 5)
                if (Rand.UpTo(100) < ch) //if we make a room
                {
                    if (x > MaxRoom && x < Wide - MaxRoom && y > MaxRoom && y < High - MaxRoom) //if we have room for a room, add it
                    {
                        GenRoom(x, y, dir); //generate room
                    }
                    break; //either way, we are done
                }
            }
        }

        private void GenRoom(int x, int y, Direction dir)
        {
            if (x < MaxRoom || x > Wide - MaxRoom || y < MaxRoom || y > High - MaxRoom) //if there isn't room for a room, return
            {
                return;
            }
            int xx = x, yy = y; //make copies of x, y
            int w = Rand.FromTo(MinRoom, MaxRoom); //get dimensions of new room
            int h = Rand.FromTo(MinRoom, MaxRoom);
            if (dir == Direction.Left) //if we are going left
            {
                xx = x - w + 1; //adjust xx
                yy = y - h / 2; //adjust yy
            }
            if (dir == Direction.Right) //if we are going right
            {
                xx = x; //no adjustment for xx
                yy = y - h / 2; //adjust yy
            }
            if (dir == Direction.Up) //if we are going up
            {
                yy = y - h + 1; //adjust yy
                xx = x - w / 2; //adjust xx
            }
            if (dir == Direction.Down) //if we are going down
            {
                yy = y; //no adjustment for yy
                xx = x - w / 2; //adjust xx
            }
            Room nr = new(xx - 5, yy - 5, w + 10, h + 10); //create new room with expanded dimensions
            if (Rooms.All(r => nr.L > r.R || nr.R < r.L || nr.T > r.B || nr.B < r.T)) //if new room doesn't touch any rooms
            {
                Rooms.Add(new(xx, yy, w, h)); //add the new room
                Doors.Add(new(x, y, Rooms[^1])); //add a door
            }
        }

        private bool DidThat(Room croom, Direction dir)
        {
            Point p = Point.Empty; //make a point
            if (dir == Direction.Left) //for each direction, pick a side of the room
            {
                p = croom.SideL;
            }
            if (dir == Direction.Right)
            {
                p = croom.SideR;
            }
            if (dir == Direction.Up)
            {
                p = croom.SideT;
            }
            if (dir == Direction.Down)
            {
                p = croom.SideB;
            }
            return Doors.Any(d => d.X == p.X && d.Y == p.Y); //if we found a door there, return true
        }
        private void CleanUpPaths()
        {
            foreach (var p in Paths) //for each path
            {
                if (p.X < 5 || p.X >= Wide - 5 || p.Y < 5 || p.Y >= High + 5) //if we hit a wall
                {
                    Paths.Remove(p); //remove that path
                    CleanUpPaths(); //restart the clean up
                    return; //and exit afterward
                }
                if (Rooms.Any(r => r.L <= p.X && r.R >= p.X && r.T <= p.Y && r.B >= p.Y)) //if we hit a room
                {
                    if (!Doors.Any(d => d.X == p.X && d.Y == p.Y)) //if there is no door there
                    {
                        Paths.Remove(p); //remove that path
                        CleanUpPaths(); //restart the clean up
                        return; //and exit afterward
                    }
                }
            }
        }
    }

    public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
    }
    public class Attack
    {
        public string Name { get; set; } = string.Empty; //name
        public int ToHit { get; set; } = 0; //to hit bonus
        public List<string> Damages { get; set; } = new(); //damages
        public List<DamageType> DamageTypes { get; set; } = new(); //damage types (should be one per each Damages)
        public Effect Effect { get; set; } = Effect.None; //if attack has an effect
        public int Range { get; set; } = 0; //range of attack (0 is melee only)
        public int MaxRange { get; set; } = 0; //maximum range of attack (0 is melee only)
        public int Area { get; set; } = 0; //area of attack (0 is targetted creature)

        public Attack() { } //default constructor
        public Attack(Attack a) //copy constructor
        {
            Name = a.Name;
            ToHit = a.ToHit;
            Damages.Clear();
            foreach (var s in a.Damages)
            {
                Damages.Add(s);
            } //Damages = a.Damages;
            DamageTypes.Clear();
            foreach (var t in a.DamageTypes)
            {
                DamageTypes.Add(t);
            } //DamageTypes = a.DamageTypes;
            Effect = a.Effect;
            Range = a.Range;
            MaxRange = a.MaxRange;
            Area = a.Area;
        }
        public string Describe()
        {
            string desc = $"{Name}: {ToHit}, {Damages.Count}:";
            for (int i = 0; i < Damages.Count; i++)
            {
                desc += $" {Damages[i]} {DamageTypes[i]}";
            }
            desc += $" Effect: {Effect}, Range: {Range}/{MaxRange}, Area: {Area}";
            return desc;
        }
        public Attack(Thing player, Thing thing) //copy from thing constructor
        {
            int dexbonus = player.DEX / 2 - 5;
            int strbonus = player.STR / 2 - 5;
            ///
            ///TODO: Account for Finesse weapons
            ///
            Info.Out($"Making an attack from {thing.Name}:  ");
            Name = thing.Name; //get name from thing
            ToHit = thing.Bonus + dexbonus; //compute to hit bonus with dex bonus
            Info.Out($"ToHit: {thing.Bonus + dexbonus}  ");
            Damages.Clear();
            foreach (var dam in thing.Damages) //for each damage in Damages from thing
            {
                Info.Out(dam);
                Damages.Add($"{dam}+{strbonus}"); //add player's STR bonus to damage
                Info.Out($"+{strbonus}  ");
            }
            DamageTypes.Clear();
            foreach (var damtype in thing.DamageTypes) //for each damagetype in thing
            {
                DamageTypes.Add(damtype); //add in damagetype
            }
            Effect = thing.Effect;
            Range = thing.Range;
            if (Range == 0)
            {
                Info.OutL("melee.");
            }
            else
            {
                Info.OutL("ranged");
            }
            MaxRange = thing.MaxRange;
            Area = thing.Area;
        }
        public bool IsFromWeapon(Thing thing)
        {
            if (string.Compare(Name,thing.Name) == 0)
            {
                return true;
            }
            return false;
        }
        public Attack(StreamReader sr) => Load(sr); //file load constructor
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
    }
    public enum AbilityType
    {
        None,
        STR,
        DEX,
        CON,
        INT,
        WIS,
        CHA,
    }
    public class Thing
    {
        //variables for all things
        public int X { get; set; } //X, Y are needed for everything
        public int Y { get; set; } //use -1, -1 if inside a container, inventory, etc
        public Spot Spot { get; set; } = new(); //whether the thing is visible and what kind of thing it is
        public string Name { get; set; } = string.Empty; //all things have a name
        public string Description { get; set; } = string.Empty; //all things have a description

        //variables for items
        public int Weight { get; set; } = 0; //how much the item weighs, use -1 for immovable objects
        public int Value { get; set; } = 0; //how much the item is worth, for gold, the actual amount
        public PotionType Potion { get; set; } = PotionType.None; //if a potion, what kind
        public ScrollType Scroll { get; set; } = ScrollType.None; //if a scroll, what kind
        public bool Consumable { get; set; } = false; //if an item is consumable
        public bool Equippable { get; set; } = false; //if an item is equippable
        public bool Equipped { get; set; } = false; //if an item is equipped
        public Effect Effect { get; set; } = Effect.None; //if an item has an effect
        public int Bonus { get; set; } = 0; //if an item has a bonus (like a +1 weapon)
        public int Armor { get; set; } = 0; //if an armor, what base AC?
        public List<string> Damages { get; set; } = new(); //if a weapon, what damages, if any?
        public List<DamageType> DamageTypes { get; set; } = new(); //if a weapon, what damage types, if any? (should be one per Damages)
        public int Range { get; set; } = 0; //if a ranged weapon, it will have a range
        public int MaxRange { get; set; } = 0; //ranged weapons have a maximum range (with disadvantage)
        public int Area { get; set; } = 0; //mostly for spells, but a very few weapons might have an area

        //variables for containers
        public bool Open { get; set; } = false; //if a container is open (inventory is always open)
        public bool Locked { get; set; } = false; //if a container is locked (inventory is never locked)
        public List<Thing> Things { get; set; } = new(); //if a container, what things are in it

        //variables for creatures
        public Alignment Alignment { get; set; } = Alignment.None; //creatures have an alignment
        public bool Dead { get; set; } = false; //if a creature is dead
        public bool Hostile { get; set; } = false; //if a creature is hostile
        public int HP { get; set; } = 0; //if a creature, how much hp
        public int MaxHP { get; set; } = 0; //if a creature, how much maximum hp
        public int Speed { get; set; } = 0; //if a creature, how fast is it
        public int FlySpeed { get; set; } = 0; //if a creature, how fast can it fly
        public int ClimbSpeed { get; set; } = 0; //if a creature, how fast can it climb
        public int SwimSpeed { get; set; } = 0; //if a creature, how fast can it swim
        public int AC { get; set; } = 0; //if a creature, what AC does it have
        public int STR { get; set; } = 0; //if a creature, what STR does it have
        public int DEX { get; set; } = 0; //if a creature, what DEX does it have
        public int CON { get; set; } = 0; //if a creature, what CON does it have
        public int INT { get; set; } = 0; //if a creature, what INT does it have
        public int WIS { get; set; } = 0; //if a creature, what WIS does it have
        public int CHA { get; set; } = 0; //if a creature, what CHA does it have
        public CreatureType Creature { get; set; } = CreatureType.None; //if a creature, what kind of creature it is
        public int Experience { get; set; } = 0; //if a creature, how much experience is it worth, otherwise how much XP we have
        public int Level { get; set; } = 0; //if a creature, what level is it (for all but player, first level it would appear on)
        public int NumberAttacks { get; set; } = 0; //if a creature, how many attacks (per round) it gets
        public List<Attack> Attacks { get; set; } = new(); //if a creature, what attacks it has
        public ClassType Class { get; set; } = ClassType.None; //if an NPC or player, what class it is
        public List<DamageType> Resistances { get; set; } = new(); //if a creature, what resistances it has (half damage)
        public List<DamageType> Immunities { get; set; } = new(); //if a creature, what immunities it has (no damage)
        public List<DamageType> Vulnerabilities { get; set; } = new(); //if a creature, what vulnerabilities it has (double damage)
        public int LightRadius { get; set; } = 0; //how far creature can see in the dark or affected by light sources
        public RaceType BaseRace { get; set; } = RaceType.Human; //base race of creature
        public string Race { get; set; } = string.Empty; //name of creature's race
        public MonsterType Monster { get; set; } = MonsterType.None; //type of monster
        public List<SpellEffect> Condition { get; set; } = new(); //list of conditions
        public List<Spell> SpellList { get; set; } = new(); //list of spells

        //private variables
        private Thing? _target;
        private int advdisadv = 0;
        private readonly int[] _levels = new int[] { 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000, 120000,
                                                    140000, 165000, 195000, 225000, 265000, 305000, 355000 };
        private List<Point> _currentpath = new();
        public Thing() { } //default constructor
        public Thing(Thing t) //copy constructor
        {
            X = t.X;
            Y = t.Y;
            Spot = t.Spot;
            Name = t.Name;
            Description = t.Description;
            Weight = t.Weight;
            Value = t.Value;
            Potion = t.Potion;
            Scroll = t.Scroll;
            Consumable = t.Consumable;
            Equippable = t.Equippable;
            Effect = t.Effect;
            Bonus = t.Bonus;
            Armor = t.Armor;
            Damages.Clear();
            foreach (var d in t.Damages)
            {
                Damages.Add(d);
            } //Damages = t.Damages;
            DamageTypes.Clear();
            foreach (var d in t.DamageTypes)
            {
                DamageTypes.Add(d);
            } //DamageTypes = t.DamageTypes;
            Range = t.Range;
            MaxRange = t.MaxRange;
            Area = t.Area;
            Open = t.Open;
            Locked = t.Locked;
            Things.Clear();
            foreach (var thing in t.Things)
            {
                Things.Add(new(thing));
            } //Things = t.Things; using copy of each
            Dead = t.Dead;
            Hostile = t.Hostile;
            Alignment = t.Alignment;
            HP = t.HP;
            MaxHP = t.MaxHP;
            Speed = t.Speed;
            FlySpeed = t.FlySpeed;
            ClimbSpeed = t.ClimbSpeed;
            SwimSpeed = t.SwimSpeed;
            AC = t.AC;
            STR = t.STR;
            DEX = t.DEX;
            CON = t.CON;
            INT = t.INT;
            WIS = t.WIS;
            CHA = t.CHA;
            Creature = t.Creature;
            Experience = t.Experience;
            Level = t.Level;
            NumberAttacks = t.NumberAttacks;
            Attacks.Clear();
            foreach (var a in t.Attacks)
            {
                Attacks.Add(new(a));
            } //Attacks = t.Attacks; using copy of each
            Class = t.Class;
            Resistances.Clear();
            foreach (var r in t.Resistances)
            {
                Resistances.Add(r);
            } //Resistances = t.Resistances;
            Immunities.Clear();
            foreach (var i in t.Immunities)
            {
                Immunities.Add(i);
            } //Immunities = t.Immunities;
            Vulnerabilities.Clear();
            foreach (var v in t.Vulnerabilities)
            {
                Vulnerabilities.Add(v);
            } //Vulnerabilities = t.Vulnerabilities;
            LightRadius = t.LightRadius;
            BaseRace = t.BaseRace;
            Race = t.Race;
            Monster = t.Monster;
            SpellList.Clear();
            foreach (var s in t.SpellList)
            {
                SpellList.Add(new(s));
            } //SpellList = t.SpellList;
        }
        public Thing(StreamReader sr) => Load(sr); //file load constructor
        public SpotType Type => Spot.Type;
        public bool StillOnPath() => _currentpath.Count > 0;
        public string Describe()
        {
            string description = $"{Name}: {Description}\n";
            description += $"Value or Gold: {Value}, Weight: {Weight}, At: {X}, {Y}\n";
            description += $"Consumable: {Consumable}, Potion: {Potion}, Scroll: {Scroll}\n";
            description += $"Equippable: {Equippable}, Equipped: {Equipped}, Effect: {Effect}, Bonus: {Bonus}\n";
            description += $"Armor: {Armor}, Damages: ";
            for (int i = 0; i < Damages.Count; i++)
            {
                description += $"{Damages[i]} {DamageTypes[i]}\n";
            }
            description += $"Range: {Range}, MaxRange: {MaxRange}, Area: {Area}\n";
            description += $"Open: {Open}, Locked: {Locked}, Things: {Things.Count}\n";
            description += $"Dead: {Dead}, Hostile: {Hostile}, Alignment: {Alignment}\n";
            description += $"HP: {HP}, MaxHP: {MaxHP}, Speed: {Speed}, FlySpeed: {FlySpeed}, ClimbSpeed: {ClimbSpeed}, SwimSpeed: {SwimSpeed}, AC: {AC}\n";
            description += $"STR: {STR}, DEX: {DEX}, CON: {CON}, INT: {INT}, WIS: {WIS}, CHA: {CHA}\n";
            description += $"Creature: {Creature}, Experience: {Experience}, Level: {Level}, NumberAttacks: {NumberAttacks}\n";
            description += $"Attacks: {Attacks.Count}, Class: {Class}, Resistances: {Resistances.Count}, Immunities: {Immunities.Count}, Vulnerabilities: {Vulnerabilities.Count}\n";
            description += $"LightRadius: {LightRadius}, BaseRace: {BaseRace}, Race: {Race}, Monster: {Monster}, Spells: {SpellList.Count}\n";
            return description;
        }
        public int ProfBonus => (int)((float)Level / 4 + 1.5);
        public int STRmod => STR / 2 - 5;
        public int DEXmod => DEX / 2 - 5;
        public int CONmod => CON / 2 - 5;
        public int INTmod => INT / 2 - 5;
        public int WISmod => WIS / 2 - 5;
        public int CHAmod => CHA / 2 - 5;
        public void AddCondition(SpellEffect effect)
        {
            Condition.Add(effect);
        }
        public bool HasEffect(Effect effect)
        {
            foreach (var condition in Condition) //for each condition thing has
            {
                if (condition.Effect == effect) //if condition has effect we are looking for
                {
                    return true; //report true
                }
            }
            return false; //report false
        }
        public bool CanMove()
        {
            //if we are asleep, incapacitated, paralyzed, petrified, unconscious, or stunned
            if (HasEffect(Effect.Sleep) || HasEffect(Effect.Incapacitated) || HasEffect(Effect.Paralyzed) || HasEffect(Effect.Petrified) ||
                HasEffect(Effect.Unconscious) || HasEffect(Effect.Stunned))
            {
                return false; //report false (no we can't move)
            }
            return true; //return true (yes we can move)
        }
        public void HandleConditions()
        {
            List<SpellEffect> conditionstoremove = new(); //our list for conditions to remove
            foreach (var condition in Condition) //for each condition we have at the moment
            {
                if (condition.Time(this)) //if the time is up (or we saved)
                {
                    conditionstoremove.Add(condition); //add condition to conditions to remove
                }
            }
            foreach (var condition in conditionstoremove) //for each condition in conditions to remove (if any)
            {
                Condition.Remove(condition); //remove condition
            }
        }
        public bool IsCreature()
        {
            //we should be able to simply use HasFlag(SpotType.Creature), but best not to trust it
            if (Type.HasFlag(SpotType.Creature) || Type.HasFlag(SpotType.Animal) || Type.HasFlag(SpotType.Monster) || 
                Type.HasFlag(SpotType.NPC) || Type.HasFlag(SpotType.Player))
            {
                return true; //report true
            }
            return false; //report false
        }
        public bool IsSpellCaster() => Data.classspellcaster[Class];
        public bool IsTakable()
        {
            //we should be able to simply use HasFlag(SpotType.Takable), but best not to trust it
            if (Type.HasFlag(SpotType.Armor) || Type.HasFlag(SpotType.Gold) || Type.HasFlag(SpotType.LightSource) ||
                Type.HasFlag(SpotType.Potion) || Type.HasFlag(SpotType.Scroll) || Type.HasFlag(SpotType.Weapon) ||
                Type.HasFlag(SpotType.Takable))
            {
                return true; //report true
            }
            return false; //report false
        }
        public bool IsPlayer => Type.HasFlag(SpotType.Player);
        public bool IsStairs()
        {
            if (Type.HasFlag(SpotType.StairsUp) || Type.HasFlag(SpotType.StairsDown)) //if stairs up or stairs down
            {
                return true; //report true
            }
            return false; //report false
        }
        public bool PickUpItem(Thing item)
        {
            if (!item.IsTakable()) //if item is not takable
            {
                return false; //report failure
            }
            if (item.Spot.Type.HasFlag(SpotType.Gold)) //if item is gold
            {
                Value += item.Value; //add amount to Value
                return true; //report success
            }
            Things.Add(item); //add item to inventory
            return true; //report success
        }
        public int Light()
        {
            int l = Data.racedata[BaseRace].DarkVision; //get race darkvision distance
            if (LightRadius > l) //if light radius is greater
            {
                l = LightRadius; //use that
            }
            return l / 5; //we divide by 5 to convert from feet to game units
        }
        public int NextLevel => _levels[Level];
        public bool CheckExperience()
        {
            if (Experience > _levels[Level]) //if we have the xp to gain a level
            {
                Level++; //increase level
                Info.Out($"{Name} went up a level to {Level}!", Color.DarkMagenta); //display info
                int hitdie = Data.classhitdice[Class]; //get hit die by class
                int hpadd = Rand.D(hitdie); //roll that die
                if (hpadd < hitdie / 2) hpadd = hitdie / 2; //if less than half the hit die, use half the hit die
                hpadd += CON / 2 - 5; //don't forget the constitution bonus!
                MaxHP += hpadd; //add to maximum HP
                HP = MaxHP; //and fully heal
                Info.OutL($" And gained {hpadd} HP for a total of {HP}!"); //display info
                if (IsSpellCaster()) //if we are a spell caster
                {
                    GetSpells.DoForm(); //get spells
                }
                if ((Level + 1) % 4 == 0) //if level is a multiple of 4
                {
                    ///
                    ///TODO: Ability Score Adjustment
                    ///
                }
                ///
                ///TODO: There is still a lot more to gaining a level
                ///
                return true;
            }
            return false;
        }
        public bool IsAt(int x, int y) => (X == x && Y == y);
        public bool IsAt(Point p) => (X == p.X && Y == p.Y);
        public bool NextTo(int x, int y) => (x - 1 <= X && X <= x + 1 && y - 1 <= Y && Y <= y + 1);
        public bool NextTo(Point p) => NextTo(p.X, p.Y);
        public bool MoveHere(Point point)
        {
            if (!CanMove(point, IsPlayer)) //if point is not a valid place to move
            {
                return false; //report failure
            }
            _currentpath.Clear(); //clear any path we had before
            if (NextTo(point)) //if spot is beside player
            {
                return MoveTo(point); //move there - we could use .Move but we would have to calculate the x,y direction
            }
            else //otherwise (we need to do some travelling)
            {
                _currentpath = PathFindingService.FindPath(new(X, Y), point, IsPlayer); //get path to point
                return Move(); //send to Move so we will use the first step of the path
            }
        }

        public bool MoveTo(int x, int y)
        {
            bool inroom = InRoom() != null; //flag for whether we are in a room
            if (CanMove(x, y, IsPlayer)) //if we can move there
            {
                (X, Y) = (x, y); //then move there
                if (!inroom && IsPlayer) //if we weren't in a room and we are player
                {
                    Room? room = InRoom(); //get room we are in
                    if (room != null) //if it exists
                    {
                        Info.OutL(room.Type switch //display a message by room type
                        {
                            RoomType.Lit => "This room is well lit!",
                            RoomType.Safe => "This room feels safe!",
                            RoomType.Shop => "Look! A merchant!",
                            _ => "Another room."
                        } );
                    }
                }
                return true; //report success
            }
            return false; //report failure
        }
        public bool MoveTo(Point point) => MoveTo(point.X, point.Y);
        public bool Move(int dx, int dy) => MoveTo(X + dx, Y + dy);
        public bool Move()
        {
            advdisadv = 0; //no advantage or disadvantage
            if (HasEffect(Effect.Confusion)) //if we are confused
            {
                advdisadv--; //set disadvantage
            }
            if (_currentpath.Count > 0) //if there is any current path
            {
                MoveTo(_currentpath[0]); //move to the next point in the current path
                _currentpath.RemoveAt(0); //get that point out of the current path
                return true; //return success
            }
            _target = null; //set target to none
            if (Type == SpotType.Player) //if we are player
            {
                return false; //return with false (failure)
            }
            if (Dead) //if we are dead
            {
                return true; //return with true (success)
            }
            Thing? thistarget = null; //target
            List<Thing> targets = VisibleCreatures(); //get list of possible targets
            if (HasEffect(Effect.Confusion)) //if we are confused
            {
                if (targets.Count > 0) //if there are targets
                {
                    if (Rand.D(100) < 10) //10% of the time
                    {
                        _target = targets[Rand.UpTo(targets.Count)]; //pick a target at random
                        if (CanAttack(_target)) //if we can attack this target
                        {
                            thistarget = _target; //set it as our active target
                        }
                    }
                }
            }
            else //otherwise (we are not confused)
            {
                foreach (var target in targets) //for each possible target
                {
                    if (ShouldAttack(target)) //if we should attack it
                    {
                        _target = target; //set target
                        if (CanAttack(target)) //if we can attack it
                        {
                            thistarget = target; //set target to this one
                        }
                    }
                }
            }
            if (thistarget != null) //if we found a target to attack
            {
                if (HasEffect(Effect.Frightened) && thistarget.IsPlayer) //if we are frightened and target is player
                {
                    double theta = Math.Atan2(thistarget.Y - Y, thistarget.X - X) + Math.PI; //convert to polar but make angle opposite angle
                    double radius = Math.Sqrt(Math.Pow(thistarget.Y - Y, 2) + Math.Pow(thistarget.X - X, 2)); //get radius
                    int towardx = (int)Math.Cos(theta); //convert back to rectangular coordinates but we are only moving a distance of 1
                    int towardy = (int)Math.Sin(theta);
                    MoveHere(new(towardx, towardy)); //move away from target
                    Move(); //make a single step
                    _currentpath.Clear(); //get rid of path
                    return true; //report success (we ran away or tried to anyway!  Yes, if there is a wall there, we will simply stay put.)
                }
                else //otherwise (not frightened and/or target isn't player)
                {
                    Info.OutL($"{thistarget.Name} targetted by {Name}.", Color.DarkRed); //display info
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Monster.wav");
                    if (thistarget == GameData.Player) //if target is player
                    {
                        GameData.Targetted = true; //set GameData's Targetted flag
                    }
                    else //otherwise (not player)
                    {
                        GameData.Targetted = false; //clear GameData's Targetted flag
                    }
                    for (int i = 0; i < NumberAttacks; i++) //for each attack
                    {
                        if (!Attack(thistarget, ChooseAttack(thistarget))) //if we are unable to attack
                        {
                            MoveHere(new(thistarget.X, thistarget.Y)); //move toward target
                            Move(); //make a single step
                            _currentpath.Clear(); //get rid of path
                        }//return true/false from attacking target
                    }
                    return true; //we attacked
                }
            }
            if (!HasEffect(Effect.Confusion)) //if we are not confused
            {
                List<Thing> stuff = GameData.CurrentLevel.Items.Where(i => (i.X - 1 <= X && X <= i.X + 1 && i.Y - 1 <= Y && Y <= i.Y + 1)).ToList();
                if (Rand.D(100) <= 5 && stuff.Count > 0) //5% chance
                {
                    Thing thing = stuff[Rand.UpTo(stuff.Count)];
                    PickUpItem(thing); //pick up the thing
                    GameData.CurrentLevel.Items.Remove(thing); //remove it from level
                    if (Spot.Seen == SeenType.Seen) //if monster is seen
                    {
                        Info.OutL($"{Name} picked up {thing.Name}.", Color.DarkOrange); //display info
                    }
                }
                if (Rand.D(100) <= 25 && Things.Count > 0) //25% chance
                {
                    Thing thing = Things[Rand.UpTo(Things.Count)];
                    thing.X = X; //move it to current position
                    thing.Y = Y;
                    GameData.CurrentLevel.Items.Add(thing); //add thing to level items list
                    if (Spot.Seen == SeenType.Seen) //if monster is seen
                    {
                        Info.OutL($"{Name} dropped {thing.Name}.", Color.DarkViolet); //display info
                    }
                    Things.Remove(thing); //delete it from inventory
                }
            }
            if (!HasEffect(Effect.Grappled)) //if we are not grappled
            {
                if (_target != null) //if we have a target
                {
                    return MoveHere(new(_target.X, _target.Y)); //move toward target
                }
                int x = Rand.FromTo(-1, 1); //pick a random direction to move
                int y = Rand.FromTo(-1, 1);
                return Move(x, y); //return true/false from trying to move that way
            }
            else
            {
                return true; //we couldn't move, but our turn is done
            }
        }
        public Attack? ChooseAttack(Thing target)
        {
            int range = GetDistance(target); //get distance to target
            if (range <= 1) //if range is less than or equal to 1,
            {
                range = 0; //set range to 0 (melee)
            }
            if (Attacks.Any(a => a.MaxRange >= range * 5)) //if within maximum range
            {
                if (Attacks.Any(a => a.Range >= range * 5)) //if within range
                {
                    return Attacks.FirstOrDefault(a => a.Range >= range * 5); //return first attack which has good enough range
                }
                advdisadv--; //set disadvantage
                return Attacks.FirstOrDefault(a => a.MaxRange >= range * 5); //return fist attack which has good enough maxrange
            }
            return Attacks.FirstOrDefault(a => a.Range >= range * 5); //return first attack which has good enough range
        }
        public bool Attack(Thing target, Attack? attack)
        {
            if (attack == null) //if there is no attack to use
            {
                Info.OutL($"{Name} cannot attack {target.Name}!", Color.DarkGreen); //display info
                return false; //return failure
            }
            Info.Out($"{Name} attacks {target.Name} ", Color.DarkRed); //display info
            //if we are blind, deafened, or confused or if target is invisible
            if (HasEffect(Effect.Blind) || HasEffect(Effect.Deafened) || HasEffect(Effect.Confusion) || target.HasEffect(Effect.Invisible))
            {
                advdisadv--; //set disadvantage
            }
            if (target.HasEffect(Effect.Prone) || target.HasEffect(Effect.Restrained) || target.HasEffect(Effect.Stunned)) //if target is prone or restrained or stunned
            {
                advdisadv++; //set advantage
            }
            int roll1 = Rand.D(20); //get hit roll
            if (advdisadv != 0) //if we have advantage / disadvantage
            {
                int roll2 = Rand.D(20); //get a second hit roll
                if (advdisadv < 0) //if it is disadvantage
                {
                    Info.Out("at a disadvantage "); //display info
                    if (roll2 < roll1) //if roll2 is less than roll1
                    {
                        roll1 = roll2; //set roll1 to roll2
                    }
                }
                if (advdisadv > 0) //if it is advantage
                {
                    Info.Out("at an advantage "); //display info
                    if (roll2 > roll1) //if roll2 is more than roll1
                    {
                        roll1 = roll2; //set roll1 to roll2
                    }
                }
            }
            bool iscrit = (roll1 == 20); //check for crit
            Info.Out($"({roll1} + {attack.ToHit} vs {target.AC}) ");
            if (iscrit || roll1 + attack.ToHit >= target.AC) //if crit or if it will beat target's AC
            {
                if (attack.Range == 0) //if attack is melee
                {
                    int rdr = Rand.D(3); //get random 1 of 3 sounds
                    if (rdr == 1) //if first one
                    {
                        SoundSystem.PlayEmbeddedSound("YARPG.Resources.Attack-Hit1.wav"); //play sound
                    }
                    if (rdr == 2) //if second one
                    {
                        SoundSystem.PlayEmbeddedSound("YARPG.Resources.Attack-Hit2.wav"); //play sound
                    }
                    if (rdr == 3) //if third one
                    {
                        SoundSystem.PlayEmbeddedSound("YARPG.Resources.Attack-Hit3.wav"); //play sound
                    }
                }
                else //otherwise (ranged attack)
                {
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Ranged-Hit.wav"); //play sound
                }
                Info.Out($"hit for ({attack.Damages.Count} attacks) ", Color.DarkRed); //display info
                int damage = 0; //damage total = 0
                for (int i = 0; i < attack.Damages.Count; i++) //for each Damages in attack
                {
                    Info.Out($"({attack.Damages[i]}) ");
                    int dam = Rand.ParseAndRoll(attack.Damages[i], iscrit); //parse and roll this Damages
                    DamageType dt = attack.DamageTypes[i]; //get damagetype
                    Info.Out($"{dam} {dt} ", Color.DarkRed); //display info
                    dam = target.ApplyVulResImm(dam, dt); //have target apply vulnerabilities, resistances, and immunities
                    Info.Out($" => {dam} recieved.  ", Color.DarkRed); //display info
                    damage += dam; //add damage to total damage
                }
                Info.OutL($"{damage} damage total"); //display info
                target.TakeHit(damage); //make target take the damage
                return true; //return true (success)
            }
            else //otherwise (we missed)
            {
                if (attack.Range == 0) //if melee attack
                {
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Attack-Miss.wav"); //play sound
                }
                else //otherwise (ranged attack)
                {
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.RangedMiss.wav"); //play sound
                }
                Info.OutL("but missed!", Color.DarkGreen); //display info
                return true; //return true (it was still a successful attack)
            }
        }
        public int ApplyVulResImm(int dam, DamageType dt)
        {
            if (Vulnerabilities.Any(v => v == dt)) //if damagetype is in target's Vulnerabilities
            {
                dam *= 2; //multiply damage by 2
                Info.Out("VULNERABLE! ", Color.DarkRed); //display info
            }
            if (Resistances.Any(r => r == dt)) //if damagetype is in target's Resistances
            {
                dam /= 2; //divide damage by 2
                Info.Out("RESISTANT! ", Color.DarkRed); //display info
            }
            if (Immunities.Any(i => i == dt)) //if damagetype is in target's Immunities
            {
                dam = 0; //set damage to 0
                Info.Out("IMMUNE! ", Color.DarkRed); //display info
            }
            return dam;
        }
        public void DropItem(Thing thing)
        {
            thing.X = X; //set position to current position
            thing.Y = Y;
            GameData.CurrentLevel.Items.Add(thing); //add to level
            Things.Remove(thing); //remove from inventory
            if (Creature == CreatureType.Player || Spot.Seen == SeenType.Seen)
            {
                Info.OutL($"{Name} dropped {thing.Name}", Color.ForestGreen);
            }
        }
        public void DropGold()
        {
            Thing item = new()
            {
                Name = "Gold"
            };
            Description = $"a pile of {Value} gold pieces";
            item.Value = Value;
            item.X = X;
            item.Y = Y;
            item.Spot.Type = SpotType.Takable | SpotType.Gold;
            GameData.CurrentLevel.Items.Add(item);
        }
        public bool TakeHit(int damage, DamageType type)
        {
            int dam = ApplyVulResImm(damage, type);
            return TakeHit(dam);
        }
        public bool TakeHit(int damage)
        {
            HP -= damage; //subtract damage from HP
            if (HP <= 0) //if HP is 0 or less
            {
                Info.OutL($"{Name} is dead!", Color.DarkRed); //display info
                Dead = true; //set Dead to true
                HP = 0; //set HP to 0
                if (Value > 0) //if we have any money
                {
                    DropGold(); //drop the gold
                }
                while (Things.Count > 0) //while we have things in inventory
                {
                    DropItem(Things[0]); //drop the first one
                }
                GameData.Targetted = false; //we are no longer targetted
            }
            return Dead; //return Dead
        }
        public int MakeSave(AbilityType ability, int savedc)
        {
            int r = Rand.D(20);
            if (r == 20) return 20;
            if (r == 1) return -1;
            r += ability switch
            {
                AbilityType.STR => STRmod,
                AbilityType.DEX => DEXmod,
                AbilityType.CON => CONmod,
                AbilityType.INT => INTmod,
                AbilityType.WIS => WISmod,
                AbilityType.CHA => CHAmod,
                _ => 0,
            };
            if (r >= savedc) return 1;
            else return 0;
        }
        public List<Thing> VisibleCreatures()
        {
            List<Thing> things = new(); //storage for list of things we will return
            List<Point> seen = GameData.CurrentLevel.GetVisible(this); //get all visible points
            foreach (var point in seen) //for each visible point
            {
                foreach (var creature in GameData.CurrentLevel.Creatures) //for every creature
                {
                    if (creature.IsAt(point.X, point.Y)) //if it is at point
                    {
                        things.Add(creature); //add creature to list (NOT a copy!)
                    }
                }
                if (GameData.Player.IsAt(point.X, point.Y)) //if player is at point
                {
                    things.Add(GameData.Player); //add player to list (NOT a copy!)
                }
            }
            return things; //return list
        }
        public Room? InRoom() => GameData.CurrentLevel.Rooms.FirstOrDefault(r => r.Inside(X, Y));
        public Door? InDoor() => GameData.CurrentLevel.Doors.FirstOrDefault(d => d.Inside(X, Y));
        public Path? InPath() => GameData.CurrentLevel.Paths.FirstOrDefault(p => p.Inside(X, Y));
        public static bool CanMove(Point point, bool isplayer) => CanMove(point.X, point.Y, isplayer);
        public static bool CanMove(int x, int y, bool isplayer)
        {
            if (GameData.CurrentLevel.Rooms.Any(r => r.Inside(x, y))) //if move is inside a room
            {
                if (isplayer) //if we are the player
                {
                    return true; //then we can move there
                }
                else //otherwise (not the player)
                {
                    if (GameData.CurrentLevel.Rooms.Any(r => r.Inside(x,y) && r.Type != RoomType.Safe)) //if it is not a safe room
                    {
                        return true; //then we can move there
                    }
                }
            }
            if (GameData.CurrentLevel.Doors.Any(d => d.Inside(x, y))) //if move is inside a door
            {
                return true; //then we can move there
            }
            if (GameData.CurrentLevel.Paths.Any(p => p.Inside(x, y))) //if move is inside a path
            {
                return true; //then we can move there
            }
            return false; //otherwise we can't move there
        }
        public int GetDistance(Thing target) => (int)Math.Sqrt(Math.Pow(target.X - X, 2) + Math.Pow(target.Y - Y, 2));
        public int GetDistance(Point point) => (int)Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2));
        public bool CanAttack(Thing target)
        {
            if (NumberAttacks == 0 || Attacks.Count == 0) //if we have no attacks
            {
                return false; //then we cannot attack
            }
            int range = GetDistance(target); //get distance to target
            if (range <= 1) //if range is less than or equal to 1,
            {
                range = 0; //set range to 0 (melee)
            }
            bool canattack = Attacks.Any(a => a.MaxRange >= range); //does any attack in Attacks have a Range >= range?
            return canattack; //return answer
        }
        public bool ShouldAttack(Thing target)
        {
            if (!target.IsCreature()) //if target is not a creature
            {
                return false; //then no, we can't attack
            }
            if (target.Creature == CreatureType.Player) //if target is player
            {
                if (HasEffect(Effect.Charmed))
                {
                    return false;  //no, we can't attack
                }
                return Hostile; //just return Hostile
            }
            if (IsEvil()) //if we are evil
            {
                if (target.IsGood() || target.IsNeutral()) //if target is not evil
                {
                    return true; //then yes, we can attack
                }
            }
            if (IsGood() && target.IsEvil()) //if we are good and target is evil
            {
                return true; //then yes, we can attack
            }
            ///
            ///TODO: We need more decision making here, like for instance, if we are attacked, we should bypass this method
            ///
            return false; //otherwise, no, we can't attack
        }
        public bool IsEvil()
        {
            return Alignment switch //return true for any evil alignment, false otherwise
            {
                Alignment.ChaoticEvil => true,
                Alignment.NeutralEvil => true,
                Alignment.LawfulEvil => true,
                _ => false,
            };
        }
        public bool IsGood()
        {
            return Alignment switch //return true for any good alignment, false otherwise
            {
                Alignment.ChaoticGood => true,
                Alignment.NeutralGood => true,
                Alignment.LawfulGood => true,
                _ => false,
            };
        }
        public bool IsNeutral()
        {
            return Alignment switch //return true for any neutral alignment, false otherwise
            {
                Alignment.ChaoticNeutral => true,
                Alignment.TrueNeutral => true,
                Alignment.LawfulNeutral => true,
                _ => false,
            };
        }
        public void Heal(string amount)
        {
            int amt = Rand.ParseAndRoll(amount, false); //roll for amount of healing
            int hpdown = MaxHP - HP; //get amount of damage
            if (amt > hpdown) //if amount of healing is more than the damage
            {
                amt = hpdown; //set amount of healing to amount of damage
            }
            HP += amt; //add amount of healing to hit points
            Info.OutL($"{Name} healed {amt} points for {HP} of {MaxHP} hit points!", Color.DarkGreen); //display info
        }
        public bool QuaffPotion(Thing potion)
        {
            if (potion.Type.HasFlag(SpotType.Potion)) //if potion IS a potion
            {
                switch (potion.Potion) //switch by potion type
                {
                    case PotionType.Healing: //if healing
                        Spell.DoSpell(this, SpellType.Healing); //cast spell of healing
                        return true; //report success
                    case PotionType.Invisibility: //if invisibility
                        Spell.DoSpell(this, SpellType.Invisibility); //cast spell of invisibility
                        return false; //report failure
                    case PotionType.ExtraHealing: //if extrahealing
                        Spell.DoSpell(this, SpellType.ExtraHealing); //cast spell of extrahealing
                        return true; //report success
                }
            }
            return false; //report failure
        }
        public bool ReadScroll(Thing scroll)
        {
            if (scroll.Type.HasFlag(SpotType.Scroll)) //if scroll IS a scroll
            {
                switch (scroll.Scroll) //switch by scroll type
                {
                    case ScrollType.MagicMapping: //if magic mapping
                        Spell.DoSpell(this, SpellType.MagicMapping); //cast spell of magic mapping
                        return true; //report success
                    case ScrollType.Confusion: //if confusion
                        Spell.DoSpell(this, SpellType.ConfuseMonsters); //cast spell of confuse monsters
                        return false; //report failure
                    case ScrollType.Sleep: //if sleep
                        Spell.DoSpell(this, SpellType.SleepMonsters); //cast spell of sleep monsters
                        return false; //report failure
                }
            }
            return false; //report failure
        }
        public bool TranscribeScroll(Thing scroll)
        {
            if (Rand.D(20) + (INT / 2 - 5) > scroll.Level + 10) //if attempt was successful
            {
                Spell spell = new() //make new spell
                {
                    Name = scroll.Name, //scroll's name will do for Name
                    Level = scroll.Level, //scroll's level will do for Level
                    Effect = scroll.Scroll switch //convert ScrollType to Effect
                    {
                        ScrollType.MagicMapping => SpellType.MagicMapping,
                        ScrollType.Confusion => SpellType.ConfuseMonsters,
                        ScrollType.Sleep => SpellType.SleepMonsters,
                        _ => SpellType.None
                    }
                };
                if (scroll.Scroll == ScrollType.Sleep) //if scroll of sleep
                {
                    spell.Area = 20; //area is 20
                    spell.Duration = 6; //duration is 6 rounds
                    spell.SaveDC = 15; //save DC is 15
                    spell.Ability = AbilityType.CON; //make it a con save
                }
                if (scroll.Scroll == ScrollType.Confusion) //if scroll of confusion
                {
                    spell.Area = 40; //area is 40
                    spell.Duration = 6; //duration is 6 rounds
                    spell.SaveDC = 15; //save DC is 15
                    spell.Ability = AbilityType.INT; //make it an int save
                }
                SpellList.Add(new(spell)); //add spell to spell list
                Info.OutL($"{Name} successfully transcribed the {scroll.Name}!", Color.DarkMagenta); //display info
                return true; //report success
            }
            else //otherwise (attempt was failure)
            {
                Info.OutL($"{Name} did not manage to transcribe the {scroll.Name}.", Color.DarkMagenta); //display info
                return false; //report failure
            }
        }
        public bool AnalyzePotion(Thing potion)
        {
            if (Rand.D(20) + INT / 2 - 5 > potion.Level + 18) //if attempt was successful
            {
                Spell spell = new() //make new spell
                {
                    Name = potion.Name, //potion's name will do for name
                    Level = potion.Level, //potion's level will do for level
                    Effect = potion.Potion switch //convert from PotionType to Effect
                    {
                        PotionType.Healing => SpellType.Healing,
                        PotionType.ExtraHealing => SpellType.ExtraHealing,
                        PotionType.Invisibility => SpellType.Invisibility,
                        _ => SpellType.None
                    }
                };
                if (potion.Potion == PotionType.Invisibility) //if it was a potion of invisibility
                {
                    spell.Duration = 10; //spell duration is 10 rounds (1 min)
                }
                SpellList.Add(new(spell)); //add spell to spell list
                Info.OutL($"{Name} successfully analyzed {potion.Name}!", Color.DarkMagenta); //display info
                return true; //report success
            }
            else //otherwise (attempt was failure)
            {
                Info.OutL($"{Name} did not manage to analyze {potion.Name}.", Color.DarkMagenta); //display info
                return false; //report failure
            }
        }
        public Thing? WearingArmor()
        {
            foreach (var item in Things) //for each item in inventory
            {
                if (item.Type.HasFlag(SpotType.Armor) && item.Equipped) //if item is armor and equipped
                {
                    return item; //return armor
                }
            }
            return null; //return null
        }
        public void RemoveArmor(Thing armor)
        {
            armor.Equipped = false; //set flag to not equipped
            AC = 10 + (DEX / 2) - 5; //compute AC
            Info.OutL($"{Name} took off {armor.Name} and now has an AC of {AC}.", Color.DarkRed);
        }
        public void WearArmor(Thing armor)
        {
            if (armor.Type.HasFlag(SpotType.Armor)) //if armor IS armor
            {
                Thing? oldarmor = WearingArmor(); //get old armor
                if (oldarmor != null) //if old armor exists
                {
                    Info.OutL($"{Name} is wearing {oldarmor.Name} and will have to take it off.");
                    RemoveArmor(oldarmor); //take it off
                }
                armor.Equipped = true;
                AC = armor.Armor;
                if (armor.Weight < 50) //if armor is medium or light
                {
                    int db = (DEX / 2) - 5; //compute dex bonus
                    if (armor.Weight > 15) //if armor is medium armor
                    {
                        if (db > 2) //if dex bonus greater than 2
                        {
                            db = 2; //set dex bonus to 2
                        }
                    }
                    AC += db; //add dex bonus to AC
                }
                Info.OutL($"{Name} is now wearing {armor.Name} and now has an AC of {AC}.", Color.DarkSeaGreen); //display info
            }
        }
        public void EquipWeapon(Thing weapon)
        {
            if (weapon.Type.HasFlag(SpotType.Weapon)) //if weapon IS a weapon
            {
                Info.OutL(weapon.Describe());
                Attack att = new(this, weapon);
                Attacks.Add(att); //add attack for weapon
                Info.OutL(att.Describe());
                weapon.Equipped = true;
                Info.OutL($"{Name} just equipped a {weapon.Name}.", Color.Navy); //display info
            }
            Info.OutL($"{Name} now has {Attacks.Count} attacks.");
        }
        public Attack? FindWeapon(Thing weapon)
        {
            foreach (var attack in Attacks) //for each attack
            {
                if (attack.IsFromWeapon(weapon)) //if attack is from weapon
                {
                    return attack; //return attack
                }
            }
            return null; //return null since we didn't find it
        }
        public void UnequipWeapon(Thing weapon)
        {
            if (weapon.Type.HasFlag(SpotType.Weapon)) //if weapon IS a weapon
            {
                if (weapon.Equipped) //if weapon is equipped
                {
                    Attack? oldattack = FindWeapon(weapon); //find attack using weapon
                    if (oldattack != null) //if found
                    {
                        Attacks.Remove(oldattack); //remove that attack
                    }
                    weapon.Equipped = false; //set weapon to not equipped
                    Info.OutL($"{Name} unequipped {weapon.Name}.", Color.DarkGreen); //display info
                }
            }
        }
        public void GetLight(Thing light)
        {
            if (light.Type.HasFlag(SpotType.LightSource)) //if light IS a lightsource
            {
                foreach (var thing in Things) //for each thing in inventory
                {
                    if (thing.Type.HasFlag(SpotType.LightSource) && thing.Equipped)
                    {
                        thing.Equipped = false;
                    }
                }
                LightRadius = light.LightRadius;
                light.Equipped = true;
            }
        }
        public void GetGold(Thing gold)
        {
            if (gold.Type.HasFlag(SpotType.Gold)) //if gold IS gold
            {
                Value += gold.Value; //add amount to total
                Things.Remove(gold); //take out of inventory
            }
        }
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
    }
    public enum Alignment
    {
        None,
        ChaoticGood,
        ChaoticNeutral,
        ChaoticEvil,
        NeutralGood,
        TrueNeutral,
        NeutralEvil,
        LawfulGood,
        LawfulNeutral,
        LawfulEvil,
    }
    public enum PotionType
    {
        None,
        Healing,
        Invisibility,
        ExtraHealing,
        //add more as needed
    }
    public enum ScrollType
    {
        None,
        MagicMapping,
        Confusion,
        Sleep,
        //add more as needed
    }
    public enum Effect
    {
        None,
        Confusion,
        Sleep,
        Slow,
        Speed,
        Blind,
        Charmed,
        Deafened,
        Frightened,
        Grappled,
        Incapacitated,
        Invisible,
        Paralyzed,
        Petrified,
        Poisoned,
        Prone,
        Restrained,
        Stunned,
        Unconscious,
        //add more as needed
    }
    public enum DamageType
    {
        None,
        Bludgeoning,
        Piercing,
        Slashing,
        Fire,
        Cold,
        Acid,
        Electricity,
        Necrotic,
        Poison,
        Force,
        //add more as needed
    }
    public enum CreatureType
    {
        None,
        Animal,
        Monster,
        NPC,
        Player,
        //add more as needed
    }
    public enum  ClassType
    {
        None,
        Artificer,
        Barbarian,
        Bard,
        Cleric,
        Druid,
        Fighter,
        Monk,
        Paladin,
        Ranger,
        Rogue,
        Sorcerer,
        Warlock,
        Wizard,
        Explorer,
        //add more as needed
    }
    public enum RaceType
    {
        Human,
        Dragonborn,
        HighElf,
        WoodElf,
        DrowElf,
        HillDwarf,
        MountainDwarf,
        ForestGnome,
        RockGnome,
        LightfootHalfling,
        StoutHalfling,
        HalfOrc,
        HalfElf,
        Tiefling,
        God,
        Other,
        //add more as needed
    }

    public enum MonsterType
    {
        None,
        Kobold_Orc,
        //add more as needed
    }

    public class RaceData
    {
        public RaceType Race { get; set; } = RaceType.Other; //default to other
        public int DarkVision { get; set; } = 0;
        public int STRBonus { get; set; } = 0; //for racial bonuses
        public int DEXBonus { get; set; } = 0;
        public int CONBonus { get; set; } = 0;
        public int INTBonus { get; set; } = 0;
        public int WISBonus { get; set; } = 0;
        public int CHABonus { get; set; } = 0;

        private readonly Dictionary<RaceType, int> racesight = new() { { RaceType.Human, 0 }, { RaceType.Dragonborn, 0 },
            { RaceType.HighElf, 60 }, { RaceType.WoodElf, 60 }, { RaceType.DrowElf, 120 }, { RaceType.HillDwarf, 60 },
            { RaceType.MountainDwarf, 60 }, { RaceType.ForestGnome, 60 }, { RaceType.RockGnome, 60 }, { RaceType.LightfootHalfling, 0 },
            { RaceType.StoutHalfling, 0 }, { RaceType.HalfOrc, 60 }, { RaceType.HalfElf, 60 }, { RaceType.Tiefling, 60 },
            { RaceType.God, 300 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> racestr = new() { { RaceType.Human, 1 }, { RaceType.Dragonborn, 2 },
            { RaceType.HighElf, 0 }, { RaceType.WoodElf, 0 }, { RaceType.DrowElf, 0 }, { RaceType.LightfootHalfling, 0 },
            { RaceType.StoutHalfling, 0 }, { RaceType.ForestGnome, 0 }, { RaceType.RockGnome, 0 }, { RaceType.HalfElf, 0 },
            { RaceType.HalfOrc, 2 }, { RaceType.Tiefling, 0 }, { RaceType.HillDwarf, 0 }, { RaceType.MountainDwarf, 2 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> racedex = new() { { RaceType.HighElf, 2 }, { RaceType.WoodElf, 2 },
            { RaceType.DrowElf, 2 }, { RaceType.LightfootHalfling, 2 }, { RaceType.StoutHalfling, 2 }, { RaceType.Human, 1 },
            { RaceType.Dragonborn, 0 }, { RaceType.ForestGnome, 1 }, { RaceType.RockGnome, 0 }, { RaceType.HalfElf, 0 },
            { RaceType.HalfOrc, 0 }, { RaceType.Tiefling, 0 }, { RaceType.HillDwarf, 0 }, { RaceType.MountainDwarf, 0 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> racecon = new() { { RaceType.HighElf, 0 }, { RaceType.WoodElf, 0 },
            { RaceType.DrowElf, 0 }, { RaceType.LightfootHalfling, 0 }, { RaceType.StoutHalfling, 1 }, { RaceType.Human, 1 },
            { RaceType.Dragonborn, 0 }, { RaceType.ForestGnome, 0 }, { RaceType.RockGnome, 1 }, { RaceType.HalfElf, 2 },
            { RaceType.HalfOrc, 1 }, { RaceType.Tiefling, 0 }, { RaceType.HillDwarf, 2 }, { RaceType.MountainDwarf, 2 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> raceint = new() { { RaceType.HighElf, 1 }, { RaceType.WoodElf, 0 },
            { RaceType.DrowElf, 0 }, { RaceType.LightfootHalfling, 0 }, { RaceType.StoutHalfling, 0 }, { RaceType.Human, 1 },
            { RaceType.Dragonborn, 0 }, { RaceType.ForestGnome, 2 }, { RaceType.RockGnome, 2 }, { RaceType.HalfElf, 0 },
            { RaceType.HalfOrc, 0 }, { RaceType.Tiefling, 1 }, { RaceType.HillDwarf, 0 }, { RaceType.MountainDwarf, 0 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> racewis = new() { { RaceType.HighElf, 0 }, { RaceType.WoodElf, 1 },
            { RaceType.DrowElf, 0 }, { RaceType.LightfootHalfling, 0 }, { RaceType.StoutHalfling, 0 }, { RaceType.Human, 1 },
            { RaceType.Dragonborn, 0 }, { RaceType.ForestGnome, 0 }, { RaceType.RockGnome, 0 }, { RaceType.HalfElf, 0 },
            { RaceType.HalfOrc, 0 }, { RaceType.Tiefling, 0 }, { RaceType.HillDwarf, 1 }, { RaceType.MountainDwarf, 0 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        private readonly Dictionary<RaceType, int> racecha = new() { { RaceType.HighElf, 0 }, { RaceType.WoodElf, 0 },
            { RaceType.DrowElf, 1 }, { RaceType.LightfootHalfling, 1 }, { RaceType.StoutHalfling, 0 }, { RaceType.Human, 1 },
            { RaceType.Dragonborn, 1 }, { RaceType.ForestGnome, 0 }, { RaceType.RockGnome, 0 }, { RaceType.HalfElf, 2 },
            { RaceType.HalfOrc, 0 }, { RaceType.Tiefling, 2 }, { RaceType.HillDwarf, 0 }, { RaceType.MountainDwarf, 0 },
            { RaceType.God, 15 }, { RaceType.Other, 0 } };
        public RaceData(RaceType race)
        {
            Race = race;
            DarkVision = racesight[Race];
            STRBonus = racestr[Race];
            DEXBonus = racedex[Race];
            CONBonus = racecon[Race];
            INTBonus = raceint[Race];
            WISBonus = racewis[Race];
            CHABonus = racecha[Race];
        }
    }
    public static class GameData
    {
        public static Level CurrentLevel { get { return _currentlevel; } set { _currentlevel = value; } }
        private static Level _currentlevel = new();
        public static Thing Player { get { return _player; } set { _player = value; } }
        private static Thing _player = new();
        public static Attack Melee { get { return _melee; } set { _melee = value; } }
        private static Attack _melee = new() { Name = "_none_" };
        public static Attack Ranged { get { return _ranged; } set { _ranged = value; } }
        private static Attack _ranged = new() { Name = "_none_" };
        public static Thing? NewThing { get { return _newthing; } set { _newthing = value; } }
        private static Thing? _newthing;
        public static Attack? NewAttack { get { return _newattack; } set { _newattack = value; } }
        private static Attack? _newattack;
        public static bool Targetted { get { return _targetted; } set { _targetted = value; } }
        private static bool _targetted = false;
    }
    public class Level
    {
        LevGen Gen { get; set; } = new(150, 150);
        public List<Room> Rooms { get; set; } = new();
        public List<Door> Doors { get; set; } = new();
        public List<Path> Paths { get; set; } = new();
        public List<Thing> Items { get; set; } = new();
        public List<Thing> Creatures { get; set; } = new();
        public Map Map { get; set; } = new();

        private readonly int wide;
        private readonly int high;

        public Level()
        {
            wide = 150;
            high = 150;
            Gen = new(wide, high);
            Rooms = Gen.Rooms;
            Doors = Gen.Doors;
            Paths = Gen.Paths;
        }
        public Level(int wide, int high)
        {
            this.wide = wide;
            this.high = high;
            Gen = new(wide, high);
            Rooms = Gen.Rooms;
            Doors = Gen.Doors;
            Paths = Gen.Paths;
        }
        public Level(int wide, int high, int numrooms)
        {
            this.wide = wide;
            this.high = high;
            Gen = new(wide, high, numrooms);
            Rooms = Gen.Rooms;
            Doors = Gen.Doors;
            Paths = Gen.Paths;
        }
        public Level(int wide, int high, int numrooms, int minroom, int maxroom)
        {
            this.wide = wide;
            this.high = high;
            Gen = new(wide, high, numrooms, minroom, maxroom);
            Rooms = Gen.Rooms;
            Doors = Gen.Doors;
            Paths = Gen.Paths;
        }
        public Level(StreamReader sr) => Load(sr);
        public Map MakeMap()
        {
            Map = new(wide, high);
            return Map;
        }
        public List<Point> GetVisible(Thing source)
        {
            int radius = source.Light(); //get radius of light
            List<Point> circle = GetCircumference(source.X, source.Y, radius); //get our circumference
            List<Point> isseen = new(); //list of points that are seen
            List<Point> isnotseen = new(); //list of points that are not seen
            List<Point> tested = new(); //list of points that have been tested
            foreach (var spot in circle) //for each point in the circumference
            {
                Point current = new(source.X, source.Y); //start at player position
                int dx = Math.Abs(spot.X - current.X);
                int dy = Math.Abs(spot.Y - current.Y);
                int sx = (current.X < spot.X) ? 1 : -1; //set direction to move in X
                int sy = (current.Y < spot.Y) ? 1 : -1; //set direction to move in Y
                int err = Math.Abs(dx) - Math.Abs(dy); //calculate error
                while (true) //do the following until a "break"
                {

                    if (!tested.Any(t => t.X == current.X && t.Y == current.Y)) //if we haven't tested this point
                    {
                        if (IsVisible(current)) //if it is visible
                        {
                            if (!isseen.Any(s => s.X == current.X && s.Y == current.Y)) //if not in isseen list
                            {
                                isseen.Add(current); //add it to isseen list
                            }
                        }
                        else //if it is not visible
                        {
                            if (!isnotseen.Any(s => s.X == current.X && s.Y == current.Y)) //if not in isnotseen list)
                            {
                                isnotseen.Add(current); //add it to isnotseen list
                            }
                        }
                        tested.Add(current); //add to tested
                    }
                    if (isnotseen.Any(s => s.X == current.X && s.Y == current.Y)) //if it is not visible
                    {
                        break; //break because nothing beyond it will be visible
                    }
                    if (current.X == spot.X && current.Y == spot.Y) //if we have reached the spot
                    {
                        break; //break because we are done
                    }
                    int e2 = 2 * err; //calculate error * 2
                    if (e2 > -dy) //if more than negative distance in Y
                    {
                        err -= dy; //subtract distance in y from error
                        current.X += sx; //move in x direction
                    }
                    if (e2 < dx) //if less than distance in x
                    {
                        err += dx; //add distance in x to error
                        current.Y += sy; //move in y direction
                    }
                }
            }
            return isseen; //return isseen list
        }
        private bool IsVisible(Point p)
        {
            if (Rooms.Any(r => r.Inside(p))) //if any room contains point
            {
                return true; //return true (visible)
            }
            if (Doors.Any(d => d.Inside(p))) //if any door contains point
            {
                return true; //return true (visible)
            }
            if (Paths.Any(path => path.Inside(p))) //if any path contains point
            {
                return true; //return true (visible)
            }
            return false; //otherwise return false (not visible)
        }
        public static List<Point> ExpandVisible(List<Point> seen)
        {
            List<Point> expanded = new(); //make expanded list
            foreach (var point in seen) //for each point
            {
                for (int x = point.X - 1; x <= point.X + 1; x++) //from left to right
                {
                    for (int y = point.Y - 1; y <= point.Y + 1; y++) //from top to bottom
                    {
                        if (!expanded.Any(e => e.X == x && e.Y == y)) //if not in list
                        {
                            expanded.Add(new(x, y)); //add it in
                        }
                    }
                }
            }
            return expanded; //return expanded list
        }
        private static List<Point> GetCircumference(int x, int y, int radius)
        {
            List<Point> circumference = new(); //make a new list for our circumference points
            for (int angle = 0; angle < 360; angle++) //iterate through 360 degrees
            {
                double radians = angle * Math.PI / 180; //convert to radians
                int xx = (int)Math.Round(x + radius * Math.Cos(radians)); //compute x
                int yy = (int)Math.Round(y + radius * Math.Sin(radians)); //compute y
                if (!circumference.Any(c => c.X == xx && c.Y == yy)) //if we don't already have this point
                {
                    circumference.Add(new(xx, yy)); //add it to the list
                }
            }
            return circumference; //return the list
        }
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
    }
    public class MapSpot
    {
        public MapType Type { get; set; } = MapType.Stone;
        public Spot Spot { get; set; } = new();
        public Point Pos { get; set; } = Point.Empty;
    }
    public class Map
    {
        public int Wide { get; set; } = 150;
        public int High { get; set; } = 150;
        public List<MapSpot> MapSpots { get; set; } = new();
        public Map()
        {
            Recalc();
        }

        public Map(int wide, int high)
        {
            Wide = wide;
            High = high;
            Recalc();
        }
        public void Recalc()
        {
            for (int y = 0; y < High; y++)
            {
                for (int x = 0; x < Wide; x++)
                {
                    MapSpot mapspot = new()
                    {
                        Pos = new(x, y),
                        Type = MapType.Stone,
                        Spot = new() { Type = SpotType.None, Seen = SeenType.NotSeen }
                    };
                    MapSpots.Add(mapspot);
                }
            }
            if (GameData.CurrentLevel == null)
            {
                return;
            }
            foreach (var room in GameData.CurrentLevel.Rooms)
            {
                for (int y = room.T; y <= room.B; y++)
                {
                    for (int x = room.L; x <= room.R; x++)
                    {
                        ///
                        ///TODO: We could probably use some error handling here
                        ///
                        int i = GetIndex(x, y);
                        if (i == -1)
                        {
                            Info.DebugL($"Got invalid index for {x}, {y} in a room!");
                            return;
                        }
                        SpotType wall = SpotType.Solid | SpotType.Room | SpotType.Wall;
                        if (x == room.L && y == room.T)
                        {
                            MapSpots[i].Type = MapType.ULCorner;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (x == room.L && y == room.B)
                        {
                            MapSpots[i].Type = MapType.LLCorner;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (x == room.R && y == room.T)
                        {
                            MapSpots[i].Type = MapType.URCorner;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (x == room.R && y == room.B)
                        {
                            MapSpots[i].Type = MapType.LRCorner;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (x == room.L)
                        {
                            MapSpots[i].Type = MapType.LeftWall;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (x == room.R)
                        {
                            MapSpots[i].Type = MapType.RightWall;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (y == room.T)
                        {
                            MapSpots[i].Type = MapType.TopWall;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else if (y == room.B)
                        {
                            MapSpots[i].Type = MapType.BottomWall;
                            MapSpots[i].Spot.Type = wall;
                        }
                        else
                        {
                            MapSpots[i].Type = MapType.Room;
                            MapSpots[i].Spot.Type = SpotType.Room;
                        }
                    }
                }
            }
            foreach (var path in GameData.CurrentLevel.Paths)
            {
                int i = GetIndex(path.X, path.Y);
                if (i == -1)
                {
                    Info.DebugL($"Got invalid index for {path.X}, {path.Y} in a path!");
                    return;
                }
                MapSpots[i].Type = MapType.Path;
                MapSpots[i].Spot.Type = SpotType.Path;
            }
            foreach (var door in GameData.CurrentLevel.Doors)
            {
                int i = GetIndex(door.X, door.Y);
                if (i == -1)
                {
                    Info.DebugL($"Got invalid index for {door.X}, {door.Y} in a door!");
                    return;
                }
                switch (door.GetSide())
                {
                    case 1: //left
                        MapSpots[i].Type = MapType.LeftDoor; break;
                    case 2: //right
                        MapSpots[i].Type = MapType.RightDoor; break;
                    case 3: //top
                        MapSpots[i].Type = MapType.TopDoor; break;
                    case 4: //bottom
                        MapSpots[i].Type = MapType.BottomDoor; break;
                }
                MapSpots[i].Spot.Type = SpotType.Path | SpotType.Room | SpotType.Door;
            }
        }
        public int GetIndex(int x, int y) => MapSpots.FindIndex(m => m.Pos.X == x && m.Pos.Y == y);
    }
    public enum MapType
    {
        Stone,
        Path,
        LeftDoor,
        RightDoor,
        TopDoor,
        BottomDoor,
        Room, //interior
        LeftWall,
        RightWall,
        TopWall,
        BottomWall,
        ULCorner,
        URCorner,
        LLCorner,
        LRCorner,
    }
    public static class Info
    {
        private static RichTextBox? RTB;
        public static void SetRTB(RichTextBox rtb) => RTB = rtb;
        public static void ClearRTB() => RTB?.Clear();
        public static void Out(string s)
        {
            if (RTB != null)
            {
                RTB.AppendText(s);
                RTB.SelectionStart = RTB.Text.Length;
                RTB.ScrollToCaret();
            }
        }
        public static void Out(string s, Color color)
        {
            if (RTB != null)
            {
                RTB.SelectionColor = color;
                Out(s);
                RTB.SelectionColor = Color.Black;
            }
        }
        public static void OutL(string s) => Out($"{s}\n");
        public static void OutL(string s, Color color) => Out($"{s}\n", color);
        public static void Debug(string s)
        {
            if (RTB != null)
            {
                RTB.SelectionColor = Color.DarkRed;
                Out($"DEBUG: {s}");
                RTB.SelectionColor = Color.Black;
            }
        }
        public static void DebugL(string s) => Debug($"{s}\n");
        public static void DEBUG(string s)
        {
            if (RTB != null)
            {
                Font oldfont = RTB.SelectionFont;
                RTB.SelectionFont = new("Arial Black", 10, FontStyle.Bold);
                RTB.SelectionColor = Color.DarkRed;
                OutL(s);
                RTB.SelectionColor = Color.Black;
                RTB.SelectionFont = oldfont;
            }
        }
        public static void DEBUG(string s, SeenType seen)
        {
            if (seen == SeenType.Seen)
            {
                DEBUG(s);
            }
        }
    }
    public static class Rand
    {
        private static readonly Random rand = new();
        public static int D(int die) => rand.Next(die) + 1;
        public static int D(int num, int die)
        {
            int sum = 0;
            for (int i = 0; i < num; i++)
            {
                sum += D(die);
            }
            return sum;
        }
        public static int D(int die, bool crit)
        {
            int sum = D(die);
            if (crit)
            {
                sum += D(die);
            }
            return sum;
        }
        public static int D(int num, int die, bool crit)
        {
            int sum = 0;
            for (int i = 0; i < num; i++)
            {
                sum += D(die, crit);
            }
            return sum;
        }
        public static int ParseAndRoll(string input) => ParseAndRoll(input, false);
        public static int ParseAndRoll(string input, bool crit)
        {
            string pattern = @"(?<=[d+-])|(?=[d+-])"; // Split on 'd', '+', or '-'
            List<string> substrings = Regex.Split(input, pattern).ToList();
            if (Regex.IsMatch(substrings[0], pattern))
            {
                substrings.Insert(0, "1");
            }
            if (Regex.IsMatch(substrings[^1], pattern))
            {
                substrings.Add("0");
            }
            for (int i = 0; i < substrings.Count; i++)
            {
                if (substrings[i] == "-")
                {
                    substrings[i] = "+";
                    substrings[i + 1] = $"-{substrings[i + 1]}";
                }
                if (substrings[i] == "d")
                {
                    _ = int.TryParse(substrings[i - 1], out int nd);
                    _ = int.TryParse(substrings[i + 1], out int dt);
                    int nv = D(nd, dt, crit);
                    substrings[i - 1] = $"{nv}";
                    substrings.RemoveAt(i + 1);
                    substrings.RemoveAt(i);
                }
            }
            List<string> sub2 = substrings.Where(s => s != "+").ToList();
            int total = 0;
            foreach (var str in sub2)
            {
                _ = int.TryParse(str, out int val);
                total += val;
            }
            return total;
        }
        public static int UpTo(int num) => rand.Next(num);
        public static int FromTo(int min, int max) => rand.Next(min, max + 1);
        public static int FromUpTo(int min, int max) => rand.Next(min, max);
    }
    public static class Data
    {
        public static readonly Dictionary<string, SpotType> spottypes = new() { { "None", SpotType.None }, { "Solid", SpotType.Solid },
            { "Room", SpotType.Room }, { "Wall", SpotType.Wall }, { "Door", SpotType.Door }, { "Path", SpotType.Path },
            { "Stairs Up", SpotType.StairsUp }, { "Stairs Down", SpotType.StairsDown }, { "Takable", SpotType.Takable },
            { "Gold", SpotType.Gold }, { "Potion", SpotType.Potion }, { "Scroll", SpotType.Scroll }, { "Weapon", SpotType.Weapon },
            { "Armor", SpotType.Armor }, { "Light Source", SpotType.LightSource }, { "Creature", SpotType.Creature }, { "Animal", SpotType.Animal },
            { "Monster", SpotType.Monster }, { "NPC", SpotType.NPC }, { "Player", SpotType.Player } };
        public static readonly Dictionary<string, SeenType> seentypes = new() { { "Hidden", SeenType.NotSeen }, { "Remembered", SeenType.Remembered },
            { "Seen", SeenType.Seen } };
        public static readonly Dictionary<string, Alignment> alignments = new() { { "None", Alignment.None },
            { "Chaotic Good", Alignment.ChaoticGood }, { "Chaotic Neutral", Alignment.ChaoticNeutral }, { "Chaotic Evil", Alignment.ChaoticEvil },
            { "Neutral Good", Alignment.NeutralGood }, { "True Neutral", Alignment.TrueNeutral }, { "Neutral Evil", Alignment.NeutralEvil },
            { "Lawful Good", Alignment.LawfulGood }, { "Lawful Neutral", Alignment.LawfulNeutral }, { "Lawful Evil", Alignment.LawfulEvil } };
        public static readonly Dictionary<string, PotionType> potiontypes = new() { { "None", PotionType.None }, { "Healing", PotionType.Healing },
            { "Invisibility", PotionType.Invisibility }, { "Extra Healing", PotionType.ExtraHealing } };
        public static readonly Dictionary<string, ScrollType> scrolltypes = new() { { "None", ScrollType.None },
            { "Magic Mapping", ScrollType.MagicMapping }, { "Confusion", ScrollType.Confusion }, { "Sleep", ScrollType.Sleep } };
        public static readonly Dictionary<string, Effect> effecttypes = new() { { "None", Effect.None }, { "Confusion", Effect.Confusion }, { "Sleep", Effect.Sleep },
            { "Slow", Effect.Slow }, { "Speed", Effect.Speed }, { "Blind", Effect.Blind }, { "Charmed", Effect.Charmed }, { "Deafened", Effect.Deafened },
            { "Frightened", Effect.Frightened }, { "Grappled", Effect.Grappled }, { "Incapacitated", Effect.Incapacitated }, { "Invisible", Effect.Invisible },
            { "Paralyzed", Effect.Paralyzed }, { "Petrified", Effect.Petrified }, { "Poisoned", Effect.Poisoned }, { "Prone", Effect.Prone }, { "Restrained", Effect.Restrained },
            { "Stunned", Effect.Stunned }, { "Unconscious", Effect.Unconscious } };
        public static readonly Dictionary<string, DamageType> damagetypes = new() { { "None", DamageType.None },
            { "Bludgeoning", DamageType.Bludgeoning }, { "Piercing", DamageType.Piercing }, { "Slashing", DamageType.Slashing },
            { "Fire", DamageType.Fire }, { "Cold", DamageType.Cold }, { "Acid", DamageType.Acid }, { "Electricity", DamageType.Electricity } };
        public static readonly Dictionary<string, CreatureType> creaturetypes = new() { { "None", CreatureType.None }, { "Animal", CreatureType.Animal },
            { "Monster", CreatureType.Monster }, { "NPC", CreatureType.NPC }, { "Player", CreatureType.Player } };
        public static readonly Dictionary<string, ClassType> classtypes = new() { { "None", ClassType.None }, { "Artificer", ClassType.Artificer },
            { "Barbarian", ClassType.Barbarian }, { "Bard", ClassType.Bard }, { "Cleric", ClassType.Cleric }, { "Druid", ClassType.Druid },
            { "Fighter", ClassType.Fighter }, { "Monk", ClassType.Monk }, { "Paladin", ClassType.Paladin }, { "Ranger", ClassType.Ranger },
            { "Rogue", ClassType.Rogue }, { "Sorcerer", ClassType.Sorcerer }, { "Warlock", ClassType.Warlock },
            { "Wizard", ClassType.Wizard }, { "Explorer", ClassType.Explorer } };
        public static readonly Dictionary<string, RaceType> racetypes = new() { { "Other", RaceType.Other }, { "Human", RaceType.Human },
            { "Dragonborn", RaceType.Dragonborn }, { "High Elf", RaceType.HighElf }, { "Wood Elf", RaceType.WoodElf },
            { "Hill Dwarf", RaceType.HillDwarf }, { "Mountain Dwarf", RaceType.MountainDwarf }, { "Forest Gnome", RaceType.ForestGnome },
            { "Rock Gnome", RaceType.RockGnome }, { "God", RaceType.God },
            { "Lightfoot Halfling", RaceType.LightfootHalfling }, { "Stout Halfling", RaceType.StoutHalfling }, { "Drow", RaceType.DrowElf },
            { "Half Orc", RaceType.HalfOrc }, { "Half Elf", RaceType.HalfElf }, { "Tiefling", RaceType.Tiefling } };
        public static readonly Dictionary<string, MonsterType> monstertypes = new() { { "None", MonsterType.None },
            { "Kobold, Goblin, Orc", MonsterType.Kobold_Orc } };
        public static readonly Dictionary<RaceType, RaceData> racedata = new() { { RaceType.Other, new(RaceType.Other) },
            { RaceType.Dragonborn, new(RaceType.Dragonborn) }, { RaceType.DrowElf, new(RaceType.DrowElf) },
            { RaceType.ForestGnome, new(RaceType.ForestGnome) }, { RaceType.HalfElf, new(RaceType.HalfElf) },
            { RaceType.HalfOrc, new(RaceType.HalfOrc) }, { RaceType.HighElf, new(RaceType.HighElf) },
            { RaceType.HillDwarf, new(RaceType.HillDwarf) }, { RaceType.Human, new(RaceType.Human) },
            { RaceType.LightfootHalfling, new(RaceType.LightfootHalfling) }, { RaceType.MountainDwarf, new(RaceType.MountainDwarf) },
            { RaceType.RockGnome, new(RaceType.RockGnome) }, { RaceType.StoutHalfling, new(RaceType.StoutHalfling) },
            { RaceType.Tiefling, new(RaceType.Tiefling) }, { RaceType.WoodElf, new(RaceType.WoodElf) },
            { RaceType.God, new(RaceType.God) } };
        public static readonly Dictionary<ClassType, int> classhitdice = new() { { ClassType.Artificer, 8 }, { ClassType.Barbarian, 12 },
            { ClassType.Bard, 8 }, { ClassType.Cleric, 8 }, { ClassType.Druid, 8 }, { ClassType.Fighter, 10 }, { ClassType.Monk, 10 },
            { ClassType.Paladin, 10 }, { ClassType.Ranger, 10 }, { ClassType.Rogue, 8 }, { ClassType.Sorcerer, 6 },
            { ClassType.Warlock, 6 }, { ClassType.Wizard, 6 }, { ClassType.Explorer, 25 }, { ClassType.None, 8 } };
        public static readonly Dictionary<ClassType, string> classgold = new() { { ClassType.Artificer, "4d4" },
            { ClassType.Barbarian, "2d4" }, { ClassType.Bard, "5d4" }, { ClassType.Cleric, "5d4" }, { ClassType.Druid, "2d4" },
            { ClassType.Fighter, "5d4" }, { ClassType.Monk, "5d4" }, { ClassType.Paladin, "5d4" }, { ClassType.Ranger, "5d4" },
            { ClassType.Rogue, "4d4" }, { ClassType.Sorcerer, "3d4" }, { ClassType.Warlock, "4d4" }, { ClassType.Wizard, "4d4" },
            { ClassType.Explorer, "20d20" }, { ClassType.None, "1d1" } };
        public static readonly Dictionary<ClassType, bool> classspellcaster = new() { { ClassType.Artificer, false }, { ClassType.Barbarian, false },
            { ClassType.Bard, true }, { ClassType.Cleric, true }, { ClassType.Druid, true }, { ClassType.Fighter, false }, { ClassType.Monk, false }, { ClassType.Paladin, true },
            { ClassType.Ranger, true }, { ClassType.Rogue, false }, { ClassType.Sorcerer, true }, { ClassType.Warlock, true }, { ClassType.Wizard, true },
            { ClassType.Explorer, true } };
        public static readonly Dictionary<ClassType, int[,]> classspellbook = new() { { ClassType.Artificer, new int[20, 10] }, { ClassType.Barbarian, new int[20, 10] },
            { ClassType.Bard, new int[20, 10] { { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 9
                                                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 10
                                                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 11
                                                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, //level 13
                                                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, //level 15
                                                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //level 17
                                                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 18
                                                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 19
                                                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 } } }, //level 20
            { ClassType.Cleric, new int[20, 10] { { 3, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                  { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                  { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                  { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                  { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                  { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 9
                                                  { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 10
                                                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 11
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                  { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, //level 13
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, //level 15
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //level 17
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 18
                                                  { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 19
                                                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 } } }, //level 20
            { ClassType.Druid, new int[20, 10] { { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                 { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                 { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                 { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                 { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                 { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                 { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                 { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                 { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 9
                                                 { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 10
                                                 { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 11
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                 { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, //level 13
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, //level 15
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //level 17
                                                 { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 18
                                                 { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 19
                                                 { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 } } }, //level 20
            { ClassType.Paladin, new int[20, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                   { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                   { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                   { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 5
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 6
                                                   { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 7
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 8
                                                   { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 9
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 10
                                                   { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 11
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 13
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 15
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                   { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 17
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 18
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 19
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } } }, //level 20
            { ClassType.Ranger, new int[20, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                  { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                  { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 5
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 6
                                                  { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 7
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 8
                                                  { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 9
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 10
                                                  { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 11
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 13
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 15
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                  { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 17
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 18
                                                  { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 19
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } } }, //level 20
            { ClassType.Rogue, new int[20, 10] }, { ClassType.None, new int[20, 10] }, { ClassType.Monk, new int[20, 10] },
            { ClassType.Sorcerer, new int[20,10] { { 4, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                   { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                   { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                   { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                   { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                   { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                   { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 9
                                                   { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 10
                                                   { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 11
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                   { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, //level 13
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, //level 15
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //level 17
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 18
                                                   { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 19
                                                   { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 } } }, //level 20
            { ClassType.Warlock, new int[20, 10] { { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                   { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                   { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                   { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                   { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 5
                                                   { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 9
                                                   { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 10
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 11
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 13
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                   { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 }, //level 15
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                   { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 17
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 18
                                                   { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 19
                                                   { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 } } }, //level 20
            { ClassType.Wizard, new int[20, 10] { { 3, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                  { 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                  { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                  { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                  { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, //level 6
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 7
                                                  { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, //level 8
                                                  { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //level 9
                                                  { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 10
                                                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 11
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 12
                                                  { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, //level 13
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 14
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, //level 15
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 16
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //level 17
                                                  { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 }, //level 18
                                                  { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //level 19
                                                  { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 } } }, //level 20
            { ClassType.Explorer, new int[20, 10] { { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 1
                                                    { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, //level 2
                                                    { 0, 2, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 3
                                                    { 1, 0, 2, 0, 0, 0, 0, 0, 0, 0 }, //level 4
                                                    { 0, 0, 2, 2, 0, 0, 0, 0, 0, 0 }, //level 5
                                                    { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 }, //level 6
                                                    { 0, 0, 0, 2, 2, 0, 0, 0, 0, 0 }, //level 7
                                                    { 1, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, //level 8
                                                    { 0, 0, 0, 0, 2, 2, 0, 0, 0, 0 }, //level 9
                                                    { 0, 0, 0, 0, 0, 2, 0, 0, 0, 0 }, //level 10
                                                    { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 }, //level 11
                                                    { 1, 0, 0, 0, 0, 0, 2, 0, 0, 0 }, //level 12
                                                    { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0 }, //level 13
                                                    { 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 }, //level 14
                                                    { 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 }, //level 15
                                                    { 1, 0, 0, 0, 0, 0, 0, 0, 2, 0 }, //level 16
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 2, 2 }, //level 17
                                                    { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, //level 18
                                                    { 0, 1, 1, 0, 0, 1, 1, 0, 0, 0 }, //level 19
                                                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 } } }, //level 20
            };
        public static readonly Dictionary<SpellType, SpellData> spelldata = new() { { SpellType.None, new(SpellType.None) },
            { SpellType.Healing, new(SpellType.Healing) }, { SpellType.ExtraHealing, new(SpellType.ExtraHealing) }, { SpellType.Invisibility, new(SpellType.Invisibility) },
            { SpellType.MagicMapping, new(SpellType.MagicMapping) }, { SpellType.SlowMonsters, new(SpellType.SlowMonsters) }, { SpellType.SleepMonsters, new(SpellType.SleepMonsters) },
            { SpellType.ConfuseMonsters, new(SpellType.ConfuseMonsters) }, { SpellType.AcidSplash, new(SpellType.AcidSplash) }, { SpellType.ChillTouch, new(SpellType.ChillTouch) },
            { SpellType.FireBolt, new(SpellType.FireBolt) }, { SpellType.Friends, new(SpellType.Friends) }, { SpellType.Light, new(SpellType.Light) },
            { SpellType.PoisonSpray, new(SpellType.PoisonSpray) }, { SpellType.RayOfFrost, new(SpellType.RayOfFrost) }, { SpellType.ShockingGrasp, new(SpellType.ShockingGrasp) },
            { SpellType.BurningHands, new(SpellType.BurningHands) }, { SpellType.MageArmor, new(SpellType.MageArmor) }, { SpellType.MagicMissile, new(SpellType.MagicMissile) },
            { SpellType.TashasHideousLaughter, new(SpellType.TashasHideousLaughter) }, { SpellType.WitchBolt, new(SpellType.WitchBolt) } };
        public static string GetClassName(ClassType classtype) => classtypes.FirstOrDefault(ct => ct.Value == classtype).Key;
    }
}
