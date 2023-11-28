namespace YARPG
{
    public partial class PlayerInitialization : Form
    {
        readonly Thing thing = new();
        readonly int[] _stats = new int[6];
        public PlayerInitialization()
        {
            InitializeComponent();
        }

        private void BaseRaceChanged(object sender, EventArgs e)
        {
            string race = (string)comboBaseRace.SelectedItem;
            AdjustStats(false); //take away bonuses from previous race
            thing.BaseRace = Data.racetypes[race]; //set BaseRace
            thing.Race = textRace.Text = race; //set Race textbox and player's Race
            AdjustStats(true); //add bonuses from race
            ComputeAC(); //compute armor class
            ComputeHP(); //compute hit points
        }

        private void AlignmentChanged(object sender, EventArgs e)
        {
            thing.Alignment = Data.alignments[(string)comboAlignment.SelectedItem];
        }

        private void ClassChanged(object sender, EventArgs e)
        {
            thing.Class = Data.classtypes[(string)comboClass.SelectedItem];
            ComputeHP();
            ComputeAC();
            ComputeGold();
        }

        private void StrChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownSTR, 0);
        }

        private void DexChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownDEX, 1);
        }

        private void ConChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownCON, 2);
            ComputeHP();
        }

        private void IntChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownINT, 3);
        }

        private void WisChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownWIS, 4);
        }

        private void ChaChanged(object sender, EventArgs e)
        {
            StatChanged(numericUpDownCHA, 5);
        }

        private void ReRollStats(object sender, EventArgs e)
        {
            RollStats();
        }

        private void Shop(object sender, EventArgs e)
        {
            Thing backup = GameData.Player;
            SavePlayer(false);
            Shop shopform = new();
            shopform.ShowDialog();
            LoadPlayer();
            GameData.Player = backup;
        }

        private void Cancel(object sender, EventArgs e)
        {
            Close();
        }

        private void Done(object sender, EventArgs e)
        {
            SavePlayer(true);
            Close();
        }

        private void PILoad(object sender, EventArgs e)
        {
            ItemCreation.LoadCombo(comboBaseRace, Data.racetypes);
            ItemCreation.LoadCombo(comboAlignment, Data.alignments);
            ItemCreation.LoadCombo(comboClass, Data.classtypes);
            thing.BaseRace = RaceType.Other;
            RollStats();
        }
        private void RollStats()
        {
            List<int> rolls = new();
            int stat;
            for (int i = 0; i < 6; i++)
            {
                rolls.Clear();
                for (int j = 0; j < 3; j++)
                {
                    int roll = Rand.FromTo(2, 6);
                    rolls.Add(roll);
                }
                rolls.Remove(rolls.Min());
                stat = rolls.Sum() + 6;
                _stats[i] = stat;
            }
            textBonus.Text = "0";
            AdjustStats(true);
        }

        private void StatChanged(NumericUpDown nud, int stat)
        {
            int nudv = (int)nud.Value;
            _ = int.TryParse(textBonus.Text, out int bonus);
            if (nudv != _stats[stat])
            {
                bonus += _stats[stat] - nudv;
                textBonus.Text = $"{bonus}";
                _stats[stat] = nudv;
            }
            ComputeAC();
        }

        private void AdjustStats(bool increase)
        {
            List<NumericUpDown> controls = new() { numericUpDownSTR, numericUpDownDEX, numericUpDownCON, numericUpDownINT, numericUpDownWIS, numericUpDownCHA };
            _stats[0] += Data.racedata[thing.BaseRace].STRBonus * (increase ? 1 : -1);
            _stats[1] += Data.racedata[thing.BaseRace].DEXBonus * (increase ? 1 : -1);
            _stats[2] += Data.racedata[thing.BaseRace].CONBonus * (increase ? 1 : -1);
            _stats[3] += Data.racedata[thing.BaseRace].INTBonus * (increase ? 1 : -1);
            _stats[4] += Data.racedata[thing.BaseRace].WISBonus * (increase ? 1 : -1);
            _stats[5] += Data.racedata[thing.BaseRace].CHABonus * (increase ? 1 : -1);
            textDarkVision.Text = $"{Data.racedata[thing.BaseRace].DarkVision}";
            for (int s = 0; s < 6; s++)
            {
                controls[s].Value = _stats[s];
            }
        }

        private void ComputeHP()
        {
            int hp = Data.classhitdice[thing.Class] + (_stats[2] / 2 - 5); //compute hit points
            if (thing.BaseRace == RaceType.God) //if race is God
            {
                hp += 100; //add 100 to hit points
            }
            textHP.Text = $"{hp}"; //set value for display
        }

        private void ComputeGold()
        {
            int gold = Rand.ParseAndRoll(Data.classgold[thing.Class]) * 10; //randomly compute gold
            textGold.Text = $"{gold}"; //set textbox
        }

        private void ComputeAC()
        {
            int ac; //variable for ac
            if (thing.Class == ClassType.Barbarian) //if we are a barbarian
            {
                ac = 10 + (_stats[1] / 2 - 5) + (_stats[2] / 2 - 5); //ac = 10 + Dex, and Con bonuses
            }
            else if (thing.Class == ClassType.Monk) //if we are a monk
            {
                ac = 10 + (_stats[1] / 2 - 5) + (_stats[4] / 2 - 5); //ac = 10 + Dex, and Wis bonuses
            }
            else if (thing.Class == ClassType.Explorer) //if we are an explorer
            {
                ac = 20 + (_stats[0] / 2 - 5) + (_stats[1] / 2 - 5) + (_stats[2] / 2 - 5); //ac = 20 + Str, Dex, and Con bonuses
            }
            else //otherwise (most classes)
            {
                ac = 10 + (_stats[1] / 2 - 5); //ac = 10 + Dex bonus
            }
            if (thing.BaseRace == RaceType.God) //if race is God
            {
                ac += 50; //add 50 to AC
            }
            textAC.Text = $"{ac}"; //set textbox
        }

        private void SavePlayer(bool gift)
        {
            thing.Name = textName.Text; //get player info from form
            thing.STR = _stats[0];
            thing.DEX = _stats[1];
            thing.CON = _stats[2];
            thing.INT = _stats[3];
            thing.WIS = _stats[4];
            thing.CHA = _stats[5];
            _ = int.TryParse(textGold.Text, out int gold);
            thing.Value = gold;
            _ = int.TryParse(textAC.Text, out int ac);
            thing.AC = ac;
            thing.Creature = CreatureType.Player;
            thing.Spot.Type = SpotType.Creature | SpotType.Player;
            _ = int.TryParse(textHP.Text, out int hp);
            thing.MaxHP = thing.HP = hp;
            thing.NumberAttacks = 1;
            if (gift)
            {
                _ = int.TryParse(textDarkVision.Text, out int dv);
                if (dv == 0 && !thing.Things.Any(t => t.LightRadius > 0)) //if player doesn't have a lightsource and no darkvision
                {
                    MessageBox.Show("Giving a torch to player", "Light", MessageBoxButtons.OK); //let's give player a torch
                    thing.LightRadius = 20;
                    Thing torch = new()
                    {
                        Name = "Torch",
                        Description = "a very basic light source"
                    };
                    torch.Spot.Type = SpotType.Takable | SpotType.LightSource;
                    torch.LightRadius = 20;
                    torch.Equippable = torch.Equipped = true;
                    thing.Things.Add(torch);
                }
                else //otherwise (we have a light source or darkvision)
                {
                    if (dv == 0) //if we don't have darkvision (therefore player bought a light source)
                    {
                        MessageBox.Show("You may need to equip your light source", "Reminder", MessageBoxButtons.OK); //remind them to use it
                    }
                }
                if (!thing.Things.Any(t => t.Spot.Type.HasFlag(SpotType.Weapon))) //if player has no weapons
                {
                    //give player a dagger and a sling
                    MessageBox.Show("Giving a dagger to player", "Melee", MessageBoxButtons.OK);
                    Thing dagger = new()
                    {
                        Name = "Dagger",
                        Description = "the simplest and least powerful of weapons"
                    };
                    dagger.Spot.Type = SpotType.Takable | SpotType.Weapon;
                    dagger.Bonus = 0;
                    dagger.Equippable = dagger.Equipped = true;
                    dagger.Damages.Add(new("1d4"));
                    dagger.DamageTypes.Add(DamageType.Piercing);
                    dagger.Value = 1;
                    thing.Things.Add(dagger);
                    Attack dagattack = new()
                    {
                        Name = "Dagger",
                        ToHit = _stats[1] / 2 - 5
                    };
                    dagattack.Damages.Add($"1d4+{_stats[0] / 2 - 5}");
                    dagattack.DamageTypes.Add(DamageType.Piercing);
                    thing.Attacks.Add(dagattack);
                    GameData.Melee = dagattack;
                    MessageBox.Show("Giving a sling to player", "Ranged", MessageBoxButtons.OK);
                    Thing sling = new()
                    {
                        Name = "Sling",
                        Description = "the simplest and least powerful of ranged weapons"
                    };
                    sling.Spot.Type = SpotType.Takable | SpotType.Weapon;
                    sling.Bonus = 0;
                    sling.Equippable = sling.Equipped = true;
                    sling.Damages.Add(new("1d4"));
                    sling.DamageTypes.Add(DamageType.Bludgeoning);
                    sling.Value = 1;
                    sling.Range = 30;
                    sling.MaxRange = 120;
                    thing.Things.Add(sling);
                    Attack slattack = new()
                    {
                        Name = "Sling",
                        ToHit = _stats[1] / 2 - 5
                    };
                    slattack.Damages.Add($"1d4+{_stats[1] / 2 - 5}");
                    slattack.DamageTypes.Add(DamageType.Bludgeoning);
                    slattack.Range = 30;
                    slattack.MaxRange = 120;
                    thing.Attacks.Add(slattack);
                    GameData.Ranged = slattack;
                }
                else //otherwise (player bought weapons)
                {
                    MessageBox.Show("You probably need to equip your weapons", "Reminder", MessageBoxButtons.OK); //remind player to use them
                }
            }
            GameData.Player = new(thing); //set the player to use the data
        }
        private void LoadPlayer()
        {
            Thing player = GameData.Player;
            thing.Name = player.Name;
            textName.Text = thing.Name;
            _stats[0] = player.STR;
            numericUpDownSTR.Value = _stats[0];
            _stats[1] = player.DEX;
            numericUpDownDEX.Value = _stats[1];
            _stats[2] = player.CON;
            numericUpDownCON.Value = _stats[2];
            _stats[3] = player.INT;
            numericUpDownINT.Value = _stats[3];
            _stats[4] = player.WIS;
            numericUpDownWIS.Value = _stats[4];
            _stats[5] = player.CHA;
            numericUpDownCHA.Value = _stats[5];
            thing.Value = player.Value;
            textGold.Text = $"{thing.Value}";
            thing.AC = player.AC;
            textAC.Text = $"{thing.AC}";
            thing.HP = player.HP;
            textHP.Text = $"{thing.HP}";
            thing.Things = new();
            foreach (var pthing in player.Things)
            {
                thing.Things.Add(new(pthing));
            }
            foreach (var att in player.Attacks)
            {
                thing.Attacks.Add(new(att));
            }
        }
    }
}
