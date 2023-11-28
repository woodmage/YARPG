namespace YARPG
{
    public partial class ItemCreation : Form
    {
        readonly Thing thing = new();
        readonly List<string> multimenu = new() { "List values", "Add value", "Delete value", "Clear list" };

        public ItemCreation()
        {
            InitializeComponent();
        }

        private void ICload(object sender, EventArgs e)
        {
            LoadCombo(comboAttacks, multimenu);
            LoadCombo(comboBaseRace, Data.racetypes);
            LoadCombo(comboClass, Data.classtypes);
            LoadCombo(comboCreature, Data.creaturetypes);
            LoadCombo(comboDamages, multimenu);
            LoadCombo(comboDamageTypes, multimenu);
            LoadCombo(comboEffect, Data.effecttypes);
            LoadCombo(comboImmunities, multimenu);
            LoadCombo(comboMonster, Data.monstertypes);
            LoadCombo(comboPotion, Data.potiontypes);
            LoadCombo(comboResistances, multimenu);
            LoadCombo(comboScroll, Data.scrolltypes);
            LoadCombo(comboSeenType, Data.seentypes);
            LoadCombo(comboSpotType, Data.spottypes);
            LoadCombo(comboThings, multimenu);
            LoadCombo(comboVulnerabilities, multimenu);
            LoadCombo(comboAlignment, Data.alignments);
        }

        private void SpotTypeChanged(object sender, EventArgs e)
        {
            thing.Spot.Type = Data.spottypes[GetCombo(comboSpotType)];
        }

        private void SeenTypeChanged(object sender, EventArgs e)
        {
            thing.Spot.Seen = Data.seentypes[GetCombo(comboSeenType)];
        }

        private void PotionChanged(object sender, EventArgs e)
        {
            thing.Potion = Data.potiontypes[GetCombo(comboPotion)];
        }

        private void ScrollChanged(object sender, EventArgs e)
        {
            thing.Scroll = Data.scrolltypes[GetCombo(comboScroll)];
        }

        private void EffectChanged(object sender, EventArgs e)
        {
            thing.Effect = Data.effecttypes[GetCombo(comboEffect)];
        }

        private void CreatureChanged(object sender, EventArgs e)
        {
            thing.Creature = Data.creaturetypes[GetCombo(comboCreature)];
        }

        private void BaseRaceChanged(object sender, EventArgs e)
        {
            thing.BaseRace = Data.racetypes[GetCombo(comboBaseRace)];
        }

        private void MonsterChanged(object sender, EventArgs e)
        {
            thing.Monster = Data.monstertypes[GetCombo(comboMonster)];
        }

        private void ClassChanged(object sender, EventArgs e)
        {
            thing.Class = Data.classtypes[GetCombo(comboClass)];
        }

        private void DamagesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboDamages));
            if (index == 0)
            {
                ListStrings(thing.Damages);
            }
            if (index == 1)
            {
                thing.Damages.Add(InputString());
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Damages.Clear();
            }
        }

        private void DamageTypesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboDamageTypes));
            if (index == 0)
            {
                ListDamageTypes(thing.DamageTypes);
            }
            if (index == 1)
            {
                thing.DamageTypes.Add(InputDamageType());
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.DamageTypes.Clear();
            }
        }

        private void ThingsChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboThings));
            if (index == 0)
            {
                ListThings(thing.Things);
            }
            if (index == 1)
            {
                Thing? thing = InputThing();
                thing?.Things.Add(thing);
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Things.Clear();
            }
        }

        private void AttacksChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboAttacks));
            if (index == 0)
            {
                ListAttacks(thing.Attacks);
            }
            if (index == 1)
            {
                Attack? att = InputAttack();
                if (att != null)
                {
                    thing.Attacks.Add(att);
                }
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Attacks.Clear();
            }
        }

        private void ResistancesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboResistances));
            if (index == 0)
            {
                ListDamageTypes(thing.Resistances);
            }
            if (index == 1)
            {
                thing.Resistances.Add(InputDamageType());
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Resistances.Clear();
            }
        }

        private void ImmunitiesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboImmunities));
            if (index == 0)
            {
                ListDamageTypes(thing.Immunities);
            }
            if (index == 1)
            {
                thing.Immunities.Add(InputDamageType());
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Immunities.Clear();
            }
        }

        private void VulnerabilitiesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf(GetCombo(comboVulnerabilities));
            if (index == 0)
            {
                ListDamageTypes(thing.Vulnerabilities);
            }
            if (index == 1)
            {
                thing.Vulnerabilities.Add(InputDamageType());
            }
            if (index == 2)
            {
                NotAvailable();
            }
            if (index == 3)
            {
                thing.Vulnerabilities.Clear();
            }
        }

        private void CancelClick(object sender, EventArgs e)
        {
            GameData.NewThing = null;
            Close();
        }

        private void OkayClick(object sender, EventArgs e)
        {
            thing.Name = textName.Text;
            thing.Description = textDescription.Text;
            thing.X = GetIntValue(textX, -1);
            thing.Y = GetIntValue(textY, -1);
            thing.Weight = GetIntValue(textWeight, 0);
            thing.Value = GetIntValue(textValue, 0);
            thing.Consumable = checkConsumable.Checked;
            thing.Equippable = checkEquippable.Checked;
            thing.Equipped = checkEquipped.Checked;
            thing.Bonus = GetIntValue(textBonus, 0);
            thing.Armor = GetIntValue(textArmor, 0);
            thing.Range = GetIntValue(textRange, 0);
            thing.MaxRange = GetIntValue(textMaxRange, 0);
            thing.Area = GetIntValue(textArea, 0);
            thing.Open = checkOpen.Checked;
            thing.Locked = checkLocked.Checked;
            thing.Dead = checkDead.Checked;
            thing.Hostile = checkHostile.Checked;
            thing.HP = GetIntValue(textHP, 0);
            thing.MaxHP = GetIntValue(textMaxHP, 0);
            thing.Speed = GetIntValue(textSpeed, 0);
            thing.FlySpeed = GetIntValue(textFlySpeed, 0);
            thing.ClimbSpeed = GetIntValue(textClimbSpeed, 0);
            thing.SwimSpeed = GetIntValue(textSwimSpeed, 0);
            thing.AC = GetIntValue(textAC, 0);
            thing.STR = GetIntValue(textSTR, 0);
            thing.DEX = GetIntValue(textDEX, 0);
            thing.CON = GetIntValue(textCON, 0);
            thing.INT = GetIntValue(textINT, 0);
            thing.WIS = GetIntValue(textWIS, 0);
            thing.CHA = GetIntValue(textCHA, 0);
            thing.NumberAttacks = GetIntValue(textNumberAttacks, 0);
            thing.Experience = GetIntValue(textExperience, 0);
            thing.Level = GetIntValue(textLevel, 0);
            thing.LightRadius = GetIntValue(textLightRadius, 0);
            thing.Race = textRace.Text;
            GameData.NewThing = thing;
            Close();
        }

        public static void LoadCombo<TEnum>(ComboBox combo, Dictionary<string, TEnum> dict)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            foreach (var str in dict.Keys)
            {
                combo.Items.Add(str);
            }
            combo.EndUpdate();
        }
        public static void LoadCombo(ComboBox combo, List<string> strings)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            foreach (var str in strings)
            {
                combo.Items.Add(str);
            }
            combo.EndUpdate();
        }

        private static string GetCombo(ComboBox combo) => (string)combo.SelectedItem;
        public static int GetIntValue(TextBox tbox, int defvalue)
        {
            if (int.TryParse(tbox.Text, out int n)) //if we can parse text into an integer,
            {
                return n; //return that
            }
            return defvalue; //otherwise, return default value
        }

        public static void ListStrings(List<string> strings)
        {
            string all = string.Join("\n", strings);
            MessageBox.Show(all, "Listing", MessageBoxButtons.OK);
        }
        public static void ListDamageTypes(List<DamageType> types)
        {
            List<string> all = new();
            foreach (var type in types)
            {
                string tstr = Data.damagetypes.FirstOrDefault(x => x.Value == type).Key;
                all.Add(tstr);
            }
            ListStrings(all);
        }
        private static void ListThings(List<Thing> things)
        {
            List<string> all = new();
            foreach (var thing in things)
            {
                all.Add(thing.Name);
            }
            ListStrings(all);
        }
        private static void ListAttacks(List<Attack> attacks)
        {
            List<string> all = new();
            foreach (var attack in attacks)
            {
                string adstr = string.Join(", ", attack.Damages);
                string tstr = $"+{attack.ToHit} to hit, Damage(s): {adstr}";
                all.Add(tstr);
            }
            ListStrings(all);
        }
        private static string InputString()
        {
            string rstr = Dialogs.GetString();
            return rstr;
        }
        private static DamageType InputDamageType()
        {
            DamageType type = Dialogs.GetDamageType();
            return type;
        }
        private static Attack? InputAttack()
        {
            AttackCreation attcre = new();
            attcre.ShowDialog();
            return GameData.NewAttack;
        }
        private static Thing? InputThing()
        {
            Thing? thing = GameData.NewThing; //make a backup copy of current thing
            ItemCreation newitem = new(); //set up to use this form again
            newitem.ShowDialog(); //show the form
            (thing, GameData.NewThing) = (GameData.NewThing, thing); //swap backup and current thing
            return thing; //return thing
        }
        public static void NotAvailable() => MessageBox.Show("Sorry, this function is not available.", "Error", MessageBoxButtons.OK);

        private void AlignmentChanged(object sender, EventArgs e)
        {
            thing.Alignment = Data.alignments[GetCombo(comboAlignment)];
        }
    }
    public static class Dialogs
    {
        private static Form form = new();
        private static Label label = new();
        private static TextBox textbox = new();
        private static ComboBox combobox = new();
        private static Button cancelbtn = new();
        private static Button okaybtn = new();
        private static string returnstr = string.Empty;
        private static DamageType damagetype = DamageType.None;
        private static bool isstring = false;

        public static string GetString()
        {
            InitializeForm<string>(); //make the form
            return returnstr; //return the string
        }

        public static DamageType GetDamageType()
        {
            InitializeForm<DamageType>(); //make the form
            return damagetype; //return the damagetype
        }

        private static void CancelClick(object? sender, EventArgs e)
        {
            returnstr = string.Empty; //set string to empty
            damagetype = DamageType.None; //set damagetype to none
            form.Close(); //close the form
        }

        private static void OkayClick(object? sender, EventArgs e)
        {
            if (isstring) //if we are using a string
            {
                returnstr = textbox.Text; //set string to textbox text
            }
            //else //otherwise
            //{
            //damagetype = Data.damagetypes[(string)combobox.SelectedItem]; //set damagetype to combobox selection
            //}
            form.Close(); //close the form
        }
        private static void InitializeForm<Type>()
        {
            form = new()
            {
                Text = "Input",
                ClientSize = new(180, 100)
            }; //make the form itself
            label = new()
            {
                Location = new(10, 10),
                Size = new(50, 30)
            }; //make a label
            textbox = new()
            {
                Location = new(70, 10),
                Size = new(100, 30),
                Enabled = false,
                Visible = false
            }; //make a textbox
            if (typeof(Type) == typeof(string)) //if we are doing a string
            {
                textbox.Enabled = true; //enable the textbox
                textbox.Visible = true; //show the textbox
                label.Text = "String?"; //set the label to say string
                isstring = true;
            }
            combobox = new ComboBox
            {
                Location = new(70, 10),
                Size = new(100, 30),
                Enabled = false,
                Visible = false
            }; //make a combobox
            if (typeof(Type) == typeof(DamageType)) //if we are doing a DamageType
            {
                combobox.Enabled = true; //enable the combobox
                combobox.Visible = true; //show the combobox
                combobox.SelectedIndexChanged += UpdateCombobox;
                ItemCreation.LoadCombo<DamageType>(combobox, Data.damagetypes); //fill the combobox with damagetypes
                label.Text = "Type?"; //set the label to say type
            }
            okaybtn = new()
            {
                Text = "Okay",
                Location = new(20, 50),
                Size = new(60, 40)
            }; //make an okay button
            cancelbtn = new()
            {
                Text = "Cancel",
                Location = new(100, 50),
                Size = new(60, 40)
            }; //make a cancel button
            form.Controls.Add(label); //add the controls
            form.Controls.Add(textbox);
            form.Controls.Add(combobox);
            form.Controls.Add(okaybtn);
            form.Controls.Add(cancelbtn);
            form.AcceptButton = okaybtn; //set accept button
            form.CancelButton = cancelbtn; //set cancel button
            okaybtn.Click += OkayClick; //set handler for okay button
            cancelbtn.Click += CancelClick; //set handler for cancel button
            form.ShowDialog(); //show the form
        }

        private static void UpdateCombobox(object? sender, EventArgs e)
        {
            damagetype = Data.damagetypes[(string)combobox.SelectedItem];
            //throw new NotImplementedException();
        }
    }
}
