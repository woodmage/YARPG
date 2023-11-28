using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YARPG
{
    public class Spell
    {
        public string Name { get; set; } = string.Empty; //every spell needs a name
        public int Level { get; set; } //every spell has a level
        public int Range { get; set; } //most spells have a range
        public int Area { get; set; } //many spells have an area of effect
        public int Duration { get; set; } //many spells have a duration
        public bool IsAttack { get; set; } //is it a spell attack
        public int SpellcastingBonus { get; set; } //this will be a plus to hit and stuff
        public int ToHit { get; set; } //to hit bonus for spell attacks
        public int SaveDC { get; set; } //save DC
        public AbilityType Ability { get; set; } //ability type to use with saves
        public SpellType Effect { get; set; } //effect of spell
        public List<ClassType> Classes { get; set; } = new(); //list of classes that can use the spell
        private static int LightRadius;
        private static int AC;
        public Spell() { } //default constructor
        public Spell(Spell s) //copy constructor
        {
            Name = s.Name;
            Level = s.Level;
            Range = s.Range;
            Area = s.Area;
            Duration = s.Duration;
            IsAttack = s.IsAttack;
            SpellcastingBonus = s.SpellcastingBonus;
            ToHit = s.ToHit;
            SaveDC = s.SaveDC;
            Ability = s.Ability;
            Effect = s.Effect;
            Classes.Clear();
            foreach (var c in s.Classes)
            {
                Classes.Add(c);
            } //Classes = s.Classes
        }
        public Spell(Thing caster, SpellType effect)
        {
            SpellData data = new(effect); //get a new spelldata from spell effect
            Effect = effect; //save effect
            Name = data.GetName(); //get name from spelldata
            Level = data.GetLevel(); //get level from spelldata
            Range = data.GetRange(); //get range from spelldata
            Area = data.GetArea(); //get area from spelldata
            Duration = data.GetDuration(); //get duration from spelldata
            IsAttack = data.GetIsAttack(); //get isattack from spelldata
            SpellcastingBonus = SpellData.GetSpellcastingBonus(caster); //get spellcasting bonus using caster
            ToHit = SpellData.GetToHit(caster); //get tohit bonus using caster
            SaveDC = SpellData.GetSaveDC(caster); //get savedc using caster
            Ability = data.GetAbility(); //get ability from spelldata
            Classes = data.GetClasses(); //get classes from spelldata
        }
        public Spell(StreamReader sr) => Load(sr);
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
        ///
        ///TODO: This is where we will be adding many spell executions
        ///
        public void CastIt(Thing caster) => DoSpell(caster, Effect);
        public static int HandleSpellSave(string damstr1, string damstr2, string damstr3, string damstr4, SpellType spell, Thing caster, Thing target)
        {
            if (caster.Level < 5) //if caster's level is less than five
            {
                return HandleSpellSave(damstr1, spell, caster, target); //handle spell save with first damage string
            }
            if (caster.Level < 11) //if caster's level is less than 11
            {
                return HandleSpellSave(damstr2, spell, caster, target); //handle spell save with second damage string
            }
            if (caster.Level < 17) //if caster's level is less than 17
            {
                return HandleSpellSave(damstr3, spell, caster, target); //handle spell save with third damage string
            }
            return HandleSpellSave(damstr4, spell, caster, target); //caster's level is more than 16, handle spell save with fourth damage string
        }
        public static int HandleSpellAttack(string damstr1, string damstr2, string damstr3, string damstr4, SpellType spell, Thing caster, Thing target)
        {
            if (caster.Level < 5) //if caster's level is less than five
            {
                return HandleSpellAttack(damstr1, spell, caster, target); //handle spell attack with first damage string
            }
            if (caster.Level < 11) //if caster's level is less than 11
            {
                return HandleSpellAttack(damstr2, spell, caster, target); //handle spell attack with second damage string
            }
            if (caster.Level < 17) //if caster's level is less than 17
            {
                return HandleSpellAttack(damstr3, spell, caster, target); //handle spell attack with third damage string
            }
            return HandleSpellAttack(damstr4, spell, caster, target); //caster's level is more than 16, handle spell attack with fourth damage string
        }
        public static int HandleSpellSave(string damstr, SpellType spell, Thing caster, Thing target)
        {
            Info.Out($"{caster.Name} casts a {Data.spelldata[spell].GetName()} spell!  {target.Name} rolls a saving throw...  "); //display info
            int damage = Rand.ParseAndRoll(damstr); //roll for damage
            int save = target.MakeSave(Data.spelldata[spell].GetAbility(), SpellData.GetSaveDC(caster)); //get save roll
            if (save == -1) //if crit fail
            {
                Info.Out("Critical Fail  ", Color.DarkGreen); //display info
                damage *= 2; //damage multiplied by 2
            }
            else if (save == 0) //if fail
            {
                Info.Out("Fail  ", Color.DarkGreen); //display info
            }
            else if (save == 1) //if saved
            {
                Info.Out("Success  ", Color.DarkRed); //display info
                damage /= 2; //damage divided by 2
            }
            else if (save == 20) //if crit save
            {
                Info.Out("Critical Success  ", Color.DarkRed); //display info
                damage = 0; //damage is 0
            }
            Info.OutL($"{damage} damage."); //display info
            return damage; //return damage
        }
        public static int HandleSpellAttack(string damstr, SpellType spell, Thing caster, Thing target)
        {
            Info.Out($"{caster.Name} attacks with a {Data.spelldata[spell].GetName()} spell!  "); //display info
            bool crit = false; //assume no crit
            int attackroll = Rand.D(20); //roll to hit
            if (attackroll == 20) //if we got a 20
            {
                Info.Out("Critical ");
                crit = true; //then it is a crit
            }
            if (attackroll == 1 || attackroll + SpellData.GetToHit(caster) < target.AC) //if we missed
            {
                Info.OutL($"Missed!", Color.DarkRed); //display info
                return 0; //return 0 damage
            }
            int damage = Rand.ParseAndRoll(damstr, crit); //roll for damage
            Info.OutL($"Hit for {damage} damage!", Color.DarkGreen); //display info
            return damage; //return damage
        }
        public static void DoExperience(Thing caster, Thing target)
        {
            Info.OutL($"{caster.Name} killed {target.Name}!"); //display info
            caster.Experience += target.Experience; //add experience
            caster.CheckExperience(); //check experience (and handle level up)
            ///
            ///NOTE: We are removing the getting rid of target because it could be in an enumeration loop
            ///
            //GameData.CurrentLevel.Creatures.Remove(target); //get rid of target
            target.Dead = true; //make sure target's Dead flag is set so GameLoop will get rid of it
            TargetingSystem.Remove(target); //remove target from targeting system
        }
        public static bool DoSpell(Thing caster, SpellType spell)
        {
            //list of spells that self-target
            SpellType[] selftarget = new SpellType[] { SpellType.Healing, SpellType.ExtraHealing, SpellType.Invisibility, SpellType.MagicMapping, SpellType.Light };
            if (TargetingSystem.IsNone()) //if we don't have a target
            {
                if (!selftarget.Contains(spell)) //if spell isn't one that self-targets
                {
                    Info.OutL("You don't have anything targetted!"); //display info
                    return false; //report failure
                }
            }
            Thing target = TargetingSystem.GetSingle();
            List<Thing> targetlist = new();
            int damage; //variable for damage
            switch (spell) //switch by spell
            {
                case SpellType.Healing: //if healing
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Healing.wav"); //play healing sound
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell.");
                    caster.Heal($"1d8+{SpellData.GetSpellcastingBonus(caster)}"); //heal
                    return true; //report success
                case SpellType.ExtraHealing: //if extra healing
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Healing2.wav"); //play second healing sound
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell.");
                    caster.Heal($"10d4+{SpellData.GetSpellcastingBonus(caster)}"); //heal
                    return true; //report success
                case SpellType.MagicMapping: //if magic mapping
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell.");
                    if (!Settings.DoNotUpdateMainOnMM)
                    {
                        foreach (var s in GameData.CurrentLevel.Map.MapSpots) //for each spot in map
                        {
                            s.Spot.See(); //make it seen
                            s.Spot.DontSee(); //and now not seen
                        }
                        foreach (var i in GameData.CurrentLevel.Items.Where(i => i.IsStairs())) //for each stairs down or stairs up
                        {
                            i.Spot.See(); //make it seen
                        }
                    }
                    if (Settings.MiniMapDrawOnMM) //if we draw the minimap on magic mapping
                    {
                        Form1.ShowMiniMap(); //then draw it
                    }
                    return true; //report success
                case SpellType.Invisibility: //if invisibility
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    SpellData sd = new(SpellType.Invisibility); //get spell data for invisibility
                    SpellEffect se = new(YARPG.Effect.Invisible, sd.Duration); //make a new spell effect
                    caster.AddCondition(se); //add to conditions
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell and is invisible for {sd.Duration} rounds.");
                    return true;
                case SpellType.TashasHideousLaughter: //if Tasha's hideous laughter
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic3.wav"); //play magic sound
                    Info.Out($"{caster.Name} casts a {Data.spelldata[spell].GetName()} spell!  {target.Name} rolls a saving throw...  "); //display info
                    int save = target.MakeSave(Data.spelldata[spell].GetAbility(), SpellData.GetSaveDC(caster)); //get save roll
                    if (save < 1)
                    {
                        Info.OutL($"Failed.  {target.Name} is on the floor laughing helplessly.");
                        SpellData stasha = new(SpellType.TashasHideousLaughter); //get spell data for tasha's hideous laughter
                        SpellEffect setasha = new(spell, stasha.Duration, SpellData.GetSaveDC(caster), Data.spelldata[spell].Ability); //get spell effect for spell, duration, savedc, and abiltiy
                        target.AddCondition(new(SpellType.TashasHideousLaughter, stasha.Duration, SpellData.GetSaveDC(caster), Data.spelldata[spell].Ability)); //add condition for spell effect
                        DoSpellEffect(target, setasha); //do spell effect
                    }
                    else
                    {
                        Info.OutL($"Success.  {target.Name} is not affected.");
                    }
                    return true;
                case SpellType.AcidSplash: //if acid splash
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic2.wav"); //play magic sound
                    damage = HandleSpellSave("1d6", "2d6", "3d6", "4d6", spell, caster, target); //get damage, handling save
                    if (target.TakeHit(damage, DamageType.Acid)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.ChillTouch: //if chill touch
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic2.wav"); //play magic sound
                    damage = HandleSpellAttack("1d8", "2d8", "3d8", "4d8", spell, caster, target); //get damage
                    if (target.TakeHit(damage, DamageType.Necrotic)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.FireBolt: //if fire bolt
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic2.wav"); //play magic sound
                    damage = HandleSpellAttack("1d10", "2d10", "3d10", "4d10", spell, caster, target); //get damage
                    if (target.TakeHit(damage, DamageType.Fire)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.Friends: //if friends
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    Info.OutL($"{caster.Name} casts the {Data.spelldata[spell].GetName()} spell and {target.Name} is now friendly!"); //display info
                    target.Hostile = false; //turn off hostility
                    return true; //report success
                case SpellType.Light: //if light
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    LightRadius = caster.LightRadius; //save light radius for when spell ends
                    Info.OutL($"{caster.Name} casts the {Data.spelldata[spell].Name} spell."); //display info
                    SpellData slight = new(SpellType.Light); //make new spell data (for duration)
                    caster.AddCondition(new(SpellType.Light, Data.spelldata[SpellType.Light].Duration)); //add condition to caster with spell and duration
                    return true; //report success
                case SpellType.MageArmor: //if mage armor
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    AC = target.AC; //save AC for when spell ends
                    Info.OutL($"{caster.Name} casts the {Data.spelldata[spell].Name} spell and seems a bit better protected."); //display info
                    SpellData smage = new(SpellType.MageArmor); //make new spell data (for duration)
                    target.AddCondition(new(SpellType.MageArmor, Data.spelldata[SpellType.MageArmor].Duration)); //add condition to target with spell and duration
                    return true; //report success
                case SpellType.WitchBolt: //if witch bolt
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Shock1.wav"); //play shock sound
                    Info.Out($"{caster.Name} casts the {Data.spelldata[spell].Name} spell at {target.Name} "); //display info
                    int casterlevel = GetCasterLevel(caster); //get spellcaster's level
                    damage = GetSpellDamage("1d12", 1, "1d12", casterlevel); //get spell damage
                    Info.OutL($"and hit for {damage} electrical damage.");
                    if (target.TakeHit(damage, DamageType.Electricity)) //have target take damage, if target is killed
                    {
                        DoExperience(caster, target); //give experience to caster
                    }
                    else
                    {
                        SpellData swbolt = new(SpellType.WitchBolt); //make a new spelldata (for duration)
                        SpellEffect sewbolt = new(SpellType.WitchBolt, Data.spelldata[SpellType.WitchBolt].Duration)
                        {
                            SpellCaster = caster //set spellcaster in spelleffect
                        }; //make a new spelleffect
                        target.AddCondition(sewbolt); //add spelleffect to target
                    }
                    return true;
                case SpellType.PoisonSpray: //if poison spray
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic2.wav"); //play magic sound
                    damage = HandleSpellSave("1d12", "2d12", "3d12", "4d12", spell, caster, target); //get damage, handling save
                    if (target.TakeHit(damage, DamageType.Poison)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.RayOfFrost: //if ray of frost
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic2.wav"); //play magic sound
                    damage = HandleSpellAttack("1d8", "2d8", "3d8", "4d8", spell, caster, target); //get damage
                    if (target.TakeHit(damage, DamageType.Cold)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.ShockingGrasp: //if shocking grasp
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Shock2.wav"); //sound for shock damage
                    damage = HandleSpellAttack("1d8", "2d8", "3d8", "4d8", spell, caster, target); //get damage
                    if (target.TakeHit(damage, DamageType.Electricity)) //apply damage, if it kills target
                    {
                        DoExperience(caster, target); //do experience
                    }
                    return true; //report success
                case SpellType.ConfuseMonsters: //if confuse monsters
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell!");
                    targetlist = TargetingSystem.GetWithinArea(2); //get list of targets
                    foreach (var t in targetlist) //for each target
                    {
                        if (t.MakeSave(Data.spelldata[spell].GetAbility(), SpellData.GetSaveDC(caster)) < 1) //have target attempt a save, if they fail
                        {
                            //add the confusion effect to the target
                            t.AddCondition(new(YARPG.Effect.Confusion, Data.spelldata[spell].Duration, SpellData.GetSaveDC(caster), Data.spelldata[spell].Ability));
                        }
                    }
                    return true; //report success
                case SpellType.SlowMonsters: //if slow monsters
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    targetlist = TargetingSystem.GetMultiple(); //get list of targets
                    Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell!");
                    foreach (var t in targetlist) //for each target
                    {
                        if (t.MakeSave(Data.spelldata[spell].Ability, SpellData.GetSaveDC(caster)) < 1) //have target attempt a save, if they fail
                        {
                            //add the slow effect to the target
                            t.AddCondition(new(YARPG.Effect.Slow, Data.spelldata[spell].Duration, SpellData.GetSaveDC(caster), Data.spelldata[spell].Ability));
                        }
                    }
                    return true; //report success
                case SpellType.MagicMissile: //if magic missile
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.MagicMissiles.wav"); //sound for magic missiles
                    if (TargetingSystem.IsMultiple())
                    {
                        Info.OutL($"{caster.Name} casts a {Data.spelldata[spell].Name} spell on multiple targets.");
                        targetlist = TargetingSystem.GetMultiple(); //get list of targets
                        int nummissiles = GetCasterLevel(caster) + 2; //compute number of missiles
                        for (int i = 0; i < nummissiles; i++) //for each missile
                        {
                            if (targetlist.Count == 0) //if we are out of targets
                            {
                                return true; //report success
                            }
                            Thing currenttarget = targetlist[i % targetlist.Count]; //set current target
                            damage = Rand.ParseAndRoll("1d4+1"); //get damage for that target
                            Info.OutL($"It hit {currenttarget.Name} for {damage}");
                            if (currenttarget.TakeHit(damage, DamageType.Force)) //give damage to target, if it dies
                            {
                                DoExperience(caster, currenttarget); //handle experience
                                targetlist.Remove(currenttarget); //remove target from list of targets
                            }
                        }
                    }
                    else
                    {
                        Info.Out($"{caster.Name} cast a {Data.spelldata[spell].Name} spell on {target.Name} ");
                        damage = GetSpellDamage("3d4+3", 1, "1d4+1", GetCasterLevel(caster));
                        Info.OutL($"and hit for {damage} force damage.");
                        if (target.TakeHit(damage, DamageType.Force)) //give damage to target, if it dies
                        {
                            DoExperience(caster, target); //handle experience
                        }
                    }
                    return true; //report success
                case SpellType.BurningHands: //if burning hands
                case SpellType.SleepMonsters: //if sleep monsters
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic3.wav"); //play magic sound
                    MessageBox.Show("This type of multiple selection is not yet implemented.", "Oops!", MessageBoxButtons.OK); //show message
                    break;
            }
            MessageBox.Show($"Sorry, {Data.spelldata[spell].GetName()} is not yet implemented.", "Oops!", MessageBoxButtons.OK); //show message
            return false; //report failure
        }
        public static int GetSpellDamage(string damstr, int spelllevel, string incstr, int casterlevel)
        {
            int damage = Rand.ParseAndRoll(damstr); //roll for initial damage
            for (int i = spelllevel; i < casterlevel; i++) //then for each level above the spelllevel
            {
                damage += Rand.ParseAndRoll(incstr); //roll to add that damage
            }
            return damage; //return the amount of damage
        }
        public static int GetCasterLevel(Thing caster)
        {
            int[] spelllevels = Enumerable.Repeat(0, 10).ToArray(); //storage for the accumulated levels for the spellcaster
            for (int i = 0; i <= caster.Level; i++) //for each level up to and including the caster's level
            {
                for (int j = 0; j < 10; j++) //for each spelllevel (cantrip, level 1 - level 9)
                {
                    spelllevels[j] += Data.classspellbook[caster.Class][i, j]; //add to accumulated levels
                }
            }
            for (int i = 9; i >= 0; i--) //for each spelllevel, counting down
            {
                if (spelllevels[i] != 0) //if there are spells available for that level,
                {
                    return i; //return that level as it is the highest level the caster can cast
                }
            }
            return -1; //this value indicates the caster is not a spellcaster!
        }
        public static void DoSpellEffect(Thing target, SpellEffect spell)
        {
            int casterlevel = GetCasterLevel(spell.SpellCaster); //get spell caster's spell level
            switch (spell.Spell) //switch on spell used
            {
                case SpellType.Light: //if light
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    target.LightRadius = 40; //set target's light radius to 40
                    break;
                case SpellType.MageArmor: //if mage armor
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic1.wav"); //play magic sound
                    target.AC = AC + 3; //set target's AC to 3 more than it was
                    break;
                case SpellType.WitchBolt: //if witch bolt
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Shock2.wav"); //play shock sound
                    int damage = GetSpellDamage("1d12", 1, "1d12", casterlevel); //calculate damage
                    Info.OutL($"{Data.spelldata[spell.Spell].Name} does {damage} electrical damage to {target.Name}.");
                    if (target.TakeHit(damage, DamageType.Electricity)) //give damage to target, if target is killed
                    {
                        DoExperience(spell.SpellCaster, target); //handle experience
                    }
                    break;
                case SpellType.TashasHideousLaughter: //if Tasha's hideous laughter
                    SoundSystem.PlayEmbeddedSound("YARPG.Resources.Magic3.wav"); //play magic sound
                    target.AddCondition(new(YARPG.Effect.Prone, 1)); //set target to prone for this round
                    target.AddCondition(new(YARPG.Effect.Incapacitated, 1)); //set target to incapacitated for this round
                    break;
            }
        }
        public static void EndSpellEffect(Thing target, SpellEffect spell)
        {
            switch (spell.Spell) //spell which we are ending
            {
                case SpellType.Light: //if light
                    target.LightRadius = LightRadius; //restore target's light radius
                    break;
                case SpellType.MageArmor: //if mage armor
                    target.AC = AC; //restore target's AC
                    break;
                case SpellType.WitchBolt: //if witch bolt
                case SpellType.TashasHideousLaughter: //if Tasha's hideous laughter
                    break; //just break (no action taken)
            }
        }
    }
    public enum SpellType
    {
        None,
        Healing,
        ExtraHealing,
        Invisibility,
        MagicMapping,
        SlowMonsters,
        SleepMonsters,
        ConfuseMonsters,
        AcidSplash,
        ChillTouch,
        FireBolt,
        Friends,
        Light,
        PoisonSpray,
        RayOfFrost,
        ShockingGrasp,
        BurningHands,
        MageArmor,
        MagicMissile,
        TashasHideousLaughter,
        WitchBolt,
    }
    public class SpellEffect
    {
        private Effect effect; //what effect
        private SpellType spell; //spell
        private int timeleft; //how much time is left for it
        private int savedc; //save dc
        private AbilityType ability; //ability to use for saving
        public Effect Effect { get => effect; set => effect = value; } //what effect
        public SpellType Spell { get => spell; set => spell = value; } //what spell
        public int Timeleft { get => timeleft; set => timeleft = value; } //how much time is left for it
        public int SaveDC { get => savedc; set => savedc = value; } //save dc
        public AbilityType Ability { get => ability; set => ability = value; } //ability to use for saving
        public Thing SpellCaster { get; set; } = GameData.Player; //who cast the spell or caused the effect
        public SpellEffect(Effect effect, int duration)
        {
            Effect = effect; //copy effect and duration
            Timeleft = duration;
            Spell = SpellType.None; //set spell to none to show no spell
            Ability = AbilityType.None; //set ability to none to show no saves
        }
        public SpellEffect(SpellType spell, int duration)
        {
            Spell = spell; //copy spell and duration
            Timeleft = duration;
            Effect = Effect.None; //set effect to none to show no effect
            Ability = AbilityType.None; //set ability to none to show no saves
        }
        public SpellEffect(Effect effect, int duration, int savedc, AbilityType ability)
        {
            Effect = effect; //copy effect, duration, savedc, and ability
            Timeleft = duration;
            SaveDC = savedc;
            Ability = ability;
            Spell = SpellType.None; //set spell to none to show no spell
        }
        public SpellEffect(SpellType spell, int duration, int savedc, AbilityType ability)
        {
            Spell = spell; //copy spell, duration, savedc, and ability
            Timeleft = duration;
            SaveDC = savedc;
            Ability = ability;
            Effect = Effect.None; //set effect to none to show no effect
        }
        public SpellEffect(SpellEffect se) //copy constructor
        {
            Effect = se.Effect; //copy effect, spell, duration, savedc, and ability
            Spell = se.Spell;
            Timeleft = se.Timeleft;
            SaveDC = se.SaveDC;
            Ability = se.Ability;
        }
        public SpellEffect(StreamReader sr) => Load(sr);
        public bool ExtraSave(Thing target)
        {
            bool done = false;
            if (Effect != Effect.None) //if it is an effect
            {
                Info.Out($"{target.Name} saving against {Writer.EffectToWord(Effect)}...  "); //display info
            }
            else //otherwise (it is a spell)
            {
                Info.Out($"{target.Name} saving against {Data.spelldata[Spell].Name}...  "); //display info
            }
            int save = target.MakeSave(Ability, SaveDC); //get save roll
            if (save < 1) //if fail
            {
                Info.OutL("Fail!  Effect continues.", Color.DarkGreen); //display info
            }
            else //otherwise (if saved)
            {
                Info.OutL("Success!  Effect ends.", Color.DarkRed); //display info
                YARPG.Spell.EndSpellEffect(target, this); //end spell effect
                done = true; //we are done with effect
            }
            return done; //return true for effect done, false for effect continues
        }
        public bool Time(Thing target)
        {
            bool done = false; //we are not yet done
            if (Ability != AbilityType.None) //if we have an ability score
            {
                if (Effect != Effect.None) //if it is an effect
                {
                    Info.Out($"{target.Name} saving against {Writer.EffectToWord(Effect)}...  "); //display info
                }
                else //otherwise (it is a spell)
                {
                    Info.Out($"{target.Name} saving against {Data.spelldata[Spell].Name}...  "); //display info
                }
                int save = target.MakeSave(Ability, SaveDC); //get save roll
                if (save < 1) //if fail
                {
                    Info.Out("Fail  ", Color.DarkGreen); //display info
                }
                else if (save > 0) //if saved
                {
                    Info.Out("Success  ", Color.DarkRed); //display info
                    done = true; //we are done with effect
                }
            }
            else
            {
                if (Effect != Effect.None) //if it is an effect
                {
                    Info.Out($"{target.Name} is affected by {Writer.EffectToWord(Effect)}...  "); //display info
                }
                else //otherwise (it is a spell)
                {
                    Info.Out($"{target.Name} is affected by {Data.spelldata[Spell].Name}...  "); //display info
                }
            }
            if (!done) //if we are not done with effect
            {
                Timeleft--; //take one away from time left
                if (Timeleft < 1) //if time left is less than 1
                {
                    done = true; //we are done with effect
                }
            }
            if (done) //if we are done with effect
            {
                Info.OutL("Effect is done."); //display info
                YARPG.Spell.EndSpellEffect(target, this); //end spell effect
                return true; //report effect completed
            }
            Info.OutL("Effect continues."); //display info
            YARPG.Spell.DoSpellEffect(target, this); //do spell effect
            return false; //report effect continuing
        }
        public void Load(StreamReader sr) => InputOutput.Load(this, sr);
        public void Save(StreamWriter sw) => InputOutput.Save(this, sw);
    }
    public class SpellData
    {
        public readonly Dictionary<SpellType, string> spellname = new() { { SpellType.None, "None" }, { SpellType.Healing, "Healing" }, { SpellType.ExtraHealing, "Extra Healing" },
            { SpellType.Invisibility, "Invisibility" }, { SpellType.MagicMapping, "Magic Mapping" }, { SpellType.SlowMonsters, "Slow" }, { SpellType.SleepMonsters, "Sleep" },
            { SpellType.ConfuseMonsters, "Confusion" }, { SpellType.AcidSplash, "Acid Splash" }, { SpellType.ChillTouch, "Chill Touch" }, { SpellType.FireBolt, "Fire Bolt" },
            { SpellType.Friends, "Friends" }, { SpellType.Light, "Light" }, { SpellType.PoisonSpray, "Poison Spray" }, { SpellType.RayOfFrost, "Ray of Frost" },
            { SpellType.ShockingGrasp, "Shocking Grasp" }, { SpellType.BurningHands, "Burning Hands" }, { SpellType.MageArmor, "Mage Armor" },
            { SpellType.MagicMissile, "Magic Missile" }, { SpellType.TashasHideousLaughter, "Tasha's Hideous Laughter" }, { SpellType.WitchBolt, "Witch Bolt" } };
        public readonly Dictionary<SpellType, string> spelldescription = new() { { SpellType.None, "Um, not a spell" }, { SpellType.Healing, "A basic Healing spell" }, 
            { SpellType.ExtraHealing, "A bit more advanced Healing spell" }, { SpellType.Invisibility, "A spell to turn the caster invisible" }, 
            { SpellType.MagicMapping, "A spell to show all the rooms, doors, and paths" }, { SpellType.SlowMonsters, "A spell to slow a group of monsters" }, 
            { SpellType.SleepMonsters, "Your basic Sleep spell" }, { SpellType.ConfuseMonsters, "Your basic Confusion spell" }, 
            { SpellType.AcidSplash, "Splash a bit of acid on your opponents" }, { SpellType.ChillTouch, "Brrr.  Necromantic damage" }, { SpellType.FireBolt, "A bolt of fire" }, 
            { SpellType.Friends, "Make friends of your enemies" }, { SpellType.Light, "Make a magical light" }, { SpellType.PoisonSpray, "Spray a bit of poison" }, 
            { SpellType.RayOfFrost, "Make a ray of coldness" }, { SpellType.ShockingGrasp, "Electrical damage" }, { SpellType.BurningHands, "Spread some fire damage" }, 
            { SpellType.MageArmor, "Magical armor" }, { SpellType.MagicMissile, "Little magic darts" }, { SpellType.TashasHideousLaughter, "Make an opponent helpless with laughter" },
            { SpellType.WitchBolt, "A bit of magic damage" } };
        public readonly Dictionary<SpellType, int> spelllevel = new() { { SpellType.None, -1 }, { SpellType.Healing, 1 }, { SpellType.ExtraHealing, 3 }, { SpellType.Invisibility, 1 },
            { SpellType.MagicMapping, 1 }, { SpellType.SlowMonsters, 1 }, { SpellType.SleepMonsters, 1 }, { SpellType.ConfuseMonsters, 1 }, { SpellType.AcidSplash, 0 },
            { SpellType.ChillTouch, 0 }, { SpellType.FireBolt, 0 }, { SpellType.Friends, 0 }, { SpellType.Light, 0 }, { SpellType.PoisonSpray, 0 }, { SpellType.RayOfFrost, 0 },
            { SpellType.ShockingGrasp, 0 }, { SpellType.BurningHands, 1 }, { SpellType.MageArmor, 1 }, { SpellType.MagicMissile, 1 }, { SpellType.TashasHideousLaughter, 1 },
            { SpellType.WitchBolt, 1 } };
        public readonly Dictionary<SpellType, int> spellrange = new() { { SpellType.None, -1 }, { SpellType.Healing, 0 }, { SpellType.ExtraHealing, 0 }, { SpellType.Invisibility, 0 },
            { SpellType.MagicMapping, 0 }, { SpellType.SlowMonsters, 120 }, { SpellType.SleepMonsters, 90 }, { SpellType.ConfuseMonsters, 90 }, { SpellType.AcidSplash, 60 },
            { SpellType.ChillTouch, 120 }, { SpellType.FireBolt, 120 }, { SpellType.Friends, 0 }, { SpellType.Light, 0 }, { SpellType.PoisonSpray, 10 }, { SpellType.RayOfFrost, 60 },
            { SpellType.ShockingGrasp, 0 }, { SpellType.BurningHands, 0 }, { SpellType.MageArmor, 0 }, { SpellType.MagicMissile, 120 }, { SpellType.TashasHideousLaughter, 30 },
            { SpellType.WitchBolt, 20 } };
        public readonly Dictionary<SpellType, int> spellarea = new() { { SpellType.None, 0 }, { SpellType.Healing, 0 }, { SpellType.ExtraHealing, 0 }, { SpellType.Invisibility, 0 },
            { SpellType.MagicMapping, 0 }, { SpellType.SlowMonsters, -6 }, { SpellType.SleepMonsters, 40 }, { SpellType.ConfuseMonsters, 20 }, { SpellType.AcidSplash, -2 },
            { SpellType.ChillTouch, 0 }, { SpellType.FireBolt, 0 }, { SpellType.Friends, 0 }, { SpellType.Light, 40 }, { SpellType.PoisonSpray, 0 }, { SpellType.RayOfFrost, 0 },
            { SpellType.ShockingGrasp, 0 }, { SpellType.BurningHands, 0 }, { SpellType.MageArmor, 0 }, { SpellType.MagicMissile, -3 }, { SpellType.TashasHideousLaughter, 0 },
            { SpellType.WitchBolt, 0 } };
        public readonly Dictionary<SpellType, int> spellduration = new() { { SpellType.None, 0 }, { SpellType.Healing, 0 }, { SpellType.ExtraHealing, 0 }, { SpellType.Invisibility, 0 },
            { SpellType.MagicMapping, 0 }, { SpellType.SlowMonsters, 10 }, { SpellType.SleepMonsters, 10 }, { SpellType.ConfuseMonsters, 10 }, { SpellType.AcidSplash, 0 },
            { SpellType.ChillTouch, 0 }, { SpellType.FireBolt, 0 }, { SpellType.Friends, 0 }, { SpellType.Light, 600 }, { SpellType.PoisonSpray, 0 }, { SpellType.RayOfFrost, 0 },
            { SpellType.ShockingGrasp, 0 }, { SpellType.BurningHands, 0 }, { SpellType.MageArmor, 4800 }, { SpellType.MagicMissile, 0 }, { SpellType.TashasHideousLaughter, 10 },
            { SpellType.WitchBolt, 10 } };
        public readonly Dictionary<SpellType, bool> spellisattack = new() { { SpellType.None, false }, { SpellType.Healing, false }, { SpellType.ExtraHealing, false },
            { SpellType.Invisibility, false }, { SpellType.MagicMapping, false }, { SpellType.SlowMonsters, false }, { SpellType.SleepMonsters, false },
            { SpellType.ConfuseMonsters, false }, { SpellType.AcidSplash, false }, { SpellType.ChillTouch, true }, { SpellType.FireBolt, true }, { SpellType.Friends, false },
            { SpellType.Light, false }, { SpellType.PoisonSpray, false }, { SpellType.RayOfFrost, true }, { SpellType.ShockingGrasp, true }, { SpellType.BurningHands, false },
            { SpellType.MageArmor, false }, { SpellType.MagicMissile, false }, { SpellType.TashasHideousLaughter, false }, { SpellType.WitchBolt, true } };
        public readonly Dictionary<SpellType, AbilityType> spellability = new() { { SpellType.None, AbilityType.None }, { SpellType.Healing, AbilityType.None },
            { SpellType.ExtraHealing, AbilityType.None }, { SpellType.Invisibility, AbilityType.None }, { SpellType.MagicMapping, AbilityType.None },
            { SpellType.SlowMonsters, AbilityType.WIS }, { SpellType.SleepMonsters, AbilityType.None }, { SpellType.ConfuseMonsters, AbilityType.WIS },
            { SpellType.AcidSplash, AbilityType.DEX }, { SpellType.ChillTouch, AbilityType.None }, { SpellType.FireBolt, AbilityType.None }, { SpellType.Friends, AbilityType.None },
            { SpellType.Light, AbilityType.None }, { SpellType.PoisonSpray, AbilityType.CON }, { SpellType.RayOfFrost, AbilityType.None },
            { SpellType.ShockingGrasp, AbilityType.None }, { SpellType.BurningHands, AbilityType.DEX }, { SpellType.MageArmor, AbilityType.None },
            { SpellType.MagicMissile, AbilityType.None }, { SpellType.TashasHideousLaughter, AbilityType.WIS }, { SpellType.WitchBolt, AbilityType.None } };
        public readonly Dictionary<SpellType, string> spellclasses = new() { { SpellType.None, "" }, { SpellType.Healing, "bcdpr" }, { SpellType.ExtraHealing, "bcdpr" },
            { SpellType.Invisibility, "bswW" }, { SpellType.MagicMapping, "bcdprswW" }, { SpellType.SlowMonsters, "bcdprswW" }, { SpellType.SleepMonsters, "bswW" },
            { SpellType.ConfuseMonsters, "bcdprswW" }, { SpellType.AcidSplash, "swW" }, {SpellType.ChillTouch, "swW" }, { SpellType.FireBolt, "drswW" }, { SpellType.Friends, "bcdprswW" },
            { SpellType.Light, "bcdprswW" }, { SpellType.PoisonSpray, "swW" }, { SpellType.RayOfFrost, "swW" }, { SpellType.ShockingGrasp, "swW" }, { SpellType.BurningHands, "swW" },
            { SpellType.MageArmor, "swW" }, { SpellType.MagicMissile, "swW" }, { SpellType.TashasHideousLaughter, "bswW" }, { SpellType.WitchBolt, "bswW" } };
        public SpellType Spell { get; set; }
        public Thing Caster { get; set; } = GameData.Player;
        public SpellData(SpellType spell)
        {
            Spell = spell;
        }
        public void SetCaster(Thing caster) => Caster = caster;
        public string GetName() => spellname[Spell];
        public string GetDescription() => spelldescription[Spell];
        public int GetLevel() => spelllevel[Spell];
        public int GetRange() => spellrange[Spell];
        public int GetArea() => spellarea[Spell];
        public int GetDuration() => spellduration[Spell];
        public bool GetIsAttack() => spellisattack[Spell];
        public AbilityType GetAbility() => spellability[Spell];
        public static int GetSpellcastingBonus(Thing caster)
        {
            return caster.Class switch
            {
                ClassType.Artificer => caster.INTmod,
                ClassType.Barbarian => caster.WISmod,
                ClassType.Bard => caster.CHAmod,
                ClassType.Cleric => caster.WISmod,
                ClassType.Druid => caster.WISmod,
                ClassType.Explorer => caster.INTmod,
                ClassType.Fighter => caster.INTmod,
                ClassType.Monk => caster.WISmod,
                ClassType.Paladin => caster.CHAmod,
                ClassType.Ranger => caster.WISmod,
                ClassType.Rogue => caster.INTmod,
                ClassType.Sorcerer => caster.CHAmod,
                ClassType.Warlock => caster.CHAmod,
                ClassType.Wizard => caster.INTmod,
                _ => caster.INTmod,
            };
        }
        public static int GetToHit(Thing caster) => GetSpellcastingBonus(caster) + caster.ProfBonus;
        public static int GetSaveDC(Thing caster) => GetToHit(caster) + 8;
        public List<ClassType> GetClasses()
        {
            List<ClassType> classes = new();
            foreach (char c in spellclasses[Spell])
            {
                classes.Add(c switch
                {
                    'a' => ClassType.Artificer,
                    'b' => ClassType.Bard,
                    'B' => ClassType.Barbarian,
                    'c' => ClassType.Cleric,
                    'd' => ClassType.Druid,
                    'f' => ClassType.Fighter,
                    'p' => ClassType.Paladin,
                    'r' => ClassType.Ranger,
                    'R' => ClassType.Rogue,
                    's' => ClassType.Sorcerer,
                    'w' => ClassType.Warlock,
                    'W' => ClassType.Wizard,
                    _ => ClassType.Explorer,
                });
            }
            if (Spell != SpellType.None)
            {
                classes.Add(ClassType.Explorer);
            }
            return classes;
        }
        public string Name => GetName();
        public string Description => GetDescription();
        public int Level => GetLevel();
        public int Range => GetRange();
        public int Area => GetArea();
        public int Duration => GetDuration();
        public bool IsAttack => GetIsAttack();
        public AbilityType Ability => GetAbility();
        public List<ClassType> Classes => GetClasses();
    }

    /// <summary>
    /// GetSpells class - provides a form for adding spells to the player's spell list
    /// </summary>
    public static class GetSpells
    {
        private static bool informcreation; //used to keep spell data from changing while form is being set up
        private static readonly Thing caster = GameData.Player; //spellcaster is player
        private static List<Spell> spellbook = new(); //spellbook of available spells
        private static readonly Form form = new() //a form
        {
            ClientSize = new(600, 750), //clientsize of 600 by 750
            Text = "Choose your spells" //text for caption
        };
        private static readonly Label instructionslabel = new() //a label for instuctions
        {
            Size = new(580, 30), //size of 580 by 30
            Location = new(10, 10), //location is 10, 10
        };
        private static readonly CheckedListBox spellslistbox = new() //a checked listbox for spells
        {
            Size = new(580, 570), //size of 580 by 650
            Location = new(10, 130), //location is 10, 50
            CheckOnClick = true, //single click will check / uncheck
            ThreeDCheckBoxes = false //use 2-state - no indeterminate state
        };
        private static readonly Button cancelbutton = new() //a button for cancel
        {
            Size = new(75, 30), //size of 75 by 30
            Location = new(150, 710), //location is 150, 710
            Text = "Cancel" //text on button is "Cancel"
        };
        private static readonly Button acceptbutton = new() //a button for accept
        {
            Size = new(75, 30), //size of 75 by 30
            Location = new(375, 710), //location is 375, 710
            Text = "Accept" //text on button is "Accept"
        };
        private static readonly Label[] spellleveldesc = new Label[10];
        private static readonly Label[] spelllevelnumb = new Label[10];
        private static readonly string[] spelllevelstr = new string[10];
        private static readonly int[] spelllevels = new int[10];
        static GetSpells()
        {
            int len = (600 - 110) / 10; //this would be 48
            for (int i = 0; i < 10; i++) //for each spell level
            {
                if (i == 0) //if first pass through
                {
                    spelllevelstr[i] = "Cantrip"; //then spell level string is cantrips
                }
                else //otherwise (1 to 9)
                {
                    spelllevelstr[i] = $"Level {i}"; //set spell level string to spell level
                }
                spellleveldesc[i] = new(); //make a spell level description label
                spelllevelnumb[i] = new(); //make a spell level number label
                spellleveldesc[i].Size = new(len, 30); //set size of spell level description label
                spelllevelnumb[i].Size = new(len, 30); //and size of spell level number label
                spellleveldesc[i].Location = new(10 + (len + 10) * i, 50); //set location of spell level description label
                spelllevelnumb[i].Location = new(10 + (len + 10) * i, 90); //set location of spell level number label
                spellleveldesc[i].Text = spelllevelstr[i]; //set text for spell level description label
            }
        }
        public static void DoForm()
        {
            ///
            ///NOTE: As the following information may change from level to level of the caster, it therefore needs to be regenerated each time the DoForm method is called
            ///
            informcreation = true; //put a hold on changing spell data while the form is being set up
            instructionslabel.Text = "You may choose "; //start the instructions label text
            for (int i = 0; i < 10; i++) //for each spell level
            {
                spelllevels[i] = Data.classspellbook[caster.Class][caster.Level, i]; //compute spells for the spell level
                spelllevelnumb[i].Text = $"{spelllevels[i]}"; //set text for spell level number label
                form.Controls.Add(spellleveldesc[i]); //add spell level description label to form
                form.Controls.Add(spelllevelnumb[i]); //add spell level number label to form
                instructionslabel.Text += $"{spelllevelnumb[i].Text} {spelllevelstr[i]}, "; //continue building instructions label text
                if (i == 8) //if this is the next to the last spell level
                {
                    instructionslabel.Text += "and "; //put in a nice connector word in instructions label text
                }
            }
            ///
            ///NOTE: The following code block was in a static constructor because we were assigning spellbook to its sorted form and spellbook was a readonly variable
            ///      Also, this information should not change if the static method DoForm is called again (like on a level up).  
            ///      However, since we are getting an index out of range error on level ups, we're moving it back here again and have taken away the readonly modifier.
            ///
            spellbook.Clear(); //clear the spellbook since this class may get called again on levelling up
            foreach (var d in Data.spelldata) //for each dictionary entry in spelldata (all spells)
            {
                List<ClassType> classes = d.Value.GetClasses(); //get list of classes from spelldata entry
                if (classes.Contains(caster.Class)) //if list of classes contains spellcaster's class
                {
                    spellbook.Add(new(caster, d.Key)); //add spell to spellbook
                }
            }
            spellbook = spellbook.OrderBy(spell => spell.Level).ThenBy(spell => spell.Name).ToList(); //sort the spell list
            spellslistbox.SuspendLayout(); //suspend layout for spells checkedlistbox while we add items to it
            spellslistbox.Items.Clear(); //this is needed for when the form is called again like on a level up
            foreach (var spell in spellbook) //for every spell in spellbook
            {
                spellslistbox.Items.Add($"{spell.Name} ({spelllevelstr[spell.Level]}): {Data.spelldata[spell.Effect].GetDescription()}"); //add entry in spells checkedlistbox
                int index = spellslistbox.Items.Count - 1; //get index of entry
                if (caster.SpellList.Any(playerspell => playerspell.Effect == spell.Effect)) //if spell is in spellcaster's spell list
                {
                    spellslistbox.SetItemCheckState(index, CheckState.Checked); //set spell checked
                }
                else //otherwise (spell is new)
                {
                    spellslistbox.SetItemCheckState(index, CheckState.Unchecked); //set spell unchecked
                }
            }
            spellslistbox.ResumeLayout(); //resume layout for spells checkedlistbox
            spellslistbox.ItemCheck += SpellCheck; //set eventhandler for spells checkedlistbox item checking
            acceptbutton.Click += AcceptSpells; //set eventhandler for accept button
            cancelbutton.Click += CancelSpells; //set eventhandler for cancel button
            form.Controls.Add(instructionslabel); //add instructions label to form
            form.Controls.Add(spellslistbox); //add spells checkedlistbox to form
            form.Controls.Add(acceptbutton); //add accept button to form
            form.Controls.Add(cancelbutton); //add cancel button to form
            form.AcceptButton = acceptbutton; //set accept button as form's accept button
            ///
            ///NOTE: For some reason, setting the form cancel button to our cancel button causes it to bypass my validate on cancel logic!
            ///      Do not uncomment the form.CancelButton line below.
            ///
            //form.CancelButton = cancelbutton; //set cancel button as form's cancel button -- NOTE!  Setting this bypasses my validate on cancel logic!  Do not use!
            informcreation = false; //allow checking / unchecking spells to change the spell data
            form.ShowDialog(); //show form and wait for response
        }

        private static void SpellCheck(object? sender, ItemCheckEventArgs e)
        {
            if (informcreation) //if we are in form creation
            {
                return; //quick exit
            }
            int index = e.Index; //get index of item that was checked/unchecked
            int level = spellbook[index].Level; //get level of spell
            if (!spellslistbox.GetItemChecked(index)) //if the item is now checked
            {
                spelllevels[level]--; //subtract one from appropriate spell level
            }
            else //otherwise (item is now unchecked)
            {
                spelllevels[level]++; //add one to appropriate spell level
            }
            spelllevelnumb[level].Text = $"{spelllevels[level]}"; //update spell level number label
            spelllevelnumb[level].Invalidate(); //tell windows to update the label
        }

        private static void CancelSpells(object? sender, EventArgs e)
        {
            if (!CheckSpellLevels(false)) //if checking spell levels, the user decided to abort
            {
                return; //quick exit
            }
            acceptbutton.Click -= AcceptSpells; //detach event handlers
            cancelbutton.Click -= CancelSpells;
            spellslistbox.ItemCheck -= SpellCheck;
            form.Close(); //just close the form, we are done
        }

        private static int ComputeSpellLevels(bool isforaccept)
        {
            ///
            ///NOTE: The way this is set up, the user may elect to not take an available level 2 spell but to take 2 extra level 1 spells or similar.
            ///      This makes the selection really about getting the appropriate number of spell levels rather than the specific numbers allotted.
            ///      This differs from RAW D&D 5.0 in that in RAW, you may take ONE lower level spell instead of a higher level spell.
            ///      This does feel more "fair" however.
            ///      This does not at this time verify cantrips as they do not calculate into the spell levels.
            ///
            int totalspelllevels = 0; //set total spell levels to 0
            for (int i = 1; i < 10; i++) //for each spell level - we start at 1 since cantrips would be multiplied by 0 anyway.
            {
                if (isforaccept) //if this computation is for accepting the spell list
                {
                    totalspelllevels += spelllevels[i] * i; //add each spell level times its level to total spell levels
                }
                else //otherwise (it is for cancelling the spell list)
                {
                    totalspelllevels += Data.classspellbook[caster.Class][caster.Level, i] * i; //compute potential spell level times its level and add to total spell levels
                }
            }
            return totalspelllevels; //return total spell levels
        }

        private static bool GetYesNo(string prompt) => Form1.GetYesNo($"{prompt}. Are you sure this is correct?");

        private static bool CheckSpellLevels(bool isforaccept)
        {
            int totalspelllevels = ComputeSpellLevels(isforaccept); //get total spell levels
            if (totalspelllevels < 0) //if it is less than 0 (meaning we took too many spells)
            {
                if (GetYesNo($"You took {-totalspelllevels} too many spell levels")) //if user agrees
                {
                    return true; //report continue
                }
                return false; //report abort
            }
            if (totalspelllevels > 0) //if it is more than 0 (meaning we still have spell levels we can take)
            {
                if (GetYesNo($"You have {totalspelllevels} spell levels available")) //if user agrees
                {
                    return true; //report continue
                }
                return false; //report abort
            }
            return true; //report continue
        }

        private static void AcceptSpells(object? sender, EventArgs e)
        {
            if (!CheckSpellLevels(true)) //if checking spell levels, the user decided to abort
            {
                return; //quick exit
            }
            for (int i = 0; i < spellslistbox.Items.Count; i++) //for each spell in the list
            {
                var spell = spellbook[i]; //make a local copy of the spellbook entry
                if (spellslistbox.GetItemChecked(i)) //if a spell is checked
                {
                    if (!caster.SpellList.Any(playerspell => playerspell.Effect == spell.Effect)) //if spell is not in caster's SpellList
                    {
                        caster.SpellList.Add(spell); //add it in
                    }
                }
                else //otherwise (spell is not checked)
                {
                    if (caster.SpellList.Any(playerspell => playerspell.Effect == spell.Effect)) //if spell is in caster's SpellList
                    {
                        caster.SpellList.Remove(spell); //remove it
                    }
                }
            }
            acceptbutton.Click -= AcceptSpells; //detach event handlers
            cancelbutton.Click -= CancelSpells;
            spellslistbox.ItemCheck -= SpellCheck;
            form.Close(); //and now we can close the form, thus returning control
        }
    }
}
