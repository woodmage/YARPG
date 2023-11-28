namespace YARPG
{
    public partial class AttackCreation : Form
    {
        readonly Attack attack = new();
        readonly List<string> multimenu = new() { "List values", "Add value", "Delete value", "Clear all values" };
        public AttackCreation()
        {
            InitializeComponent();
        }

        private void EffectChanged(object sender, EventArgs e)
        {
            attack.Effect = Data.effecttypes[(string)comboEffect.SelectedItem];
        }

        private void DamagesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf((string)comboDamages.SelectedItem);
            if (index == 0)
            {
                ItemCreation.ListStrings(attack.Damages);
            }
            if (index == 1)
            {
                attack.Damages.Add(Dialogs.GetString());
            }
            if (index == 2)
            {
                ItemCreation.NotAvailable();
            }
            if (index == 3)
            {
                attack.Damages.Clear();
            }
        }

        private void DamageTypesChanged(object sender, EventArgs e)
        {
            int index = multimenu.IndexOf((string)comboDamageTypes.SelectedItem);
            if (index == 0)
            {
                ItemCreation.ListDamageTypes(attack.DamageTypes);
            }
            if (index == 1)
            {
                attack.DamageTypes.Add(Dialogs.GetDamageType());
            }
            if (index == 2)
            {
                ItemCreation.NotAvailable();
            }
            if (index == 3)
            {
                attack.DamageTypes.Clear();
            }
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            GameData.NewAttack = null;
            Close();
        }

        private void OkayClicked(object sender, EventArgs e)
        {
            attack.Name = textName.Text;
            attack.ToHit = ItemCreation.GetIntValue(textToHit, 0);
            attack.Range = ItemCreation.GetIntValue(textRange, 0);
            attack.MaxRange = ItemCreation.GetIntValue(textMaxRange, 0);
            attack.Area = ItemCreation.GetIntValue(textArea, 0);
            GameData.NewAttack = attack;
            Close();
        }

        private void ACload(object sender, EventArgs e)
        {
            ItemCreation.LoadCombo(comboEffect, Data.effecttypes);
            ItemCreation.LoadCombo(comboDamages, multimenu);
            ItemCreation.LoadCombo(comboDamageTypes, multimenu);
        }
    }
}
