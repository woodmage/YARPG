namespace YARPG
{
    public partial class Shop : Form
    {
        readonly Thing player = GameData.Player;
        bool allowtransaction = false;
        readonly Thing BackupPlayer;
        readonly List<Thing> weapons = new();
        readonly List<Thing> armors = new();
        readonly List<Thing> potions = new();
        readonly List<Thing> scrolls = new();
        readonly List<Thing> lights = new();
        public Shop()
        {
            InitializeComponent();
            BackupPlayer = new(GameData.Player); //make a backup of player data
        }

        private void ShopLoad(object sender, EventArgs e)
        {
            MakeInventory(); //make the shop's inventory
            PopulateLists(); //populate the listboxes
        }

        private void MakeInventory()
        {
            List<Thing> things = new(); //list of things
            using StreamReader sr = File.OpenText("items.txt"); //use a StreamReader from the text file items.txt
            {
                while (!sr.EndOfStream) //while we haven't reached the end of the file
                {
                    things.Add(new(sr)); //read in an item and add to list of things
                }
            }
            weapons.Clear(); //clear the lists
            armors.Clear();
            potions.Clear();
            scrolls.Clear();
            lights.Clear();
            foreach (var thing in things) //for each thing in the list of things
            {
                if (thing.Spot.Type.HasFlag(SpotType.Weapon)) //if it is a weapon
                {
                    weapons.Add(thing); //add it to weapons list
                }
                else if (thing.Spot.Type.HasFlag(SpotType.Armor)) //if it is an armor
                {
                    armors.Add(thing); //add it to armors list
                }
                else if (thing.Spot.Type.HasFlag(SpotType.Potion)) //if it is a potion
                {
                    potions.Add(thing); //add it to potions list
                }
                else if (thing.Spot.Type.HasFlag(SpotType.Scroll)) //if it is a scroll
                {
                    scrolls.Add(thing); //add it to the scrolls list
                }
                else if (thing.Spot.Type.HasFlag(SpotType.LightSource)) //if it is a light
                {
                    lights.Add(thing); //add it to the lights list
                }
                else if (thing.Spot.Type.HasFlag(SpotType.Gold)) //if it is gold
                {
                    ; //do nothing
                }
                else //otherwise
                {
                    MessageBox.Show($"Item {thing.Name}: {thing.Description} has a bad item type: {thing.Spot.Type}!",
                        "Error", MessageBoxButtons.OK); //display info about the thing
                }
            }
        }
        private void PopulateLists()
        {
            SetDataSource(listWeapons, weapons); //set each listbox to use a List<Thing> source
            SetDataSource(listArmors, armors);
            SetDataSource(listPotions, potions);
            SetDataSource(listScrolls, scrolls);
            SetDataSource(listLights, lights);
            SetDataSource(listInventory, player.Things);
            textGold.Text = $"Gold: {player.Value}"; //show the gold player has
            allowtransaction = true; //allow transactions
        }

        private void WeaponSelect(object sender, EventArgs e)
        {
            Thing weapon = (Thing)listWeapons.SelectedItem; //get weapon
            if (allowtransaction) //if transaction is allowed
            {
                Purchase(weapon); //purchase it
            }
        }
        private void ArmorSelect(object sender, EventArgs e)
        {
            Thing armor = (Thing)listArmors.SelectedItem; //get armor
            if (allowtransaction) //if transaction is allowed
            {
                Purchase(armor); //purchase it
            }
        }
        private void PotionSelect(object sender, EventArgs e)
        {
            Thing potion = (Thing)listPotions.SelectedItem; //get potion
            if (allowtransaction) //if transaction is allowed
            {
                Purchase(potion); //purchase it
            }
        }
        private void ScrollSelect(object sender, EventArgs e)
        {
            Thing scroll = (Thing)listScrolls.SelectedItem; //get scroll
            if (allowtransaction) //if transaction is allowed
            {
                Purchase(scroll); //purchase it
            }
        }
        private void LightSelect(object sender, EventArgs e)
        {
            Thing light = (Thing)listLights.SelectedItem; //get light source
            if (allowtransaction) //if transaction is allowed
            {
                Purchase(light); //purchase it
            }
        }
        private void InventorySelect(object sender, EventArgs e)
        {
            Thing thing = (Thing)listInventory.SelectedItem; //get inventory item
            if (allowtransaction) //if transaction is allowed
            {
                Sell(thing); //sell it
            }
        }
        private static void SetDataSource(ListBox lbox, List<Thing> list)
        {
            lbox.DataSource = null; //unset any datasource the listbox could be using
            lbox.DataSource = list; //set the listbox to use the list for its datasource
            lbox.DisplayMember = "Name"; //tell the listbox to use the .Name variable for display
        }
        private bool Purchase(Thing thing)
        {
            bool boughtit = false; //flag for whether player purchased item
            allowtransaction = false; //transactions are not allowed while purchasing
            if (player.Value >= thing.Value) //if player has enough money to buy the thing
            {
                //if player agrees to buy the thing
                if (MessageBox.Show($"You have {player.Value} gold, buy {thing.Name} for {thing.Value}?", "Purchase", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    player.Value -= thing.Value; //take away the money from the player
                    player.Things.Add(new(thing)); //put (a copy of) the thing in the player's inventory
                    SetDataSource(listInventory, player.Things); //refresh the inventory listbox
                    MessageBox.Show($"You have {player.Value} gold left.", "Done", MessageBoxButtons.OK); //show money remaining
                    boughtit = true; //player bought item
                }
                else //otherwise (player decided not to buy)
                {
                    //acknowledge player not purchasing thing and show balance
                    MessageBox.Show($"That's fine.  You still have {player.Value} gold", "Balance", MessageBoxButtons.OK);
                }
            }
            else //otherwise (not enough money)
            {
                MessageBox.Show($"Sorry, you can't afford {thing.Name}", "Not Enough Money", MessageBoxButtons.OK); //tell player
            }
            textGold.Text = $"Gold: {player.Value}"; //update player's gold
            allowtransaction = true; //allow transactions again
            return boughtit; //return boolean for whether item was purchased
        }
        private void Sell(Thing thing)
        {
            allowtransaction = false; //do not allow transactions during a sale
            //if player decides to sell a thing
            if (MessageBox.Show($"You wish to sell your {thing.Name} for {thing.Value}?", "Sales", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                player.Value += thing.Value; //add to player's gold
                player.Things.Remove(thing); //remove the thing from inventory
                SetDataSource(listInventory, player.Things); //refresh the inventory listbox
                MessageBox.Show($"You now have {player.Value} gold.", "Balance", MessageBoxButtons.OK); //tell the player
            }
            else //otherwise (player didn't sell the thing)
            {
                //acknowledge player's decision and give balance
                MessageBox.Show($"Alright, you still have {player.Value} gold.", "Decline", MessageBoxButtons.OK);
            }
            textGold.Text = $"Gold: {player.Value}"; //update player's gold
            allowtransaction = true; //allow transactions again
        }

        private void Cancel(object sender, EventArgs e)
        {
            GameData.Player = new(BackupPlayer); //load player back from backup to remove any changes
            Close(); //close the form
        }

        private void Done(object sender, EventArgs e)
        {
            Close(); //close the form
        }
    }
}
