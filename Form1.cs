using Microsoft.VisualBasic.Devices;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using YARPG.Properties;
using Timer = System.Windows.Forms.Timer;

namespace YARPG
{
    public partial class Form1 : Form
    {
        public const string Version = "0.0.80 alpha";
        public bool dodebug = false;
        public bool lriactive = true;
        public bool ambusy = false;
        public bool hasdonemmthislevel = false;
        public bool slowturn = false;
        public bool fastturn = false;
        public bool readytoplay = false;
        Timer timer = new();
        private readonly Image _00rock = Resources._00rock;
        private readonly Image _01cortl = Resources._01cortl;
        private readonly Image _02cortr = Resources._02cortr;
        private readonly Image _03corbl = Resources._03corbl;
        private readonly Image _04corbr = Resources._04corbr;
        private readonly Image _05hort = Resources._05hort;
        private readonly Image _06horb = Resources._06horb;
        private readonly Image _07verl = Resources._07verl;
        private readonly Image _08verr = Resources._08verr;
        private readonly Image _09doort = Resources._09doort;
        private readonly Image _10doorb = Resources._10doorb;
        private readonly Image _11doorl = Resources._11doorl;
        private readonly Image _12doorr = Resources._12doorr;
        private readonly Image _13room = Resources._13room;
        private readonly Image _14path = Resources._14path;
        private readonly Image _15stairu = Resources._15stairu;
        private readonly Image _16staird = Resources._16staird;
        private readonly Image _17player = Resources._17player;
        private readonly Image _18potion = Resources._18potion;
        private readonly Image _19armor = Resources._19armor;
        private readonly Image _20scroll = Resources._20scroll;
        private readonly Image _21weapon = Resources._21weapon;
        private readonly Image _22gold = Resources._22gold;
        private readonly Image _23lantern = Resources._23lantern;
        private readonly Image _24kobold_orc = Resources._24kobold_orc;
        private readonly Image _25shopkeep = Resources._25shopkeep;
        private readonly Image _9999creaturetarget = Resources._9999creaturetarget;
        private readonly Image _9999positiontarget = Resources._9999positiontarget;
        public int wide = 200;
        public int high = 200;
        public int posx = 100;
        public int posy = 100;
        public int numrooms = 25;
        public int minroom = 9;
        public int maxroom = 19;
        public bool followplayer = true;
        readonly Level[] levels = new Level[100];
        readonly int[] didlevels = new int[100];
        public Level currentlevel;
        int level = 0;
        public Thing player;
        readonly ToolTip tip = new();
        public Point mouse;
        public Point mousegame;
        public Point target;
        Map map = new();
        public static PictureBox MiniMap { get; set; } = new();
        public Form1()
        {
            InitializeComponent(); //windows needs this
            Info.SetRTB(info); //set info as our info area
            currentlevel = new Level(wide, high, numrooms, minroom, maxroom); //make a new level for currentlevel
            levels[level] = currentlevel; //save it in our levels array
            didlevels[level] = 1; //mark new level as done
            GameData.CurrentLevel = currentlevel; //save currentlevel to our GameData
            map = currentlevel.MakeMap();
            player = new(); //make a new player
            (player.X, player.Y) = currentlevel.Rooms[0].Place(); //set player in first room
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            box.MouseWheel += Box_MouseWheel; //set box mouse wheel handler
            _ = box.Focus(); //focus on box (so it gets mouse input)
            tip.SetToolTip(box, ""); //set tip to box
            tip.Hide(box); //hide tip
            tip.IsBalloon = false; //we don't use balloon, animation, or fading
            tip.UseAnimation = false;
            tip.UseFading = false;
            MakePlayer(); //make player
            MakeStuff(); //make stuff
            Settings.Defaults(); //load default values for settings
            readytoplay = true; //now we are ready to play
            Display(); //display game
            SoundSystem.PlayEmbeddedSound("YARPG.Resources.GameStart.wav"); //play a sound to show we have started the game
            Info.OutL($"YARPG - Yet Another Role Playing Game - version {Version}", Color.DarkRed);  //display info
            Info.OutL(" copyright © 2023 by John Worthington - woodmage@gmail.com\n", Color.DarkBlue);
            Info.OutL("Press F1 for help.  Please, have fun!");
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized) //if we haven't been minimized
            {
                Size client = ClientSize; //get clientsize
                Size boxsz = client; //compute box size
                boxsz.Height -= 200;
                panel.Size = boxsz; //set panel size to that
                panel.Location = new(0, 0); //upper left corner of window
                box.Size = boxsz; //set box size to that
                box.Location = new(0, 0); //upper left corner of panel
                boxsz.Width -= 200; //take 200 from width
                info.Size = new(boxsz.Width, 200); //set info size to that
                info.Location = new(0, boxsz.Height); //set location below box
                minimap.Size = new(200, 200); //set minimap size
                minimap.Location = new(boxsz.Width, boxsz.Height); //position it below box and to right of info
                MiniMap = minimap; //make a copy of minimap for static use
                Display(); //display screen
            }
        }
        private static void HelpLine(string keys, string desc)
        {
            Info.Out($"{keys}", Color.DarkGreen); //display key(s)
            Info.OutL($" - {desc}"); //display description
        }
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode) //switch by key code
            {
                case Keys.F1: //help
                    if (e.Control) //if it was a Ctrl-F1
                    {
                        MakeNewItem(false); //make a new item
                    }
                    else //otherwise
                    {
                        Info.OutL("YARPG Keyboard Help\n", Color.DarkRed); //display keyboard help
                        HelpLine("F1", "this help.");
                        HelpLine("F2", "toggle minimap.");
                        HelpLine("F3", "save game.");
                        HelpLine("F4", "load game.");
                        HelpLine("F5", "toggle follow player.");
                        HelpLine("F6", "open settings.");
                        HelpLine("F7", "show attacks.");
                        HelpLine("F8", "choose melee attack.");
                        HelpLine("F9", "choose ranged attack.");
                        HelpLine("F10, Escape", "exit game.");
                        HelpLine("F11", "toggle light radius indicator");
                        HelpLine("F12", "toggle status bars");
                        HelpLine("Q", "move player left and up.");
                        HelpLine("W", "move player up.");
                        HelpLine("E", "move player right and up.");
                        HelpLine("A", "move player left.");
                        HelpLine("S", "move player down.");
                        HelpLine("D", "move player right.");
                        HelpLine("Z", "move player left and down.");
                        HelpLine("C", "move player right and down.");
                        HelpLine("Space", "stop moving.");
                        HelpLine("R", "get room # for room we are in.");
                        HelpLine("Up, Left, Down, Right", "move screen.");
                        HelpLine("Add", "zoom in.");
                        HelpLine("Multiply", "zoom all the way in.");
                        HelpLine("Subtract", "zoom out.");
                        HelpLine("Divide", "zoom all the way out.");
                        HelpLine("Enter", "regenerate level.");
                        Info.OutL("");
                    }
                    break;
                case Keys.F2: //minimap toggle
                    if (e.Control) //if it was a Ctrl-F2
                    {
                        try
                        {
                            MakeNewItem(true); //make a new item
                            Thing newthing = GameData.Player.Things[^1]; //easier link to new item
                            string desc1 = newthing.Describe(); //get description
                            using StreamWriter sw = new("debug.txt"); //create a file for writing
                            {
                                newthing.Save(sw); //save item there
                                sw.Close();
                            }
                            using StreamReader sr = File.OpenText("debug.txt"); //open same file for reading
                            {
                                newthing.Load(sr); //load item
                                sr.Close();
                            }
                            string desc2 = newthing.Describe(); //get description
                            if (string.Equals(desc1, desc2))
                            {
                                Info.OutL("Identical item descriptions.", Color.DarkBlue);
                            }
                            else
                            {
                                Info.OutL("Different item descriptions.", Color.DarkRed);
                            }
                            using StreamWriter sw2 = new("debugcomp.txt"); //open a new file for writing
                            {
                                sw2.WriteLine(desc1);
                                sw2.WriteLine();
                                sw2.WriteLine(desc2);
                                sw2.Close();
                            }
                            player.Things.Remove(newthing); //get rid of new item
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show($"We got a {exc.Message} error.", "ERROR!", MessageBoxButtons.OK);
                            Application.Exit();
                            return;
                        }
                    }
                    else //otherwise (just F2)
                    {
                        Settings.UseMiniMap = !Settings.UseMiniMap; //toggle minimap
                    }
                    break;
                case Keys.F3: //save game
                    if (e.Control) //if Ctrl was held
                    {
                        player.Experience = player.NextLevel + 1; //set player's experience to new level
                        player.CheckExperience(); //let player go up a level
                    }
                    else //otherwise (just F3)
                    {
                        SaveGame(); //save the game
                    }
                    break;
                case Keys.F4: //load game
                    LoadGame();
                    break;
                case Keys.F5: //follow player
                    followplayer = !followplayer;
                    break;
                case Keys.F6: //open settings
                    Settings settings = new(); 
                    settings.ShowDialog();
                    break;
                case Keys.Enter: //regenerate level
                    currentlevel = new(wide, high, numrooms, minroom, maxroom);
                    (player.X, player.Y) = currentlevel.Rooms[0].Place();
                    MakeStuff();
                    break;
                case Keys.F7: //attacks info
                    ShowAttacks();
                    break;
                case Keys.F8: //choose Melee attack
                    ChooseMelee();
                    break;
                case Keys.F9: //choose Ranged Attack
                    ChooseRanged();
                    break;
                case Keys.F10:
                case Keys.Escape:
                    Application.Exit();
                    return; //if we did a "break" here, it would update the display, etc.
                case Keys.F11:
                    lriactive = !lriactive;
                    break;
                case Keys.F12:
                    Settings.DoPlayerStatus = !Settings.DoPlayerStatus;
                    break;
                case Keys.A: //move player left
                    PlayerMove(-1, 0);
                    break;
                case Keys.D: //move player right
                    PlayerMove(1, 0);
                    break;
                case Keys.W: //move player up
                    PlayerMove(0, -1);
                    break;
                case Keys.S: //move player down
                    PlayerMove(0, 1);
                    break;
                case Keys.Q: //move player left and up
                    PlayerMove(-1, -1);
                    break;
                case Keys.E: //move player right and up
                    PlayerMove(1, -1);
                    break;
                case Keys.Z: //move player left and down
                    PlayerMove(-1, 1);
                    break;
                case Keys.C: //move player right and down
                    PlayerMove(1, 1);
                    break;
                case Keys.Space: //stop moving
                    timer.Stop(); //stop timer
                    break;
                case Keys.R:
                    DisplayRoom(player);
                    break;
                case Keys.Left: //move view left
                    ScreenShift(-1, 0);
                    break;
                case Keys.Right: //move view right
                    ScreenShift(1, 0);
                    break;
                case Keys.Up: //move view up
                    ScreenShift(0, -1);
                    break;
                case Keys.Down: //move view down
                    ScreenShift(0, 1);
                    break;
                case Keys.Home: //move view up and left
                    ScreenShift(-1, - 1);
                    break;
                case Keys.End: //move view down and left
                    ScreenShift(-1, 1);
                    break;
                case Keys.PageUp: //move view up and right
                    ScreenShift(1, -1);
                    break;
                case Keys.PageDown: //move view down and right
                    ScreenShift(1, 1);
                    break;
                case Keys.Add: //zoom in
                    AddZoom(1);
                    break;
                case Keys.Multiply: //zoom all the way in
                    AddZoom(50);
                    break;
                case Keys.Subtract: //zoom out
                    AddZoom(-1);
                    break;
                case Keys.Divide: //zoom all the way out
                    AddZoom(-50);
                    break;
                default:
                    return;
            }
            e.Handled = true; //tell Windows we handled the key
            Display(); //display game
        }

        private void PlayerMove(int dx, int dy)
        {
            followplayer = true; //we will be following player now
            player.Move(dx, dy); //do the actual movement
            MoveCreatures(); //let the creatures have their turn
            ShowStuffHere(); //show stuff at this location
        }

        private void ScreenShift(int dx, int dy)
        {
            followplayer = false; //we will not be following player now
            posx += dx; //adjust screen position
            posy += dy;
            if (posx < 0) //keep it within boundaries
            {
                posx = 0;
            }
            if (posx >= wide)
            {
                posx = wide - 1;
            }
            if (posy < 0)
            {
                posy = 0;
            }
            if (posy >= high)
            {
                posy = high - 1;
            }
        }

        private static void AddZoom(int dif)
        {
            Settings.Zoom += dif; //adjust zoom
            if (Settings.Zoom < 5) //keep it within boundaries
            {
                Settings.Zoom = 5;
            }
            if (Settings.Zoom > 50)
            {
                Settings.Zoom = 50;
            }
        }

        private void ShowAttacks()
        {
            Info.OutL($"{player.Name} has {player.Attacks.Count} attacks:"); //display info
            foreach (var att in player.Attacks) //for every attack player has
            {
                ShowAttack("  Attack", att); //show attack
            }
            ShowAttack("Melee", GameData.Melee); //show current melee attack
            ShowAttack("Ranged", GameData.Ranged); //show current ranged attack
        }

        private static void ShowAttack(string name, Attack attack)
        {
            Info.Out($"{name}: "); //display info
            if (attack.Name == "_none_") //if attack is a "null" attack
            {
                Info.OutL("NO ATTACK", Color.DarkRed); //display info
            }
            else //otherwise (attack is a valid attack)
            {
                Info.OutL(attack.Describe()); //display attack information
            }
        }

        private void ChooseMelee()
        {
            List<Attack> meleeattacks = player.Attacks.Where(a => a.Range == 0).ToList(); //get list of melee attack
            AttackMenu(meleeattacks, ChooseMeleeClick); //make a menu of them
        }

        private void ChooseMeleeClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender, so...
            {
                return;
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Attack attack = (Attack)menuitem.Tag; //get attack from tag
            GameData.Melee = attack; //set melee attack
        }

        private void ChooseRanged()
        {
            List<Attack> rangedattacks = player.Attacks.Where(a => a.Range > 0).ToList(); //get list of ranged attacks
            AttackMenu(rangedattacks, ChooseRangedClick); //make a menu of them
        }

        private void ChooseRangedClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need a sender, so...
            {
                return;
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Attack attack = (Attack)menuitem.Tag; //get attack from tag
            GameData.Ranged = attack; //set ranged attack
        }

        private void ChooseSpellForCasting()
        {
            List<Spell> spells = player.SpellList; //make copy of spell list (do we really need to do this?)
            SpellMenu(spells, CastSpellClick);
        }

        private void CastSpellClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender, so...
            {
                return; //if there is no sender, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Spell spell = (Spell)menuitem.Tag; //get spell from tag
            spell.CastIt(player); //cast the spell
            if (spell.Effect == SpellType.MagicMapping) //if spell was magic mapping
            {
                hasdonemmthislevel = true;
            }
            MoveCreatures(); //give the creatures their turn
            Display(); //redraw game screen
        }

        private void SetTarget(object? sender, EventArgs e)
        {
            TargetingSystem.Clear(); //clear the targeting system
            TargetingSystem.Add(target); //add point at mousegame coordinates
            Display(); //redraw game screen
        }

        private void ChooseScrollForTranscription()
        {
            List<Thing> scrolls = player.Things.Where(t => t.Type.HasFlag(SpotType.Scroll)).ToList(); //make list of scrolls
            MakeMenu(scrolls, false, TranscribeScrollClick);
        }

        private void TranscribeScrollClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender, so...
            {
                return; //if we don't have sender, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Thing scroll = (Thing)menuitem.Tag; //get scroll from tag
            if (MessageBox.Show("This will erase scroll whether it works or not, sure you wanna do it?","Verify",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                player.TranscribeScroll(scroll); //try to transcribe the scroll
                player.Things.Remove(scroll); //remove the scroll
                MoveCreatures(); //let creatures have their turn
                Display(); //redraw display
            }
        }

        private void ChoosePotionForAnalysis()
        {
            List<Thing> potions = player.Things.Where(t => t.Type.HasFlag(SpotType.Potion)).ToList(); //make list of potions
            MakeMenu(potions, false, AnalyzePotionClick);
        }

        private void AnalyzePotionClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender, so...
            {
                return; //if we don't have sender, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Thing potion = (Thing)menuitem.Tag; //get potion from tag
            if (MessageBox.Show("This will destroy the potion and requires great intelligence, sure you wanna try?", "Verify", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                player.AnalyzePotion(potion); //try to analyze the potion
                player.Things.Remove(potion); //remove the potion
                MoveCreatures(); //let creatures have their turn
                Display(); //redraw display
            }
        }

        private void SpellMenu(List<Spell> spells, EventHandler? handler)
        {
            ContextMenuStrip menu = new(); //make a new menu
            ToolStripMenuItem menuitem; //variable for menuitem
            foreach (var spell in spells) //for each spell in list
            {
                menuitem = new($"{spell.Name}: {Data.spelldata[spell.Effect].GetDescription()}")
                {
                    Tag = spell
                }; //make a menu item from spell
                menuitem.Click += handler; //set handler for menu item
                menu.Items.Add(menuitem); ///add menu item to menu
            }
            menuitem = new("Cancel"); //make a menu item for cancel
            menu.Items.Add(menuitem); //add to menu
            DisplayMenu(menu); //display the menu
        }

        private void AttackMenu(List<Attack> attacks, EventHandler? handler)
        {
            ContextMenuStrip menu = new(); //make a new menu
            ToolStripMenuItem menuitem; //variable for menuitem
            foreach (var attack in attacks) //for each attack in list
            {
                menuitem = new($"{attack.Name}")
                {
                    Tag = attack
                }; //make a menu item from attack
                menuitem.Click += handler; //set handler for menu item
                menu.Items.Add(menuitem); //add menu item to menu
            }
            menuitem = new("Cancel"); //make a menu item for cancel
            menu.Items.Add(menuitem); //add to menu
            DisplayMenu(menu); //display the menu
        }

        private Point ScreenToUnit(Point p)
        {
            int px = posx, py = posy; //get current position
            if (followplayer)
            {
                (px, py) = (player.X, player.Y); //if following player, get player position
            }
            float gx = ((p.X - box.Width / 2) / (float)Settings.Zoom) + px; //compute x, y game units
            float gy = ((p.Y - box.Height / 2) / (float)Settings.Zoom) + py;
            return new Point((int)gx, (int)gy); //return a point made from game units
        }

        private static Point AdjustPoint(Point p) => new(p.X + 10, p.Y - 10);

        private void Box_MouseDown(object? sender, MouseEventArgs e)
        {
            target = mousegame = ScreenToUnit(mouse = e.Location); //set mouse and mousegame in game units
            if (e.Button == MouseButtons.Left) //if left mouse button was pressed
            {
                Box_LeftMouse(); //handle left mouse button
            }
            if (e.Button == MouseButtons.Right) //if right mouse button was pressed
            {
                Box_RightMouse(); //handle right mouse button
            }
        }

        private void Box_LeftMouse()
        {
            List<Thing> items = currentlevel.Items.Where(i => i.IsAt(mousegame)).ToList(); //get list of items there
            List<Thing> creatures = currentlevel.Creatures.Where(c => c.IsAt(mousegame)).ToList(); //get list of creatures there
            if (creatures.Count > 0) //if there are creatures there
            {
                if (creatures.Count == 1) //if there is only one
                {
                    AttackCreature(creatures[0]); //attack the one and only creature
                }
                else //otherwise (more than one creature)
                {
                    AttackMenu(creatures); //make a menu for creatures
                }
                return; //we're done
            }
            if (items.Count > 0) //if there are items
            {
                if (items.Count == 1) //if there is only one
                {
                    PickUpItem(items[0]); //pick up the one and only item
                }
                else //otherwise (more than one item)
                {
                    PickUpMenu(items); //make a menu for items
                }
                return; //we're done
            }
            MoveHere(); //move to the spot
        }

        private void AttackCreature(Thing creature)
        {
            if (creature.Type.HasFlag(SpotType.NPC)) //if it is an NPC
            {
                Form shop = new Shop(); //make a shop form
                shop.ShowDialog(); //and display it
            }
            else
            {
                if (player.CanAttack(creature)) //if we CAN attack the creature
                {
                    if (player.GetDistance(creature) < 2) //if it is in melee range
                    {
                        player.Attack(creature, GameData.Melee); //perform a melee attack
                    }
                    else //otherwise
                    {
                        player.Attack(creature, GameData.Ranged); //perform a ranged attack
                    }
                    if (creature.Dead) //if the creature is now dead
                    {
                        player.Experience += creature.Experience; //get experience points
                        if (player.CheckExperience()) //if we went up a level
                        {
                            Info.OutL($"Congratulations!", Color.DarkOliveGreen); //display info
                        }
                        currentlevel.Creatures.Remove(creature); //remove creature from level
                    }
                    MoveCreatures(); //monster turn
                    Display(); //display the game
                }
                else //otherwise (we couldn't attack the creature)
                {
                    Info.OutL($"{player.Name} was unable to attack {creature.Name}!"); //display info
                }
            }
        }

        private void AttackMenu(List<Thing> creatures)
        {
            MakeMenu(creatures, true, AttackClick); //make a menu with creatures
        }

        private void AttackClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender to do anything, so
            {
                return; //return if no sender
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem
            Thing? target = (Thing)menuitem.Tag; //get target Thing from tag
            if (target != null) //if target is not null (Cancel)
            {
                AttackCreature(target); //attack the target creature
            }
        }

        private void PickUpItem(Thing item)
        {
            if (player.PickUpItem(item)) //try to pick up item, if it works,
            {
                currentlevel.Items.Remove(item); //then remove item from level
                Info.OutL($"{player.Name} picked up {item.Name}.", Color.DarkBlue); //display info
                MoveCreatures(); //monster turn
            }
            else //otherwise (we couldn't take it)
            {
                if (item.IsStairs()) //if item is stairs
                {
                    if (item.Type.HasFlag(SpotType.StairsUp)) //if it is stairs up
                    {
                        if (level > 0) //if level is more than first level
                        {
                            level--; //take away a level
                            MakeLevel(); //make a new level
                        }
                        else //otherwise (retreating from dungeon)
                        {
                            if (GetYesNo("You're on the top level.  Exit the dungeon?"))
                            {
                                MessageBox.Show("You exit the dungeon.  Coward!", "Ran Away", MessageBoxButtons.OK); //Give a popup message
                                Application.Exit(); //exit the game
                            }
                        }
                    }
                    else //else (stairs down)
                    {
                        if (level < 99) //if level is in first 100 levels (including going down)
                        {
                            level++; //go down the stairs to next level
                            MakeLevel(); //make a new level
                        }
                        else
                        {
                            MessageBox.Show("You made it to the bottom of the dungeon!", "Exit", MessageBoxButtons.OK); //give a popup message
                            Application.Exit(); //exit the game
                        }
                    }
                }
                else //otherwise (not pick upable and not stairs
                {
                    Info.OutL($"{player.Name} tried to pick up {item.Name}!  Silly {player.Name}!"); //display info
                }
            }
            Display(); //display the game
        }

        private void PickUpMenu(List<Thing> items)
        {
            MakeMenu(items, false, PickupClick); //make a menu with items
        }

        private void MakeMenu(List<Thing> things, bool ismonster, EventHandler? clickhandler)
        {
            ContextMenuStrip contextmenu = new(); //make a new context menu strip
            foreach (var thing in things) //for each thing
            {
                ToolStripMenuItem menuitem; //we will need a new menu item
                if (ismonster) //if it is a monster
                {
                    menuitem = new($"{thing.Name} with {thing.HP} of {thing.MaxHP} HP"); //set menuitem display
                }
                else //otherwise (it is an item)
                {
                    menuitem = new($"{thing.Name}: {thing.Description}"); //set menuitem display
                }
                menuitem.Click += clickhandler; //set handler for item selection
                menuitem.Tag = thing; //put Thing in tag
                contextmenu.Items.Add(menuitem); //add menu item to context menu
            }
            ToolStripMenuItem menucancel = new("Cancel") //menu item for cancel
            {
                Tag = null
            };
            contextmenu.Items.Add(menucancel); //add to context menu
            DisplayMenu(contextmenu); //display menu
        }

        private void PickupClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender to do anything, so...
            {
                return;
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menu item
            Thing? target = (Thing)menuitem.Tag; //get target item from tag
            if (target != null) //if target exists
            {
                PickUpItem(target); //pick it up
            }
        }

        private void DisplayMenu(ContextMenuStrip menu)
        {
            int maxx = box.Width - menu.Width; //set maximum x value
            int maxy = box.Height - menu.Height; //set maximum y value
            int x = Math.Min(maxx, mouse.X); //set x coordinate
            int y = Math.Min(maxy, mouse.Y); //set y coordinate
            Point point = new(x, y); //make back into a point
            point = box.PointToScreen(point); //convert point to screen coordinates
            point = PointToClient(point); //convert point to form coordinates
            menu.Show(box, point); //show the menu
        }

        private void Box_RightMouse()
        {
            ContextMenuStrip menu = new(); //make a new menu
            ToolStripMenuItem item = new("Center Here"); //first choice is centering at mouse location
            item.Click += CenterHereClick; //set handler for this item
            menu.Items.Add(item); //add item to menu
            item = new("Move Here"); //next choice is to move to mouse location
            item.Click += MoveHereClick;
            menu.Items.Add(item);
            if (player.Things.Count > 0) //if we have any inventory
            {
                item = new("Inventory"); //next choice is inventory
                item.Click += InventoryClick;
                menu.Items.Add(item);
                item = new("Drop Item"); //next choice is to drop an item
                item.Click += DropItem;
                menu.Items.Add(item);
                if (player.Things.Where(t => t.Type.HasFlag(SpotType.Scroll)).Any()) //if we have any scrolls
                {
                    item = new("Transcribe Scroll"); //next choice is transcribe
                    item.Click += Transcribe;
                    menu.Items.Add(item);
                }
                if (player.Things.Where(t => t.Type.HasFlag(SpotType.Potion)).Any()) //if we have any potions
                {
                    item = new("Analyze Potion"); //next choice is analyze
                    item.Click += Analyze;
                    menu.Items.Add(item);
                }
            }
            if (player.Attacks.Where(a => a.Range == 0).Any()) //if we have any melee attacks
            {
                item = new("Choose Melee Attack"); //next choice is choose melee attack
                item.Click += ChooseMelee;
                menu.Items.Add(item);
            }
            if (player.Attacks.Where(a => a.Range > 0).Any()) //if we have any ranged attacks
            {
                item = new("Choose Ranged Attack"); //next choice is choose ranged attack
                item.Click += ChooseRanged;
                menu.Items.Add(item);
            }
            if (player.SpellList.Count > 0) //if we have any spells
            {
                item = new("Cast Spell"); //next choice is cast spell
                item.Click += CastSpell;
                menu.Items.Add(item);
                if (TargetingSystem.IsNone() == false)
                {
                    item = new("Add Spell Target"); //next choice is to add spell target
                    item.Click += AddTarget;
                    menu.Items.Add(item);
                }
                item = new("Set Spell Target"); //next choice is to set spell target
                item.Click += SetTarget;
                menu.Items.Add(item);
            }
            Room? room = player.InRoom(); //get room player is in, if any
            if (room != null) //if player is in a room
            {
                if (room.Type == RoomType.Safe) //if it is a safe room
                {
                    item = new("Camp Here"); //next choice is to camp here
                    item.Click += CampHere;
                    menu.Items.Add(item);
                }
            }
            item = new("Description"); //next choice is description
            item.Click += DescriptionClick;
            menu.Items.Add(item);
            DisplayMenu(menu); //display the menu
        }

        private void AddTarget(object? sender, EventArgs e)
        {
            TargetingSystem.Add(target); //add point to targeting
            Display(); //redraw game screen
        }

        private void CampHere(object? sender, EventArgs e)
        {
            Info.OutL($"{player.Name} feels much better after a good rest!"); //display info
            player.HP = player.MaxHP; //fully heal
        }

        private void CastSpell(object? sender, EventArgs e) => ChooseSpellForCasting();

        private void Transcribe(object? sender, EventArgs e) => ChooseScrollForTranscription();

        private void Analyze(object? sender, EventArgs e) => ChoosePotionForAnalysis();

        private void ChooseRanged(object? sender, EventArgs e) => ChooseRanged();

        private void ChooseMelee(object? sender, EventArgs e) => ChooseMelee();

        private void DescriptionClick(object? sender, EventArgs e)
        {
            List<Thing> nearbystuff = VisibleStuff(); //get list of nearby stuff
            nearbystuff.Add(player); //add the player to the list
            MakeMenu(nearbystuff, false, DescriptionItemClick); //make a menu of all these things
        }

        public List<Thing> VisibleStuff()
        {
            List<Thing> things = new(); //storage for list of things we will return
            List<Point> seen = currentlevel.GetVisible(player); //get all visible points
            foreach (var point in seen) //for each visible point
            {
                foreach (var creature in currentlevel.Creatures.Where(c => c.IsAt(point))) //for every creature at point
                {
                    things.Add(creature); //add creature to list (NOT a copy!)
                }
                foreach (var item in currentlevel.Items.Where(i => i.IsAt(point))) //for every item at the point
                {
                    things.Add(item); //add item to list (NOT a copy!)
                }
            }
            return things; //return list
        }

        private void DescriptionItemClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender to do anything, so...
            {
                return; //if we don't have one, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Thing thing = (Thing)menuitem.Tag; //get thing from menuitem's tag
            ShowInfo(thing); //show info on thing
        }

        private void InventoryClick(object? sender, EventArgs e)
        {
            ContextMenuStrip contextmenu = new(); //make a new context menu strip
            foreach (var thing in player.Things) //for each thing
            {
                ToolStripMenuItem menuitem; //we will need a new menu item
                string itemname = $"{thing.Name}: {thing.Description}"; //set up item: description string
                if (thing.Equipped) //if thing is equipped
                {
                    itemname = $"● {itemname}"; //put a dot before item name
                }
                menuitem = new($"{itemname}"); //set menuitem display
                menuitem.Click += InventoryItemClick; //set handler for picking up item
                menuitem.Tag = thing; //put Thing in tag
                contextmenu.Items.Add(menuitem); //add menu item to context menu
            }
            ToolStripMenuItem menucancel = new("Cancel"); //menu item for cancel
            contextmenu.Items.Add(menucancel); //add to context menu
            DisplayMenu(contextmenu); //display menu
        }

        private void DropItem(object? sender, EventArgs e)
        {
            MakeMenu(player.Things, false, DropItemClick); //make a menu of things player has
        }

        private void DropItemClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender to do anything, so...
            {
                return; //if sender doesn't exist, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Thing item = (Thing)menuitem.Tag; //get item from menuitem's tag
            if (MessageBox.Show($"Drop {item.Name}?  You sure?", "Verify", MessageBoxButtons.YesNo) == DialogResult.Yes) //if player agrees
            {
                player.DropItem(item); //drop the item
            }
            else //otherwise
            {
                Info.OutL($"{player.Name} decided not to drop {item.Name}", Color.Chocolate); //display info
            }
        }

        private void InventoryItemClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need sender to do anything, so...
            {
                return; //if no sender, return
            }
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender; //get menuitem from sender
            Thing item = (Thing)menuitem.Tag; //get item from menuitem's tag
            SpotType stype = item.Type; //get stype from item's spot type
            if (stype.HasFlag(SpotType.Scroll)) //if it is a scroll
            {
                if (player.ReadScroll(item)) //if we can read the scroll
                {
                    player.Things.Remove(item); //get rid of it
                }
            }
            else if (stype.HasFlag(SpotType.Potion)) //if it is a potion
            {
                if (player.QuaffPotion(item)) //if we can quaff the potion
                {
                    player.Things.Remove(item); //get rid of it
                }
            }
            else if (stype.HasFlag(SpotType.Armor)) //if it is armor
            {
                if (item.Equipped) //if it is equipped
                {
                    player.RemoveArmor(item); //remove it
                }
                else //otherwise (armor is not equipped)
                {
                    player.WearArmor(item); //wear the armor
                }
            }
            else if (stype.HasFlag(SpotType.Weapon)) //if it is a weapon
            {
                if (item.Equipped) //if it is equipped
                {
                    player.UnequipWeapon(item); //unequip it
                }
                else //otherwise (weapon is not equipped)
                {
                    player.EquipWeapon(item); //equip weapon
                    SetAttacks(); //set attacks
                }
            }
            else if (stype.HasFlag(SpotType.LightSource)) //if it is a light
            {
                if (item.Equipped) //if it is equipped
                {
                    item.Equipped = false; //unequip it
                    player.LightRadius = 0; //get rid of light radius from it
                }
                else //otherwise (light is not equipped)
                {
                    player.GetLight(item); //use light
                }
            }
            else if (stype.HasFlag(SpotType.Gold)) //if it is gold
            {
                player.GetGold(item); //take the gold
            }
            Display(); //display the level
        }

        private void SetAttacks()
        {
            Attack? ranged = player.Attacks.Find(a => a.Range > 0); //find a ranged attack
            Attack? melee = player.Attacks.Find(a => a.Range == 0); //find a melee attack
            if (GameData.Ranged.Name == "_none_") //if we don't have a ranged weapon
            {
                if (ranged != null)
                {
                    GameData.Ranged = ranged; //set ranged attack
                }
            }
            if (GameData.Melee.Name == "_none_") //if we don't have a melee weapon
            {
                if (melee != null)
                {
                    GameData.Melee = melee; //set melee attack
                }
            }
        }

        private void MoveHereClick(object? sender, EventArgs e)
        {
            MoveHere(); //move to the spot
        }

        private void MoveHere()
        {
            player.MoveHere(mousegame); //move to the spot
            timer.Stop(); //stop the timer (though it shouldn't be running, but still...)
            timer.Dispose(); //displose of the timer
            timer = new()
            {
                Interval = 50 //set up timer interval
            }; //get a new timer
            timer.Tick += MoveTimer; //set up timer handler
            timer.Start(); //start timer
        }

        private void MoveTimer(object? sender, EventArgs e)
        {
            if (ambusy) //if we are busy
            {
                return; //return
            }
            timer.Stop(); //stop timer
            Info.Out("Moving...  "); //display info
            if (GameData.Targetted) //if player is targetted
            {
                Info.Out("Player is targetted...  "); //display info
                if (MessageBox.Show("Stop?", "Targetted", MessageBoxButtons.YesNo) == DialogResult.Yes) //if player wants to stop moving
                {
                    Info.OutL("Player decided to stop."); //display info
                    return; //return
                }
                else //otherwise (player doesn't want to stop)
                {
                    Info.Out("Continuing...  "); //display info
                    GameData.Targetted = false; //remove targetted flag (which will likely be back next round)
                }
            }
            if (player.StillOnPath()) //if player is still on path
            {
                Info.OutL("Took a step."); //display info
                player.Move(); //take another step
                MoveCreatures(); //move creatures
                Display(); //update display
                timer.Start(); //start timer back up
            }
            else
            {
                Info.OutL("Got to destination."); //display info
                Display(); //update display
            }
            ShowStuffHere(); //show stuff at this location
        }

        private void ShowStuffHere()
        {
            List<Thing> itemshere = currentlevel.Items.FindAll(i => i.X == player.X && i.Y == player.Y).ToList(); //get list of items at player's position
            if (itemshere.Count == 0) //if there were no items found
            {
                return; //quick exit
            }
            Info.Out($"{player.Name} steps over"); //display info
            foreach(var item in itemshere) //for each item
            {
                Info.Out($" {item.Name}"); //display item's name
            }
            Info.OutL("."); //end sentence
        }

        private void CenterHereClick(object? sender, EventArgs e)
        {
            if (sender == null) //we need a sender, so...
            {
                return; //return if null
            }
            (posx, posy) = (mousegame.X, mousegame.Y); //set position from mouse position
            followplayer = false; //set to not follow player
            Display(); //display the game
        }

        private void Box_MouseMove(object? sender, MouseEventArgs e)
        {
            mouse = e.Location; //get mouse location
            mousegame = ScreenToUnit(mouse); //set to game units
            string ttip = $"X: {mousegame.X} Y: {mousegame.Y}  "; //make tooltip string
            if (currentlevel.Rooms.Any(r => r.Touches(mousegame))) //if touching a room
            {
                if (currentlevel.Rooms.Any(r => r.Inside(mousegame))) //if inside a room
                {
                    ttip += "in room";
                }
                else //otherwise (touching but not inside a room)
                {
                    if (currentlevel.Doors.Any(d => d.Inside(mousegame))) //if inside a door
                    {
                        ttip += "in door";
                    }
                    else //in wall
                    {
                        ttip += "in wall";
                    }
                }
            }
            else if (currentlevel.Paths.Any(p => p.Inside(mousegame))) //if inside a path
            {
                ttip += "in path";
            }
            else if (mousegame.X >= 0 && mousegame.X < wide && mousegame.Y >= 0 && mousegame.Y < high) //if inside level map
            {
                ttip += "in solid stone";
            }
            else //otherwise
            {
                ttip += "in outer space";
            }
            List<Thing> allthings = new(currentlevel.Items.Where(i => i.IsAt(mousegame))); //make a list of items at the spot that are seen
            allthings.AddRange(currentlevel.Creatures.Where(c => c.IsAt(mousegame))); //add creatures at the spot that are seen to the list
            allthings.ForEach(a => ttip += $"\n{a.Name}: {a.Description}"); //for each thing in list add name and description to tool tip
            if (player.IsAt(mousegame)) //if player here
            {
                ttip += $"\n{player.Name}: a level {player.Level} {player.Class}"; //add player name and description to tool tip
            }
            tip.Show(ttip, box, AdjustPoint(mouse)); //show tooltip
        }

        private void Box_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta > 0) //if we scrolled up
            {
                if (Settings.Zoom < 50) //if less than maximum
                {
                    Settings.Zoom++; //zoom in
                    Display(); //display game
                }
            }
            if (e.Delta < 0) //if we scrolled down
            {
                if (Settings.Zoom > 5) //if more than minimum
                {
                    Settings.Zoom--; //zoom out
                    Display(); //display game
                }
            }
        }

        private static void ShowInfo(Thing thing)
        {
            Info.OutL(thing.Describe(), Color.Black); //display description of thing
        }

        private void MakePlayer()
        {
            GameData.Player.Name = "_old_"; //give player a unique name for testing
            PlayerInitialization piform = new(); //make a new form for player initialization
            piform.ShowDialog(); //show the form
            if (GameData.Player.Name == "_old_") //if player's name is the unique one (player canceled initialization)
            {
                player.Creature = CreatureType.Player; //player is a player
                player.Spot.Type = SpotType.Creature | SpotType.Player; //more player is a player
                player.STR = player.DEX = player.CON = player.INT = player.WIS = player.CHA = 25; //all stats super high
                player.Name = "Player"; //set player's name
                player.Class = ClassType.Explorer; //make player an Explorer
                player.MaxHP = player.HP = Rand.D(2, 25); //starting HP is 2d25
                player.NumberAttacks = 1; //player gets one attack per round
                Attack melee = new(); //new melee attack
                melee.Damages.Add("3d20+20"); //melee attack deals 3d20+20 damage
                melee.DamageTypes.Add(DamageType.Slashing); // that's slashing damage
                melee.ToHit = 10; //plus 10 to hit
                player.Attacks.Add(melee); //add our melee attack to list of attacks
                GameData.Melee = melee; //set it to be our melee attack
                Attack ranged = new(); //new ranged attack
                ranged.Damages.Add("2d20+10"); //ranged attack deals 2d20+10 damage
                ranged.DamageTypes.Add(DamageType.Piercing); //that's piercing damage
                ranged.Range = 120; //range is 120' (60 game units)
                ranged.MaxRange = 300; //maximum range is 300' (150 game units)
                player.Attacks.Add(ranged); //add our ranged attack to list of attacks
                GameData.Ranged = ranged; //set it to be our new ranged attack
                player.LightRadius = 10; //let's give the player a bit of light
                player.AC = 25; //give player a fantastic armor class
                GameData.Player = player; //make a copy for GameData
            }
            else //otherwise (we did not cancel player initialization)
            {
                player = GameData.Player; //copy player
            }
            if (player.IsSpellCaster()) //if it is a spell caster
            {
                GetSpells.DoForm(); //get spells
            }
            player.Description = "the player"; //add a description
            (player.X, player.Y) = currentlevel.Rooms[0].Place(); //place player within first room
        }

        private void MakeLevel()
        {
            if (didlevels[level] == 0) //if we haven't seen this level before
            {
                currentlevel = new Level(wide, high, numrooms, minroom, maxroom); //make a new level for currentlevel
                levels[level] = currentlevel; //save it in our levels array
                GameData.CurrentLevel = currentlevel; //save currentlevel to our GameData
                didlevels[level] = 1; //record level as done
                MakeStuff(); //populate the level
            }
            else //otherwise (we have seen this level)
            {
                currentlevel = levels[level]; //assign currentlevel to saved level
                GameData.CurrentLevel = currentlevel; //assign to our GameData
            }
            (player.X, player.Y) = currentlevel.Rooms[0].Place(); //put player in first room
            map = currentlevel.MakeMap(); //make a map
            hasdonemmthislevel = false;
        }

        private void MakeStuff()
        {
            currentlevel.Rooms[0].Type = RoomType.Lit; //make first room lit
            currentlevel.Items.Clear(); //clear any items in currentlevel
            Thing stup = new()
            {
                Name = "stairs up", //name it
                Description = "stairs that go up to the previous level"
            }; //make stairs up thing
            stup.Spot.Type = SpotType.StairsUp; //stairs up
            stup.Spot.Seen = SeenType.NotSeen; //not seen
            (stup.X, stup.Y) = currentlevel.Rooms[0].Place(); //place it in first room
            currentlevel.Items.Add(stup); //add it to the current level
            Thing stdn = new()
            {
                Name = "stairs down", //name it
                Description = "stairs that go down to the next level"
            }; //make stairs down thing
            stdn.Spot.Type = SpotType.StairsDown; //stairs down
            stdn.Spot.Seen = SeenType.NotSeen; //not seen
            (stdn.X, stdn.Y) = currentlevel.Rooms[^1].Place(); //place it in last room
            currentlevel.Items.Add(stdn); //add it to the current level
            List<Thing> itemthings = new(); //make a list of things
            using StreamReader sr = File.OpenText("items.txt"); //open the file items.txt
            {
                while (!sr.EndOfStream) //while the file is not at the end
                {
                    itemthings.Add(new(sr)); //read each thing into our list of things
                }
            }
            Info.Out($"There are {itemthings.Count} items, "); //display information
            itemthings = itemthings.Where(t => t.Level <= level).ToList();  //parse list of things to things that are appropriate for level
            Info.OutL($"with {itemthings.Count} items appropriate for this level."); //display information
            List<Thing> creaturethings = new(); //make a list of creatures
            using StreamReader sread = File.OpenText("creatures.txt"); //open the file creatures.txt
            {
                while (!sread.EndOfStream) //while the file is not at the end
                {
                    creaturethings.Add(new(sread)); //read each creature into our list of things
                }
            }
            Info.Out($"There are {creaturethings.Count} monsters, "); //display information
            creaturethings = creaturethings.Where(t => t.Level <= level).ToList();  //parse list of things to things that are appropriate for level
            Info.OutL($"with {creaturethings.Count} monsters appropriate for this level."); //display information
            currentlevel.Creatures.Clear(); //ckear creatures for this level
            foreach (var room in currentlevel.Rooms) //for each room in this level
            {
                if (itemthings.Count > 0) //if there are any items
                {
                    int itemindex = Rand.UpTo(itemthings.Count); //random thing
                    Thing item = new(itemthings[itemindex]); //make new thing
                    (item.X, item.Y) = room.Place(); //place in room
                    currentlevel.Items.Add(new(item)); //copy to current level
                }
                if (Rand.UpTo(100) < 15) //15% of the time
                {
                    room.Type = RoomType.Lit; //make room lit
                }
                if (Rand.UpTo(100) < 10) //10% of the time
                {
                    room.Type = RoomType.Safe; //make room a safe room
                }
                if (Rand.UpTo(100) < 5) //5% of the time
                {
                    if (currentlevel.Rooms.IndexOf(room) != 0) //if it is not the first room
                    {
                        room.Type = RoomType.Shop; //make room a shop
                    }
                }
                if (creaturethings.Count > 0 && room.Type != RoomType.Safe && room.Type != RoomType.Shop) //if there are any monsters
                {
                    int creatureindex = Rand.UpTo(creaturethings.Count); //random thing
                    Thing creature = new(creaturethings[creatureindex]); //make new thing
                    (creature.X, creature.Y) = room.Place(); //place in room
                    currentlevel.Creatures.Add(new(creature)); //copy to current level
                }
                if (room.Type == RoomType.Shop) //if it is a shop
                {
                    Thing shopkeep = new() //set up a shopkeep
                    {
                        Name = "ShopKeep",
                        Description = "A merchant who will buy and sell pretty much anything",
                        AC = 25, //make shopkeep hard to hit
                        Alignment = Alignment.ChaoticNeutral, //make shopkeep CN
                        BaseRace = RaceType.DrowElf, //make shopkeep Drow for sight
                        NumberAttacks = 3, //give shopkeep 3 attacks per round
                        Spot = new() 
                        { 
                            Type = SpotType.Creature | SpotType.NPC, 
                            Seen = SeenType.NotSeen 
                        }, //set shopkeep NPC and not seen
                        HP = 100, MaxHP = 100, //give shopkeep high HP
                        Attacks = new() //we will have two attacks
                        {
                            new() //melee attack
                            {
                                Damages = new() //high damage
                                {
                                    "5d12+4"
                                },
                                DamageTypes = new()
                                {
                                    DamageType.Slashing
                                },
                                ToHit = 9
                            },
                            new() //ranged attack
                            {
                                Damages = new() //also high damage
                                {
                                    "5d12+4"
                                },
                                DamageTypes = new()
                                {
                                    DamageType.Piercing
                                },
                                ToHit = 9,
                                Range = 120, //decent enough range
                                MaxRange = 300
                            }
                        }, //give shopkeep heavy attacks
                        STR = 18, //give shopkeep awesome stats
                        DEX = 18,
                        CON = 18,
                        INT = 18,
                        WIS = 18,
                        CHA = 18
                    };
                    (shopkeep.X, shopkeep.Y) = room.Place(); //place shopkeep in room
                    currentlevel.Creatures.Add(new(shopkeep)); //copy shopkeep to current level
                }
            }
        }

        private void DisplayRoom(Thing thing)
        {
            Room? room = thing.InRoom(); //get room thing is in
            if (room != null) //if room exists
            {
                int index = currentlevel.Rooms.IndexOf(room); //get index number for room
                Info.OutL($"Room # {index}", Color.DarkSalmon); //display info
            }
        }

        private void MoveThisCreature(Thing creature)
        {
            if (creature.CanMove()) //if the creature can move
            {
                if (creature.HasEffect(Effect.Slow)) //if the creature is slowed
                {
                    if (slowturn) //if slowturn
                    {
                        creature.Move(); //give it its move
                    }
                    slowturn = !slowturn; //toggle slowturn
                }
                else //otherwise (not slowed)
                {
                    creature.Move(); //give it its move
                    if (creature.HasEffect(Effect.Speed)) //if it is speedy
                    {
                        creature.Move(); //give it another turn
                    }
                }
            }
        }

        private void MoveCreatures()
        {
            List<Thing> deadcreatures = new();
            foreach (var creature in currentlevel.Creatures) //for every creature on level
            {
                if (!creature.Type.HasFlag(SpotType.Player)) //if it isn't player
                {
                    creature.HandleConditions(); //handle any conditions creature has
                    if (creature.Dead) //if the creature died
                    {
                        deadcreatures.Add(creature); //add it to the list of dead creatures
                    }
                    else //otherwise
                    {
                        if (player.HasEffect(Effect.Speed)) //if player is sped up
                        {
                            if (fastturn) //if fastturn
                            {
                                MoveThisCreature(creature); //move the creature
                            }
                            fastturn = !fastturn; //toggle fastturn
                        }
                        else //otherwise (player is not sped up)
                        {
                            MoveThisCreature(creature); //move the creature
                            if (player.HasEffect(Effect.Slow)) //if the player is slowed
                            {
                                MoveThisCreature(creature); //move the creature again
                            }
                        }
                    }
                }
            }
            foreach (var creature in deadcreatures) //for creatures that are dead
            {
                currentlevel.Creatures.Remove(creature); //remove them from the list of creatures
            }
            player.HandleConditions(); //handle any conditions player has
            if (player.Dead) //if the player is dead
            {
                Info.OutL($"{player.Name} is dead!"); //display info
                if (player.Class == ClassType.Explorer) //if the player is an explorer
                {
                    if (!GetYesNo("You died!  Want to die?")) //if user wants to die
                    {
                        Info.OutL($"Resurrecting {player.Name} and setting HP to {player.MaxHP}."); //display info
                        player.HP = player.MaxHP; //set hp to maximum
                        player.Dead = false; //reset dead flag
                        return; //quick exit
                    }
                }
                MessageBox.Show($"{player.Name} died as a Level {player.Level} {Data.GetClassName(player.Class)}.", "Uh oh!", MessageBoxButtons.OK); //show message
                Application.Exit(); //exit the game
            }
        }

        private void See()
        {
            List<Point> isseen = currentlevel.GetVisible(player); //get visible area
            List<Point> expseen = Level.ExpandVisible(isseen); //expand it to show walls of rooms and such
            expseen.ForEach(p => //foreach p in expanded isseen
            {
                map.MapSpots[map.GetIndex(p.X, p.Y)].Spot.See(); //mark seen in map
            });
            Room? room = currentlevel.Rooms.Find(r => r.Inside(player.X, player.Y)); //get room player is in
            if (room != null) //if it exists (player is in that room)
            {
                if (room.Type == RoomType.Lit) //and if it is a lit room
                {
                    for (int x = room.L; x <= room.R; x++) //for every horizontal position in room
                    {
                        for (int y = room.T; y <= room.B; y++) //for every vertical position in room
                        {
                            map.MapSpots[map.GetIndex(x, y)].Spot.See(); //mark position seen in map
                        }
                    }
                }
            }
        }

        public static void ShowMiniMap()
        {
            Map map = GameData.CurrentLevel.Map;
            int wide = map.Wide;
            int high = map.High;
            Bitmap mini = new(wide, high); //set up a bitmap
            using Graphics gm = Graphics.FromImage(mini); //using graphics object made from mini bitmap
            {
                for (int x = 0; x < wide; x++) //for each x position
                {
                    for (int y = 0; y < high; y++) //for each y position
                    {
                        int i = map.GetIndex(x, y); //get map index
                        if (i != -1) //if we got an index
                        {
                            SeenType seen = map.MapSpots[i].Spot.Seen; //get seen for this map spot
                            switch (map.MapSpots[i].Type)
                            {
                                case MapType.BottomDoor: //if it is a door
                                case MapType.TopDoor:
                                case MapType.LeftDoor:
                                case MapType.RightDoor:
                                    DrawMini(gm, x, y, Color.Green); //use the color green
                                    break;
                                case MapType.BottomWall: //if it is a wall or corner
                                case MapType.TopWall:
                                case MapType.LeftWall:
                                case MapType.RightWall:
                                case MapType.LLCorner:
                                case MapType.LRCorner:
                                case MapType.ULCorner:
                                case MapType.URCorner:
                                    DrawMini(gm, x, y, Color.Gray); //use the color gray
                                    break;
                                case MapType.Room: //if it is a room
                                    DrawMini(gm, x, y, Color.Silver); //use the color silver
                                    break;
                                case MapType.Path: //if it is a path
                                    DrawMini(gm, x, y, Color.SeaShell); //make the color seashell
                                    break;
                                case MapType.Stone: //if it is stone
                                    DrawMini(gm, x, y, Color.Brown); //make the color brown
                                    break;

                            }
                        }
                    }
                }
            }
            MiniMap.Image = mini;
        }

        private void DisplayMiniMap(bool inuse)
        {
            if (!readytoplay) //if we are not yet ready to play
            {
                return; //quick exit
            }
            if (inuse) //if we are in use
            {
                See(); //we'll need to have seen flags set
            }
            Bitmap mini = new(minimap.Width, minimap.Height); //make a bitmap for minimap
            using Graphics gm = Graphics.FromImage(mini); //using a graphics object made from the mini bitmap
            {
                for (int x = 0; x < wide; x++) //for each x position
                {
                    for (int y = 0; y < high; y++) //for each y position
                    {
                        int i = map.GetIndex(x, y); //get map index
                        if (i != -1) //if we got an index
                        {
                            SeenType seen = map.MapSpots[i].Spot.Seen; //get seen for this map spot
                            if (Settings.UseMiniMap || (Settings.MiniMapDrawOnMM && hasdonemmthislevel)) //if we are doing minimap
                            {
                                switch (map.MapSpots[i].Type) //switch by map type
                                {
                                    case MapType.BottomDoor: //if it is a door
                                    case MapType.TopDoor:
                                    case MapType.LeftDoor:
                                    case MapType.RightDoor:
                                        DrawMini(gm, x, y, Color.Green, seen, inuse); //use the color green
                                        break;
                                    case MapType.BottomWall: //if it is a wall or corner
                                    case MapType.TopWall:
                                    case MapType.LeftWall:
                                    case MapType.RightWall:
                                    case MapType.LLCorner:
                                    case MapType.LRCorner:
                                    case MapType.ULCorner:
                                    case MapType.URCorner:
                                        DrawMini(gm, x, y, Color.Gray, seen, inuse); //use the color gray
                                        break;
                                    case MapType.Room: //if it is a room
                                        DrawMini(gm, x, y, Color.Silver, seen, inuse); //use the color silver
                                        break;
                                    case MapType.Path: //if it is a path
                                        DrawMini(gm, x, y, Color.SeaShell, seen, inuse); //make the color seashell
                                        break;
                                    case MapType.Stone: //if it is stone
                                        DrawMini(gm, x, y, Color.Brown, seen, inuse); //make the color brown
                                        break;
                                }
                            }

                        }
                    }
                }
                if (Settings.UseMiniMap) //if we are doing minimap
                {
                    int vleft = posx - (box.Width / (2 * Settings.Zoom)); //compute boundaries
                    int vright = posx + (box.Width / (2 * Settings.Zoom));
                    int vtop = posy - (box.Height / (2 * Settings.Zoom));
                    int vbottom = posy + (box.Height / (2 * Settings.Zoom));
                    int vCenterX = (vright - vleft) / 2 + vleft; //figure center position
                    int vCenterY = (vbottom - vtop) / 2 + vtop;
                    int vWide = vright - vleft + 1; //compute width and height
                    int vHigh = vbottom - vtop + 1;
                    MiniArea(gm, vCenterX, vCenterY, vWide, vHigh, Color.White, ShapeType.Square, .15f); //draw area as a square
                }
                else //otherwise
                {
                    if (!hasdonemmthislevel || !Settings.MiniMapDrawOnMM)
                    {
                        gm.Clear(Color.Black); //clear the minimap
                    }
                }
            }
            minimap.Image = mini; //set minimap image
        }

        private void Display()
        {
            if (!readytoplay) //if we are not yet ready to play
            {
                return; //quick exit
            }
            ambusy = true; //we are busy now
            See(); //Handle what is seen / hidden
            if (followplayer) //if we are following player
            {
                (posx, posy) = (player.X, player.Y); //update posx, posy with player position
            }
            int vleft = posx - (box.Width / (2 * Settings.Zoom)); //compute boundaries
            int vright = posx + (box.Width / (2 * Settings.Zoom));
            int vtop = posy - (box.Height / (2 * Settings.Zoom));
            int vbottom = posy + (box.Height / (2 * Settings.Zoom));
            Bitmap viewport = new(box.Width, box.Height); //set up view port
            Bitmap mini = new(minimap.Width, minimap.Height); //set up mini map
            using Graphics g = Graphics.FromImage(viewport); //get graphics object
            {
                for (int x = vleft; x <= vright; x++) //for every map tile
                {
                    for (int y = vtop; y <= vbottom; y++)
                    {
                        int i = map.GetIndex(x, y); //get map index
                        if (i != -1) //if we got an index
                        {
                            SeenType seen = map.MapSpots[i].Spot.Seen; //get seen for this map spot
                            if (x >= vleft && x <= vright && y >= vtop && y <= vbottom) //if in bounds of screen
                            {
                                switch (map.MapSpots[i].Type) //show proper picture according to map type
                                {
                                    case MapType.BottomDoor:
                                        DrawZoomedImage(g, _10doorb, x, y, seen);
                                        break;
                                    case MapType.TopDoor:
                                        DrawZoomedImage(g, _09doort, x, y, seen);
                                        break;
                                    case MapType.LeftDoor:
                                        DrawZoomedImage(g, _11doorl, x, y, seen);
                                        break;
                                    case MapType.RightDoor:
                                        DrawZoomedImage(g, _12doorr, x, y, seen);
                                        break;
                                    case MapType.BottomWall:
                                        DrawZoomedImage(g, _06horb, x, y, seen);
                                        break;
                                    case MapType.TopWall:
                                        DrawZoomedImage(g, _05hort, x, y, seen);
                                        break;
                                    case MapType.LeftWall:
                                        DrawZoomedImage(g, _07verl, x, y, seen);
                                        break;
                                    case MapType.RightWall:
                                        DrawZoomedImage(g, _08verr, x, y, seen);
                                        break;
                                    case MapType.LLCorner:
                                        DrawZoomedImage(g, _03corbl, x, y, seen);
                                        break;
                                    case MapType.LRCorner:
                                        DrawZoomedImage(g, _04corbr, x, y, seen);
                                        break;
                                    case MapType.ULCorner:
                                        DrawZoomedImage(g, _01cortl, x, y, seen);
                                        break;
                                    case MapType.URCorner:
                                        DrawZoomedImage(g, _02cortr, x, y, seen);
                                        break;
                                    case MapType.Room:
                                        DrawZoomedImage(g, _13room, x, y, seen);
                                        break;
                                    case MapType.Path:
                                        DrawZoomedImage(g, _14path, x, y, seen);
                                        break;
                                    case MapType.Stone:
                                        DrawZoomedImage(g, _00rock, x, y, seen); //display rock
                                        break;
                                }
                            }
                            if (seen != SeenType.NotSeen) //if not hidden
                            {
                                foreach (var item in currentlevel.Items.Where(i => i.X == x && i.Y == y)) //for every item
                                {
                                    if (item.IsStairs()) //if stairs
                                    {
                                        DrawItem(g, item); //draw them (always seen)
                                    }
                                    else if (seen == SeenType.Seen) //otherwise if actually seen
                                    {
                                        DrawItem(g, item); //draw it
                                    }
                                }
                                if (TargetingSystem.IsPoint()) //if we have a point targetted
                                {
                                    Point tpoint = TargetingSystem.GetPoint(); //get the target point
                                    if (tpoint.X == x && tpoint.Y == y) //if that point is the same as the current point
                                    {
                                        DrawThing(g, _9999positiontarget, x, y, SeenType.Seen); //draw position target
                                    }
                                }
                            }
                            if (seen == SeenType.Seen) //if seen
                            {
                                foreach (var creature in currentlevel.Creatures.Where(c => c.X == x && c.Y == y)) //for every creature
                                {
                                    DrawCreature(g, creature); //draw it
                                }
                            }
                            map.MapSpots[i].Spot.DontSee(); //now that we've displayed the spot, we can hide it from view again
                        }
                    }
                }
                DrawThing(g, _17player, player.X, player.Y, SeenType.Seen); //player is always seen!
                if (lriactive) //if light radius indicator active
                {
                    using SolidBrush whitebrush = new(Color.FromArgb(64, Color.White)); //use a partially transparent white brush
                    {
                        //figure out the rectangle that encloses the ellipse
                        Rectangle lightrect = new((player.X - posx) * Settings.Zoom + box.Width / 2, (player.Y - posy) * Settings.Zoom + box.Height / 2,
                            (player.Light() + 1) * Settings.Zoom * 2, (player.Light() + 1) * Settings.Zoom * 2);
                        lightrect.X += Settings.Zoom / 2 - lightrect.Width / 2;
                        lightrect.Y += Settings.Zoom / 2 - lightrect.Height / 2;
                        g.FillEllipse(whitebrush, lightrect); //fill the ellipse
                    }
                }
                if (Settings.DoPlayerStatus) //if status bar active
                {
                    DrawStatusBars(g); //draw status bars
                }
            }
            box.Image = viewport; //set viewport image
            if (Settings.UseMiniMap) //if we are using minimap
            {
                DisplayMiniMap(true); //then we better display it
            }
            ambusy = false; //we are no longer busy
        }

        private void DrawItem(Graphics g, Thing thing)
        {
            if (thing.Type == SpotType.StairsUp) //if it is stairs up
            {
                DrawThing(g, _15stairu, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.StairsDown) //if it is stairs down
            {
                DrawThing(g, _16staird, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.Potion) //if it is a potion
            {
                DrawThing(g, _18potion, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.Armor) //if it is armor
            {
                DrawThing(g, _19armor, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.Scroll) //if it is a scroll
            {
                DrawThing(g, _20scroll, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.Weapon) //if it is a weapon
            {
                DrawThing(g, _21weapon, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.Gold) //if it is gold
            {
                DrawThing(g, _22gold, thing.X, thing.Y, SeenType.Seen); //draw it
            }
            if (thing.Type == SpotType.LightSource) //if it is a light source
            {
                DrawThing(g, _23lantern, thing.X, thing.Y, SeenType.Seen); //draw it
            }
        }

        private void DrawCreature(Graphics g, Thing creature)
        {
            if (creature.Monster == MonsterType.Kobold_Orc) //if it is kobold, goblin, orc
            {
                DrawThing(g, _24kobold_orc, creature.X, creature.Y, SeenType.Seen); //draw it
            }
            if (creature.Type.HasFlag(SpotType.NPC)) //if it is an NPC
            {
                DrawThing(g, _25shopkeep, creature.X, creature.Y, SeenType.Seen); //draw it
            }
            if (TargetingSystem.IsTargetted(creature)) //if it is targetted
            {
                DrawThing(g, _9999creaturetarget, creature.X, creature.Y, SeenType.Seen, 0.25f); //draw target square
            }
            if (Settings.DoMonsterStatus) //if we are doing monster status bars
            {
                if (!Settings.DoMonsterStatusOnlyLowHP || (creature.HP < creature.MaxHP)) //if we are not doing only for hurt or if creature is hurt
                {
                    DrawStatusBars(g, creature); //draw status bars for creature
                }
            }
        }

        private void DrawZoomedImage(Graphics g, Image i, int x, int y, SeenType seen)
        {
            Rectangle dst = new((x - posx) * Settings.Zoom + box.Width / 2, (y - posy) * Settings.Zoom + box.Height / 2, Settings.Zoom, Settings.Zoom); //destination rectangle
            Rectangle src = new(0, 0, i.Width, i.Height); //source rectangle
            DrawZoomedImage(g, i, dst, src, seen); //draw the image
        }

        private void DrawThing(Graphics g, Image i, int x, int y, SeenType seen)
        {
            Rectangle dst = new((x - posx) * Settings.Zoom + box.Width / 2, (y - posy) * Settings.Zoom + box.Height / 2, Settings.Zoom, Settings.Zoom); //destination rectangle
            Rectangle src = new(0, 0, i.Width, i.Height); //source rectangle
            if (seen == SeenType.Seen) //if thing is seen
            {
                g.DrawImage(i, dst, src, GraphicsUnit.Pixel); //draw the image
            }
        }

        private void DrawThing(Graphics g, Image i, int x, int y, SeenType seen, float opacity)
        {
            Rectangle dst = new((x - posx) * Settings.Zoom + box.Width / 2, (y - posy) * Settings.Zoom + box.Height / 2, Settings.Zoom, Settings.Zoom); //destination rectangle
            //Rectangle src = new(0, 0, i.Width, i.Height); //source rectangle
            if (seen == SeenType.Seen) //if thing is seen
            {
                ColorMatrix matrix = new()
                {
                    Matrix33 = opacity //set the alpha component (opacity / transparency)
                }; //create a ColorMatrix to adjust opacity
                using ImageAttributes attrib = new(); //use image attributes for drawing
                {
                    attrib.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); //set for transparency
                    g.DrawImage(i, dst, 0, 0, i.Width, i.Height, GraphicsUnit.Pixel, attrib); //draw with transparency
                }
            }
        }

        private static void DrawZoomedImage(Graphics g, Image i, Rectangle dst, Rectangle src, SeenType seen)
        {
            float opacity = 1.0f; //fully opaque
            if (seen == SeenType.Remembered)
            {
                opacity = 0.5f; //half opaque
            }
            else if (seen == SeenType.NotSeen)
            {
                opacity = 0; //fully transparent
            }
            using Bitmap b = MakeDarkVersion(i, opacity); //using a darkened version of image
            {
                g.DrawImage(b, dst, src, GraphicsUnit.Pixel); //draw it
            }
        }

        private static Bitmap MakeDarkVersion(Image pic, float opacity)
        {
            Bitmap background = new(pic.Width, pic.Height); //set up background to use pic's dimensions
            using Bitmap overlay = new(pic); //set up overlay picture from pic
            {
                using Graphics g = Graphics.FromImage(background); //use graphics object from background
                {
                    g.Clear(Color.Black); //color it black
                    ColorMatrix matrix = new()
                    {
                        Matrix33 = opacity //set the alpha component (opacity / transparency)
                    }; //create a ColorMatrix to adjust opacity
                    using ImageAttributes attrib = new();
                    {
                        attrib.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); //set for transparency
                        Rectangle rect = new(0, 0, pic.Width, pic.Height);
                        g.DrawImage(overlay, rect, 0, 0, pic.Width, pic.Height, GraphicsUnit.Pixel, attrib); //draw overlay using transparency
                    }
                }
            }
            return background; //return our new image
        }

        private static void DrawMini(Graphics g, int x, int y, Color c)
        {
            using Bitmap bmp = new(1, 1);
            {
                using Graphics gb = Graphics.FromImage(bmp);
                {
                    gb.Clear(c);
                }
                g.DrawImage(bmp, x, y); //draw it
            }
        }

        private static void DrawMini(Graphics g, int x, int y, Color c, SeenType seen, bool inuse)
        {
            using Bitmap bmp = new(1, 1);
            {
                using Graphics gb = Graphics.FromImage(bmp);
                {
                    gb.Clear(c);
                }
                float opacity = 1.0f; //fully opaque
                if (inuse)
                {
                    if (seen == SeenType.Remembered)
                    {
                        opacity = 0.5f; //half opaque
                    }
                    else if (seen == SeenType.NotSeen)
                    {
                        opacity = 0; //fully transparent
                    }
                }
                using Bitmap b = MakeDarkVersion(bmp, opacity); //using a darkened version of image
                {
                    g.DrawImage(b, x, y); //draw it
                }
            }
        }

        private static void MiniArea(Graphics g, int x, int y, int w, int h, Color c, ShapeType shape, float opacity)
        {
            Bitmap overlay = new(w, h, PixelFormat.Format32bppArgb); //make an overlay image with transparency
            using Graphics g2 = Graphics.FromImage(overlay); //get graphics object from it
            {
                //g2.Clear(Color.Transparent); //clear it to transparent - no need for this
                SolidBrush brush = new(c); //make a brush from the provided color
                if (shape == ShapeType.Circle) //if it's a circle
                {
                    g2.FillEllipse(brush, 0, 0, w, h); //draw the circle
                }
                if (shape == ShapeType.Square) //if it's a square
                {
                    g2.FillRectangle(brush, 0, 0, w, h); //draw the square
                }
                if (shape == ShapeType.Cone) //if it's a cone
                {
                    int tx = x + w; //compute target x, y
                    int ty = y + h;
                    float d = (float)Math.Sqrt(Math.Pow(tx - x, 2) + Math.Pow(ty - y, 2)); //compute distance
                    int d2 = (int)(d * 2); //compute diagonal
                    float angle = (float)Math.Atan2(ty - y, tx - x); //calculate angle between start and target
                    int ax = (int)(x + d * Math.Cos(angle + Math.PI)); //calculate opposite point
                    int ay = (int)(y + d * Math.Sin(angle + Math.PI));
                    int start = (int)(angle * 180 / Math.PI); //compute start angle
                    g2.FillPie(brush, ax, ay, d2, d2, start, 60); //fill the pie slice
                }
                if (shape == ShapeType.Line) //if it's a line
                {
                    ///
                    ///TODO: we need to handle lines for spells like lightning bolt
                    ///
                }
                if (shape == ShapeType.Cylinder) //if it's a cylinder
                {
                    ///
                    ///TODO: We need to handle cylinders though I'm not sure what for
                    ///
                }
            }
            ColorMatrix matrix = new()
            {
                Matrix33 = opacity //set the alpha component (opacity / transparency)
            }; //create a ColorMatrix to adjust opacity
            using ImageAttributes attrib = new();
            {
                attrib.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); //set for transparency
                //draw overlay using transparency
                g.DrawImage(overlay, new Rectangle(x - w / 2, y - h / 2, w, h), 0, 0, w, h, GraphicsUnit.Pixel, attrib);
            }
        }

        private void DrawStatusBars(Graphics g)
        {
            SolidBrush greybrush = new(Color.FromArgb(128, Color.Gray)); //set up colors
            SolidBrush redbrush = new(Color.FromArgb(128, Color.Red));
            SolidBrush bluebrush = new(Color.FromArgb(128, Color.Blue));
            int hppercent = (player.HP * Settings.Zoom / player.MaxHP); //compute rectangles
            int xppercent = (player.Experience * Settings.Zoom / player.NextLevel);
            Rectangle hprect1 = new((player.X - posx) * Settings.Zoom + box.Width / 2, (player.Y - posy) * Settings.Zoom + box.Height / 2 - 5, hppercent, 2);
            Rectangle hprect2 = new(hprect1.Left + hprect1.Width, hprect1.Top, Settings.Zoom - hppercent, 2);
            Rectangle xprect1 = new(hprect1.Left, hprect1.Top + 3, xppercent, 2);
            Rectangle xprect2 = new(xprect1.Left + xprect1.Width, xprect1.Top, Settings.Zoom - xppercent, 2);
            g.FillRectangle(redbrush, hprect1); //draw the status bars
            g.FillRectangle(greybrush, hprect2);
            g.FillRectangle(bluebrush, xprect1);
            g.FillRectangle(greybrush, xprect2);
        }

        private void DrawStatusBars(Graphics g, Thing creature)
        {
            SolidBrush greybrush = new(Color.FromArgb(128, Color.Gray)); //set up colors
            SolidBrush redbrush = new(Color.FromArgb(128, Color.Red));
            int hppercent = (creature.HP * Settings.Zoom / creature.MaxHP); //compute rectangles
            Rectangle hprect1 = new((creature.X - posx) * Settings.Zoom + box.Width / 2, (creature.Y - posy) * Settings.Zoom + box.Height / 2 - 3, hppercent, 2);
            Rectangle hprect2 = new(hprect1.Left + hprect1.Width, hprect1.Top, Settings.Zoom - hppercent, 2);
            g.FillRectangle(redbrush, hprect1); //draw the status bars
            g.FillRectangle(greybrush, hprect2);
        }
        private void MakeNewItem(bool inventory)
        {
            ItemCreation ic = new(); //set up item creation form
            ic.ShowDialog(); //display it as a dialog
            if (GameData.NewThing == null) //if nothing was generated
            {
                return; //we are done
            }
            if (inventory)
            {
                GameData.Player.Things.Add(GameData.NewThing);
            }
            else
            {
                (GameData.NewThing.X, GameData.NewThing.Y) = (player.X, player.Y); //put it at player's feet
                if (GameData.NewThing.IsCreature()) //if it is a creature
                {
                    currentlevel.Creatures.Add(GameData.NewThing); //add it to creatures
                    Info.OutL($"Creature {GameData.NewThing.Name} added"); //display info
                    MaybeAppend("creatures.txt", GameData.NewThing); //possibly append it to creatures.txt
                }
                else //otherwise (it is an item)
                {
                    currentlevel.Items.Add(GameData.NewThing); //add it to items
                    Info.OutL($"Item {GameData.NewThing.Name} added"); //display info
                    MaybeAppend("items.txt", GameData.NewThing); //possibly append it to items.txt
                }
            }
        }
        public static bool GetYesNo(string prompt)
        {
            if (MessageBox.Show(prompt, "Query", MessageBoxButtons.YesNo) == DialogResult.Yes) //if messagebox is responded to with Yes
            {
                return true; //return true
            }
            return false; //otherwise return false
        }
        private static void MaybeAppend(string file, Thing thing)
        {
            if (GetYesNo($"Append {thing.Name} to {file}")) //if user says to append it
            {
                if (!File.Exists(file)) //if file doesn't exist
                {
                    using StreamWriter swdiscard = File.CreateText(file);
                    { } //create it
                    Info.OutL($"Created new file {file}."); //info display
                }
                using StreamWriter sw = File.AppendText(file); //get StreamWriter from file in append mode
                {
                    thing.Save(sw); //use thing to save to StreamWriter
                }
                Info.OutL($"Appended {thing.Name} to {file}."); //info display
            }
        }
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog1 = new() //make a new save file dialog
            {
                Title = "Save Game",
                CheckPathExists = true,
                DefaultExt = "sav",
                Filter = "Save files (*.sav)|*.sav|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) //display it, if user selected a filename
            {
                using StreamWriter sw = new(File.Create(saveFileDialog1.FileName)); //use a streamwriter with this filename
                {
                    GameData.Player.Save(sw); //save player
                    currentlevel.Save(sw); //save currentlevel
                }
            }
        }
        private void LoadGame()
        {
            OpenFileDialog openFileDialog1 = new() //set up a open file dialog
            {
                Title = "Browse Save Files",
                CheckPathExists = true,
                DefaultExt = "sav",
                Filter = "Save files (*.sav)|*.sav",
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //display it, if user selected a file
            {
                using StreamReader sr = File.OpenText(openFileDialog1.FileName); //use a streamreader with this file
                {
                    GameData.Player.Load(sr); //load the player
                    player = GameData.Player; //make sure player matches GameData's Player
                    currentlevel.Load(sr); //load the currentlevel
                    (posx, posy) = (GameData.Player.X, GameData.Player.Y); //set display position
                    Display(); //display
                }
            }
        }
    }
    public enum ShapeType
    {
        Square,
        Circle,
        Cone,
        Line,
        Cylinder,
        //add to as needed
    }
}
